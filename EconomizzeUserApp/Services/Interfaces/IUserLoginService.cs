using Economizze.Library;

namespace EconomizzeUserApp.Services.Interfaces
{
    interface IUserLoginService : IService<UserLogin>
    {
        Task VerifyAsync(UserLogin userLogin);
        Task<bool> AuthenticateUserAsync(UserLogin loginModel);
        Task<bool> ChangePasswordAsync(UserLogin changePassword);
    }
}
