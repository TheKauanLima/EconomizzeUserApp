using Economizze.Library;
using EconomizzeUserApp.Services.Classes.Generic;
using EconomizzeUserApp.Services.Classes.Handler;
using EconomizzeUserApp.Services.Interfaces;

namespace StoreApp.Services.Repositories
{
    public class StreetService : BaseService<Street>, IStreetService
    {
        public StreetService(StatusHandler statusHandler)
            : base(statusHandler) { }

        #region SET LIST VALUES
        public void SetListValues()
        {
            Entities.AddRange(new List<Street>
            {
                new Street
                {
                    StreetId = 1,
                    StreetName = "Avenida Brasil Sul",
                    StreetNameAscii = "avenida_brasil_sul",
                    Zipcode = "75074230",
                    Longitude = -48.9568,
                    Latitude = -16.3267,
                    NeighborhoodId = 101,
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now.AddMonths(-2),
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now
                },
                new Street
                {
                    StreetId = 2,
                    StreetName = "Rua Engenheiro Portela",
                    StreetNameAscii = "rua_engenheiro_portela",
                    Zipcode = "75074310",
                    Longitude = -48.9582,
                    Latitude = -16.3289,
                    NeighborhoodId = 102,
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now.AddMonths(-2),
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now
                },
                new Street
                {
                    StreetId = 3,
                    StreetName = "Avenida Goiás",
                    StreetNameAscii = "avenida_goias",
                    Zipcode = "75074010",
                    Longitude = -48.9537,
                    Latitude = -16.3245,
                    NeighborhoodId = 103,
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now.AddMonths(-3),
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now
                },
                new Street
                {
                    StreetId = 4,
                    StreetName = "Rua Coronel Batista",
                    StreetNameAscii = "rua_coronel_batista",
                    Zipcode = "75074180",
                    Longitude = -48.9571,
                    Latitude = -16.3270,
                    NeighborhoodId = 104,
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now.AddMonths(-3),
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now
                },
                new Street
                {
                    StreetId = 5,
                    StreetName = "Rua Padre Félix",
                    StreetNameAscii = "rua_padre_felix",
                    Zipcode = "75074220",
                    Longitude = -48.9546,
                    Latitude = -16.3261,
                    NeighborhoodId = 105,
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now.AddMonths(-1),
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now
                }
            });
        }
        #endregion

        #region CREATE
        public async Task<Street> CreateAsync(Street entity)
        {
            entity.StreetId = Entities.Any() ? Entities.Max(s => s.StreetId) + 1 : 1;
            entity.CreatedOn = DateTime.Now;
            entity.ModifiedOn = DateTime.Now;
            Entities.Add(entity);
            CurrentEntity = entity;
            return await Task.FromResult(CurrentEntity!);
        }
        #endregion

        #region READ BY ID
        public async Task<Street?> ReadByIdAsync(object id)
        {
            if (id is int streetId)
            {
                var result = Entities.FirstOrDefault(s => s.StreetId == streetId);
                return await Task.FromResult(result);
            }
            return null;
        }
        #endregion

        #region READ ALL
        public async Task<IEnumerable<Street>> ReadAllAsync()
        {
            return await Task.FromResult(Entities.AsEnumerable());
        }
        #endregion

        #region UPDATE
        public async Task<Street?> UpdateAsync(Street entity)
        {
            var existingStreet = Entities.FirstOrDefault(s => s.StreetId == entity.StreetId);
            if (existingStreet != null)
            {
                existingStreet.StreetName = entity.StreetName;
                existingStreet.StreetNameAscii = entity.StreetNameAscii;
                existingStreet.Zipcode = entity.Zipcode;
                existingStreet.Longitude = entity.Longitude;
                existingStreet.Latitude = entity.Latitude;
                existingStreet.NeighborhoodId = entity.NeighborhoodId;
                existingStreet.ModifiedBy = entity.ModifiedBy;
                existingStreet.ModifiedOn = DateTime.Now;
                return await Task.FromResult(existingStreet);
            }
            return null!;
        }
        #endregion

        #region DELETE
        public async Task DeleteAsync(object id)
        {
            if (id is int sid)
            {
                var street = Entities.FirstOrDefault(s => s.StreetId == sid);
                if (street != null)
                {
                    Entities.Remove(street);
                }
            }
            await Task.CompletedTask;
        }
        #endregion

        #region READ BY ZIP CODE
        public async Task<Street?> ReadByZipCodeAsync(string zipCode)
        {
            var result = Entities.FirstOrDefault(s => s.Zipcode == zipCode);
            return await Task.FromResult(result);
        }
        #endregion
    }
}
