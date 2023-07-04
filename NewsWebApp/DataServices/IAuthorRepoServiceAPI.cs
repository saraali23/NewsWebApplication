using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using NewsAppClasses;
using NewsAppClasses.Dtos;

namespace NewsWebApp.DataServices
{
    public interface IAuthorRepoServiceAPI 
    {
       
        public abstract void Add(AuthorWriteDto author, string token);

        public abstract bool Delete(int id, string token);

        public abstract Task<Author> Get(int id);

        public abstract Task<IEnumerable<Author>> GetAll();

        public abstract void Update(AuthorUpdateDto author, string token);
    }
}
