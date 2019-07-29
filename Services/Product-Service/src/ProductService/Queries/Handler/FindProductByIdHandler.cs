namespace ProductService.Queries.Handler {
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using AutoMapper;
    using MediatR;
    using ProductService.Domain;

    /// <summary>
    /// Consists of methods to handle Product By Id Queries
    /// </summary>
    /// <typeparam name="FindProductByIdQuery">The input  to execute query</typeparam>
    /// <typeparam name="Product">The result after executing the query</typeparam>
    public class FindProductByIdHandler : IRequestHandler<FindProductByIdQuery, Product> {
        private readonly IProductRepository _repository;

        /// <summary>
        /// Constructor for Dependency Injection
        /// </summary>
        /// <param name="repository">IProductRepository Dependency</param>
        public FindProductByIdHandler (IProductRepository repository) {
            _repository = repository ??
                throw new ArgumentNullException (nameof (repository));
        }

        /// <summary>
        ///  Handles FindProductByIdQuery request
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Product in store</returns>
        public async Task<Product> Handle (FindProductByIdQuery request, CancellationToken cancellationToken = default (CancellationToken)) {
            //Call the Repository method to execute query.
            return await _repository.FindById (request.ProductId);
        }
    }
}