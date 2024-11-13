using EconomizzeUserApp.Services.Classes;

namespace EconomizzeUserApp.Services.Interfaces
{
    public interface IService<TEntity>
    {
        StatusHandler _statusHandler { get; set; }
        TEntity? CurrentEntity { get; set; }
        List<TEntity> Entities { get; set; }

        void SetListValues();
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity?> ReadByIdAsync(object id);
        Task<IEnumerable<TEntity>> ReadAllAsync();
        Task<TEntity?> UpdateAsync(TEntity entity);
        Task DeleteAsync(object id);
    }
}
