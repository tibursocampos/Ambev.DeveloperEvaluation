using Microsoft.Extensions.Options;

using RabbitMQ.Client;

using System.Text;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Common.RabbitMQ;

/// <summary>
/// Service for sending messages to a RabbitMQ queue.
/// </summary>
public class RabbitMQService : IRabbitMQService
{
    private readonly RabbitMQSettings _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMQService"/> class.
    /// </summary>
    /// <param name="options">The RabbitMQ settings options.</param>
    public RabbitMQService(IOptions<RabbitMQSettings> options)
    {
        _settings = options.Value;
    }

    /// <summary>
    /// Sends a message to the RabbitMQ queue.
    /// </summary>
    /// <typeparam name="T">The type of the message to send.</typeparam>
    /// <param name="message">The message to send.</param>
    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: _settings.QueueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish(exchange: "",
                             routingKey: _settings.QueueName,
                             basicProperties: null,
                             body: body);
    }
}