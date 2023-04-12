using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpPost]
        public async Task<ApiResultDataModel> Post(ProductDTO product)
        {
            try
            {
                var result = new ApiResultDataModel();
                var data = await _productService.CreateProduct(product);
                result.Data = data;
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResultDataModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorMessageDetail = ex.ToString()
                };
            }
        }

        [HttpGet]
        public async Task<ApiResultDataModel> Get(int? startProductId, int? endProductId)
        {
            try
            {
                var result = new ApiResultDataModel();
                var data = await _productService.GetProductList(startProductId, endProductId);
                result.Data = data;
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResultDataModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorMessageDetail = ex.ToString()
                };
            }
        }

        [HttpPut]
        public async Task<ApiResultDataModel> Put(ProductDTO product)
        {
            try
            {
                var result = new ApiResultDataModel();
                var data = await _productService.UpdateProduct(product);
                result.Data = data;
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResultDataModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorMessageDetail = ex.ToString()
                };
            }
        }
    }
}
