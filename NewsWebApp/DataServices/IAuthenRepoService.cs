using NewsAppClasses.Dtos;

namespace NewsWebApp.DataServices
{
    public interface IAuthenRepoService
    {
        public abstract Task<string?> Login(LogInDTO logIn);
        public abstract Task<int> Logout();

    }
}
