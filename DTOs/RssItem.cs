using System.Security;

namespace RssParser.DTOs
{
    public class RssItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
