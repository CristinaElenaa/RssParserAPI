using Microsoft.EntityFrameworkCore;
using RssParser.Models;

namespace RssParser.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<News> News { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<NewsGroup> NewsGroups { get; set; }

    }
}
