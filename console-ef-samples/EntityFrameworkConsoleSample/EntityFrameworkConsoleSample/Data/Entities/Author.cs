using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkConsoleSample.Data.Entities;

public class Author
{
    [Key]
    public int Id { get; set; } //AuthorId

    [Required]
    public string Name { get; set; }

    public DateTime BirthDate { get; set; }

    //navigation properties
    public int HistoricalPeriodId { get; set; }

    public HistoricalPeriod HistoricalPeriod { get; set; }

    //nav properties
    public List<AuthorBook> AuthorBooks { get; set; }
}

//datetime -> 1.1.1970 00:00:00:0000
//datetime2 -> 1.1.0000 00:00:00