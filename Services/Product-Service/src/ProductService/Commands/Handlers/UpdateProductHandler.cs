namespace ProductService.Commands.Handlers {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using ProductService.Domain;
    using ProductService.Events;

    /// <summary>
    /// Use the class to invoke the UpdateProduct Command and raise corresponding events.
    /// </summary>
    /// <typeparam name="UpdateProductCommand">Command to be executed</typeparam>
    /// <typeparam name="Unit">void</typeparam>
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Unit> {
        private readonly IProductRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateProductHandler> _logger;

        /// <summary>
        /// Constructor for DI
        /// </summary>
        /// <param name="repository">IProductRepository Dependency</param>
        /// <param name="mediator">IMediator Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public UpdateProductHandler (IProductRepository repository, IMediator mediator, ILogger<UpdateProductHandler> logger) {
            _repository = repository ??
                throw new ArgumentNullException (nameof (repository));
            _mediator = mediator ??
                throw new ArgumentNullException (nameof (mediator));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
        }

        /// <summary>
        /// Handles the UpdateProduct Command and raises corresponding events.
        /// </summary>
        /// <param name="request">Command Object</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task<Unit> Handle (UpdateProductCommand request, CancellationToken cancellationToken = default (CancellationToken)) {
            //Create Product from command
            Product product = new Product {
            ProductId = request.ProductId,
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive,
            UnitPrice = request.UnitPrice,
            UnitsInStock = request.UnitsInStock
            };

            //Log information
            _logger.LogInformation ($"Update product with name:{product.Name} and {product.Description} to store.");

            //Updated Product
            await _repository.Update (product);

            //Log information
            _logger.LogInformation ($"Updated product to store , productId : {product.ProductId}");

            //Create ProductUpdatedEvent from request
            ProductUpdatedEvent @event = new ProductUpdatedEvent (product.ProductId, product.Name, product.Description, product.UnitPrice, product.UnitsInStock, product.IsActive);

            //Publish ProductUpdated Event
            await _mediator.Publish (@event, cancellationToken);

            //Return void
            return new Unit ();
        }
    }
}