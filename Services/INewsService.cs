using RssParser.DTOs;
using RssParser.Models;
using System.ServiceModel.Syndication;

namespace RssParser.Services
{
    public interface INewsService
    {
        List<RssItem> GetNews(string url);
        List<RssItem> SortNewsItems(List<RssItem> newsItems, string sortOption);
        List<RssItem> SearchNewsItems(List<RssItem> newsItems, string keyword);
        Task<List<News>> AddNewsList(List<RssItem> newsItems);
    }
}
