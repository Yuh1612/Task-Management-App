using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;
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

        private IProjectMemberRepository _projectMemberRepository;

        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext,
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            IListTaskRepository listTaskRepository,
            IProjectMemberRepository projectMemberRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _listTaskRepository = listTaskRepository;
            _projectMemberRepository = projectMemberRepository;
        }

        public IUserRepository userRepository => _userRepository;

        public IProjectRepository projectRepository => _projectRepository;

        public IListTaskRepository listTaskRepository => _listTaskRepository;

        public IProjectMemberRepository projectMemberRepository => _projectMemberRepository;

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

        public async Task CommitTransaction()
        {
            await _dbContext.SaveEntitiesAsync();

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