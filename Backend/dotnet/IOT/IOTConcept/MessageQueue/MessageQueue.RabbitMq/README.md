# Dkeshri.MessageQueue.RabbitMq
This will help the user manage the connection and allow them to send messages to the RabbitMQ queue. 
Additionally, there is a RabbitMQ queue receiver that retrieves the messages.

## How to Configure

This package uses the `IServiceCollection` to setup. We have an Extension **AddRabbitMqServices** Method is use to setup RabbitMq Connections.

**Register Sender**
> Note Make sure to set `RegisterSenderServices` to `true` while configuration then only it will Provide `IMessageReceiver`
```csharp
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
```

**Register Receiver**

> Note Make sure to set `RegisterReceiverServices` to `true` while configuration then only it will Provide `ISendMessage`

```csharp
services.AddRabbitMqServices((config) =>
{
    config.HostName = "RabbitMqHost";
    config.Port = 5672; // your RabbitMq Port
    config.QueueName = "YourQueueName";
    config.UserName = "RabblitMqUserName";
    config.Password = "password";
    config.ClientProvidedName = "ProviderName"; // Receiver or Any name you like
    config.RegisterReceiverServices = true;
});
```

> Note: Idealy One Application is Sender and other application is receiver. but you can configure both sender/receiver in one application too. 


## How to Use

### In Sender Application

For the Sender it Provide `ISendMessage` interface. Having following methods.
`SendToQueue(string message)`, `SendToQueue(string queueName, string message)` 
You can also provide queueName while sending Messages to rabbitMQ.

`ISendMessage` is provided during **confugration**  in `IServiceCollection` dependency injection Container and inject in Constructor to your class as below code.

> Note Make sure to also register you class in `IServiceCollection` before running application.


**Example**

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

There is an interface `IMessageReceiver` that has delegate `Action<object?, BasicDeliverEventArgs, IModel>? MessageHandler` .
Allow you to provide your callback method so that it can be called when message received from rabbitMq Queue.

> Note Make sure to set `RegisterReceiverServices` to `true` while configuration then only it will Provide `IMessageReceiver`

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













# Rabbit MQ SetpUp

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
 

