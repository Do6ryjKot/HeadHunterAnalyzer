using System.Linq.Expressions;

namespace Contracts.DataServices {
	
	/// <summary>
	/// Интейрфейс репозитория БД.
	/// </summary>
	/// <typeparam name="T">Модель</typeparam>
	public interface IRepositoryBase<T> {

		public IQueryable<T> FindAll(bool trackChanges);
		public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
		void Create(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}
