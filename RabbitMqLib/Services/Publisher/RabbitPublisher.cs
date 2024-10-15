using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace RabbitMqLib.Services;

public class RabbitPublisher : IRabbitPublisher
{
    private readonly IModel _channel;

    public RabbitPublisher(string clientName, string userName, string password)
    {
        _channel = RabbitConnection(clientName, userName, password);
    }

    public IModel RabbitConnection(string clientName, string userName, string password)
    {
        ConnectionFactory factory = new()
        {
            Uri = new Uri($"amqp://{userName}:{password}@localhost:5672"),
            ClientProvidedName = clientName
        };

        var cnn = factory.CreateConnection();
        var channel = cnn.CreateModel();

        return channel;
    }

    public void CreateExchange(string exchangeName, string type, bool durable = true, bool autoDelete = false)
    {
        switch (type.ToLower())
        {
            case "fanout":
                _channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: true, autoDelete: false,
                    arguments: null);
                break;
            case "headers":
                _channel.ExchangeDeclare(exchangeName, ExchangeType.Headers, durable: true, autoDelete: false,
                    arguments: null);
                break;
            case "topic":
                _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, durable: true, autoDelete: false,
                    arguments: null);
                break;
            default:
                _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true, autoDelete: false,
                    arguments: null);
                break;
        }
    }

    public void CreateQueue(string queueName, bool durable = true,
        bool exclusive = false, bool autoDelete = false, bool dlx = false)
    {
        if (dlx)
        {
            var args = CreateDeadLetter();
            _channel.QueueDeclare(queueName, durable: durable, exclusive: exclusive, autoDelete: autoDelete,
                arguments: args);
        }
        else
            _channel.QueueDeclare(queueName, durable: durable, exclusive: exclusive, autoDelete: autoDelete,
                arguments: null);
    }

    public Dictionary<string, object> CreateDeadLetter()
    {
        _channel.ExchangeDeclare("DeadLetterExchange", ExchangeType.Fanout, true, false);
        _channel.QueueDeclare("DeadLetterQueue", true, false, false, null);
        _channel.QueueBind("DeadLetterQueue", "DeadLetterExchange", "");

        var arguments = new Dictionary<string, object>()
        {
            {"x-dead-letter-exchange", "DeadLetterExchange"}
        };

        return arguments;
    }

    public void BindQueueToExchange(string exchangeName, string queueName, string routingKey)
    {
        _channel.QueueBind(queueName, exchangeName, routingKey);
    }

    public async Task SendingMessage<T>(T message, string exchangeName, string routingKey)
    {
        try
        {
            await Task.Run(() =>
            {
                var jsonString = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonString);
                _channel.BasicPublish(exchangeName,
                    routingKey, null, body);
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}