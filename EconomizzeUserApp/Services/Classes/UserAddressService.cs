using Economizze.Library;
using EconomizzeUserApp.Services.Classes.Generic;
using EconomizzeUserApp.Services.Classes.Handler;
using EconomizzeUserApp.Services.Interfaces;

namespace StoreApp.Services.Repositories
{
    public class UserAddressService : BaseService<UserAddress>, IUserAddressService
    {
        public UserAddressService(StatusHandler statusHandler)
            : base(statusHandler) { }

        #region SET LIST VALUES
        public void SetListValues()
        {
            // Optional: Initialize with sample data
            Entities.Add(new UserAddress
            {
                UserAddressId = 1,
                UserId = 1,
                StreetId = 101,
                Complement = "Near Park",
                ComplementAscii = "near_park",
                Longitude = 45.6789,
                Latitude = -73.4567,
                CreatedBy = 1,
                CreatedOn = DateTime.Now.AddDays(-30),
                ModifiedBy = 1,
                ModifiedOn = DateTime.Now
            });
        }
        #endregion

        #region CREATE
        public async Task<UserAddress> CreateAsync(UserAddress entity)
        {
            try
            {
                entity.UserAddressId = Entities.Any() ? Entities.Max(sa => sa.UserAddressId) + 1 : 1;
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedOn = DateTime.Now;
                Entities.Add(entity);
                CurrentEntity = entity;
            }
            catch (Exception ex)
            {
                _statusHandler.Message = ex.Message;
            }
            return await Task.FromResult(entity);
        }
        #endregion

        #region READ BY ID
        public async Task<UserAddress?> ReadByIdAsync(object id)
        {
            if (id is int userAddressId)
            {
                var result = Entities.FirstOrDefault(ua => ua.UserAddressId == userAddressId);
                return await Task.FromResult(result);
            }
            return null;
        }
        #endregion

        #region READ ALL
        public async Task<IEnumerable<UserAddress>> ReadAllAsync()
        {
            return await Task.FromResult(Entities.AsEnumerable());
        }
        #endregion

        #region UPDATE
        public async Task<UserAddress?> UpdateAsync(UserAddress entity)
        {
            var existingAddress = Entities.FirstOrDefault(ua => ua.UserAddressId == entity.UserAddressId);
            if (existingAddress != null)
            {
                existingAddress.StreetId = entity.StreetId;
                existingAddress.Complement = entity.Complement;
                existingAddress.ComplementAscii = entity.ComplementAscii;
                existingAddress.Longitude = entity.Longitude;
                existingAddress.Latitude = entity.Latitude;
                existingAddress.ModifiedBy = entity.ModifiedBy;
                existingAddress.ModifiedOn = DateTime.Now;
                return await Task.FromResult(existingAddress);
            }
            return null!;
        }
        #endregion

        #region DELETE
        public async Task DeleteAsync(object id)
        {
            if (id is int uaid)
            {
                var storeAddress = Entities.FirstOrDefault(ua => ua.UserAddressId == uaid);
                if (storeAddress != null)
                {
                    Entities.Remove(storeAddress);
                }
            }
            await Task.CompletedTask;
        }
        #endregion
    }
}
