using NewsAppClasses;
using NewsAppClasses.Dtos;

namespace NewsWebApp.DataServices
{
    public interface INewsRepoService
    {
        public abstract void Add(NewsWriteDto news, string token);

        public abstract bool Delete(int id, string token);

        public abstract Task<News> Get(int id);

        public abstract Task<IEnumerable<News>> GetAll();

        public abstract Task<bool> Update(NewsUpdateDto news, string token);
    }
}
