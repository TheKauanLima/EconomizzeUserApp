using Economizze.Library;
using EconomizzeUserApp.Services.Interfaces;

namespace EconomizzeUserApp.Services.Classes
{
    internal class UserLoginService : BaseService<UserLogin>, IUserLoginService
    {
        private static int _userId = 0;
        public UserLoginService(StatusHandler statusHandler)
            : base(statusHandler){ }

        #region SET VALUES IN THE LIST
        public void SetListValues()
        {
            Entities.Add(new UserLogin
            {
                UserId = ++_userId,
                UserUniqueId = Guid.NewGuid(),
                Username = "1@1.com",
                PasswordHash = "1",
                PasswordSalt = "1",
                IsActive = true,
                CreatedBy = _userId,
                CreatedOn = DateTime.Now,
                ModifiedBy = _userId,
                ModifiedOn = DateTime.Now
            });

            Entities.Add(new UserLogin
            {
                UserId = ++_userId,
                UserUniqueId = Guid.NewGuid(),
                Username = "2@2.com",
                PasswordHash = "1",
                PasswordSalt = "1",
                IsActive = true,
                CreatedBy = _userId,
                CreatedOn = DateTime.Now,
                ModifiedBy = _userId,
                ModifiedOn = DateTime.Now
            });

            Entities.Add(new UserLogin
            {
                UserId = ++_userId,
                UserUniqueId = Guid.NewGuid(),
                Username = "3@3.com",
                PasswordHash = "1",
                PasswordSalt = "1",
                IsActive = true,
                CreatedBy = _userId,
                CreatedOn = DateTime.Now,
                ModifiedBy = _userId,
                ModifiedOn = DateTime.Now
            });
        }
        #endregion

        #region CREATE
        public async Task<UserLogin> CreateAsync(UserLogin entity)
        {
            await Task.Delay(0);
            try
            {
                entity.UserId = ++_userId;
                entity.IsActive = true;
                entity.CreatedBy = _userId;
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedBy = _userId;
                entity.ModifiedOn = DateTime.Now;
                Entities!.Add(entity);
                CurrentEntity = entity;
                _statusHandler.Message = "Simular verificacao de email, clique aqui!"; ;
            }
            catch (Exception ex)
            {
                _statusHandler.Message = ex.Message;
            }
            return entity;
        }
        #endregion

        #region READ BY ID
        public async Task<UserLogin?> ReadByIdAsync(object id)
        {
            if (id is int userId)
            {
                var result = Entities.FirstOrDefault(ul => ul.UserId == userId);
                return await Task.FromResult(result);
            }
            return null;
        }
        #endregion

        #region READ ALL
        public async Task<IEnumerable<UserLogin>> ReadAllAsync()
        {
            return await Task.FromResult(Entities.AsEnumerable());
        }
        #endregion

        #region UPDATE
        public async Task<UserLogin?> UpdateAsync(UserLogin entity)
        {
            var existingUserLogin = Entities.FirstOrDefault(ul => ul.UserId == entity.UserId);
            if (existingUserLogin != null)
            {
                existingUserLogin.UserId = entity.UserId;
                existingUserLogin.UserUniqueId = entity.UserUniqueId;
                existingUserLogin.Username = entity.Username;
                existingUserLogin.PasswordHash = entity.PasswordHash;
                existingUserLogin.PasswordSalt = entity.PasswordSalt;
                existingUserLogin.IsVerified = entity.IsVerified;
                existingUserLogin.IsActive = entity.IsActive;
                existingUserLogin.IsLocked = entity.IsLocked;
                existingUserLogin.PasswordAttempts = entity.PasswordAttempts;
                existingUserLogin.ChangedInitialPassword = entity.ChangedInitialPassword;
                existingUserLogin.LockedTime = entity.LockedTime;
                existingUserLogin.ModifiedBy = entity.ModifiedBy;
                existingUserLogin.ModifiedOn = DateTime.Now;
                return await Task.FromResult(existingUserLogin);
            }
            return null!;
        }
        #endregion

        #region DELETE
        public async Task DeleteAsync(object id)
        {
            if (id is int uid)
            {
                var userId = Entities.FirstOrDefault(ul => ul.UserId == uid);
                if (userId != null)
                {
                    Entities.Remove(userId);
                }
            }
            await Task.CompletedTask;
        }
        #endregion

        #region VERIFY
        public async Task VerifyAsync(UserLogin userLogin)
        {
            await Task.Delay(0);
            var NewlyRegisteredUser = Entities[^1];
            NewlyRegisteredUser.IsActive = true;
            CurrentEntity = NewlyRegisteredUser;
        }
        #endregion

        #region AUTHENTICATE
        public Task<bool> AuthenticateUserAsync(UserLogin loginModel)
        {
            if (CurrentEntity is null)
            {
                CurrentEntity = Entities.FirstOrDefault(ul => ul.Username == loginModel.Username);
            }
            if (CurrentEntity is null)
            {
                // idfk
            }
            else if (CurrentEntity!.IsActive &&
                loginModel.Username == CurrentEntity.Username &&
                loginModel.PasswordHash == CurrentEntity.PasswordHash)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
        #endregion

        public async Task<bool> ChangePasswordAsync(UserLogin changePassword)
        {
            var existingUserLogin = Entities.FirstOrDefault(chp => chp.UserId == changePassword.UserId);
            if (existingUserLogin != null)
            {
                existingUserLogin.PasswordHash = changePassword.PasswordHash;
                existingUserLogin.PasswordSalt = changePassword.PasswordSalt;
                existingUserLogin.PasswordAttempts = 0;
                existingUserLogin.ChangedInitialPassword = true;
                existingUserLogin.ModifiedBy = changePassword.ModifiedBy;
                existingUserLogin.ModifiedOn = DateTime.Now;
                CurrentEntity = existingUserLogin;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
