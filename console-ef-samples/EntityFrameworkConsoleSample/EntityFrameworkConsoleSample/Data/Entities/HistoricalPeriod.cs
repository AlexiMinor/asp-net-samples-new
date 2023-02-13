namespace EntityFrameworkConsoleSample.Data.Entities;

public class HistoricalPeriod
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    //navigation properties
    public List<Author> Authors { get; set; }

}