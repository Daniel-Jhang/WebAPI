using Microsoft.EntityFrameworkCore;
using WebAPI.DTO;
using WebAPI.Models;
using ILogger = Serilog.ILogger;

namespace WebAPI.DAO
{
    public class ProductAccessService : IProductAccessService
    {
        private readonly NorthwindContext _dbContext;
        private readonly ILogger _logger;

        public ProductAccessService(NorthwindContext dbContext, ILogger logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        public async Task<ProductDTO> CreateProduct(ProductDTO product)
        {
            try
            {
                var dbTransaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    Product productToDB = new Product()
                    {
                        ProductName = product.ProductName,
                        SupplierId = product.SupplierId,
                        CategoryId = product.CategoryId,
                        QuantityPerUnit = product.QuantityPerUnit,
                        UnitPrice = product.UnitPrice,
                        UnitsInStock = product.UnitsInStock,
                        UnitsOnOrder = product.UnitsOnOrder,
                        ReorderLevel = product.ReorderLevel,
                        Discontinued = product.Discontinued
                    };
                    await _dbContext.Products.AddAsync(productToDB);
                    await _dbContext.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await dbTransaction.RollbackAsync();
                    _logger.Error($"資料庫交易(Transaction)時發生問題: {ex}");
                    throw new Exception($"資料庫交易(Transaction)時發生問題", ex);
                }
                finally
                {
                    await dbTransaction.DisposeAsync();
                }
                product.ProductId = GetProductByName(product.ProductName).Result.ProductId;
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<Product> GetProductById(int productId)
        {
            try
            {
                var product = await _dbContext.Products.SingleOrDefaultAsync(x => x.ProductId == productId);
                if (product == null)
                {
                    _logger.Error($"找不到產品，產品編號: {productId} 不存在");
                    throw new Exception($"找不到產品，產品編號: {productId} 不存在");
                }
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<Product> GetProductByName(string productName)
        {
            try
            {
                var product = await _dbContext.Products.SingleOrDefaultAsync(x => x.ProductName == productName);
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId)
        {
            try
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

                var productCount = productList.Count;

                // 驗證傳入資料
                if (startProductId <= 0 || endProductId > productCount)
                {
                    _logger.Error($"產品的ID介於: {1}~{productCount}，請輸入正確的數字");
                    throw new Exception($"產品的ID介於: {1}~{productCount}，請輸入正確的數字");
                }

                if (startProductId.HasValue && startProductId > 0)
                {
                    productList = productList.Where(x => x.ProductId >= startProductId.Value).ToList();
                }

                if (endProductId.HasValue && endProductId <= productCount)
                {
                    productList = productList.Where(x => x.ProductId <= endProductId.Value).ToList();
                }

                return productList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<ProductDTO> UpdateProduct(ProductDTO product)
        {
            try
            {
                var productToUpdate = await GetProductById(product.ProductId);
                var dbTransaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    productToUpdate.ProductName = product.ProductName;
                    productToUpdate.SupplierId = product.SupplierId;
                    productToUpdate.CategoryId = product.CategoryId;
                    productToUpdate.QuantityPerUnit = product.QuantityPerUnit;
                    productToUpdate.UnitPrice = product.UnitPrice;
                    productToUpdate.UnitsInStock = product.UnitsInStock;
                    productToUpdate.UnitsOnOrder = product.UnitsOnOrder;
                    productToUpdate.ReorderLevel = product.ReorderLevel;
                    productToUpdate.Discontinued = product.Discontinued;

                    await _dbContext.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await dbTransaction.RollbackAsync();
                    _logger.Error($"資料庫交易(Transaction)時發生問題: {ex}");
                    throw new Exception($"資料庫交易(Transaction)時發生問題", ex);
                }
                finally
                {
                    await dbTransaction.DisposeAsync();
                }

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<ProductDTO> DeleteProduct(int productId)
        {
            try
            {
                var productToDelete = await GetProductById(productId);

                // 檢查是否有關聯的訂單資料
                var orderList = await _dbContext.OrderDetails.Where(x => x.ProductId == productId).ToListAsync();

                var dbTransaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    if (orderList.Any())
                    {
                        // 刪除關聯的訂單資料
                        _dbContext.OrderDetails.RemoveRange(orderList);
                    }

                    _dbContext.Products.Remove(productToDelete);
                    await _dbContext.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await dbTransaction.RollbackAsync();
                    _logger.Error($"資料庫交易(Transaction)時發生問題: {ex}");
                    throw new Exception($"資料庫交易(Transaction)時發生問題", ex);
                }
                finally
                {
                    await dbTransaction.DisposeAsync();
                }

                var result = new ProductDTO
                {
                    ProductId = productToDelete.ProductId,
                    ProductName = productToDelete.ProductName,
                    SupplierId = productToDelete.SupplierId,
                    CategoryId = productToDelete.CategoryId,
                    QuantityPerUnit = productToDelete.QuantityPerUnit,
                    UnitPrice = productToDelete.UnitPrice,
                    UnitsInStock = productToDelete.UnitsInStock,
                    UnitsOnOrder = productToDelete.UnitsOnOrder,
                    ReorderLevel = productToDelete.ReorderLevel,
                    Discontinued = productToDelete.Discontinued
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
