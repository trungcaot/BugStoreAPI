
using BugStoreDAL.EF;
using BugStoreDAL.EF.Initializers;
using System.Linq;
using Xunit;

namespace BugStoreTests
{
    [Collection("BugStore.DAL")]
    public class StoreContextTests
    {
        [Fact]
        public void Should_Get_All_Categories()
        {
            using (var db = new StoreContext())
            {
                Initializer.InitializeData(db);
                Assert.Equal(2, db.Categories.Count());
            }
        }

        [Fact]
        public void Should_Get_All_Products()
        {
            using (var db = new StoreContext())
            {
                Initializer.InitializeData(db);
                Assert.Equal(10, db.Products.Count());
            }
        }

        [Fact]
        public void Should_Get_All_Orders()
        {
            using (var db = new StoreContext())
            {
                Initializer.InitializeData(db);
                Assert.Equal(1, db.Orders.Count());
            }
        }
    }
}
