﻿using System.Linq.Expressions;

namespace SolarPanelCalculatorApi.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        void Remove(T entity);
        void Update(T entity);

    }
}
