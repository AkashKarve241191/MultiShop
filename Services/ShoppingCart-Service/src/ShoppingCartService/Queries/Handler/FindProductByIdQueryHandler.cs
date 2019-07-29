namespace ShoppingCartService.Queries.Handler {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using MediatR;
    using ShoppingCartService.Domain;
    using ShoppingCartService.Services.Abstract;

    public class FindProductByIdQueryHandler : IRequestHandler<FindProductByIdQuery, Product> {
        private readonly IProductService _productService;

        public FindProductByIdQueryHandler (IProductService productService) {
            _productService = productService ??
                throw new ArgumentNullException (nameof (productService));
        }

        public async Task<Product> Handle (FindProductByIdQuery request, CancellationToken cancellationToken) {
            return await _productService.FindProductById (request.ProductId);
        }
    }
}