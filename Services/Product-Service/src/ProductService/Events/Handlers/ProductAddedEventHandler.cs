namespace ProductService.Events.Handlers {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ProductService.Configuration;
    using ProductService.Events;
    using Newtonsoft.Json;
    using ProductService.Formatters;
    using ProductService.Services.Abstract;

    /// <summary>
    ///  The class consists of events which are raised when a new Product is added.
    /// </summary>
    /// <typeparam name="ProductAddedEvent">Message to be published</typeparam>
    public class ProductAddedEventHandler : INotificationHandler<ProductAddedEvent> {
        private readonly IBusService _busService;
        private readonly IOptions<ServiceBus> _serviceBusOptions;
        private readonly ILogger<ProductAddedEventHandler> _logger;

        /// <summary>
        ///  Constructor for DI
        /// </summary>
        /// <param name="serviceBusOptions"> ServiceBus configuration Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public ProductAddedEventHandler (IBusService busService, IOptions<ServiceBus> serviceBusOptions, ILogger<ProductAddedEventHandler> logger) {
            _busService = busService ??  throw new ArgumentNullException (nameof (busService)); 
            _serviceBusOptions = serviceBusOptions ??
                throw new ArgumentNullException (nameof (serviceBusOptions));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
        }

        /// <summary>
        ///  Handles Product added event by publishing a message to a Topic
        /// </summary>
        /// <param name="notification">Msg to be Published</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>void</returns>
        public async Task Handle (ProductAddedEvent notification, CancellationToken cancellationToken = default (CancellationToken)) {
            try {
                
                  //Log Serialization
                _logger.LogInformation($"{JsonConvert.SerializeObject(notification,Formatting.Indented, new JsonDotnetFormatter(typeof(ProductUpdatedEvent)))}");

                //Publish to Service bus Topic
                await _busService.PublishEvent(notification,_serviceBusOptions.Value.ConnectionString,_serviceBusOptions.Value.ProductAddedTopic);

                //Log event published information
                _logger.LogInformation ($"Published ProductAdded event, productId: {notification.ProductId} and msg id:{notification.Id}");

            } catch (Exception ex) {

                //Log - Error while publishing Product added msg to Service Bus Topic
                _logger.LogError ($"Error in publishing Product Added msg to Topic : {_serviceBusOptions.Value.ProductAddedTopic}, productId: {notification.ProductId} and msg id:{notification.Id} - {ex}");
                throw;
            }

        }
    }
}