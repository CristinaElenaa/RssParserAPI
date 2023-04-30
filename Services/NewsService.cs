using Microsoft.EntityFrameworkCore;
using RssParser.Data;
using RssParser.DTOs;
using RssParser.Helpers;
using RssParser.Models;
using System.ServiceModel.Syndication;
using System.Xml;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace RssParser.Services
{
    public class NewsService:INewsService
    {
        private readonly DataContext _dataContext;

        public NewsService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<RssItem> GetNews(string url)
        {
            var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);

            var items = feed.Items.Select(item => new RssItem
            {
                Title = item.Title.Text,
                Description = item.Summary?.Text,
                Link = item.Links.FirstOrDefault()?.Uri.AbsoluteUri,
                PublishDate = item.PublishDate.UtcDateTime
            }).ToList();

            return items;
        }

        public List<RssItem> SortNewsItems(List<RssItem> newsItems, string sortOption)
        {
            switch (sortOption.ToLower())
            {
                case "titleasc":
                    newsItems = newsItems.OrderBy(n => n.Title, new TitleComparer()).ToList();
                    break;
                case "titledesc":
                    newsItems = newsItems.OrderByDescending(n => n.Title, new TitleComparer()).ToList();
                    break;
                case "dateasc":
                    newsItems = newsItems.OrderBy(n => n.PublishDate, new PublishDateComparer()).ToList();
                    break;
                case "datedesc":
                    newsItems = newsItems.OrderByDescending(n => n.PublishDate, new PublishDateComparer()).ToList();
                    break;
                default:
                    return null;
            }

            var items = newsItems.Select(item => new RssItem
            {
                Title = item.Title,
                Description = item.Description,
                Link = item.Link,
                PublishDate = item.PublishDate
            }).ToList();

            return items;
        }

        public List<RssItem> SearchNewsItems(List<RssItem> newsItems,string keyword)
        {
            var lowerKeyword = keyword.ToLower();
            var news = newsItems.Where(n => n.Title.ToLower().Contains(lowerKeyword) || n.Description.ToLower().Contains(lowerKeyword)).ToList();
            if(news == null)
            {
                return null;
            }

            return news;
        }

        public async  Task<List<News>> AddNewsList(List<RssItem> newsItems)
        {
            var publisher = new Publisher { Name = "News Publisher" };
            _dataContext.Publishers.Add(publisher);

            var newsGroup = new NewsGroup { Name = "News Group" };
            _dataContext.NewsGroups.Add(newsGroup);

            var newsList = new List<News>();
            foreach (var item in newsItems)
            {
                var news = new News
                {
                    Title = item.Title,
                    Description = item.Description,
                    PublishDate = item.PublishDate,
                    Link = item.Link,
                    Publisher = publisher,
                    NewsGroup = newsGroup
                };
                newsList.Add(news);
            }

            _dataContext.News.AddRange(newsList);
            await _dataContext.SaveChangesAsync();

            return await _dataContext.News.ToListAsync();
        
        }
    }
}
