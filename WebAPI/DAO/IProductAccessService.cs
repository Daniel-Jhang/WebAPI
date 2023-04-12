using WebAPI.DTO;
using WebAPI.Models;

namespace WebAPI.DAO
{
    public interface IProductAccessService
    {
        Task<Product> GetProduct(int productId);
        Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId);
        Task<ProductDTO> UpdateProduct(ProductDTO product);
    }
}