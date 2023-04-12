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
