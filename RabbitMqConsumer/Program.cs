// See https://aka.ms/new-console-template for more information

using RabbitMqLib.Services;

Console.WriteLine("Hello From the Consumer!");

var consumer = new RabbitConsumer("listener", "guest", "guest");
