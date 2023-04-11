using WebAPI.DTO;

namespace WebAPI.DAO
{
    public interface IProductAccessService
    {
        Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId);
    }
}