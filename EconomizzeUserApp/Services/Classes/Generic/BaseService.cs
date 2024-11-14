using EconomizzeUserApp.Services.Classes.Handler;

namespace EconomizzeUserApp.Services.Classes.Generic
{
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
}