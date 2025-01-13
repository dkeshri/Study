# Publish Package to Nuget.org

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
