namespace ShoppingCartService.Services {
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ShoppingCartService.Configuration;
    using ShoppingCartService.Domain;
    using ShoppingCartService.Services.Abstract;

    public class ProductService : IProductService {

        private readonly HttpClient _client;
        private readonly IOptions<ProductServiceSettings> _productServiceSettings;
        private readonly ILogger<ProductService> _logger;

        public ProductService (HttpClient client,
            IOptions<ProductServiceSettings> productServiceSettings,
            ILogger<ProductService> logger) {
            _client = client ??
                throw new ArgumentNullException (nameof (client));
            _productServiceSettings = productServiceSettings ??
                throw new ArgumentNullException (nameof (productServiceSettings));
            _logger = logger ??
                throw new ArgumentNullException (nameof (logger));
            _client.BaseAddress = new Uri (_productServiceSettings.Value.BaseUrl);
        }

        /// <summary>
        ///  Find Product By ProductId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Product</returns>
        public async Task<Product> FindProductById (int productId) {
            try {
                //Log information
                _logger.LogInformation ($"Sending Request :  {_productServiceSettings.Value.BaseUrl}/{_productServiceSettings.Value.GetRoute}/{productId}");

                HttpResponseMessage httpResponse = await _client.GetAsync ($"{_productServiceSettings.Value.GetRoute}/{productId}", new CancellationToken ());

                if (httpResponse.IsSuccessStatusCode) {
                    _logger.LogInformation ($"Received Response : {_productServiceSettings.Value.BaseUrl}/{_productServiceSettings.Value.GetRoute}/{productId}, HttpStatusCode: {httpResponse.StatusCode}");

                    // Get Product from response
                    Product product = await httpResponse.Content.ReadAsAsync<Product> ();

                    //Return product
                    return product;
                }

                //Log Response code
                _logger.LogInformation ($"Error Response : {_productServiceSettings.Value.BaseUrl}/{_productServiceSettings.Value.GetRoute}/{productId}, HttpStatusCode: {httpResponse.StatusCode}");

                //Get  Error msg
                string errorMsg = await httpResponse.Content.ReadAsStringAsync ();

                //Log Error msg
                _logger.LogError ($"Error at {_productServiceSettings.Value.BaseUrl}/{_productServiceSettings.Value.GetRoute}/{productId}:{errorMsg}");

                //Throw new HttpRequestException
                throw new HttpRequestException ($"Error at {_productServiceSettings.Value.BaseUrl}/{_productServiceSettings.Value.GetRoute}/{productId}:{errorMsg}");
            } catch (Exception ex) {

                //Log Error
                _logger.LogError ($"Error at {_productServiceSettings.Value.BaseUrl}/{_productServiceSettings.Value.GetRoute}/{productId}:{ex}");
                throw;

            }

        }
    }
}