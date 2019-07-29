using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InventoryService.Configuration;
using InventoryService.Events;
using InventoryService.Services.Abstract;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InventoryService.Services {
    public class ProductAddedEventService : IEventService {

        private ISubscriptionClient _subscriptionClient;
        private readonly IOptions<ServiceBus> _serviceBusOptions;
        private readonly IMediator _mediator;
        private readonly ILogger<ProductAddedEventService> _logger;
        
        /// <summary>
        ///  Constructor for DI
        /// </summary>
        /// <param name="mediator">IMediator Dependency</param>
        /// <param name="serviceBusOptions">ServiceBus Configuration Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public ProductAddedEventService (IMediator mediator, IOptions<ServiceBus> serviceBusOptions, ILogger<ProductAddedEventService> logger) {
            _mediator = mediator ??
                throw new ArgumentNullException (nameof (mediator));
            _serviceBusOptions = serviceBusOptions ??
                throw new ArgumentNullException (nameof (serviceBusOptions));
            _logger = logger??
            throw new ArgumentNullException (nameof (logger));
        }

        /// <summary>
        ///  Subscribe to a Topic 
        /// </summary>
        /// <param name="token">Cancellation Token</param>
        /// <returns></returns>
        public async Task Subscribe () {
            ManagementClient client = null;
            try {
                client = new ManagementClient (_serviceBusOptions.Value.ConnectionString);

                //Create Subscription if it doesn't exists
                if (!await client.SubscriptionExistsAsync (_serviceBusOptions.Value.ProductAddedTopic, _serviceBusOptions.Value.ProductAddedSubscription).ConfigureAwait(false)) {
                    await client.CreateSubscriptionAsync (_serviceBusOptions.Value.ProductAddedTopic, _serviceBusOptions.Value.ProductAddedSubscription).ConfigureAwait(false);
                }

                // Log information
                _logger.LogInformation ($"Subscribed to Topic : {_serviceBusOptions.Value.ProductAddedTopic} , Subscription Name : {_serviceBusOptions.Value.ProductAddedSubscription}");

                //Create subscription client 
                _subscriptionClient = new SubscriptionClient (_serviceBusOptions.Value.ConnectionString, _serviceBusOptions.Value.ProductAddedTopic, _serviceBusOptions.Value.ProductAddedSubscription);

                //Process Messages
                RegisterOnMessageHandlerAndReceiveMessage ();

            } catch (Exception ex) {
                _logger.LogError ($"Error in receiving message from topic {_serviceBusOptions.Value.ProductAddedTopic}, , Subscription Name : { _serviceBusOptions.Value.ProductAddedSubscription} , ex - {ex}");
                throw;
            } finally {
                await client.CloseAsync ().ConfigureAwait(false);
            }
        }

        /// <summary>
        ///  Delete/UnSubscribe from a Topic
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task UnSubscribe (CancellationToken token = default (CancellationToken)) {
            ManagementClient client = null;
            try {

                client = new ManagementClient (_serviceBusOptions.Value.ConnectionString);

                //Delete Subscription from Topic
                if (await client.SubscriptionExistsAsync (_serviceBusOptions.Value.ProductAddedTopic, _serviceBusOptions.Value.ProductAddedSubscription, token).ConfigureAwait(false)) {
                    await client.DeleteSubscriptionAsync (_serviceBusOptions.Value.ProductAddedTopic, _serviceBusOptions.Value.ProductAddedSubscription, token).ConfigureAwait(false);
                }

                //Log information
                _logger.LogInformation ($"Stopped Subscription to Topic : {_serviceBusOptions.Value.ProductAddedTopic} , Subscription Name : { _serviceBusOptions.Value.ProductAddedSubscription}");

            } catch (Exception ex) {
                _logger.LogError ($"Error in UnSubscribing from topic {_serviceBusOptions.Value.ProductAddedTopic}, ex - {ex}");
                throw;
            } finally {
                await client.CloseAsync ().ConfigureAwait(false);
            }
        }

        private void RegisterOnMessageHandlerAndReceiveMessage () {

            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions (ExceptionReceivedHandler) {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Registers a message handler and begins a new thread to receive messages
            _subscriptionClient.RegisterMessageHandler (ProcessMessagesAsync, messageHandlerOptions);

        }

        /// <summary>
        ///  Receive messages continuously from a Topic 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task ProcessMessagesAsync (Message message, CancellationToken token) {

            // Log the incoming msg
            _logger.LogInformation ($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            //Deserialize incoming msg
            ProductAddedEvent @event = JsonConvert.DeserializeObject<ProductAddedEvent> (Encoding.UTF8.GetString (message.Body));

            //Publish message to call the event handler
            await _mediator.Publish (@event, token).ConfigureAwait(false);

            // Complete the message so that it is not received again.
            await _subscriptionClient.CompleteAsync (message.SystemProperties.LockToken).ConfigureAwait(false);
        }

        /// <summary>
        ///  Capture Exception on receiving a message from Topic
        /// </summary>
        /// <param name="exceptionReceivedEventArgs">Provides data for the Microsoft.Azure.ServiceBus.MessageHandlerOptions.ExceptionReceivedHandler</param>
        /// <returns></returns>
        private Task ExceptionReceivedHandler (ExceptionReceivedEventArgs exceptionReceivedEventArgs) {
            _logger.LogError ($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            _logger.LogInformation ("Exception context for troubleshooting:");
            _logger.LogInformation ($"- Endpoint: {context.Endpoint}");
            _logger.LogInformation ($"- Entity Path: {context.EntityPath}");
            _logger.LogInformation ($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

    }
}