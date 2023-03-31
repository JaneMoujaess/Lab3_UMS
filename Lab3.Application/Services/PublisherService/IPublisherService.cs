namespace Lab3.Application.Services.PublisherService;

public interface IPublisherService
{
    void Publish<T> (string queueName,T objectToPublish);
    //void PublishMessage<T>(string queueName,T messageToPublish);
}