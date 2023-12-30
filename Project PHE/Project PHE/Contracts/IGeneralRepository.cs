using System.Linq.Expressions;

namespace Project_PHE.Contracts
{
    public interface IGeneralRepository<TEntity>
    {
        TEntity? Create(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
        IEnumerable<TEntity> GetAll();
        TEntity? GetByGuid(string guid);

        // Method untuk mendapatkan data berdasarkan kriteria tertentu (WHERE clause)
        IEnumerable<TEntity> GetByWhere(Expression<Func<TEntity, bool>> attrb);

        /// <summary>
        ///     Method untuk mendapatkan data berdasarkan kriteria tertentu (WHERE clause) dengan
        ///     pengurutan data (ORDER BY clause) dan data yang terkait (INCLUDE clause)
        /// </summary>
        /// <param name="where">Kriteria data</param>
        /// <param name="orderBy">Pengurutan data</param>
        /// <param name="includes">Data yang terkait</param>
        /// <returns>Data yang memenuhi semua kriteria</returns>
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);
    }
}