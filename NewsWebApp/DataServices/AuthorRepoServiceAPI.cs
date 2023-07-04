using Newtonsoft.Json;
using System.Net.Http.Headers;
using NewsAppClasses;
using NewsAppClasses.Dtos;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Microsoft.Extensions.Primitives;
using NuGet.Common;

namespace NewsWebApp.DataServices
{
    public class AuthorRepoServiceAPI :IAuthorRepoServiceAPI
    {
        string Url = "https://localhost:7242/api/Authors";
        private HttpClient Client { get; }

        public AuthorRepoServiceAPI( HttpClient Client)
        {
            this.Client = Client;
            
        }

        public void Add(AuthorWriteDto author,String token)
        {
            //token
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var myContent = JsonConvert.SerializeObject(author);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //var stringContent = new StringContent(car, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and
            var content = Client.PostAsync(Url+"/", byteContent).Result;

        }

        public  bool Delete(int id,string token)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = Client.DeleteAsync(Url + "/" + id).Result;
            return response.IsSuccessStatusCode;
        }

        public async Task<Author> Get(int id)
        {
            HttpResponseMessage response = Client.GetAsync(Url + "/"+ id).Result;
            if (response.IsSuccessStatusCode)
            {
                // Read the response content
                var content = await response.Content.ReadFromJsonAsync<Author>();
                return content;
            }
            return null;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            HttpResponseMessage response = Client.GetAsync(Url).Result;
            if (response.IsSuccessStatusCode)
            {
                // Read the response content
                var content = await response.Content.ReadFromJsonAsync<List<Author>>();
                return content;
            }
            return null;


        }

        public void Update(AuthorUpdateDto author,string token)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var myContent = JsonConvert.SerializeObject(author);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //var stringContent = new StringContent(car, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and
            var content = Client.PutAsync(Url + "/" + author.Id, byteContent).Result;
           
           
        }
    }
}
