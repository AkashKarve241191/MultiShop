namespace ProductService.Events.Handlers {
    using System.Dynamic;
    using System.Text;
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
    ///  The class consists of events which are raised when a Product is updated.
    /// </summary>
    /// <typeparam name="ProductUpdatedEventEvent">Message to be published</typeparam>
    public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent> {

        private readonly IBusService _busService;
        private readonly IOptions<ServiceBus> _serviceBusOptions;
        private readonly ILogger<ProductAddedEventHandler> _logger;

        /// <summary>
        ///  Constructor for DI
        /// </summary>
        /// <param name="serviceBusOptions"> ServiceBus configuration Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public ProductUpdatedEventHandler (IBusService busService, IOptions<ServiceBus> serviceBusOptions, ILogger<ProductAddedEventHandler> logger) {
            _busService = busService;
            _serviceBusOptions = serviceBusOptions ??
                throw new ArgumentNullException (nameof (serviceBusOptions));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
        }

        /// <summary>
        ///  Handles Product updated event by publishing a message to a Topic
        /// </summary>
        /// <param name="notification">Msg to be Published</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>void</returns>
        public async Task Handle (ProductUpdatedEvent notification, CancellationToken cancellationToken = default (CancellationToken)) {
            try {

                //Publish to Service bus Topic
                await _busService.PublishEvent (notification, _serviceBusOptions.Value.ConnectionString, _serviceBusOptions.Value.ProductUpdatedTopic);

                //Log event published information
                _logger.LogInformation ($"Published ProductUpdated event, productId: {notification.ProductId} and msg id:{notification.Id}");

            } catch (Exception ex) {

                //Log - Error while publishing Product updated msg to Service Bus Topic
                _logger.LogError ($"Error in publishing Product Updated msg to Topic : {_serviceBusOptions.Value.ProductUpdatedTopic}, productId: {notification.ProductId} and msg id:{notification.Id} - {ex}");
                throw;
            }
        }
    }
}