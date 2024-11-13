using Economizze.Library;
using EconomizzeUserApp.Services.Classes;
using EconomizzeUserApp.Services.Interfaces;

namespace StoreApp.Services.Repositories
{
    public class StreetDetailViewService : BaseService<StreetDetailView>, IStreetDetailViewService
    {
        public StreetDetailViewService(StatusHandler statusHandler)
            : base(statusHandler) { }

        #region SET LIST VALUES
        public void SetListValues()
        {
            // If no street details are provided, initialize with default values
            if (Entities.Count == 0)
            {
                Entities.Add(new StreetDetailView
                {
                    StreetId = 1,
                    NeighborhoodId = 101,
                    StreetName = "Main Street",
                    StreetNameAscii = "Main Street",
                    Zipcode = "12345",
                    NeighborhoodName = "Downtown",
                    CityName = "Cityville",
                    StateName = "State"
                });

                Entities.Add(new StreetDetailView
                {
                    StreetId = 2,
                    NeighborhoodId = 102,
                    StreetName = "Elm Street",
                    StreetNameAscii = "Elm Street",
                    Zipcode = "67890",
                    NeighborhoodName = "Uptown",
                    CityName = "Cityville",
                    StateName = "State"
                });
            }
        }
        #endregion

        #region READ DETAIL BY ID
        public async Task<StreetDetailView?> ReadStreetDetailByIdAsync(int streetId)
        {
            // Simulate asynchronous behavior
            var result = Entities.FirstOrDefault(sd => sd.StreetId == streetId);
            return await Task.FromResult(result);
        }
        #endregion

        #region READ DETAIL BY ZIP CODE
        public async Task<StreetDetailView> ReadStreetDetailByZipCodeAsync(string zipCode)
        {
            // Simulate asynchronous behavior
            var result = Entities.FirstOrDefault(sd => sd.Zipcode == zipCode);
            CurrentEntity = result;
            return await Task.FromResult(result!);
        }
        #endregion

        #region CREATE
        public async Task<StreetDetailView> CreateAsync(StreetDetailView entity)
        {
            try
            {
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

        #region NOT IMPLEMENTED
        public async Task<StreetDetailView?> ReadByIdAsync(object id)
        {
            await Task.Delay(0);
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<StreetDetailView>> ReadAllAsync()
        {
            await Task.Delay(0);
            throw new NotImplementedException();
        }

        public async Task<StreetDetailView?> UpdateAsync(StreetDetailView entity)
        {
            await Task.Delay(0);
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(object id)
        {
            await Task.Delay(0);
            throw new NotImplementedException();
        }
        #endregion
    }
}
