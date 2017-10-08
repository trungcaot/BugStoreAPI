using BugStoreDAL.EF;
using BugStoreDAL.Repositories.Interfaces;
using BugStoreModels;
using Microsoft.EntityFrameworkCore;

namespace BugStoreDAL.Repositories
{
    public class ProductRepository : IProductRepositoty
    {
        private readonly DbSet<Product> _product;

        public ProductRepository(StoreContext db)
        {
            _product = db.Products;
        }

        public Product Find(int id)
        {
            return _product.Find(id);
        }
    }
}
