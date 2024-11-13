using EconomizzeUserApp.Services.Classes;

public class BaseService<TEntity> where TEntity : class
{
    public StatusHandler _statusHandler { get; set; }
    public TEntity? CurrentEntity { get; set; }
    public List<TEntity> Entities { get; set; }

    public BaseService(StatusHandler statusHandler)
    {
        _statusHandler = statusHandler;
        Entities = new();
    }
}