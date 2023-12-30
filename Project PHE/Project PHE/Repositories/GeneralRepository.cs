using Microsoft.EntityFrameworkCore;
using Project_PHE.Contracts;
using System.Linq.Expressions;
using System.Linq;
using Project_PHE.Data;

namespace Project_PHE.Repositories
{
    public abstract class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : class
    {
        protected readonly PheDbContext _context;

        public GeneralRepository(PheDbContext context)
        {
            _context = context;
        }

        public TEntity? Create(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public bool Update(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>()
                    .Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>()
                .ToList();
        }

        public TEntity? GetByGuid(string guid)
        {
            var entity = _context.Set<TEntity>()
                .Find(guid);
            _context.ChangeTracker
                .Clear();
            return entity;
        }

        public IEnumerable<TEntity> GetByWhere(Expression<Func<TEntity, bool>> attrb)
        {
            return _context.Set<TEntity>().Where(attrb).ToList();
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            foreach (var include in includes) query = query.Include(include);

            if (where != null) query = query.Where(where);

            if (orderBy != null) query = orderBy(query);

            return query.ToList();
        }
    }
}