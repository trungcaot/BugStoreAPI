using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;


namespace BugStoreDAL.EF.Initializers
{
    public static class Initializer
    {
        public static void InitializeData()
        {
            var context = new StoreContext();
            InitializeData(context);
        }

        public static void InitializeData(StoreContext context)
        {
            ClearData(context);
        }

        public static void ClearData(StoreContext context)
        {
            ExecuteDeleteSQL(context, "Categories");
            ExecuteDeleteSQL(context, "Products");
            ExecuteDeleteSQL(context, "Orders");
            ResetIdentity(context);
        }

        public static void ExecuteDeleteSQL(StoreContext context, string tableName)
        {
            context.Database.ExecuteSqlCommand($"Delete from Store.{tableName}");
        }

        public static void ResetIdentity(StoreContext context)
        {
            var tables = new[] { "Categories", "Products", "Orders" };
            foreach (var itm in tables)
            {
                context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"Store.{itm}\", RESEED, 0);");
            }
        }

        public static void Seed(StoreContext context)
        {
            try
            {
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(SampleData.GetCategories());
                    context.SaveChanges();
                }
                if (!context.Products.Any())
                {
                    context.Products.AddRange(SampleData.GetProducts(context.Categories.ToList()));
                    context.SaveChanges();
                }
                if (!context.Orders.Any())
                {
                    context.Orders.AddRange(SampleData.GetCart(context));
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
