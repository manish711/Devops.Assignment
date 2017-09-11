#addin nuget:?package=SharpZipLib&version=0.86.0
#addin "Cake.Compression"
#addin "Cake.FileHelpers"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Define directories.
var buildDir = Directory("build");
var testDir = Directory("Test Summary");
var deployDir = Directory("deploy");
var artifactsDir = Directory("artifacts");

var apps = new [] {"HelloWorld","UnitTestProject"};

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    CleanDirectory(testDir);
    CleanDirectory(deployDir);
    CleanDirectory(artifactsDir);
	 CleanDirectories("src/HelloWorld/obj");
    CleanDirectories("src/HelloWorld/bin");	
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
   NuGetRestore("src/HelloWorld.sln",new NuGetRestoreSettings { MSBuildVersion = NuGetMSBuildVersion.MSBuild12 });
});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() =>
{
  MSBuild("./src/HelloWorld.sln");
});

Task("Test")
	.IsDependentOn("Build")
    .Does(() =>
{
   var testProjects = GetFiles("./src/tests/**/*.csproj");
    foreach(var testProject in testProjects) {
        DotCoverCover(
            tool => tool.DotNetCoreTest(testProject.FullPath),
            testDir + testProject.GetFilenameWithoutExtension() + ".dcvr",
            new DotCoverCoverSettings()                
                .WithFilter("-:*Tests"));
    }
    DotCoverMerge(GetFiles(testDir.Path + "/*.dcvr"), testDir + File("iae.dcvr"));
    TeamCity.ImportDotCoverCoverage(testDir + File("iae.dcvr"));
});

Task("Package")
  .IsDependentOn("Build")
  .Does(() => {
    var nuGetPackSettings   = new NuGetPackSettings {
                                    Id                      = "HelloWorld",
                                    Version                 = "0.0.0.1",
                                    Title                   = "Hello World",
                                    Authors                 = new[] {"Manish Narang"},
                                    Description             = "Devops Assignment cake.build scripts.",
                                    Summary                 = "Excellent summary of what the Cake (C# Make) build tool does.",
                                    ProjectUrl              = new Uri("https://github.com/manish711/Devops.Assignment"),
                                    Files                   = new [] {
                                                                        new NuSpecContent {Source = "HelloWorld.exe", Target = "bin"},
                                                                      },
                                    BasePath                = "./src/HelloWorld/bin/Debug",
                                    OutputDirectory         = "./nuget"
                                };

    NuGetPack(nuGetPackSettings);
  });
  
Task("Delete")
       .IsDependentOn("Package")
       .Does( ()=>
{
	CleanDirectory(buildDir);
});


// set default task
Task("Default").IsDependentOn("Delete");

RunTarget(target);