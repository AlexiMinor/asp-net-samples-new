namespace EntityFrameworkConsoleSample.Data.Entities;

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }

    public DateTime PublicationDate { get; set; }

    //nav properties
    public List<AuthorBook> AuthorBooks { get; set; }
}