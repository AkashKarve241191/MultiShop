namespace ProductService.Events.Handlers {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using ProductService.Configuration;
    using ProductService.Events;
    using ProductService.Formatters;
    using ProductService.Services.Abstract;

    /// <summary>
    ///  The class consists of events which are raised when a Product is deleted from store.
    /// </summary>
    /// <typeparam name="ProductDeletedEvent">Message to be published</typeparam>
    public class ProductDeletedEventHandler : INotificationHandler<ProductDeletedEvent> {
        private readonly IBusService _busService;
        private readonly IOptions<ServiceBus> _serviceBusOptions;
        private readonly ILogger<ProductAddedEventHandler> _logger;

        /// <summary>
        ///  Constructor for DI
        /// </summary>
        /// <param name="serviceBusOptions"> ServiceBus configuration Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public ProductDeletedEventHandler (IBusService busService, IOptions<ServiceBus> serviceBusOptions, ILogger<ProductAddedEventHandler> logger) {
            _busService = busService;
            _serviceBusOptions = serviceBusOptions ??
                throw new ArgumentNullException (nameof (serviceBusOptions));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
        }

        /// <summary>
        ///  Handles Product deleted event by publishing a message to a Topic
        /// </summary>
        /// <param name="notification">Msg to be published</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>void</returns>
        public async Task Handle (ProductDeletedEvent notification, CancellationToken cancellationToken = default (CancellationToken)) {
            try {

                //Log Serialization
                _logger.LogInformation ($"{JsonConvert.SerializeObject(notification,Formatting.Indented, new JsonDotnetFormatter(typeof(ProductUpdatedEvent)))}");

                //Publish to Service bus Topic
                await _busService.PublishEvent (notification, _serviceBusOptions.Value.ConnectionString, _serviceBusOptions.Value.ProductDeletedTopic);

                //Log event published information
                _logger.LogInformation ($"Published ProductDeleted event, productId: {notification.ProductId} and id:{notification.Id}");

            } catch (Exception ex) {

                //Log - Error while publishing Product deleted msg to Service Bus Topic
                _logger.LogError ($"Error in publishing Product Deleted msg to Topic : {_serviceBusOptions.Value.ProductDeletedTopic}, productId: {notification.ProductId} and msg id:{notification.Id} - {ex}");
                throw;
            }

        }
    }
}