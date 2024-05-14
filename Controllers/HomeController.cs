using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Newroz_Home_Task.Entities;
using Newroz_Home_Task.Models;
using Newroz_Home_Task.Repositories.Interface;
namespace Newroz_Home_Task.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {

        private readonly NewrozdbContext _db;
        private readonly IQuoteRepository _quoteRepository;

        public HomeController(NewrozdbContext db,IQuoteRepository quoteRepository)
        {
            _db = db;
            _quoteRepository = quoteRepository;
        }

        //extract-quote start

        [HttpPost]
        [Route("api/extract-quote")]
        public async Task<IActionResult> ExtractQuote()
        {
            try
            {
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync("https://www.goodreads.com/quotes");

                var quotes = doc.DocumentNode.SelectNodes("//div[@class='quoteText']");

                var quoteList = new List<Quoteinformation>();
                var cnt = 1;
                foreach (var quoteNode in quotes)
                {
                    // Extract text and author from quoteNode
                    var text = quoteNode.InnerText.Trim().Split('\n')[0].Trim();
                    var author = quoteNode.SelectSingleNode(".//span[@class='authorOrTitle']")
                        .InnerText.Trim();

                    // Add quote to the list
                    quoteList.Add(new Quoteinformation { Id = cnt, Quote = text, Author = author });
                    cnt++;
                }

                // Delete existing records
                var existingQuotes = await _db.Quoteinformations.ToListAsync();
                _db.Quoteinformations.RemoveRange(existingQuotes);
                await _db.SaveChangesAsync();

                // Add new records
                _db.Quoteinformations.AddRange(quoteList);
                await _db.SaveChangesAsync();


                var response = new
                {
                    ok = "done"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        //end



        // getall-quote start
        [HttpGet]
        [Route("api/getall-quote")]
        public async Task<IActionResult> GetQuote()
        {
            try
            {
                var QuoteList = await _quoteRepository.GetAllQuotes();
                return Ok(QuoteList);
            }
            catch (Exception ex)
            {
                // For simplicity, I'll just return a 500 Internal Server Error
                return StatusCode(500, "An error occurred while retrieving the quotes.");
            }
        }

        //end


        // Delete-quote start
        [HttpDelete]
        [Route("api/delete-quote")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            try
            {
                string ans = "";
                if (id == 0)
                {
                    ans = "Invalid Id";
                }
                else
                {
                    ans = await _quoteRepository.DeleteQuoteusingId(id);
                }

                var response = new
                {
                    ans = ans
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // For simplicity, I'll just return a 500 Internal Server Error
                return StatusCode(500, "An error occurred while deleting the quote.");
            }
        }


        //end

        // Update-quote start
        [HttpPut]
        [Route("api/update-quote")]
        public async Task<IActionResult> UpdateQuote(Quoteinformation data)
        {
            try
            {
                string result = "";
                if (data.Id != null && data.Quote != null && data.Author != null)
                {
                    result = await _quoteRepository.UpdateQuoteData(data);
                }
                else
                {
                    result = "Invalid Operation";
                }

                var response = new
                {
                    result = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // For simplicity, I'll just return a 500 Internal Server Error
                return StatusCode(500, "An error occurred while updating the quote.");
            }
        }


        //end


        // Get-quote start
        [HttpGet]
        [Route("api/get-quote")]
        public async Task<IActionResult> GetQuote(int id)
        {
            try
            {
                if (id == 0)
                {
                    var response1 = new
                    {
                        result = "Invalid Id"
                    };
                    return Ok(response1);
                }

                var ans = await _quoteRepository.GetQuoteUsingId(id);

                if (ans == null)
                {
                    var response2 = new
                    {
                        result = $"Id {id} is Not Present"
                    };
                    return Ok(response2);
                }

                var response = new
                {
                    Id = ans.Id,
                    Quote = ans.Quote,
                    Author = ans.Author,
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // For simplicity, I'll just return a 500 Internal Server Error
                return StatusCode(500, "An error occurred while retrieving the quote.");
            }
        }


        //end

        // DeleteAll-quote start
        [HttpDelete]
        [Route("api/deleteall-quote")]
        public async Task<IActionResult> DeleteAllQuote()
        {
            try
            {
                var allquote = await _db.Quoteinformations.ToListAsync();
                if (allquote != null)
                {
                    _db.RemoveRange(allquote);
                    await _db.SaveChangesAsync();
                }

                var response = new
                {
                    ans = "Deleted All Quote successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // For simplicity, I'll just return a 500 Internal Server Error
                return StatusCode(500, "An error occurred while deleting the quote.");
            }
        }


        //end

    }
}
