namespace LabWebAPI.DataAccessObject
{
    public class TodoListDao : ITodoListDao
    {
        private readonly ILogger _logger;
        private readonly LabContext _dbContext;

        public TodoListDao(ILogger logger, LabContext dbContext)
        {
            this._logger = logger;
            this._dbContext = dbContext;
        }
        public async Task<TodoListDto> CreateTodoRecord(TodoListDto newTodo)
        {
            try
            {
                var todoRecordToDB = new TodoList();
                using (var dbTransaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        todoRecordToDB.TodoId = Guid.NewGuid();
                        todoRecordToDB.Status = newTodo.Status;
                        todoRecordToDB.Context = newTodo.Context;
                        todoRecordToDB.Editing = newTodo.Editing;

                        await _dbContext.TodoLists.AddAsync(todoRecordToDB);
                        await _dbContext.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (SqlException ex)
                    {
                        // 發生例外時進行回滾
                        await dbTransaction.RollbackAsync();
                        // 將例外訊息記錄至日誌
                        _logger.Error($"資料庫交易(Transaction)時發生問題: CreateTodoRecord(), {ex.Message}");
                        throw;
                    }
                }

                return new TodoListDto
                {
                    TodoId = todoRecordToDB.TodoId.ToString(),
                    Status = todoRecordToDB.Status,
                    Context = todoRecordToDB.Context,
                    Editing = todoRecordToDB.Editing
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"新增待辦事項時發生錯誤: {ex}");
            }
        }
        public async Task<TodoListDto> GetTodoRecord(Guid? todoRecordId = null, string? context = null)
        {
            try
            {
                if (!todoRecordId.HasValue && string.IsNullOrEmpty(context))
                {
                    throw new Exception("請輸入待辦事項的ID或內容");
                }

                TodoList record = null!;
                if (todoRecordId.HasValue)
                {
                    record = await GetTodoRecordById(todoRecordId.Value.ToString());
                }
                else if (!string.IsNullOrEmpty(context))
                {
                    record = await GetTodoRecordByContext(context);
                }

                return new TodoListDto
                {
                    TodoId = record.TodoId.ToString(),
                    Status = record.Status,
                    Context = record.Context,
                    Editing = record.Editing
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"取得指定待辦事項時發生錯誤: {ex}");
            }
        }
        public async Task<List<TodoListDto>> GetAllTodoList()
        {
            try
            {
                var todoList = await _dbContext.TodoLists.Select(x => new TodoListDto
                {
                    TodoId = x.TodoId.ToString(),
                    Status = x.Status,
                    Context = x.Context,
                    Editing = x.Editing
                }).OrderBy(x => x.TodoId).AsNoTracking().ToListAsync();

                return todoList;
            }
            catch (Exception ex)
            {
                throw new Exception($"取得所有的待辦事項時發生錯誤: {ex}");
            }
        }
        public async Task<TodoListDto> UpdateTodoRecord(TodoListDto todoRecord)
        {
            try
            {
                var todoRecordToUpdate = await GetTodoRecordById(todoRecord.TodoId);
                using (var dbTransaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        todoRecordToUpdate.Status = todoRecord.Status;
                        todoRecordToUpdate.Editing = todoRecord.Editing;
                        todoRecordToUpdate.Context = todoRecord.Context;

                        await _dbContext.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (SqlException ex)
                    {
                        // 發生例外時進行回滾
                        await dbTransaction.RollbackAsync();
                        // 將例外訊息記錄至日誌
                        _logger.Error($"資料庫交易(Transaction)時發生問題: UpdateTodoRecord(), {ex.Message}");
                        throw;
                    }
                }
                return todoRecord;
            }
            catch (Exception ex)
            {
                throw new Exception($"更新待辦事項時發生錯誤: {ex}");
            }
        }
        public async Task<List<TodoListDto>> DeleteTodoRecord(string todoRecordId)
        {
            try
            {
                var todoRecordToDelete = await GetTodoRecordById(todoRecordId.ToString());
                using (var dbTransaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _dbContext.TodoLists.Remove(todoRecordToDelete);
                        await _dbContext.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (SqlException ex)
                    {
                        // 發生例外時進行回滾
                        await dbTransaction.RollbackAsync();
                        // 將例外訊息記錄至日誌
                        _logger.Error($"資料庫交易(Transaction)時發生問題: DeleteTodoRecord(), {ex.Message}");
                        throw;
                    }
                }

                var result = await GetAllTodoList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"刪除待辦事項時發生錯誤: {ex}");
            }
        }
        public async Task<List<TodoListDto>> ClearCompleted(List<string> completedIdList)
        {
            try
            {
                // 將 ID 轉換成 Guid (使用 LINQ 將 ID 轉換成 Guid，並且過濾掉無效的 Guid)
                List<Guid> idList = completedIdList.Select(item => Guid.TryParse(item, out var id) ? id : Guid.Empty)
                                                    .Where(id => id != Guid.Empty).ToList();

                using (var dbTransaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var todoRecordsToClear = _dbContext.TodoLists.Where(x => idList.Contains(x.TodoId)).ToList();
                        _dbContext.TodoLists.RemoveRange(todoRecordsToClear);

                        await _dbContext.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (SqlException ex)
                    {
                        // 發生例外時進行回滾
                        await dbTransaction.RollbackAsync();
                        // 將例外訊息記錄至日誌
                        _logger.Error($"資料庫交易(Transaction)時發生問題: ClearCompleted(), {ex.Message}");
                        throw;
                    }
                }

                var result = await GetAllTodoList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"刪除 已完成 待辦事項時發生錯誤: {ex}");
            }
        }
        public async Task<bool> CheckIsExists(string? context = null)
        {
            if (!string.IsNullOrEmpty(context))
            {
                var record = await _dbContext.TodoLists.SingleOrDefaultAsync(x => x.Context == context);
                if (record == null)
                {
                  return false;
                }
            }
            return true;
        }
        public async Task<bool> ToggleAll(bool status)
        {
            using var dbTransaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in _dbContext.TodoLists.ToList())
                {
                    item.Status = status;
                }

                await _dbContext.SaveChangesAsync();
                await dbTransaction.CommitAsync();
                return true;
            }
            catch (SqlException ex)
            {
                // 發生例外時進行回滾
                await dbTransaction.RollbackAsync();
                // 將例外訊息記錄至日誌
                _logger.Error($"資料庫交易(Transaction)時發生問題: ToggleAll(), {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"更新所有待辦事項狀態時發生錯誤: {ex}");
            }
        }
        private async Task<TodoList> GetTodoRecordById(string todoRecordId)
        {
            if (!Guid.TryParse(todoRecordId, out Guid queryTodoId))
            {
                _logger.Error($"不正確的Guid格式: {todoRecordId}，請檢察待辦事項編號");
                throw new ArgumentException($"不正確的Guid格式: {todoRecordId}，請檢察待辦事項編號");
            }

            var todoRecord = await _dbContext.TodoLists.SingleOrDefaultAsync(x => x.TodoId == queryTodoId);
            if (todoRecord == null)
            {
                _logger.Error($"找不到指定的待辦事項紀錄，待辦事項編號: {todoRecordId} 不存在");
                throw new KeyNotFoundException($"找不到指定的待辦事項紀錄，待辦事項編號: {todoRecordId} 不存在");
            }
            return todoRecord;
        }
        private async Task<TodoList> GetTodoRecordByContext(string context)
        {
            var todoRecord = await _dbContext.TodoLists.SingleOrDefaultAsync(x => x.Context == context);
            if (todoRecord == null)
            {
                _logger.Error($"找不到指定的待辦事項紀錄，待辦事項: {context} 不存在");
                throw new KeyNotFoundException($"找不到指定的待辦事項紀錄，待辦事項: {context} 不存在");
            }
            return todoRecord;
        }
    }
}
