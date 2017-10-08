using BugStoreModels;

namespace BugStoreDAL.Repositories.Interfaces
{
    public interface IProductRepositoty
    {
        Product Find(int id);
    }
}
