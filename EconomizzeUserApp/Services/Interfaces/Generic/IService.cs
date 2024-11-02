namespace EconomizzeUserApp.Services.Interfaces
{
    public interface IService<TModel>
    {
        TModel CurrentEntity { get; set; }
        bool isError { get; set; }
        string Message { get; set; }

        Task CreateAsync(TModel model);
        Task ReadByIdAsync(object id);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(object id);
    }
}
