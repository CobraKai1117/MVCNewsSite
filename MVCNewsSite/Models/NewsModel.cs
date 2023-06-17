namespace MVCNewsSite.Models
{
    public class NewsModel
    {
        public string status { get; set; }

        public int totalResults { get; set; }

        public List<NewsArticle> articles { get; set; }

    }
}
