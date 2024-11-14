using Economizze.Library;
using EconomizzeUserApp.Services.Classes.Generic;
using EconomizzeUserApp.Services.Classes.Handler;
using EconomizzeUserApp.Services.Interfaces;

namespace EconomizzeUserApp.Services.Classes
{
    internal class UserService : BaseService<User>, IUserService
    {
        public UserService(StatusHandler statusHandler)
            : base(statusHandler) { }

        #region SET LIST VALUES
        public void SetListValues()
        {
            // Initialize with sample data
            Entities.AddRange(new List<User>
            {
                new User
                {
                    UserId = 1,
                    UserFirstName = "John",
                    UserMiddleName = "A.",
                    UserLastName = "Doe",
                    Cpf = "123456789",
                    Rg = "987654321",
                    PhoneNumber = "1234567890",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now
                },
                new User
                {
                    UserId = 2,
                    UserFirstName = "Jane",
                    UserMiddleName = "B.",
                    UserLastName = "Smith",
                    Cpf = "987654321",
                    Rg = "123456789",
                    PhoneNumber = "0987654321",
                    DateOfBirth = new DateTime(1995, 5, 15),
                    CreatedBy = 2,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = 2,
                    ModifiedOn = DateTime.Now
                }
            });
        }
        #endregion

        #region CREATE
        public async Task<User> CreateAsync(User entity)
        {
            try
            {
                entity.CreatedBy = entity.UserId;
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedBy = entity.UserId;
                entity.ModifiedOn = DateTime.Now;
                Entities.Add(entity);
                CurrentEntity = entity;
                _statusHandler.Message = "User created successfully!";
            }
            catch (Exception ex)
            {
                _statusHandler.Message = ex.Message;
            }
            return await Task.FromResult(entity);
        }
        #endregion

        #region READ BY ID
        public async Task<User?> ReadByIdAsync(object id)
        {
            if (id is int userId)
            {
                var result = Entities.FirstOrDefault(u => u.UserId == userId);
                return await Task.FromResult(result);
            }
            return null;
        }
        #endregion

        #region READ ALL
        public async Task<IEnumerable<User>> ReadAllAsync()
        {
            return await Task.FromResult(Entities.AsEnumerable());
        }
        #endregion

        #region UPDATE
        public async Task<User?> UpdateAsync(User entity)
        {
            var existingUser = Entities.FirstOrDefault(u => u.UserId == entity.UserId);
            if (existingUser != null)
            {
                existingUser.UserFirstName = entity.UserFirstName;
                existingUser.UserMiddleName = entity.UserMiddleName;
                existingUser.UserLastName = entity.UserLastName;
                existingUser.Cpf = entity.Cpf;
                existingUser.Rg = entity.Rg;
                existingUser.PhoneNumber = entity.PhoneNumber;
                existingUser.DateOfBirth = entity.DateOfBirth;
                existingUser.ModifiedBy = entity.ModifiedBy;
                existingUser.ModifiedOn = DateTime.Now;
                _statusHandler.Message = "User updated successfully!";
                return await Task.FromResult(existingUser);
            }
            _statusHandler.Message = "User not found!";
            return null;
        }
        #endregion

        #region DELETE
        public async Task DeleteAsync(object id)
        {
            if (id is int userId)
            {
                var user = Entities.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    Entities.Remove(user);
                    _statusHandler.Message = "User deleted successfully!";
                }
                else
                {
                    _statusHandler.Message = "User not found!";
                }
            }
            await Task.CompletedTask;
        }
        #endregion
    }
}
