# Dkeshri.MessageQueue.RabbitMq
This will help the user manage the connection and allow them to send messages to the RabbitMQ queue. 
Additionally, there is a RabbitMQ queue receiver that retrieves the messages.

## How to Configure

This package uses the `IServiceCollection` to setup. We have an Extension **AddRabbitMqServices** Method is use to setup RabbitMq Connections.

**Register Sender**
> Note Make sure to set `RegisterSenderServices` to `true`
```csharp
services.AddRabbitMqServices((config) =>
{
    config.HostName = "RabbitMqHost";
    config.Port = 5672; // your RabbitMq Port
    config.QueueName = "YourQueueName";
    config.UserName = "RabblitMqUserName";
    config.Password = "password";
    config.ClientProvidedName = "ProviderName"; // Sender or Any name you like
    config.RegisterSenderServices = true; // Set True to register Sender services
});
```

**Register Receiver**

> Note Make sure to set `RegisterReceiverServices` to `true`

```csharp
services.AddRabbitMqServices((config) =>
{
    config.HostName = "RabbitMqHost";
    config.Port = 5672; // your RabbitMq Port
    config.QueueName = "YourQueueName";
    config.UserName = "RabblitMqUserName";
    config.Password = "password";
    config.ClientProvidedName = "ProviderName"; // Receiver or Any name you like
    config.RegisterReceiverServices = true; // Set to True to register Receiver services.
});
```

> Note: Idealy One Application is Sender and other application is receiver. but you can configure both sender/receiver in one application too. 


## How to Use

### In Sender Application

The `ISendMessage` interface is provided for the sender, offering the following methods:

* SendToQueue(string message)
* SendToQueue(string queueName, string message)

You can specify the `queueName` when sending messages to RabbitMQ.

The `ISendMessage` interface is registered during the configuration phase in the `IServiceCollection` dependency injection container and can be injected into your class constructor, as demonstrated in the code below:

```csharp
class SendMessageToRabbitMq : ISendMessageToRabbitMq
{
    private ISendMessage SendMessage { get; }
    public SendMessageToRabbiMq(ISendMessage sendMessage)
    {
        SendMessage = sendMessage;
    }

    public bool SendMessageToRabbitMq(string DataToSend)
    {
        return SendMessage.SendToQueue(DataToSend);
    }

    public bool SendMessageToRabbitMq(string queueName, string DataToSend)
    {
        return SendMessage.SendToQueue(queueName,DataToSend);
    }
}
```

Register `SendMessageToRabbiMq` in `IServiceCollection`

```csharp
var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((hostContext, services) =>
{
    services.AddRabbitMqServices((config) =>
    {
        config.HostName = "RabbitMqHost";
        config.Port = 5672; // your RabbitMq Port
        config.QueueName = "YourQueueName";
        config.UserName = "RabblitMqUserName";
        config.Password = "password";
        config.ClientProvidedName = "ProviderName"; // Sender or Any name you like
        config.RegisterSenderServices = true;
    });

    services.AddSingleton<ISendMessageToRabbitMq, SendMessageToRabbiMq>();

});
builder.RunConsoleAsync().Wait();
```


### In Receiver Application

The `IMessageReceiver` interface includes a delegate:
`Action<object?, BasicDeliverEventArgs, IModel>? MessageHandler`.

This allows you to provide a callback method that will be invoked when a message is received from the RabbitMQ queue.

> Note: Ensure that `RegisterReceiverServices` is set to `true` during configuration. This is necessary for the `IMessageReceiver` interface to be provided. 

As demonstrated in the code below:

```csharp
var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((hostContext, services) =>
{
    services.AddRabbitMqServices((config) =>
    {
        config.HostName = "RabbitMqHost";
        config.Port = 5672; // your RabbitMq Port
        config.QueueName = "YourQueueName";
        config.UserName = "RabblitMqUserName";
        config.Password = "password";
        config.ClientProvidedName = "ProviderName"; // Sender or Any name you like
        config.RegisterReceiverServices = true;
    });
    services.AddSingleton<ISendMessageToRabbitMq, SendMessageToRabbiMq>();

});

var host = builder.UseConsoleLifetime().Build();

using (var scope = host.Services.CreateScope())
{
    var messageReceiver = scope.ServiceProvider.GetRequiredService<IMessageReceiver>();
    messageReceiver.MessageHandler = (model,data,channel) =>
    {
        var message = data.Body.ToArray();
        var messageString = Encoding.UTF8.GetString(message);
        Console.WriteLine(messageString);
    };
}

host.RunAsync().Wait();
```

# Rabbit MQ SetUp

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
 

