using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Serilog;

[GitHubActions(
    "Push",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = true,
    OnPushBranchesIgnore = new[] { "master", "main" },
    OnPullRequestBranches = new[] { "master" },
    InvokedTargets = new[] { nameof(RunTests) },
    FetchDepth = 0
)]
[GitHubActions(
    "Pack",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = true,
    OnPullRequestTags = new[] { "v[0-9]+.[0-9]+.[0-9]+" },
    InvokedTargets = new[] { nameof(Pack) },
    FetchDepth = 0
)]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [Parameter] readonly AbsolutePath TestResultDirectory = RootDirectory + "/.nuke/Artifacts/Test-Results/";
    [GitRepository] GitRepository GitRepository;

    [GitVersion(UpdateBuildNumber = true, Framework = "net8.0")] GitVersion GitVersion;
    [Parameter] AbsolutePath ArtifactsDirectory => RootDirectory + "/.nuke/Artifacts";

    Target LogInformation =>
        definition =>
            definition.Executes(() =>
            {
                Log.Information($"Solution path : {Solution}");
                Log.Information($"Solution directory : {Solution.Directory}");
                Log.Information($"Configuration : {Configuration}");
                Log.Information($"TestResultDirectory : {TestResultDirectory}");
                Log.Information($"Solution path : {Solution}"); 
                Log.Information("GitVersion = {Value}", GitVersion.MajorMinorPatch);
            });

    Target Preparation =>
        definition => definition.DependsOn(LogInformation)
            .Executes(() =>
            {
                ArtifactsDirectory.CreateOrCleanDirectory();
                Log.Information($"Directory {ArtifactsDirectory.Name} : create or clean");

                TestResultDirectory.CreateOrCleanDirectory();
                Log.Information($"Directory {TestResultDirectory.Name} : create or clean");

                DotNetTasks.DotNetClean();
                Log.Information($"Solution has been clean : {Solution.Name}");
            });


    Target Restore =>
        definition =>
            definition.DependsOn(Preparation)
                .Executes(
                    () =>
                    {
                        DotNetTasks.DotNetRestore();
                    });

    Target Compile =>
        definition =>
            definition
                .DependsOn(Restore)
                .Executes(() =>
                {
                    DotNetTasks
                        .DotNetBuild(
                            build => build.SetProjectFile(Solution)
                                .SetConfiguration(Configuration)
                        );
                });

    Target RunTests =>
        definition =>
            definition.DependsOn(Compile)
                .Executes(() =>
                {
                    var testProjects = Solution.AllProjects.Where(s => s.Name.EndsWith("Tests"));

                    DotNetTasks.DotNetTest(
                        configurator =>
                            configurator.SetConfiguration(Configuration)
                                .SetNoBuild(true)
                                .SetNoRestore(true)
                                .ResetVerbosity()
                                .SetResultsDirectory(TestResultDirectory)
                                .EnableCollectCoverage()
                                .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                                .SetExcludeByFile("*.Generated.cs")
                                .EnableUseSourceLink()
                                .CombineWith(
                                    testProjects, (settings, project) => settings.SetProjectFile(project)
                                        .AddLoggers($"trx;LogFileName={project.Name}.trx")
                                        .SetCoverletOutput(TestResultDirectory + $"{project.Name}.xml")
                                )
                    );
                });

    Target Pack =>
        definition =>
            definition
                .DependsOn(RunTests)
                .Produces(ArtifactsDirectory)
                .Executes(() =>
                    {
                        DotNetTasks.DotNetPack(
                            configurator =>
                                configurator
                                    .SetProject(Solution)
                                    .SetConfiguration(Configuration)
                                    .SetVersion(GitVersion.MajorMinorPatch)
                                    .SetOutputDirectory(ArtifactsDirectory)
                                    .SetNoBuild(true)
                                    .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                                    .SetIncludeSymbols(true)
                        );
                    }
                );

    public static int Main() => Execute<Build>(x => x.Pack);
}