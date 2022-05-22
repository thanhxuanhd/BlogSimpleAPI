namespace Blog.Core.Model;

public class Comment : IEntityBase
{
    public string Title { get; set; }
    public string Content { get; set; }
}