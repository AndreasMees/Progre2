using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public abstract class BaseRepository<T> where T : Entity
    {
        protected ApplicationDbContext DbContext { get; }

        public BaseRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public virtual async Task<T> Get(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<PagedResult<T>> List(int page, int pageSize)
        {
            return await DbContext.Set<T>()
                .OrderByDescending(x => x.Id)
                .GetPagedAsync(page, pageSize);
        }

        public virtual T Add(T item)
        {
            DbContext.Set<T>().Add(item);
            return item;
        }

        public virtual T Update(T item)
        {
            DbContext.Set<T>().Update(item);
            return item;
        }

        public virtual T Remove(T item)
        {
            DbContext.Set<T>().Remove(item);
            return item;
        }

        public virtual async Task Save(T item)
        {
            if (item.Id == 0)
                DbContext.Set<T>().Add(item);
            else
                DbContext.Set<T>().Update(item);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(int id)
        {
            var item = await Get(id);
            if (item != null)
                DbContext.Set<T>().Remove(item);
            await DbContext.SaveChangesAsync();
        }
    }
}