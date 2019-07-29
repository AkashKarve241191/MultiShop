    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using AutoMapper;
    using InventoryService.Commands;
    using InventoryService.Domain;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    namespace InventoryService.Events.Handlers {
        public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent> {
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;
            private readonly ILogger<ProductUpdatedEventHandler> _logger;

            public ProductUpdatedEventHandler (IMediator mediator, IMapper mapper, ILogger<ProductUpdatedEventHandler> logger) {
                _mediator = mediator ??
                    throw new ArgumentNullException (nameof (mediator));
                _mapper = mapper ??
                    throw new ArgumentNullException (nameof (mapper));
                _logger = logger ??
                    throw new ArgumentNullException (nameof (logger));
            }

            public async Task Handle (ProductUpdatedEvent notification, CancellationToken cancellationToken) {
                _logger.LogInformation ($"Received notification for :{JsonConvert.SerializeObject (notification)}");
                await _mediator.Send (_mapper.Map<UpdateProductToInventoryCommand> (notification), cancellationToken);
            }
        }
    }