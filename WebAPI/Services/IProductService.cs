using WebAPI.DTO;

namespace WebAPI.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId);
    }
}