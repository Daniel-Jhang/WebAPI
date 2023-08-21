namespace NorthwindWebAPI.DAO
{
    public interface IProductAccessService
    {
        Task<ProductDTO> CreateProduct(ProductDTO product);
        Task<Product> GetProductById(int productId);
        Task<Product> GetProductByName(string productName);
        Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId);
        Task<ProductDTO> UpdateProduct(ProductDTO product);
        Task<ProductDTO> DeleteProduct(int productId);
    }
}