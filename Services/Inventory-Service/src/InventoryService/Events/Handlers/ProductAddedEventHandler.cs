namespace InventoryService.Events.Handlers {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using AutoMapper;
    using InventoryService.Commands;
    using InventoryService.Domain;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ProductAddedEventHandler : INotificationHandler<ProductAddedEvent> {

        private readonly IMediator _mediator;
        private readonly ILogger<ProductAddedEventHandler> _logger;
        private readonly IMapper _mapper;

        public ProductAddedEventHandler (IMediator mediator, ILogger<ProductAddedEventHandler> logger, IMapper mapper) {
            _mediator = mediator ??
                throw new ArgumentNullException (nameof (mediator));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
        }

        public async Task Handle (ProductAddedEvent notification, CancellationToken cancellationToken) {
            AddNewProductToInventoryCommand inventoryItem = _mapper.Map<AddNewProductToInventoryCommand> (notification);
            await _mediator.Send (inventoryItem, cancellationToken);
        }
    }
}