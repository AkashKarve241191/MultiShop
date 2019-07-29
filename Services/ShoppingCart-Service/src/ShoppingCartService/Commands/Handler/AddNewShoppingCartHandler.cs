namespace ShoppingCartService.Commands.Handler {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using AutoMapper;
    using MediatR;
    using ShoppingCartService.Domain;

    public class AddNewShoppingCartHandler : IRequestHandler<AddNewShoppingCartCommand, AddNewShoppingCartCommandResult> {
        private readonly IShoppingCartRepository _repository;
        private readonly IMapper _mapper;
        public AddNewShoppingCartHandler (IShoppingCartRepository repository, IMapper mapper) {
            _repository = repository ??
                throw new ArgumentNullException (nameof (repository));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
        }

        public async Task<AddNewShoppingCartCommandResult> Handle (AddNewShoppingCartCommand request, CancellationToken cancellationToken) {
            var shoppingCartId = await _repository.Add (_mapper.Map<ShoppingCart> (request));
            return new AddNewShoppingCartCommandResult { ShoppingCartId = shoppingCartId };
        }
    }
}