using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqLib.Services;

public class RabbitConsumer : IRabbitConsumer
{
    private readonly IModel _channel;

    public RabbitConsumer(string clientName, string userName, string password)
    {
        _channel = CreateConnection(clientName, userName, password);
    }

    private IModel CreateConnection(string clientName, string userName, string password)
    {
        ConnectionFactory factory = new();
        factory.Uri = new Uri($"amqp://{userName}:{password}@localhost:5672");
        factory.ClientProvidedName = clientName;

        IConnection cnn = factory.CreateConnection();

        IModel channel = cnn.CreateModel();

        return channel;
    }
    
    public void QueueListener(string queue)
    {
        
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _channel.BasicAck(eventArgs.DeliveryTag, false);
                Console.WriteLine($"Message Received: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _channel.BasicNack(eventArgs.DeliveryTag, false, false);
            }
        };

        _channel.BasicConsume(queue, false, consumer);

        Console.ReadKey();
    }
}