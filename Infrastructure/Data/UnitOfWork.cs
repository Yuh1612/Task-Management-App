using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        private IsolationLevel? _isolationLevel;

        private IUserRepository _userRepository;

        private IProjectRepository _projectRepository;

        private IListTaskRepository _listTaskRepository;

        private ITaskRepository _taskRepository;

        private ITodoRepository _todoRepository;

        private IAttachmentRepository _attachmentRepository;

        private ILabelRepository _labelRepository;

        private IHistoryRepository _historyRepository;

        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext,
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            IListTaskRepository listTaskRepository,
            ITaskRepository taskRepository,
            ITodoRepository todoRepository,
            IAttachmentRepository attachmentRepository,
            ILabelRepository labelRepository,
            IHistoryRepository historyRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _listTaskRepository = listTaskRepository;
            _taskRepository = taskRepository;
            _todoRepository = todoRepository;
            _attachmentRepository = attachmentRepository;
            _labelRepository = labelRepository;
            _historyRepository = historyRepository;
        }

        public IUserRepository userRepository => _userRepository;

        public IProjectRepository projectRepository => _projectRepository;

        public IListTaskRepository listTaskRepository => _listTaskRepository;

        public ITaskRepository taskRepository => _taskRepository;

        public ITodoRepository todoRepository => _todoRepository;

        public IAttachmentRepository attachmentRepository => _attachmentRepository;

        public ILabelRepository labelRepository => _labelRepository;

        public IHistoryRepository historyRepository => _historyRepository;

        public async Task BeginTransaction()
        {
            if (_transaction == null)
            {
                if (_isolationLevel.HasValue)
                {
                    _transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel.Value);
                }
                else
                {
                    _transaction = await _dbContext.Database.BeginTransactionAsync();
                }
            }
        }

        public async Task CommitTransaction(bool IsAuthorize = true)
        {
            await _dbContext.SaveEntitiesAsync(IsAuthorize);

            if (_transaction == null) return;
            await _transaction.CommitAsync();

            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Dispose()
        {
            if (_dbContext == null) return;

            _dbContext.Dispose();
        }

        public async Task RollbackTransaction()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();

            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveEntitiesAsync();
        }
    }
}