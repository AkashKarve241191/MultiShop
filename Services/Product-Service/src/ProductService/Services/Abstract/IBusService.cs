namespace ProductService.Services.Abstract {
    using System.Threading.Tasks;
    using ProductService.Events.Contracts;

    public interface IBusService {
        Task PublishEvent<T> (T @event, string connectionString, string topicName) where T : IIntegrationEvent;
        Task SendMessage<T> (T @msg);
    }
}