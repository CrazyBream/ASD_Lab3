using ASD_Lab3.DAL.Entities; 
using System;

namespace ASD_Lab3.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Category> Categories { get; }
        IRepository<Article> Articles { get; }
        IRepository<Comment> Comments { get; }

        void Save();
    }
}