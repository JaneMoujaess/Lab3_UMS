namespace Lab3.Application.Services.StudentService;

public interface IPublisher
{
    void PublishNotification<T> (T message);
}