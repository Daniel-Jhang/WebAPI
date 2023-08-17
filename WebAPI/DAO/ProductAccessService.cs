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

        /// <summary>
        /// 新增產品紀錄至資料庫
        /// </summary>
        /// <param name="product">接收一個 ProductDTO 型別的參數，包含產品的各個屬性值</param>
        /// <returns>回傳一個 ProductDTO 型別的物件，包含新增產品的屬性值，其中 ProductId 是資料庫自動產生的值</returns>
        public async Task<ProductDTO> CreateProduct(ProductDTO product)
        {
            try
            {
                // 建立資料庫交易，以確保所有操作都成功才進行提交
                var dbTransaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    // 將傳入的 ProductDTO 物件轉換成 Product 物件
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
                    // 新增 Product 物件至資料庫
                    await _dbContext.Products.AddAsync(productToDB);
                    await _dbContext.SaveChangesAsync();
                    // 提交資料庫交易
                    await dbTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // 發生例外時進行回滾
                    await dbTransaction.RollbackAsync();
                    // 將例外訊息記錄至日誌
                    _logger.Error($"資料庫交易(Transaction)時發生問題: {ex}");
                    throw new Exception($"資料庫交易(Transaction)時發生問題", ex);
                }
                finally
                {
                    // 釋放資料庫交易所佔用的資源
                    await dbTransaction.DisposeAsync();
                }
                // 取得剛新增產品的 ProductId
                product.ProductId = GetProductByName(product.ProductName).Result.ProductId;
                // 回傳新增的產品紀錄
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 取得指定產品編號(productId)的產品(Product)物件
        /// </summary>
        /// <param name="productId">產品編號，用來查詢資料庫的產品</param>
        /// <returns>產品紀錄</returns>
        /// <exception cref="Exception">如果找不到指定產品編號的產品，會記錄錯誤訊息並拋出例外狀況</exception>
        public async Task<Product> GetProductById(int productId)
        {
            try
            {
                // 從dbContext取得指定產品編號(productId)的產品
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

        /// <summary>
        /// 取得指定產品名稱(productName)的產品(Product)物件
        /// </summary>
        /// <param name="productName">產品名稱，用來查詢資料庫的產品</param>
        /// <returns>產品紀錄</returns>
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

        /// <summary>
        /// 取得產品列表，如果指定起始編號及結束編號則只返回該範圍內的產品
        /// </summary>
        /// <param name="startProductId">起始產品編號</param>
        /// <param name="endProductId">結束產品編號</param>
        /// <returns>返回產品列表</returns>
        public async Task<List<ProductDTO>> GetProductList(int? startProductId, int? endProductId)
        {
            try
            {
                // 從資料庫中取出所有產品
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

                // 取得產品總數
                var productCount = productList.Count;

                // 驗證傳入資料
                if (startProductId <= 0 || endProductId > productCount)
                {
                    _logger.Error($"產品的ID介於: {1}~{productCount}，請輸入正確的數字");
                    throw new Exception($"產品的ID介於: {1}~{productCount}，請輸入正確的數字");
                }

                // 如果傳入起始編號則只返回該編號及以後的產品
                if (startProductId.HasValue && startProductId > 0)
                {
                    productList = productList.Where(x => x.ProductId >= startProductId.Value).ToList();
                }

                // 如果傳入結束編號則只返回該編號及之前的產品
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

        /// <summary>
        /// 更新產品資料
        /// </summary>
        /// <param name="product">ProductDTO 物件，包含要更新的產品資料</param>
        /// <returns>更新後的 ProductDTO 物件</returns>
        public async Task<ProductDTO> UpdateProduct(ProductDTO product)
        {
            try
            {
                // 取得要更新的產品
                var productToUpdate = await GetProductById(product.ProductId);

                // 開始資料庫交易
                var dbTransaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    // 更新產品資料
                    productToUpdate.ProductName = product.ProductName;
                    productToUpdate.SupplierId = product.SupplierId;
                    productToUpdate.CategoryId = product.CategoryId;
                    productToUpdate.QuantityPerUnit = product.QuantityPerUnit;
                    productToUpdate.UnitPrice = product.UnitPrice;
                    productToUpdate.UnitsInStock = product.UnitsInStock;
                    productToUpdate.UnitsOnOrder = product.UnitsOnOrder;
                    productToUpdate.ReorderLevel = product.ReorderLevel;
                    productToUpdate.Discontinued = product.Discontinued;

                    // 提交資料庫交易
                    await _dbContext.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // 發生例外時進行回滾
                    await dbTransaction.RollbackAsync();
                    // 將例外訊息記錄至日誌
                    _logger.Error($"資料庫交易(Transaction)時發生問題: {ex}");
                    throw new Exception($"資料庫交易(Transaction)時發生問題", ex);
                }
                finally
                {
                    // 釋放資料庫交易所佔用的資源
                    await dbTransaction.DisposeAsync();
                }

                // 回傳更新後的產品資料
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 刪除產品資料，同時刪除與該產品有關的訂單資料
        /// </summary>
        /// <param name="productId">產品編號</param>
        /// <returns>被刪除的產品資料</returns>
        public async Task<ProductDTO> DeleteProduct(int productId)
        {
            try
            {
                // 取得要刪除的產品資料
                var productToDelete = await GetProductById(productId);

                // 檢查是否有關聯的訂單資料
                var orderList = await _dbContext.OrderDetails.Where(x => x.ProductId == productId).ToListAsync();

                // 開始執行資料庫交易
                var dbTransaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    if (orderList.Any())
                    {
                        // 刪除關聯的訂單資料
                        _dbContext.OrderDetails.RemoveRange(orderList);
                    }

                    // 刪除產品資料
                    _dbContext.Products.Remove(productToDelete);
                    await _dbContext.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // 發生例外時進行回滾
                    await dbTransaction.RollbackAsync();
                    // 將例外訊息記錄至日誌
                    _logger.Error($"資料庫交易(Transaction)時發生問題: {ex}");
                    throw new Exception($"資料庫交易(Transaction)時發生問題", ex);
                }
                finally
                {
                    // 釋放資料庫交易所佔用的資源
                    await dbTransaction.DisposeAsync();
                }

                // 回傳刪除後的產品資料
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
