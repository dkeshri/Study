// See https://aka.ms/new-console-template for more information
using InfluxDb;
Console.WriteLine("Press 1 to write data");
Console.WriteLine("Press 2 to Read data");
Console.WriteLine("Press 3 to Exit");
Console.Write("Enter you number : ");
int ans = Convert.ToInt32(Console.ReadLine());
InfluxMessageProcesser influxMessageProcesser = new InfluxMessageProcesser();
while (ans != 3)
{
    switch (ans)
    {
        case 1:
            Console.Clear();
            for (int i = 1; i <= 10; i++)
            {
                
                Random rnd = new Random();
                int num = rnd.Next(1, 100);
                double value = num / i;
                Message message = new Message()
                {
                    Tag = i%2==0?"India":"USA",
                    Value = value
                };
                Console.WriteLine("Writing Tag: " + message.Tag +" Value: "+message.Value);
                Task.Delay(500).Wait();
                
                influxMessageProcesser.WriteMessage(message);
            }
            
            Console.WriteLine("Data written succesfully");
            
            break;
        case 2:
            Console.Clear();
            await influxMessageProcesser.ReadMessage();
            Console.WriteLine("Data Read succesfully");
            break;
        case 3:Environment.Exit(0);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Please enter correct number");
            break;

    }
    Console.WriteLine("Press 1 to write data");
    Console.WriteLine("Press 2 to Read data");
    Console.WriteLine("Press 3 to Exit");
    Console.Write("Enter you number : ");
    ans = Convert.ToInt32(Console.ReadLine());
}





