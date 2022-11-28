using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly SocialNetworkDbContext _context;

        public Repository(SocialNetworkDbContext context)
        {
            _context= context;
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return (await GetAllAsync())
                .FirstOrDefault(e => e.Id.ToString() == id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context
                .Set<TEntity>()
                .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate)
        {
            return (await GetAllAsync())
                .Where(predicate);
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            var dbEntity = await GetByIdAsync(entity.Id.ToString());
            if (dbEntity == null)
            {
                return false;
            }
            _context.Update(entity);
            return true;
        }

        public async Task<bool> CreateAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
            return true;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Remove(entity);
            return true;
        }
    }
}
