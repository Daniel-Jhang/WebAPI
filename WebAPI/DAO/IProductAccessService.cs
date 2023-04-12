using WebAPI.DTO;
using WebAPI.Models;

namespace WebAPI.DAO
{
    public interface IProductAccessService
    {
        Task<Product> GetProductById(int productId);
        Task<Product> GetProductByName(string productName);
        Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId);
        Task<ProductDTO> UpdateProduct(ProductDTO product);
        Task<ProductDTO> CreateProduct(ProductDTO product);
    }
}