using Microsoft.AspNetCore.Http.HttpResults;
using NewsAppClasses;
using NewsAppClasses.Dtos;
using System.Text;
using System.Text.Json;

namespace NewsWebApp.DataServices
{
    public class AuthenRepoService : IAuthenRepoService
    {
        string Url = "https://localhost:7242/api/Authorization";
        private readonly IHttpClientFactory clientFactory;
        public AuthenRepoService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<string?> Login(LogInDTO cred)
        {
            using (var client = new HttpClient())
            {
                string requestBody = JsonSerializer.Serialize(cred);
                HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsJsonAsync(Url+"/Login", cred);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<TokenDTO>();
                    return content?.Token.ToString();

                }
                else if(((int)response.StatusCode) == StatusCodes.Status404NotFound)
                {
                    return "Username doesn't exist";
                }
                else if (((int)response.StatusCode) == StatusCodes.Status401Unauthorized)
                {
                    return "Wrong password";
                }
                return null;

            }

        }

        public Task<int> Logout()
        {
            throw new NotImplementedException();
        }
    }
}
