using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<CatalogController> logger;
        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            return Ok(await productRepository.GetAllProductsAsync());
        }

        [HttpGet ("{id:length(24)}", Name = "GetByID")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var prod = await productRepository.GetProductByIdAsync(id);
            if (prod == null)
            {
                logger.LogError($"Invalid {id}");
                return NotFound();
            }
            return Ok(prod);
        }


        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("/name/{name}", Name = "GetByName")]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var prod = await productRepository.GetProductByNameAsync(name);
            if (prod == null)
            {
                logger.LogError($"Invalid product name: {name} provided");
                return NotFound();
            }
            return Ok(prod);
        }


        [HttpGet("/category/{category}", Name = "GetByCategory")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByCategory([FromRoute] string category)
        {
            var prod = await productRepository.GetProductByCategoryAsync(category);
            if (prod == null)
            {
                logger.LogError($"Invalid category: {category} provided");
                return NotFound();
            }
            return Ok(prod);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            await productRepository.AddAsync(product);
            return CreatedAtRoute("GetById", new { id = product.Id }, product);

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Product product)
        {
            await productRepository.UpdateAsync(product);
            return CreatedAtRoute("GetById", new { id = product.Id }, product);
        }

        [HttpDelete("id:length(24)")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await productRepository.DeleteAsync(id);
            return Ok(result);
        }

    }
}
