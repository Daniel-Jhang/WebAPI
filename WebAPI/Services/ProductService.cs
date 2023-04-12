using WebAPI.DAO;
using WebAPI.DTO;

namespace WebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductAccessService _productAccessService;

        public ProductService(IProductAccessService productAccessService)
        {
            this._productAccessService = productAccessService;
        }

        public async Task<ProductDTO> CreateProduct(ProductDTO product)
        {
            var existProduct = await _productAccessService.GetProductByName(product.ProductName);
            if (existProduct != null)
            {
                throw new Exception("上架產品重複");
            }
            var productToCreate = await _productAccessService.CreateProduct(product);
            return productToCreate;
        }

        public async Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId)
        {
            // Business Logic
            return await _productAccessService.GetProductList(startProductId, endProductId);
        }

        public async Task<ProductDTO> UpdateProduct(ProductDTO product)
        {
            var productToUpdate = await _productAccessService.UpdateProduct(product);
            return productToUpdate;
        }
    }
}
