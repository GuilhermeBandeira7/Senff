namespace RabbitMqLib.Services;

public interface IRabbitConsumer
{
    void QueueListener(string queue);
}