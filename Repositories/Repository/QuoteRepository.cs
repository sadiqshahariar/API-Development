using Microsoft.EntityFrameworkCore;
using Newroz_Home_Task.Models;
using Newroz_Home_Task.Repositories.Interface;

namespace Newroz_Home_Task.Repositories.Repository
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly NewrozdbContext _db;

        public QuoteRepository(NewrozdbContext db)
        {
            _db = db;
        }

        public async Task<string> DeleteQuoteusingId(int id)
        {
            var quote = await _db.Quoteinformations.FindAsync(id);
            if (quote != null)
            {
                _db.Quoteinformations.Remove(quote);
                await _db.SaveChangesAsync();
                return $"Quote with ID {id} has been deleted.";
            }
            else
            {
                return $"Quote with ID {id} not found.";
            }
        }

        public async Task<IEnumerable<Quoteinformation>> GetAllQuotes()
        {
            return await _db.Quoteinformations.ToListAsync();
        }

        public async Task<Quoteinformation> GetQuoteUsingId(int id)
        {
            var quote = await _db.Quoteinformations.FindAsync(id);
            return quote;
        }

        public async Task<string> UpdateQuoteData(Quoteinformation data)
        {
            var quote = await _db.Quoteinformations.FindAsync(data.Id);
            if (quote != null)
            {
                quote.Quote=data.Quote;
                quote.Author = data.Author;

                await _db.SaveChangesAsync();
                return $"ID {data.Id} has been Updated.";
            }
            else
            {
                return $"ID {data.Id} not found.";
            }
        }
    }
}
