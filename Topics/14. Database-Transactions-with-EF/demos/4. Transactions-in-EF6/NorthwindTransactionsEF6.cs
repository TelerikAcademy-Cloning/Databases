using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4.Transactions_in_EF6
{
    class NorthwindTransactionsEF6
    {
        static void Main()
        {
            try
            {
                AddCategoriesInTransaction();
                Console.WriteLine("Categories successfully added.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add categories failed: {0} {1}",
                    ex.Message, ex.InnerException.Message);
            }

            try
            {
                ConflictingUpdateLastWins();
                Console.WriteLine("Category updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Updating category failed: {0}: {1}",
                    ex.GetType().Name, ex.Message);
            }
        }

        private static void AddCategoriesInTransaction()
        {
            NorthwindEntities context = new NorthwindEntities();

            // Add a valid new category
            Category newCategory = new Category()
            {
                CategoryName = "New Category",
                Description = "New category, just for testing"
            };
            context.Categories.Add(newCategory);

            // Add an invalid new category
            Category newCategoryLongName = new Category()
            {
                CategoryName = "New Category Loooooooong Name",
            };
            context.Categories.Add(newCategoryLongName);

            // The entire transaction will fail due to
            // insertion failure for the second category
            context.SaveChanges();
        }

        private static void ConflictingUpdateLastWins()
        {
            NorthwindEntities context = new NorthwindEntities();
            Category newCategory = new Category()
            {
                CategoryName = "New Category",
                Description = "New category, just for testing"
            };
            context.Categories.Add(newCategory);
            context.SaveChanges();

            // This context works in different transaction
            NorthwindEntities anotherContext = new NorthwindEntities();

            Category lastCategory =
                (from cat in anotherContext.Categories
                 where cat.CategoryID == newCategory.CategoryID
                 select cat).First();
            lastCategory.CategoryName = lastCategory.CategoryName + " 2";
            anotherContext.SaveChanges();

            // This will cause OptimisticConcurrencyException if
            // Categories.CategoryName has ConcurrencyMode=Fixed
            newCategory.CategoryName = newCategory.CategoryName + " 3";

            // Database wins
            bool isFailed;
            
            do
            {
                isFailed = false;
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    isFailed = true;
                    ex.Entries.Single().Reload();
                }
            } while (isFailed);

            // Client wins
            //bool isFailed;

            //do
            //{
            //    isFailed = false;
            //    try
            //    {
            //        context.SaveChanges();
            //    }
            //    catch (DbUpdateConcurrencyException ex)
            //    {
            //        isFailed = true;
            //        var entry = ex.Entries.Single();
            //        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
            //    }
            //} while (isFailed);
        }
    }
}
