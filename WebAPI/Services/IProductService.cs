using WebAPI.DTO;

namespace WebAPI.Services
{
    public interface IProductService
    {
        Task<ProductDTO> CreateProduct(ProductDTO product);
        Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId);
        Task<ProductDTO> UpdateProduct(ProductDTO product);
    }
}