# About

This library helps users manage connections and facilitates sending and receiving messages to and from RabbitMQ exchanges and queues. Additionally, it includes a RabbitMQ queue receiver for retrieving messages.

## Package Dependency
```csharp
Dkeshri.MessageQueue
```
## How to Use

This package uses the `IServiceCollection` to setup. We have an Extension Method **AddMessageBroker** in `Dkeshri.MessageQueue` provide **MessageBroker** object to setup Message Broker Propery.

There is another extension method `AddRabbitMqServices` provided in this library which is use to setup RabbitMq Connections. 

> `AddRabbitMqServices` is extention to `MessageBroker`

**Register Sender**
> Note Make sure to set `RegisterSenderServices` to `true`

**Sender For Exchange** When you want to send message to `Exchange` below is the prefered way.

Call the `UseExchange` extension method on the `RabbitMqConfig` object returned by the `AddRabbitMqServices` method.

**Step 1**

```csharp
services.AddMessageBroker(messageBroker =>
{
    messageBroker.RegisterSenderServices = true; // Set True to register Sender services
    messageBroker.ClientProvidedName = "Sender"; // Sender or Any name you like
    messageBroker.AddRabbitMqServices((rabbitMqConfig) =>
    {
        rabbitMqConfig.HostName = "RabbitMqHost";
        rabbitMqConfig.Port = 5672; // your RabbitMq Port
        rabbitMqConfig.UserName = "username";
        rabbitMqConfig.Password = "password";
    }).UseExchange(exchange =>
    {
        exchange.ExchangeName = "ExchangeName";
        exchange.IsDurable = true; // this is required for durable exchange
    });
});
```

OR

**Sender For Queue** When you want to send message to `queue`, below is the prefered way.

Call the `UseQueue` extension method on the `RabbitMqConfig` object returned by the `AddRabbitMqServices` method.

```csharp
services.AddMessageBroker(messageBroker =>
{
    messageBroker.RegisterSenderServices = true; // Set True to register Sender services
    messageBroker.ClientProvidedName = "Sender"; // Sender or Any name you like
    messageBroker.AddRabbitMqServices((rabbitMqConfig) =>
    {
        rabbitMqConfig.HostName = "RabbitMqHost";
        rabbitMqConfig.Port = 5672; // your RabbitMq Port
        rabbitMqConfig.UserName = "username";
        rabbitMqConfig.Password = "password";
    }).UseQueue(queueConfig =>
    {
        queueConfig.QueueName = "YourQueueName";
        queueConfig.IsDurable = true;
    });
});
```

**Step 2**

There is an `IStartup` Interface provided by Message Broker, that will use Init `Exchange`, Please call it in main method, this interface is Registed in `IServiceCollection` DI container.
```csharp
internal class MessageBrokerInitService : IMessageBrokerInitService
{
    IStartup _startup;
    public MessageBrokerInitService(IStartup startup)
    {
        _startup = startup;
    }

    public void InitMessageBroker()
    {
        _startup.OnStart();
    }
}
```

**Register Receiver**

> **Note**: Ensure that `RegisterReceiverServices` is set to true. 
Provide `ExchangeName` and `RoutingKeys` only if the sender application is sending messages to the exchange; otherwise, omit them.

* When configuring the queue using the `UseQueue` extension method, ensure that the exchange name matches the one specified in the `Sender` application.
* Additionally, provide the same **routing key** used for sending messages to the exchange in the `Sender` application.
```csharp
services.AddMessageBroker(messageBroker =>
{
    messageBroker.RegisterReceiverServices = true; // Set to True to register Receiver services.
    messageBroker.ClientProvidedName = "Receiver"; // Receiver or Any name you like
    messageBroker.AddRabbitMqServices((rabbitMqConfig) =>
    {
        rabbitMqConfig.HostName = "RabbitMqHost";
        rabbitMqConfig.Port = 5672; // your RabbitMq Port
        rabbitMqConfig.UserName = "username";
        rabbitMqConfig.Password = "password";
    }).UseQueue(config =>
    {
        config.QueueName = "YourQueueName";
        config.IsDurable = true;
        // Use below Propery only and only if you are using `UseExchange` to send the message to RabbitMq Exchnage. 
        // so the Queue will bind to exchange via routing key
        config.ExchangeName = "YourSenderExchangeName"; // 
        config.RoutingKeys = ["routingKey1"];

    });
});
```

> Note: Idealy One Application is Sender and other application is receiver. but you can configure both sender/receiver in one application too. 


## How to Use

### In Sender Application

The `IMessageSender` interface is provided for the sender, offering the following methods:

* SendToQueue(string message)
* SendToQueue(string queueName, string message)
* SendToExchange(string message, string? routingKey);

