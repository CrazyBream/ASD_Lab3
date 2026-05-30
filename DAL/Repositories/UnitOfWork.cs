using System;
using ASD_Lab3.DAL.EF;
using ASD_Lab3.DAL.Entities;
using ASD_Lab3.DAL.Interfaces;

namespace ASD_Lab3.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        private EFRepository<User> _userRepository;
        private EFRepository<Category> _categoryRepository;
        private EFRepository<Article> _articleRepository;
        private EFRepository<Comment> _commentRepository;

        public EFUnitOfWork(AppDbContext context)
        {
            _db = context;
        }

        public IRepository<User> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new EFRepository<User>(_db);
                return _userRepository;
            }
        }

        public IRepository<Category> Categories
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new EFRepository<Category>(_db);
                return _categoryRepository;
            }
        }

        public IRepository<Article> Articles
        {
            get
            {
                if (_articleRepository == null)
                    _articleRepository = new EFRepository<Article>(_db);
                return _articleRepository;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new EFRepository<Comment>(_db);
                return _commentRepository;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}