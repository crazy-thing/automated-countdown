public class BibleVerseModel
{
    public Book book {get; set;}
    public int chapterId {get; set;}
    public int verseId {get; set;}
    public string verse {get; set;}
}

public class Book
{
    public string name {get; set;}
}