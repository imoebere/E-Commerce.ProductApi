using E_Commerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversion;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            //Get all product from repository
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
                return NotFound("No products detected in the DB");

            // Convert data from entity to DTO and return
            var (_, list) = ProductConversions.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound("No products found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            //Get single product from repository
            var product = await productInterface.FindByIdAsync(id);
            if (product is null)
                return NotFound("Product requested is not found");

            // Convert data from entity to DTO and return
            var (_product, _) = ProductConversions.FromEntity(product, null!);
            return _product is not null ? Ok(_product) : NotFound("Product not found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            // Check for model state is all data annotation are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Convert to entity 
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            // Check for model state is all data annotation are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Convert to entity 
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            // Convert to entity 
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.DeleteAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
