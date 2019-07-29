namespace ShoppingCartService.Commands.Handler {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using AutoMapper;
    using MediatR;
    using ShoppingCartService.Domain;

    public class UpdateShoppingCartHandler : IRequestHandler<UpdateShoppingCartCommand, Unit> {
        private readonly IShoppingCartRepository _repository;
        private readonly IMapper _mapper;
        public UpdateShoppingCartHandler (IShoppingCartRepository repository, IMapper mapper) {
            _repository = repository ??
                throw new ArgumentNullException (nameof (repository));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
        }

        public async Task<Unit> Handle (UpdateShoppingCartCommand request, CancellationToken cancellationToken) {
            await _repository.Update (_mapper.Map<ShoppingCart> (request));
            return new Unit();
        }
    }
}