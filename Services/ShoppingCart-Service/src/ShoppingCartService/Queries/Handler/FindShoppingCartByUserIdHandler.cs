namespace ShoppingCartService.Queries.Handler
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using ShoppingCartService.Domain;

    public class FindShoppingCartByUserIdHandler : IRequestHandler<FindShoppingCartByUserIdQuery,ShoppingCart>
    {
        private readonly IShoppingCartRepository _repository;
        public FindShoppingCartByUserIdHandler(IShoppingCartRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ShoppingCart> Handle(FindShoppingCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.FindByUserId(request.UserId);
        }
    }
}