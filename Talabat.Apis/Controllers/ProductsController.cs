using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Apis.Dtos;
using Talabat.Apis.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Models;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductCategory> _categoriesRepo;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;

        public ProductsController(IGenericRepository<Product> productRepo,
            IGenericRepository<ProductCategory> categoriesRepo,
            IGenericRepository<ProductBrand> brandsRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _categoriesRepo = categoriesRepo;
            _brandsRepo = brandsRepo;
        }


        // GET: BaseUrl/api/Products
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams) 
        {
            var specs = new ProductWithBrandAndCategorySpecifications(specParams);

            var products = await _productRepo.GetAllWithSpecAsync(specs);

            var mappedProducts = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            
            var countSpecs = new ProductsWithFilterationForCountSpecification(specParams);

            // Onthor Query For Get Count Without Pagination
            var count = await _productRepo.GetCountAsync(countSpecs);

            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, count, mappedProducts));
        }

        // GET: BaseUrl/api/Products/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(id);
            var product = await _productRepo.GetWithSpecAsync(specs);

            var mappedProduct = _mapper.Map<ProductToReturnDto>(product);
            if (product == null)
                return NotFound($"Product with ID {id} not found."); // 404
            return Ok(mappedProduct); // 200 
        }


        // Get All Categories

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _categoriesRepo.GetAllAsync();
            if (categories == null || !categories.Any())
                return NotFound(new ApiResponse(404, "No categories found")); // 404
            return Ok(categories); // 200
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandsRepo.GetAllAsync();
            if (brands == null || !brands.Any())
                return NotFound(new ApiResponse(404, "No brands found")); // 404
            return Ok(brands); // 200
        }
    }
}
