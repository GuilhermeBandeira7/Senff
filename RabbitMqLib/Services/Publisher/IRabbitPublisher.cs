using RabbitMQ.Client;

namespace RabbitMqLib.Services;

public interface IRabbitPublisher
{
    public IModel RabbitConnection(string clientName, string userName, string password);
    public void CreateExchange(string exchangeName,string type, bool durable = true, bool autoDelete = false);
    public void CreateQueue(string queueName, bool durable = true, bool exclusive = false, bool autoDelete = false, bool dlx = false);
    public void BindQueueToExchange(string exchangeName, string queueName, string routingKey);
    public Task SendingMessage<T>(T message,string exchangeName,string routingKey);
    public Dictionary<string, object> CreateDeadLetter();
}