# Rabbit MQ

Generally in Message Queue system __Producer(Sender)__ and __Consumer(Reciver)__ are saperate application therefore 
best practice to implement RabbitMq Configuration is to have one __Connection__ per `Process` Or `Application` and one __Channel__ per thread.

> We follow this convention to have one connection per process and one channel per thread.

So in our current implimentation sender and reciver are on same application so we share one connection to both sender and reciver of message queue.
and also we have one channel through of application.

but we can also create two channel one for sender and one for reciver.

> Note: If our sender and receiver have two application then we also create two connection. and each connection have there respective channels depending upon there need. 

## Docker Container Creation

> Run below command to create RabbitMq Docker container

```bash
docker run -d -v rabbitmqv:/var/log/rabbitmq --hostname rmq --name RabbitMqServer -p 5672:5672 -p 8080:15
672 rabbitmq:3.13-management
```
### Port Detail

Port `8080` is for management portal and access by below mention __Login credentials__.

Click on the link for <a href='http://localhost:8080/'>Admin Portal</a>

Port `5672` is use in communication during producing and consuming of message.

### Login crediential

> Default login crediential if we not specifiy during creation of docker container

<small style='color:green'>_Username_</small> `guest` and <small style='color:green'>_Password_</small> `guest`
 

# Publish RabbitMq App to nuget package.

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

Once published, your `.dll` will be available as a NuGet package that others can install and use.

