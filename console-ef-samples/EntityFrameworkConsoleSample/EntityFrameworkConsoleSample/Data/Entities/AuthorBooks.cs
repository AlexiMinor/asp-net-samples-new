namespace EntityFrameworkConsoleSample.Data.Entities;

public class AuthorBook
{
    public int Id { get; set; }

    public int SomeAdditionalProperty { get; set; }

    //nav props
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
}