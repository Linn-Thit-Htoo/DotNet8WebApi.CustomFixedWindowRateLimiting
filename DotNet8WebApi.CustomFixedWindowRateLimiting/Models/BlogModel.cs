namespace DotNet8WebApi.CustomFixedWindowRateLimiting.Models
{
    public class BlogModel
    {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogAuthor { get; set; }
        public string BlogContent { get; set; }

        public BlogModel(int blogId, string blogTitle, string blogAuthor, string blogContent)
        {
            BlogId = blogId;
            BlogTitle = blogTitle;
            BlogAuthor = blogAuthor;
            BlogContent = blogContent;
        }
    }
}
