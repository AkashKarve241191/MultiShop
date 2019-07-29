namespace ProductService.Queries
{
    using System.Collections.Generic;
    using MediatR;
    using ProductService.Domain;

    /// <summary>
    /// FindAllProductsQuery query object.
    /// </summary>
    /// <typeparam name="Product">Product in store</typeparam>
    public class FindAllProductsQuery :IRequest<IEnumerable<Product>>
    {
       
    }
}