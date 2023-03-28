namespace Lab3.Application.Services.StudentService;

public interface IMessageProducer
{
    void SendMessage<T> (T message);
}