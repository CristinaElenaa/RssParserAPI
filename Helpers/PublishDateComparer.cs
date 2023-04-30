namespace RssParser.Helpers
{
    public class PublishDateComparer: IComparer<DateTime>
    {
        public int Compare(DateTime x, DateTime y)
        {
            return x.CompareTo(y);
        }
    }
}
