using System.Text;
using Lab3.Application.Services.StudentService;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Lab3.Application.Services.PublisherService;

public class PublisherService:IPublisherService
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public PublisherService()
    {
        _connectionFactory = new ConnectionFactory { HostName = "localhost" };
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    public void Publish<T>(string queueName,T notification)
    {
        _channel.QueueDeclare(queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonConvert.SerializeObject(notification);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
    }

    /*public void PublishMessage<T>(string queueName,T messageToPublish)
    {
        _channel.QueueDeclare(queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonConvert.SerializeObject(messageToPublish);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
    }*/
}