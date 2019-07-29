namespace ProductService.Queries.Handler {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using AutoMapper;
    using MediatR;
    using ProductService.Domain;

    /// <summary>
    /// Consists of methods to handle Product(s) Queries
    /// </summary>
    /// <typeparam name="Product">The result after executing the query</typeparam>
    public class FindAllProductsHandler : IRequestHandler<FindAllProductsQuery, IEnumerable<Product>> {
        private readonly IProductRepository _repository;

        /// <summary>
        /// Constructor for Dependency Injection
        /// </summary>
        /// <param name="repository">IProductRepository Dependency</param>
        public FindAllProductsHandler (IProductRepository repository) {
            _repository = repository ??
                throw new ArgumentNullException (nameof (repository));
        }

        /// <summary>
        ///  Handles FindAllProductsQuery request
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Product in store</returns>
        public async Task<IEnumerable<Product>> Handle (FindAllProductsQuery request, CancellationToken cancellationToken = default (CancellationToken)) {
            //Call the Repository method to execute query.
            return await _repository.GetAll ();
        }
    }
}