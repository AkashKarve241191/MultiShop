using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductService.Commands;
using ProductService.Configuration;
using ProductService.Domain;
using ProductService.Queries;
using ProductService.Queries.Handler;
using ProductService.Services.Abstract;

namespace ProductService.Controllers {
    /// <summary>
    /// APIs to access Product
    /// </summary>
    [Route ("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;

        private readonly IBusService _busService;

        /// <summary>
        /// Constructor for DI
        /// </summary>
        /// <param name="mediator"> IMediator Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public ProductsController (IMediator mediator, IBusService busService, ILogger<ProductsController> logger) {
            _mediator = mediator ??
                throw new ArgumentNullException (nameof (mediator));
            _busService = busService ??
                throw new ArgumentNullException (nameof (busService));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));

        }

        // GET api/products
        /// <summary>
        ///  Get all products
        /// </summary>
        /// <returns></returns>
        /// <response code="200">List of Products in store</response>
        /// <response code="500">Error Result Code</response>
        [HttpGet]
        [ProducesResponseType (typeof (IEnumerable<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType (typeof (StatusCodeResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get () {
            try {
                //Log Information - Request
                _logger.LogInformation ($"Received Request : HTTPGET api/products");

                await _busService.SendMessage (new FindAllProductsQuery ());

                // Query to return all the products in store
                IEnumerable<Product> products = await _mediator.Send (new FindAllProductsQuery ());

                //Return result
                return Ok (products);
            } catch (Exception ex) {
                // Log the failure and return HTTP 500
                _logger.LogError ($"Error : HTTPGET api/products: {ex}");
                return StatusCode (StatusCodes.Status500InternalServerError);
            }

        }

        // GET api/products/5
        /// <summary>
        ///  Get Product by Id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns></returns>
        /// <response code="200">Returns Product in store</response>
        /// <response code="400">Returns Product not found</response>
        /// <response code="500">Error Result Code</response>
        [HttpGet ("{id}")]
        [ProducesResponseType (typeof (Product), StatusCodes.Status200OK)]
        [ProducesResponseType (typeof (NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType (typeof (StatusCodeResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get (int id) {
            try {
                //Log Information - Request
                _logger.LogInformation ($"Received Request : HTTPGET api/products/{id}");

                // Query to find Product by id
                Product product = await _mediator.Send (new FindProductByIdQuery () { ProductId = id });

                if (product == null) {
                    // Log warning and return HTTP 404
                    _logger.LogWarning ($"Not Found : HTTPGET api/products/{id}");
                    return NotFound ();
                }

                //Return result 
                return Ok (product);
            } catch (Exception ex) {
                // Log the failure and return HTTP 500
                _logger.LogError ($"Error at HTTPGET api/products/{id} :{ex}");
                return StatusCode (StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/products
        /// <summary>
        /// Add Product to store
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <response code="201">Returns Product added to store</response>
        /// <response code="500">Error Re sult Code</response>
        [HttpPost]
        [ProducesResponseType (typeof (Product), StatusCodes.Status201Created)]
        [ProducesResponseType (typeof (StatusCodeResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post ([FromBody] Product product) {
            try {
                //Log Information - Request
                _logger.LogInformation ($"Received Request : HTTPPOST api/products");

                //Create command from Input 
                AddNewProductCommand addCmd = new AddNewProductCommand {
                    Name = product.Name,
                    Description = product.Description,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock
                };

                //Send Command
                AddNewProductResult result = await _mediator.Send (addCmd);

                //Return result
                return CreatedAtRoute ("", new { id = result.ProductId });
            } catch (Exception ex) {
                // Log the failure and return HTTP 500
                _logger.LogError ($"Error at HTTPPOST api/products :{ex}");
                return StatusCode (StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/products/5
        /// <summary>
        ///  Update Product in store
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="product">Product</param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        /// <response code="404">Returns Product not found</response>
        /// <response code="500">Error Result Code</response>
        [HttpPut ("{id}")]
        [ProducesResponseType (typeof (NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType (typeof (NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType (typeof (StatusCodeResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put (int id, [FromBody] Product product) {
            try {
                //Log Information - Request
                _logger.LogInformation ($"Received Request : HTTPPUT api/products");

                //Check if Product exists.
                Product productInDb = await _mediator.Send (new FindProductByIdQuery () { ProductId = id });

                if (productInDb == null) {
                    // Log warning and return HTTP 404
                    _logger.LogWarning ($"Not Found : HTTPPUT api/products/{id}");
                    return NotFound ();
                }

                //Create command object
                UpdateProductCommand updateCmd = new UpdateProductCommand {
                    ProductId = product.ProductId,
                    Description = product.Description,
                    Name = product.Name,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    IsActive = product.IsActive
                };

                //Send command
                await _mediator.Send (updateCmd);

                //Return
                return NoContent ();
            } catch (Exception ex) {
                // Log the failure and return HTTP 500
                _logger.LogError ($"Error at HTTPPUT api/products/{id}:{ex}");
                return StatusCode (StatusCodes.Status500InternalServerError);
            }

        }

        public async Task<IActionResult> Delete (float id) {
            //Check if Product exists.
            Product productInDb = await _mediator.Send (new FindProductByIdQuery () { ProductId = 1 });
            //Return
            return NoContent ();
        }

        // DELETE api/products/5
        /// <summary>
        ///  Delete Product in store
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        /// <response code="404">Returns Product not found</response>
        /// <response code="500">Error Result Code</response>
        [HttpDelete ("{id}")]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [ProducesResponseType (StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete (int id) {
            try {
                //Log Information - Request
                _logger.LogInformation ($"Received Request: HTTPDELETE api/products/{id}");

                //Check if Product exists.
                Product productInDb = await _mediator.Send (new FindProductByIdQuery () { ProductId = id });

                if (productInDb == null) {
                    // Log warning and return HTTP 404
                    _logger.LogWarning ($"Not Found : HTTPDELETE api/products/{id}");
                    return NotFound ();
                }

                //Create command object
                DeleteProductCommand deleteCmd = new DeleteProductCommand { ProductId = id };

                //Send command
                await _mediator.Send (deleteCmd);

                //Return
                return NoContent ();
            } catch (Exception ex) {
                // Log the failure and return HTTP 500
                _logger.LogError ($"Error at HTTPDELETE api/products/{id}:{ex}");
                return StatusCode (StatusCodes.Status500InternalServerError);
            }

        }

    }
}