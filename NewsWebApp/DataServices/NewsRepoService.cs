    using NewsAppClasses;
using NewsAppClasses.Dtos;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace NewsWebApp.DataServices
{
    public class NewsRepoService : INewsRepoService
    {

        string Url = "https://localhost:7242/api/News";
        private readonly IHttpClientFactory client;
        HttpClient Client { get; set; }
        public NewsRepoService(IHttpClientFactory clientFactory)
        {
            this.client = clientFactory;
            Client=client.CreateClient("myclient");


        }

        public async void  Add(NewsWriteDto news,string token )
        {

            using (var cl = new HttpClient())
            {
                cl.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string requestBody = JsonSerializer.Serialize(news);
                HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await cl.PostAsJsonAsync(Url, news);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                }

            }

            
           

        }

        public bool Delete(int id, string token)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = Client.DeleteAsync(Url + "/" + id).Result;
            return response.IsSuccessStatusCode;
        }

        public async Task<News> Get(int id)
        {
            HttpResponseMessage response = Client.GetAsync(Url + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                // Read the response content
                var content = await response.Content.ReadFromJsonAsync<News>();
                return content;
            }
            return null;
        }

        public async Task<IEnumerable<News>> GetAll()
        {
            HttpResponseMessage response = Client.GetAsync(Url).Result;
            if (response.IsSuccessStatusCode)
            {
                // Read the response content
                var content = await response.Content.ReadFromJsonAsync<List<News>>();
                return content;
            }
            return null;
        }

        public async Task<bool> Update(NewsUpdateDto news, string token)
        {
            using (var cl = new HttpClient())
            {
                cl.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string requestBody = JsonSerializer.Serialize(news);
                HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await cl.PutAsync(Url + "/" + news.Id.ToString(), httpContent);
                return response.IsSuccessStatusCode;

            }

                  

        }
    }
}
