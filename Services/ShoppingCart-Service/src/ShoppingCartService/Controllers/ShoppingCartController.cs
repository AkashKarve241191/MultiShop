using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCartService.Commands;
using ShoppingCartService.Domain;
using ShoppingCartService.Queries;

namespace ShoppingCartService.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ShoppingCartController> _logger;

        /// <summary>
        /// Constructor for DI
        /// </summary>
        /// <param name="mediator"> IMediator Dependency</param>
        /// <param name="logger">ILogger Dependency</param>
        public ShoppingCartController (IMediator mediator, IMapper mapper, ILogger<ShoppingCartController> logger) {
            _mediator = mediator ??
                throw new ArgumentNullException (nameof (mediator));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
        }

        // GET api/shoppingcart/userId
        [HttpGet ("{userId}")]
        [ProducesResponseType (typeof (ShoppingCart), StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get (int userId) {
            try {
                _logger.LogInformation ($"Received Request: HTTPGET api/shoppingcart/{userId}");

                ShoppingCart cart = await _mediator.Send (new FindShoppingCartByUserIdQuery () { UserId = userId });

                if (cart == null) {
                    // Log warning and return HTTP 404
                    _logger.LogWarning ($"Not Found : HTTPGET api/shoppingcart/{userId}");
                    return NotFound ();
                }

                //Return result
                return Ok (cart);

            } catch (Exception ex) {
                // Log the failure and return HTTP 500
                _logger.LogError ($"Error at HTTPGET api/shoppingcart/{userId}: {ex}");
                return StatusCode (StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/values
        [HttpPost ("{userId}")]
        [ProducesResponseType (typeof (ShoppingCart), StatusCodes.Status201Created)]
        [ProducesResponseType (StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post ([FromBody] ShoppingCart cart, int userId) {
            try {
                //Log information
                _logger.LogInformation ($"Received Request: HTTPPOST api/shoppingcart/{userId}");

                //Initialize 
                ShoppingCart shoppingCart = new ShoppingCart ();
                shoppingCart.ShoppingCartItems = new List<ShoppingCartItem> ();

                foreach (var item in cart.ShoppingCartItems) {
                    ShoppingCartItem cartItem = new ShoppingCartItem ();
                    var product = await _mediator.Send (new FindProductByIdQuery () { ProductId = item.ProductId });
                    cartItem.ProductId = item.ProductId;
                    cartItem.ProductName = product.Name;
                    cartItem.ProductDescription = product.Description;
                    cartItem.Quantity = item.Quantity;
                    cartItem.UnitPrice = product.UnitPrice;
                    shoppingCart.ShoppingCartItems.Add (cartItem);
                }
                shoppingCart.UserId = userId;

                //Send Command
                AddNewShoppingCartCommandResult cmdResult = await _mediator.Send (_mapper.Map<AddNewShoppingCartCommand> (shoppingCart));

                //Return result
                return CreatedAtRoute ("", new { id = userId });

            } catch (Exception ex) {
                _logger.LogError ($"Error at HTTPPOST api/shoppingcart/{userId}:{ex}");
                return StatusCode (StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/shoppingcart/userId
        [HttpPut ("{userId}")]
        [ProducesResponseType (typeof (ShoppingCart), StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [ProducesResponseType (StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put ([FromBody] ShoppingCart cart, int userId) {
            try {
                _logger.LogInformation ($"Received : HTTPPUT api/shoppingcart/{userId}");

                ShoppingCart cartInDb = await _mediator.Send (new FindShoppingCartByUserIdQuery () { UserId = userId });

                if (cartInDb == null) {
                    // Log warning and return HTTP 404
                    _logger.LogWarning ($"Not Found : HTTPPUT api/shoppingcart/{userId}");
                    return NotFound ();
                }

                foreach (var item in cart.ShoppingCartItems) {
                    ShoppingCartItem cartItem = new ShoppingCartItem ();
                    var product = await _mediator.Send (new FindProductByIdQuery () { ProductId = item.ProductId });
                    cartItem.ProductId = item.ProductId;
                    cartItem.ProductName = product.Name;
                    cartItem.ProductDescription = product.Description;
                    cartItem.Quantity = item.Quantity;
                    cartItem.UnitPrice = product.UnitPrice;
                    cartInDb.ShoppingCartItems.Add (cartItem);
                }

                //Update item(s) in Cart
                await _mediator.Send (_mapper.Map<UpdateShoppingCartCommand> (cartInDb));

                return NoContent ();

            } catch (Exception ex) {
                _logger.LogError ($"Error : HTTPPUT api/shoppingcart/{userId}:{ex}");
                return StatusCode (StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/shoppingcart/5
        [HttpDelete ("{userId}")]
        public async Task<IActionResult> Delete (int userId) {
            try {
                _logger.LogInformation ($"Received : HTTPDELETE api/shoppingcart/{userId} request");

                ShoppingCart cartInDb = await _mediator.Send (new FindShoppingCartByUserIdQuery () { UserId = userId });

                if (cartInDb == null) {
                    // Log Error and return HTTP 404
                    _logger.LogWarning ($"Not Found : HTTPGET api/shoppingcart/{userId}");
                    return NotFound ();
                }

                //Send command
                await _mediator.Send (_mapper.Map<DeleteShoppingCartCommand> (userId));

                return NoContent ();

            } catch (Exception ex) {
                _logger.LogError ($"Error at HTTPDELETE api/shoppingcart/{userId}:{ex}");
                return StatusCode (StatusCodes.Status500InternalServerError);
            }

        }
    }
}