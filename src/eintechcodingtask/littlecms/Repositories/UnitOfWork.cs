
using core.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Transactions;

namespace littlecms.Repositories
{
    public interface IUnitOfWorkScope : IDisposable
    {
        void BeginTransaction();

        void Commit();

        void Rollback();

        ICmsRepository CmsRepository { get; }  
    } 


    internal class UnitOfWorkScope : IUnitOfWorkScope
    {
        private readonly ConcurrentDictionary<string, IRepository> _repositories;
         
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        internal UnitOfWorkScope(IApplicationConfiguration config)
        {
            _repositories = new ConcurrentDictionary<string, IRepository>();
            var confixx = config;
            _connection = new MySqlConnection(config.DbConnectionString);
            _connection.Open();
        }

        public void BeginTransaction()
        {
            if (_transaction != null)
                throw new TransactionException("A transaction for this scope has already been created.");

            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction == null)
                throw new TransactionException("A transaction has not been created for this scope.");

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void Rollback()
        {
            if (_transaction == null)
                throw new TransactionException("A transaction has not been created for this scope.");

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public ICmsRepository CmsRepository
        {
            get
            {
                const string key = nameof(BetRepository);
                return (ICmsRepository)_repositories.GetOrAdd(key, new CmsRepository(_connection, _transaction));
            }
        } 
        

        public void Dispose()
        {
            // Clean the transaction by performing a rollback if the transaction exists
            if (_transaction?.Connection.State == ConnectionState.Open)
                _transaction?.Rollback();

            _connection?.Dispose();
            _connection = null;
        }
    }
}
