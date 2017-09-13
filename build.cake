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

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    MSTest("./src/tests/UnitTestProject/bin/debug/UnitTestProject.dll");
});

Task("Run-Integration-Tests")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    MSTest("./src/tests/IntegrationTestProject/bin/debug/IntegrationTestProject.dll");
});

Task("Package")
  .IsDependentOn("Run-Integration-Tests")
  .Does(() => {
		ZipCompress("./src/HelloWorld/bin/Debug", deployDir + File("HelloWorld.zip") );     
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