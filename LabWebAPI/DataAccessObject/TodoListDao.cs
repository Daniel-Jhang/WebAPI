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
            using var dbTransaction = await _dbContext.Database.BeginTransactionAsync();
            var todoRecordToDB = new TodoList();
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
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                _logger.Error($"資料庫交易(Transaction)時發生問題: {ex}");
                throw new Exception("資料庫交易(Transaction)時發生問題", ex);
            }
            return new TodoListDto
            {
                TodoId = todoRecordToDB.TodoId,
                Status = todoRecordToDB.Status,
                Context = todoRecordToDB.Context,
                Editing = todoRecordToDB.Editing
            };
        }

        public async Task<TodoListDto> GetTodoRecord(Guid? todoRecordId = null, string? context = null)
        {
            if (!todoRecordId.HasValue && string.IsNullOrEmpty(context))
            {
                return new TodoListDto();
            }
            var record = todoRecordId.HasValue ? await GetTodoRecordById(todoRecordId.Value) : null;
            record ??= !string.IsNullOrEmpty(context) ? await GetTodoRecordByContext(context) : null;

            if (record == null)
            {
                return new TodoListDto();
            }

            return new TodoListDto
            {
                Status = record.Status,
                Context = record.Context,
                Editing = record.Editing
            };
        }

        public async Task<List<TodoListDto>> GetAllTodoList()
        {
            try
            {
                var todoList = await _dbContext.TodoLists.Select(x => new TodoListDto
                {
                    TodoId = x.TodoId,
                    Status = x.Status,
                    Context = x.Context,
                    Editing = x.Editing
                }).OrderBy(x => x.TodoId).AsNoTracking().ToListAsync();

                return todoList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<TodoListDto> UpdateTodoRecord(TodoListDto todoRecord)
        {
            try
            {
                var todoRecordToUpdate = await GetTodoRecordById(todoRecord.TodoId);

                var dbTransaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    todoRecordToUpdate.Status = todoRecord.Status;
                    todoRecordToUpdate.Editing = todoRecord.Editing;
                    todoRecordToUpdate.Context = todoRecord.Context;

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
                    await dbTransaction.DisposeAsync();
                }

                return todoRecord;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<List<TodoListDto>> DeleteTodoRecord(Guid todoRecordId)
        {
            try
            {
                var todoRecordToDelete = await GetTodoRecordById(todoRecordId);

                var dbTransaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    _dbContext.TodoLists.Remove(todoRecordToDelete);
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
                    await dbTransaction.DisposeAsync();
                }

                var result = await GetAllTodoList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<List<TodoListDto>> ClearCompleted(List<Guid> todoRecordIdList)
        {
            throw new NotImplementedException();
            try
            {
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private async Task<TodoList> GetTodoRecordById(Guid? todoRecordId)
        {
            try
            {
                var todoRecord = await _dbContext.TodoLists.SingleOrDefaultAsync(x => x.TodoId == todoRecordId);
                if (todoRecord == null)
                {
                    _logger.Error($"找不到紀錄，紀錄編號: {todoRecordId} 不存在");
                    throw new Exception($"找不到紀錄，紀錄編號: {todoRecordId} 不存在");
                }
                return todoRecord;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private async Task<TodoList> GetTodoRecordByContext(string context)
        {
            try
            {
                var todoRecord = await _dbContext.TodoLists.SingleOrDefaultAsync(x => x.Context == context);
                return todoRecord = null!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
