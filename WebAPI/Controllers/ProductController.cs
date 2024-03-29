﻿namespace NorthwindWebAPI.Controllers
{
    /// <summary>
    /// 使用EF Core操作Northwind資料庫，實作基本的CRUD和3-Tier架構(Controller, Service, DataAccess)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
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

        [HttpDelete]
        public async Task<ApiResultDataModel> Delete(int productId)
        {
            try
            {
                var result = new ApiResultDataModel();
                var data = await _productService.DeleteProduct(productId);
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
