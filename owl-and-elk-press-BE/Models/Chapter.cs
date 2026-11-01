namespace owl_and_elk_press_BE.Models;

public class Chapter
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public long StoryId { get; set; }
    public short ChapterNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}