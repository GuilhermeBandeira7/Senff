// See https://aka.ms/new-console-template for more information

using RabbitMqLib.Services;

Console.WriteLine("Hello from the publisher!");

var publisher = new RabbitPublisher("Products Publisher", "guest", "guest");

publisher.CreateExchange("senffapi", "direct");
publisher.CreateQueue("providers", dlx: true);
publisher.CreateQueue("publishers", dlx: true);
publisher.BindQueueToExchange("senffapi", "providers", "prov");
publisher.BindQueueToExchange("senffapi", "publishers", "prod");

 for(int i = 0; i < 10; i++)
    await publisher.SendingMessage($"Hello from the publisher {i}", "senffapi", "prod");

for(int i = 0; i < 19; i++) 
    await publisher.SendingMessage($"Hello from the publisher {i}", "senffapi", "prov");



