namespace Ambev.DeveloperEvaluation.Common.RabbitMQ;

/// <summary>
/// Interface for RabbitMQ service to send messages to a queue.
/// </summary>
public interface IRabbitMQService
{
    /// <summary>
    /// Sends a message to the RabbitMQ queue.
    /// </summary>
    /// <typeparam name="T">The type of the message to send.</typeparam>
    /// <param name="message">The message to send.</param>
    void SendMessage<T>(T message);
}