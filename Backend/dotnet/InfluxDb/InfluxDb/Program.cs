// See https://aka.ms/new-console-template for more information
using InfluxDb;

Console.WriteLine("Hello, World!");



InfluxMessageProcesser influxMessageProcesser = new InfluxMessageProcesser();

Message message = new Message()
{
    Tag = "copper",
    Value = 45D
};

try
{
    influxMessageProcesser.WriteMessage(message);

    await influxMessageProcesser.ReadMessage();
}
catch(Exception ex)
{

}