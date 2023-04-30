namespace RssParser.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime PublishDate { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public int NewsGroupId { get; set; }
        public NewsGroup NewsGroup { get; set; }


    }
}
