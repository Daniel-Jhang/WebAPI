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
    }
}
