namespace TestNewLine.Data.Models
{
    public class PostBlog :BaseEntity
    {

        public string AuthorId { get; set; }
        public User Author { get; set; }
        public string? Title { get; set; }
        public string? SubTittle { get; set; }
        public string? Image { get; set; }
        public string? Body { get; set; }
    }
}
