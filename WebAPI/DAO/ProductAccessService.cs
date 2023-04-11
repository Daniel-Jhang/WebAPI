using Microsoft.EntityFrameworkCore;
using WebAPI.DTO;
using WebAPI.Models;

namespace WebAPI.DAO
{
    public class ProductAccessService : IProductAccessService
    {
        private readonly NorthwindContext _dbContext;

        public ProductAccessService(NorthwindContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId)
        {
            var productList = await _dbContext.Products.Select(x => new ProductDTO
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                SupplierId = x.SupplierId,
                CategoryId = x.CategoryId,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                UnitsOnOrder = x.UnitsOnOrder,
                ReorderLevel = x.ReorderLevel,
                Discontinued = x.Discontinued,
            }).OrderBy(x => x.ProductId).ToListAsync();

            if (startProductId.HasValue)
            {
                productList = productList.Where(x => x.ProductId >= startProductId.Value).ToList();
            }

            if (endProductId.HasValue)
            {
                productList = productList.Where(x => x.ProductId <= endProductId.Value).ToList();
            }

            return productList;
        }
    }
}
