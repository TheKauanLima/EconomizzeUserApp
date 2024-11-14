using Economizze.Library;
using EconomizzeUserApp.Services.Interfaces.Generic;

namespace EconomizzeUserApp.Services.Interfaces
{
    interface IUserLoginService : IService<UserLogin>
    {
        Task VerifyAsync(UserLogin userLogin);
        Task<bool> AuthenticateUserAsync(UserLogin loginModel);
        Task<bool> ChangePasswordAsync(UserLogin changePassword);
    }
}
