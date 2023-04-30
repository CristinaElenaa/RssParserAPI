using Microsoft.AspNetCore.Mvc;
using RssParser.DTOs;
using RssParser.Models;
using RssParser.Services;


namespace RssParser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly string _url = "https://rss.nytimes.com/services/xml/rss/nyt/World.xml";

        public NewsController(INewsService newsService) 
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RssItem>>> GetAllNews([FromQuery] string url)
        {
            var items = _newsService.GetNews(url);
            return Ok(items);
        }

        [HttpGet("sortNews")]
        public ActionResult<List<RssItem>> GetSortedNews(string sort)
        {
            var newsItems = _newsService.GetNews(_url);
            if (newsItems == null)
            {
                return BadRequest("Invalid url!");
            }

            var sortedNewsItems = _newsService.SortNewsItems(newsItems, sort);
            if(sortedNewsItems == null)
            {
                return BadRequest("Invalid Sort Option!");
            }

            return Ok(sortedNewsItems);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RssItem>>> SearchNewsList(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest("Please provide a valid search keyword.");
            }

            var newsItems = _newsService.GetNews(_url);
            var searchedNewsItems = _newsService.SearchNewsItems(newsItems, keyword);

            if (searchedNewsItems == null)
            {
                return BadRequest("Keyword not found!");
            }

            return Ok(searchedNewsItems);
        }

        [HttpPost]
        public async Task<ActionResult<List<News>>> SaveNewsListToDb()
        {
            var newsItems = _newsService.GetNews(_url);
            var result = await _newsService.AddNewsList(newsItems);

            return Ok(result);
        }

    }
}
