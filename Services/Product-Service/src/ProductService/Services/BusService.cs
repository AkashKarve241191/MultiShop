namespace ProductService.Services {
    using System.Text;
    using System.Threading.Tasks;
    using System;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using ProductService.Configuration;
    using ProductService.Events.Contracts;
    using ProductService.Policies;
    using ProductService.Services.Abstract;

    public class BusService : IBusService {
        private readonly ILogger<BusService> _logger;
        private readonly IOptions<ServiceBus> _serviceBusConfiguration;
        public BusService (ILogger<BusService> logger, IOptions<ServiceBus> serviceBusConfiguration) {
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
            _serviceBusConfiguration = serviceBusConfiguration ??
                throw new ArgumentNullException (nameof (serviceBusConfiguration));
        } 

        public async Task PublishEvent<T> (T @event, string connectionString, string topicName) where T : IIntegrationEvent {
            ITopicClient topicClient = null;
            try {
                //Create Topic Client
                topicClient = new TopicClient (connectionString, topicName);

                //Create msg from @event
                var msg = new Message (Encoding.UTF8.GetBytes (JsonConvert.SerializeObject (@event)));

                //Publish msg to a Topic
                await topicClient.SendAsync (msg);
            } catch (Exception ex) {
                //Log - Error while publishing msg to Topic
                _logger.LogError ($"Error in publishing event# {nameof(@event)} with id# {@event.Id} to Topic {topicName}, exception - {ex}");
                throw;
            } finally {
                //Close Topic Client
                await topicClient.CloseAsync ();
            }
        }

        public async Task SendMessage<T> (T @msg) {
            IQueueClient queueClient = null;
            try {
                //Create Topic Client
                queueClient = new QueueClient (_serviceBusConfiguration.Value.ConnectionString,_serviceBusConfiguration.Value.RequestReplyQueue);
                //Create msg
                var message = new Message (Encoding.UTF8.GetBytes (JsonConvert.SerializeObject (@msg)));

                //message.SessionId = "NewSession";

                //Publish msg to Topic
                await queueClient.SendAsync (message);

            } catch (Exception ex) {
                _logger.LogError ($"Exception - {ex}");
            } finally {
                await queueClient.CloseAsync ();
            }

        }
    }
}