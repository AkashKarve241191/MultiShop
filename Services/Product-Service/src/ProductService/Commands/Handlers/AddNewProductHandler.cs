namespace ProductService.Commands.Handlers {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using ProductService.Domain;
    using ProductService.Events;

    /// <summary>
    /// Use the class to invoke the AddNewProduct Command and raise corresponding events.
    /// </summary>
    /// <typeparam name="AddNewProductCommand">Command to executed</typeparam>
    /// <typeparam name="AddNewProductResult">Result from command execution</typeparam>
    public class AddNewProductHandler : IRequestHandler<AddNewProductCommand, AddNewProductResult> {
        private readonly IProductRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<AddNewProductHandler> _logger;

        /// <summary>
        /// Constructor for DI
        /// </summary>
        /// <param name="repository">IProductRepository Dependency</param>
        /// <param name="mediator">IMediator Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public AddNewProductHandler (IProductRepository repository, IMediator mediator, ILogger<AddNewProductHandler> logger) {
            _repository = repository ??
                throw new ArgumentNullException (nameof (repository));
            _mediator = mediator ??
                throw new ArgumentNullException (nameof (mediator));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
        }

        /// <summary>
        /// Handles the AddNewProduct Command and raises corresponding events.
        /// </summary>
        /// <param name="request">Command object</param>
        /// <param name="cancellationToken"> Propagates notification that operations should be canceled.</param>
        /// <returns>AddNewProductResult a result of the command execution</returns>
        public async Task<AddNewProductResult> Handle (AddNewProductCommand request, CancellationToken cancellationToken = default (CancellationToken)) {
            //Create Product from AddNewProductCommand request 
            Product product = new Product {
            Name = request.Name,
            Description = request.Description,
            IsActive = true,
            UnitPrice = request.UnitPrice,
            UnitsInStock = request.UnitsInStock
            };

            //Log information
            _logger.LogInformation ($"Add new product with name:{product.Name} and description : {product.Description} to store.");

            //Add Product to store.
            int productId = await _repository.Add (product);

            //Log information
            _logger.LogInformation ($"Added product to store , new productId : {productId}");

            //Create ProductAddedEvent from request
            ProductAddedEvent @event = new ProductAddedEvent (product.ProductId, product.Name, product.Description, product.UnitPrice, product.UnitsInStock, product.IsActive);

            //Publish ProductAdded Event
            await _mediator.Publish (@event, cancellationToken);

            //Return result
            return new AddNewProductResult {
                ProductId = productId
            };
        }
    }
}