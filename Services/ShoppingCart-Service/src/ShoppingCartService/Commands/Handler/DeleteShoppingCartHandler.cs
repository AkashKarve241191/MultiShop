namespace ShoppingCartService.Commands.Handler {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using AutoMapper;
    using MediatR;
    using ShoppingCartService.Domain;

    public class DeleteShoppingCartHandler : IRequestHandler<DeleteShoppingCartCommand, Unit> {
        private readonly IShoppingCartRepository _repository;
        private readonly IMapper _mapper;
        public DeleteShoppingCartHandler (IShoppingCartRepository repository, IMapper mapper) {
            _repository = repository ??
                throw new ArgumentNullException (nameof (repository));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
        }

        public async Task<Unit> Handle (DeleteShoppingCartCommand request, CancellationToken cancellationToken) {
            await _repository.Remove (request.UserId);
            return new Unit();
        }
    }
}