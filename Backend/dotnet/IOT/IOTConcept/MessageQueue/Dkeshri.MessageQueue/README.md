# About

This is an MessageBroker interface that allow to implement various MessageBroker service like RabbitMq, AzureSericeBus etc.

## Interfaces

`ISendMessage`, `IMessageReceiver`, `MessageBrokerFactory`

## How it work

`AddMessageBroker` serves as the starting point. Clients use this method with `IServiceCollection` and provide the MessageBroker configuration.

It then utilizes the MessageBrokerFactory to create the sender (`ISendMessage`) and receiver (`IMessageReceiver`) objects. 
These objects are registered in the `IServiceCollection` dependency injection (DI) container, 
 allowing clients to use these interfaces to send and receive messages.


## How to Use

The extension method `AddMessageBroker` for `IServiceCollection` helps register services in the **IServiceCollection** dependency injection (DI) container.

**MessageBrokerFactory** 

Is an abstract class contains `CreateSender` and `CreateReceiver`, Implemented library need to provide The CreaterFactory and Provide that Factory in `IServiceCollection`
```chasrp
public abstract class MessageBrokerFactory
    {
        public abstract ISendMessage CreateSender();
        public abstract IMessageReceiver CreateReceiver();
    }
```


### In your library

Create a ExtentionMethod to `MessageBroker`, this will give is some basic messageBroker configuration and `IServiceCollection` Property
help you to register your services.


```csharp
public static class ServiceCollectionExtensions
{      
    public static void AddRabbitMqServices(this MessageBroker messageBroker, YourConfigClass config)
    {
        messageBroker.Services.AddSingleton<MessageBrokerFactory, RabbitMqMessageBroker>();

        // add rest of your services in IServiceCollection


    }

}
```


Provide Concreate Implementation of `MessageBrokerFactory`

**Example** :
```csharp
class RabbitMqMessageBroker : MessageBrokerFactory
{
    private readonly IRabbitMqConnection _connection;
    private IMessageHandler? _messageHandler;
    public RabbitMqMessageBroker(IRabbitMqConnection rabbitMqConnection,IMessageHandler? messageHandler = null)
    {
        _connection = rabbitMqConnection;
        _messageHandler = messageHandler;
    }

    public override IMessageReceiver CreateReceiver()
    {
        return _messageHandler as IMessageReceiver
            ?? throw new InvalidCastException("The injected IMessageHandler does not implement IMessageReceiver.");
    }

    public override ISendMessage CreateSender()
    {
        return new SendMessage(_connection);
    }
}
```


