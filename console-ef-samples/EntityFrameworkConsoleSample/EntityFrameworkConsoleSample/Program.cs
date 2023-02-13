using System.Xml.Serialization;
using EntityFrameworkConsoleSample.Data;
using EntityFrameworkConsoleSample.Data.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityFrameworkConsoleSample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await SamplesWithJoin();

            using (var db = new LibraryContext())
            {
                //get many elements
                var authors = await db.Authors
                    .ToListAsync();

                var authors2 = await db.Authors
                    .Where(author => author.HistoricalPeriodId == 2)
                    .ToListAsync();

                //projection from table 
                var authorNames = await db.Authors
                    .Select(author => author.Name)
                    .ToListAsync();

                //get one element
                var histPeriod = await db.HistoricalPeriods.FirstOrDefaultAsync(period => period.Id == 2);

                //NonTrackedQuery(only for read operations) 
                var untrackedHistPeriod = await db.HistoricalPeriods
                    .AsNoTracking()
                    .FirstOrDefaultAsync(period => period.Id == 2);


                //IQueryable
                var authorsList = await db.Authors
                    .Where(author => author.Name.StartsWith("A"))
                    .ToListAsync();

                var authorsAsQueryable = db.Authors
                    .Where(author => author.Name.StartsWith("A"));

                authorsAsQueryable = authorsAsQueryable.Where(author => author.BirthDate <= DateTime.Now);

                //execution in same code line
                // FirstOrDefault(), LastOrDefault(), SingleOrDefault(), First(), Last(), Single()
                // .ToList(), ToArray(), ToDictionary(), etc... 
                // .Count()
                // foreach-> danger


                var authorsEnts = await authorsAsQueryable.ToListAsync();

                var authorEntities = (await db.Authors.ToListAsync())
                    .Where(author => author.BirthDate <= DateTime.Now).ToList();

                //Authors -> 2.6M -> 1kb each

                var a1 = await db.Authors
                    .Where(author => author.Name.StartsWith("A"))
                    .ToListAsync();//100k ~ 100mb 

                var a2 = (await db.Authors.ToListAsync())//~2.6GB 
                    .Where(author => author.Name.StartsWith("A"))
                    .ToList(); //~100mb 

            }
        }

        static async Task Sample()
        {
            using (var db = new LibraryContext())
            {
                var data = db.Authors
                    .Where(a => !string.IsNullOrEmpty(a.Name));

                //will never be executed
            }
        } //don't repeat

        static async Task AddOperations()
        {
            using (var db = new LibraryContext())
            {
                //add one entity
                var newHistoricalPeriod = await db.HistoricalPeriods
                    .AddAsync(
                        new HistoricalPeriod()
                        {
                            Name = "Silver Century"
                        });

                //newHistoricalPeriod.Entity.

                //add many entities
                await db.HistoricalPeriods
                    .AddRangeAsync(
                        new List<HistoricalPeriod>()
                    {
                        new HistoricalPeriod()
                        {
                            Name = "Gold Century"
                        },
                        new HistoricalPeriod()
                        {
                            Name = "Medieval Centuries"
                        },
                    });

                // when operations of insert => SqlBulkCopy

                //fixation 'changes'
                //db.SaveChanges();
                await db.SaveChangesAsync();

                //worst scenario ever
                //foreach (var element in list)
                //{
                //    //db.dbSet.add(element)
                      //db.SaveChanges()
                //}
            }
        }

        private static async Task DeleteOperations()
        {
            using (var db = new LibraryContext())
            {
                var entityForDelete = db.Authors.FirstOrDefault(a => a.Name == "");
                if (entityForDelete != null)
                {
                    db.Authors.Remove(entityForDelete);
                    await db.SaveChangesAsync();
                }

                var entitiesForDelete = await db.Authors.Where(a => a.Name == "").ToListAsync();
                if (entitiesForDelete.Any())
                {
                    db.Authors.RemoveRange(entitiesForDelete);
                    await db.SaveChangesAsync();
                }

                //remember about cascade Delete
            }
        }


        private static async Task EditOperations()
        {
            using (var db = new LibraryContext())
            {
                //be aware about this method 
                var entityForEdit = db.Authors.FirstOrDefault(a => a.Name == "");
                if (entityForEdit != null)
                {
                    entityForEdit.Name = "Vasily Pupkin";

                    db.Authors.Update(entityForEdit);
                    await db.SaveChangesAsync();
                }

                //best practice
                var entityForEdit2 = db.Authors.FirstOrDefault(a => a.Name == "");
                if (entityForEdit2 != null)
                {
                    entityForEdit2.Name = "Vasily Pupkin";
                    await db.SaveChangesAsync();
                }
                
            }
        }

        private static async Task SamplesWithJoin()
        {
            using (var db = new LibraryContext())
            {
                var authors2 = await db.Authors
                    .AsNoTracking()
                    .Where(author => author.HistoricalPeriodId == 2)
                    .Include(author => author.HistoricalPeriod)

                    .Include(author => author.AuthorBooks)
                    .ThenInclude(authorBook => authorBook.Book)
                    .ToListAsync();

                var z = 0;
            }
        }
    }
}