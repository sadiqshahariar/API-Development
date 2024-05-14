using Newroz_Home_Task.Models;

namespace Newroz_Home_Task.Repositories.Interface
{
    public interface IQuoteRepository
    {
        Task<string> DeleteQuoteusingId(int id);

        Task<IEnumerable<Quoteinformation>> GetAllQuotes();
        Task<Quoteinformation> GetQuoteUsingId(int id);
        Task<string> UpdateQuoteData(Quoteinformation data);
    }
}
