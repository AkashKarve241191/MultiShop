namespace ProductService.Services
{
    using System.Threading.Tasks;
    using ProductService.Events.Contracts;
    using ProductService.Services.Abstract;
    public class KafkaService : IBusService
    {
        public Task PublishEvent<T>(T @event, string connectionString, string topicName) where T : IIntegrationEvent
        {
            throw new System.NotImplementedException();
        }

        public Task SendMessage<T>(T msg)
        {
            throw new System.NotImplementedException();
        }
    }
}