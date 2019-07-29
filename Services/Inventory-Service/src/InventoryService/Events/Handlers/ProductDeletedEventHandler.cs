    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using AutoMapper;
    using InventoryService.Commands;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    namespace InventoryService.Events.Handlers {
        public class ProductDeletedEventHandler : INotificationHandler<ProductDeletedEvent> {
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;
            private readonly ILogger<ProductDeletedEventHandler> _logger;

            public ProductDeletedEventHandler (IMediator mediator, IMapper mapper, ILogger<ProductDeletedEventHandler> logger) {
                _mediator = mediator ??
                    throw new ArgumentNullException (nameof (mediator));
                _mapper = mapper ??
                    throw new ArgumentNullException (nameof (mapper));
                _logger = logger ??
                    throw new ArgumentNullException (nameof (logger));
            }

            public async Task Handle (ProductDeletedEvent notification, CancellationToken cancellationToken) {
                _logger.LogInformation ($"Received notification for :{JsonConvert.SerializeObject (notification)}");
                await _mediator.Send (_mapper.Map<DeleteProductFromInventoryCommand> (notification), cancellationToken);

            }
        }
    }