You can specify the `queueName` when sending messages to RabbitMQ.

The `IMessageSender` interface is registered during the configuration phase in the `IServiceCollection` dependency injection container and can be injected into your class constructor, as demonstrated in the code below:

```csharp
class SendMessageToRabbitMq : ISendMessageToRabbitMq
{
    private IMessageSender SendMessage { get; }
    public SendMessageToRabbiMq(IMessageSender sendMessage)
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
    public bool SendMessageToRabbitMqExchange(string queueName, string DataToSend)
    {
        return SendMessage.SendToExchange(DataToSend,"routingKey1");
    }

}
```
Register `SendMessageToRabbiMq` in `IServiceCollection`

```csharp
var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((hostContext, services) =>
{
    services.AddMessageBroker(messageBroker =>
    {
        messageBroker.RegisterSenderServices = true; // Set True to register Sender services
        messageBroker.ClientProvidedName = "SenderTest"; // Sender or Any name you like
        messageBroker.AddRabbitMqServices((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = "RabbitMqHost";
            rabbitMqConfig.Port = 5672; // your RabbitMq Port
            rabbitMqConfig.UserName = "username";
            rabbitMqConfig.Password = "password";
        }).UseExchange(exchange =>
        {
            exchange.ExchangeName = "ExchangeName";
            exchange.IsDurable = true; // this is required for durable exchange
        });
    });

    // register Sender Service in Services
    services.AddSingleton<ISendMessageToRabbitMq, SendMessageToRabbiMq>();

});


var host = builder.UseConsoleLifetime().Build();

using (var scope = host.Services.CreateScope())
{
    var startUp = scope.ServiceProvider.GetRequiredService<IStartup>();
    startUp.OnStart();
}

host.RunAsync().Wait();
```


### In Receiver Application

The `IMessageReceiver` interface includes a delegate:
`Predicate<string>? MessageHandler`.

This allows you to provide a callback method that will be invoked when a message is received from the RabbitMQ queue.

> Note: Ensure that `RegisterReceiverServices` is set to true. This is necessary for the `IMessageReceiver` interface to be provided. 
Provide `ExchangeName` and `RoutingKeys` only if the sender application is sending messages to the exchange; otherwise, omit them.

* When configuring the queue using the `UseQueue` extension method, ensure that the exchange name matches the one specified in the `Sender` application.
* Additionally, provide the same **routing key** used for sending messages to the exchange in the `Sender` application.

As demonstrated in the code below:

```csharp
var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((hostContext, services) =>
{
    services.AddMessageBroker(messageBroker =>
    {
        messageBroker.RegisterReceiverServices = true; // Set to True to register Receiver services.
        messageBroker.ClientProvidedName = "Receiver"; // Receiver or Any name you like
        messageBroker.AddRabbitMqServices((rabbitMqConfig) =>
        {
            rabbitMqConfig.HostName = "RabbitMqHost";
            rabbitMqConfig.Port = 5672; // your RabbitMq Port
            rabbitMqConfig.UserName = "username";
            rabbitMqConfig.Password = "password";
        }).UseQueue(config =>
        {
            config.QueueName = "YourQueueName";
            config.IsDurable = true;
            // Use below Propery only and only if you are using `UseExchange` to send the message to RabbitMq Exchnage. 
            // so the Queue will bind to exchange via routing key
            config.ExchangeName = "YourSenderExchangeName"; // 
            config.RoutingKeys = ["routingKey1"];

        });
    });
});

var host = builder.UseConsoleLifetime().Build();

using (var scope = host.Services.CreateScope())
{
    var messageReceiver = scope.ServiceProvider.GetRequiredService<IMessageReceiver>();
    var startup = scope.ServiceProvider.GetRequiredService<IStartup>();
    // make sure to call OnStart(), it Is important to create Queue.
    startup.OnStart();
    messageReceiver.MessageHandler = (message) =>
    {
        // After Process Message and return boolean value.
        Console.WriteLine(message);

        // message will only acknowledge to RabbitMq Service if return true.
        return true; 
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
docker run -d -v rabbitmqv:/var/log/rabbitmq --hostname rmq --name RabbitMqServer \
-p 5672:5672 -p 8080:15672 rabbitmq:3.13-management
```
### Port Detail

Port `8080` is for management portal and access by below mention __Login credentials__.

Click on the link for <a href='http://localhost:8080/'>Admin Portal</a>

Port `5672` is use in communication during producing and consuming of message.

### Login crediential

> Default login crediential if we not specifiy during creation of docker container

<small style='color:green'>_Username_</small> `guest` and <small style='color:green'>_Password_</small> `guest`
 

