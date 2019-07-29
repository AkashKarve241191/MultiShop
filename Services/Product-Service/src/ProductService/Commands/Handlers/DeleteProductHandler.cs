namespace ProductService.Commands.Handlers {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using ProductService.Domain;
    using ProductService.Events;

    /// <summary>
    /// Use the class to invoke the DeleteProduct Command and raise corresponding events.
    /// </summary>
    /// <typeparam name="DeleteProductCommand">Command to be executed</typeparam>
    /// <typeparam name="Unit">Void</typeparam>
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit> {
        private readonly IProductRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<DeleteProductHandler> _logger;

        /// <summary>
        /// Constructor for DI
        /// </summary>
        /// <param name="repository">IProductRepository Dependency</param>
        /// <param name="mediator">IMediator Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public DeleteProductHandler (IProductRepository repository, IMediator mediator, ILogger<DeleteProductHandler> logger) {
            _repository = repository ??
                throw new ArgumentNullException (nameof (repository));
            _mediator = mediator ??
                throw new ArgumentNullException (nameof (mediator));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
        }

        /// <summary>
        /// Handles the DeleteProduct Command and raises corresponding events.
        /// </summary>
        /// <param name="request">Command object</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>void</returns>
        public async Task<Unit> Handle (DeleteProductCommand request, CancellationToken cancellationToken = default (CancellationToken)) {

            //Delete Product
            await _repository.Delete (request.ProductId);

            //Log information
            _logger.LogInformation ($"Deleted product with productId : {request.ProductId} from store.");

            //Create ProductDeletedEvent from request
            ProductDeletedEvent @event = new ProductDeletedEvent (request.ProductId);

            //Publish Event
            await _mediator.Publish (@event, cancellationToken);

            //Return void
            return new Unit ();
        }
    }
}