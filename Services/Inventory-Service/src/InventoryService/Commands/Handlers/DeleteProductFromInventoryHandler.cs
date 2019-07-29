namespace InventoryService.Commands.Handlers {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using AutoMapper;
    using InventoryService.Domain;
    using MediatR;
    using Microsoft.Extensions.Logging;
    public class DeleteProductFromInventoryHandler
        : IRequestHandler<DeleteProductFromInventoryCommand, Unit> {
            private readonly IInventoryStoreRepository _repository;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;
            private readonly ILogger<DeleteProductFromInventoryHandler> _logger;

            /// <summary>
            /// Constructor for DI
            /// </summary>
            /// <param name="repository">IProductRepository Dependency</param>
            /// <param name="mediator">IMediator Dependency</param>
            /// <param name="logger">ILogger Dependency</param>
            public DeleteProductFromInventoryHandler (IInventoryStoreRepository repository, IMediator mediator, IMapper mapper, ILogger<DeleteProductFromInventoryHandler> logger) {
                _repository = repository ??
                    throw new ArgumentNullException (nameof (repository));
                _mediator = mediator ??
                    throw new ArgumentNullException (nameof (mediator));
                _mapper = mapper ??
                    throw new ArgumentNullException (nameof (mapper));
                _logger = logger ??
                    throw new ArgumentNullException (nameof (logger));
            }

            public async Task<Unit> Handle (DeleteProductFromInventoryCommand request, CancellationToken cancellationToken) {
                await _repository.Delete (request.ProductId);
                return new Unit ();
            }
        }
}
