# Publish Package to Nuget.org

## Dotnet Cli

> Move to the project (.csproj) directory.

1. Pack the Project
The `dotnet pack` command is used to create a `.nupkg` file from your project.
```bash
dotnet pack --configuration Release
```
**Common Options:**
* --configuration <Debug|Release>: Specifies the build configuration (default is Debug). Use Release for production packages.
* --output <path>: Specifies the folder to output the .nupkg file. By default, it goes to bin/<configuration>/.
* --version-suffix <suffix>: Adds a version suffix (e.g., 1.0.0-alpha).

**Example**
```bash
dotnet pack --configuration Release --output ./nuget/
```

2. Push the Package

The `dotnet nuget` push command is used to push the `.nupkg` file to a NuGet feed.

```bash
dotnet nuget push <path-to-nupkg> --api-key <api-key> --source <source-url>
```

**Parameters:**
* <path-to-nupkg>: Path to your .nupkg file.
* --api-key <api-key>: Your NuGet API key, required for authentication.
* --source <source-url>: The URL of the NuGet feed (e.g., https://api.nuget.org/v3/index.json for the public NuGet gallery).

**Example**
```bash
dotnet nuget push ./nuget/MyPackage.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

3. Example Workflow

```bash
# Step 1: Pack the project
dotnet pack --configuration Release --output ./nuget/

# Step 2: Push the package to NuGet
dotnet nuget push ./nuget/MyPackage.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json

```
### Specifying Package Version

Use the `--property:Version` option during the `dotnet pack` command.

```bash
dotnet pack --configuration Release --property:Version=1.2.3
```
This sets the version of the package to 1.2.3.

**Alternative:** Set Version in `.csproj`
You can also define the version directly in the project file (.csproj) by adding the <Version> property:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <Version>1.2.3</Version>
  </PropertyGroup>
</Project>
```
This version will be used whenever you run dotnet pack

**Using a Suffix (e.g., Pre-release Versions)**

To append a suffix to the version (e.g., `1.2.3-beta`), use the `--version-suffix` option:

```bash
dotnet pack --configuration Release --version-suffix beta
```
This will produce a package version like `1.2.3-beta`, assuming `1.2.3` is specified in the `.csproj` or through other means.


**Overriding Project File Settings**

If you specify the version in both the .csproj file and the dotnet pack command, the command-line version takes precedence.

Example:
```bash
dotnet pack --configuration Release --property:Version=2.0.0
```
Even if the `.csproj` has `<Version>1.2.3</Version>`, the package will have the version `2.0.0`.


### Centralize Version Management
Step 1: Use a common property for the version to ensure consistency across both projects. You can achieve this by using:

**Directory.Build.props**

Create a `Directory.Build.props` file in the root directory of your solution (or wherever the common parent directory of the projects is).

Example `Directory.Build.props`:
```bash
<Project>
  <PropertyGroup>
    <Version>1.2.0</Version>
  </PropertyGroup>
</Project>
```
This will apply the `Version` property to all projects within the directory structure.


**Step 2: Reference the Common Version**

Each project will automatically inherit the Version property from `Directory.Build.props`. You don’t need to specify the version explicitly in individual `.csproj`files.

Example `.csproj` for Project A:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>
</Project>
```

Example .csproj for Project B (which references Project A):

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\ProjectA\ProjectA.csproj" />
  </ItemGroup>
</Project>

```

**Step 3: Update the Version Dynamically**

When incrementing the version (e.g., for a new build), you only need to update it in the `Directory.Build.props` file.

**Step 4: Pack and Push Both Projects**

Use the `dotnet pack` and `dotnet nuget` push commands to pack and push both projects in the correct dependency order:
> Note Ensure You’re in the Correct Directory
Navigate to the directory containing your `.csproj` or `.sln` file before running the command.

`For my case I need to pack all the package so My current diroctory is .sln directory where I will run below command`
**Pack Command:**
```bash
dotnet pack --configuration Release
```
This will create .nupkg files for both projects, with consistent versions.

**Push Command:**

```bash
dotnet nuget push ./ProjectA/bin/Release/ProjectA.1.2.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
dotnet nuget push ./ProjectB/bin/Release/ProjectB.1.2.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```




## Visual Studio

### Set Up the Package Information in .csproj

In your project’s .csproj file, make sure you have defined the necessary NuGet metadata like PackageId, Version, and other details:

```xml
<PropertyGroup>
  <TargetFramework>net7.0</TargetFramework>  <!-- Your target framework -->
  <PackageId>MyLibrary</PackageId>
  <Version>1.0.0</Version>
  <Authors>Your Name</Authors>
  <Company>Your Company</Company>
  <Description>A description of your package</Description>
  <PackageTags>rabbitmq;connection;manager</PackageTags>
</PropertyGroup>
```

### Build the Project
Before packaging, make sure your project is built so that the .dll is available.

Go to Build in the top menu and select Build Solution (or press Ctrl + Shift + B).
This will generate the .dll in the bin\Release or bin\Debug folder.


### Pack the Project in Visual Studio
You can use Visual Studio’s built-in tools to create a NuGet package. Here’s how:

Right-click the project in Solution Explorer.
Select Publish or Pack (depending on your version of Visual Studio).
If you see Publish:
Click Publish and choose NuGet Package as the destination (if it prompts you to choose a publishing target).
This will create a .nupkg file.
If you see **Pack** directly:
* Select **Pack** to create the .nupkg file.
The `.nupkg` file will be placed in the `/bin/Release` or `/bin/Debug` folder (based on your build configuration).

### Publish the Package (Optional)
Once you have the .nupkg file, you can push it to NuGet.org or any other NuGet feed.

* Push to NuGet.org:
	1. Create an API key on NuGet.org.
	2. Use the following command to push the package:

```bash
dotnet nuget push bin/Release/RabbitMqSenderReceiver.1.0.0.nupkg --api-key Api_Key --source https://api.nuget.org/v3/index.json
```

> Note: Once published, your `.dll` will be available as a NuGet package that others can install and use.
