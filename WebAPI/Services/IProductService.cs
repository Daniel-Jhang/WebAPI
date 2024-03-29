﻿namespace NorthwindWebAPI.Services
{
    public interface IProductService
    {
        Task<ProductDTO> CreateProduct(ProductDTO product);
        Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId);
        Task<ProductDTO> UpdateProduct(ProductDTO product);
        Task<ProductDTO> DeleteProduct(int productId);
    }
}