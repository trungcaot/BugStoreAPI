using AutoMapper;
using BugStoreAPI.Tests.Base;
using BugStoreDAL.EF;
using BugStoreDAL.EF.Initializers;
using BugStoreModels;
using BugStoreModels.ViewModels;
using Xunit;

namespace BugStoreAPI.Tests
{
    [Collection("Service Testing")]
    public partial class OrderControllerTests : BaseTestClass
    {
        private int _customerId = 1;
        private int _productId = 7;
        private readonly StoreContext _db;
        public OrderControllerTests()
        {
            RootAddress = "order";
            _db = new StoreContext();
            Initializer.InitializeData(_db);
            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<OrderProductInfo, Order>();
                });
        }

        public override void Dispose()
        {
            _db.Dispose();
        }


    }
}
