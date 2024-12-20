﻿namespace EconomizzeUserApp.Model
{
    public class QuoteModel
    {
        public int QuoteId { get; set; }
        public int UserId { get; set; }
        public int NeighborhoodId { get; set; }
        public bool IsExpired { get; set; }
        public bool IsFulfilled { get; set; }
        public String QuotePrice { get; set; } = String.Empty;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
