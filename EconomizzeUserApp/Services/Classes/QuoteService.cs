using Economizze.Library;
using EconomizzeUserApp.Services.Classes;
using EconomizzeUserApp.Services.Interfaces;

namespace StoreApp.Services.Repositories
{
    public class QuoteService : BaseService<Quote>, IQuoteService
    {
        public QuoteService(StatusHandler statusHandler)
            : base(statusHandler) { }

        #region SET LIST VALUES
        public void SetListValues()
        {
            //Entities.AddRange(new List<Quote>
            //{
            //    new Quote
            //    {
            //        QuoteId = 1,
            //        UserId = 1,
            //        NeighborhoodId = 101,
            //        IsExpired = false,
            //        IsFullfiled = true,
            //        CreatedBy = 1,
            //        CreatedOn = DateTime.Now,
            //        ExpiresOn = DateTime.MaxValue,
            //        ModifiedBy = 1,
            //        ModifiedOn = DateTime.Now
            //    }
            //});
            //CurrentEntity = Entities[^1];
        }
        #endregion

        #region CREATE
        public async Task<Quote> CreateAsync(Quote entity)
        {
            entity.QuoteId = Entities.Any() ? Entities.Max(s => s.QuoteId) + 1 : 1;
            entity.CreatedOn = DateTime.Now;
            entity.ModifiedOn = DateTime.Now;
            Entities.Add(entity);
            CurrentEntity = entity;
            return await Task.FromResult(CurrentEntity!);
        }
        #endregion

        #region READ BY ID
        public async Task<Quote?> ReadByIdAsync(object id)
        {
            if (id is int quoteId)
            {
                var result = Entities.FirstOrDefault(q => q.QuoteId == quoteId);
                return await Task.FromResult(result);
            }
            return null;
        }
        #endregion

        #region READ ALL
        public async Task<IEnumerable<Quote>> ReadAllAsync()
        {
            return await Task.FromResult(Entities.AsEnumerable());
        }
        #endregion

        #region UPDATE
        public async Task<Quote?> UpdateAsync(Quote entity)
        {
            var existingQuote = Entities.FirstOrDefault(q => q.QuoteId == entity.QuoteId);
            if (existingQuote != null)
            {
                return await Task.FromResult(existingQuote);
            }
            return null!;
        }
        #endregion

        #region DELETE
        public async Task DeleteAsync(object id)
        {
            if (id is int qid)
            {
                var quote = Entities.FirstOrDefault(q => q.QuoteId == qid);
                if (quote != null)
                {
                    Entities.Remove(quote);
                }
            }
            await Task.CompletedTask;
        }
        #endregion
    }
}
