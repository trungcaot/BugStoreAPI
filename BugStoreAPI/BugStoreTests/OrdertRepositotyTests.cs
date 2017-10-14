using System;
using Microsoft.EntityFrameworkCore;
using BugStoreDAL.EF.Initializers;
using BugStoreDAL.EF;
using BugStoreDAL.Exceptions;
using BugStoreDAL.Repositories;
using BugStoreModels;
using Xunit;
using System.Linq;

namespace BugStoreTests
{
    public class OrderRepositotyTests : IDisposable
    {
        private readonly OrderRepositoty _orderRepository;
        private readonly StoreContext _db;

        public OrderRepositotyTests()
        {
            _db = new StoreContext();
            _orderRepository = new OrderRepositoty(new ProductRepository(_db), _db, true);
            Initializer.InitializeData(_db);
        }

        public void Dispose()
        {
            _orderRepository.Dispose();
            _db.Dispose();
        }

        [Fact]
        public void Should_Success_Get_Count_Orders()
        {
            Assert.Equal(1, _orderRepository.Count);
        }

        [Fact]
        public void Should_Success_Find_OrderById()
        {
            Assert.Equal(1, _orderRepository.Find(1).CustomerId);
        }

        [Fact]
        public void Should_Success_Find_order()
        {
            Assert.Equal(1, _orderRepository.Find(1, 7).CustomerId);
        }

        [Fact]
        public void Should_Success_GetAll()
        {
            Assert.Equal(1, _orderRepository.GetAll().ToList().Count);
        }
        [Fact]
        public void Should_Success_Delete_Order()
        {
            var item = _orderRepository.Find(1, 7);
            _orderRepository.Delete(item);
            Assert.Equal(0, _orderRepository.GetAll().Count());
        }
        [Fact]
        public void Should_Success_Delete_OrderByIdAndTS()
        {
            var item = _orderRepository.Find(1, 7);
            _orderRepository.Delete(item.Id, item.TimeStamp);
            Assert.Equal(0, _orderRepository.GetAll().ToList().Count());
        }

        [Fact]
        public void Should_Success_Delete_Order_ByIdAndTSNewContext()
        {
            var item = _orderRepository.FindUnTracked(1, 7);
            using (var db = new StoreContext())
            {
                var repo = new OrderRepositoty(new ProductRepository(db), db);
                repo.Delete(item.Id, item.TimeStamp);
                repo.Dispose();
            }
            Assert.Equal(0, _orderRepository.GetAll().Count());
        }


        [Fact]
        public void Should_Add_An_Item_To_Order()
        {
            var item = new Order()
            {
                ProductId = 2,
                Quantity = 3,
                DateCreated = DateTime.Now,
                CustomerId = 1
            };
            _orderRepository.Add(item);
            var Orders = _orderRepository.GetAll().ToList();
            Assert.Equal(2, Orders.Count);
            Assert.Equal(2, Orders[0].ProductId);
            Assert.Equal(3, Orders[0].Quantity);
        }

        [Fact]
        public void Should_Update_Quantity_On_Add_If_Already_In_Order()
        {
            var item = new Order()
            {
                ProductId = 7,
                Quantity = 1,
                DateCreated = DateTime.Now,
                CustomerId = 1
            };
            _orderRepository.Add(item);
            var Orders = _orderRepository.GetAll().ToList();
            Assert.Equal(1, Orders.Count);
            Assert.Equal(2, Orders[0].Quantity);
        }

        [Fact]
        public void Should_Delete_On_Add_If_Quantity_After_Add_Equals_Zero()
        {
            var item = new Order()
            {
                ProductId = 7,
                Quantity = -1,
                DateCreated = DateTime.Now,
                CustomerId = 1
            };
            _orderRepository.Add(item);
            Assert.Equal(0, _orderRepository.Count);
        }

        [Fact]
        public void Should_Delete_On_Add_If_Quantity_Less_Than_Zero()
        {
            var item = new Order()
            {
                ProductId = 7,
                Quantity = -10,
                DateCreated = DateTime.Now,
                CustomerId = 1
            };
            _orderRepository.Add(item);
            Assert.Equal(0, _orderRepository.Count);
        }

        [Fact]
        public void Should_Update_Quantity()
        {
            var item = _orderRepository.Find(1, 7);
            item.Quantity = 5;
            item.DateCreated = DateTime.Now;
            _orderRepository.Update(item);
            var Orders = _orderRepository.GetAll().ToList();
            Assert.Equal(1, Orders.Count);
            Assert.Equal(5, Orders[0].Quantity);
        }

        [Fact]
        public void Should_Delete_On_Update_If_Quantity_Equals_Zero()
        {
            var item = _orderRepository.Find(1, 7);
            item.Quantity = 0;
            item.DateCreated = DateTime.Now;
            _orderRepository.Update(item);
            Assert.Equal(0, _orderRepository.Count);
        }

        [Fact]
        public void Should_Delete_On_Update_If_Quantity_Less_Than_Zero()
        {
            var item = _orderRepository.Find(1, 7);
            item.Quantity = -10;
            item.DateCreated = DateTime.Now;
            _orderRepository.Update(item);
            Assert.Equal(0, _orderRepository.Count);
        }


        [Fact]
        public void Should_Not_Delete_Missing_Order()
        {
            var item = _orderRepository.Find(1, 7);
            Assert.Throws<DbUpdateConcurrencyException>(() => _orderRepository.Delete(200, item.TimeStamp));
        }

        [Fact]
        public void Should_Throw_When_Adding_ToMuch_Quantity()
        {
            _orderRepository.SaveChanges();
            var item = new Order()
            {
                ProductId = 7,
                Quantity = 500,
                DateCreated = DateTime.Now,
                CustomerId = 1
            };
            var ex = Assert.Throws<InvalidQuantityException>(() => _orderRepository.Update(item));
            Assert.Equal("Can't add more product than available in stock", ex.Message);
        }

        [Fact]
        public void Should_Throw_When_Updating_Too_Much_Quantity()
        {
            var item = _orderRepository.Find(1, 7);
            item.Quantity = 100;
            item.DateCreated = DateTime.Now;
            var ex = Assert.Throws<InvalidQuantityException>(() => _orderRepository.Update(item));
            Assert.Equal("Can't add more product than available in stock", ex.Message);
        }

        [Fact]
        public void Should_Get_Flattened_ViewModel_For_Oneorder()
        {
            var order = _orderRepository.GetOrder(1, 7);
            Assert.Equal(1, order.Id);
            Assert.Equal(1, order.CustomerId);
            Assert.Equal(7, order.ProductId);
            Assert.Equal(1, order.Quantity);
            Assert.Equal(2, order.CategoryId);
            Assert.Equal("Samsung", order.CategoryName);
            Assert.Equal(9999.99M, order.CurrentPrice);
            Assert.Equal(9999.99M, order.LineItemTotal);
            Assert.Equal(false, order.IsFeatured);
            Assert.Equal("Cloaking Device", order.ModelName);
            Assert.Equal("CITSME9", order.ModelNumber);
            Assert.Equal("product-image.png", order.ProductImage);
            Assert.Equal(9999.99M, order.UnitCost);
            Assert.Equal(5, order.UnitsInStock);
        }
        [Fact]
        public void ShouldGetFlattenedViewModelForCart()
        {
            var orders = _orderRepository.GetOrders(1).ToList();
            var order = orders[0];
            Assert.Equal(1, order.Id);
            Assert.Equal(1, order.CustomerId);
            Assert.Equal(7, order.ProductId);
            Assert.Equal(1, order.Quantity);
            Assert.Equal(2, order.CategoryId);
            Assert.Equal("Samsung", order.CategoryName);
            Assert.Equal(9999.99M, order.CurrentPrice);
            Assert.Equal(9999.99M, order.LineItemTotal);
            Assert.Equal(false, order.IsFeatured);
            Assert.Equal("Cloaking Device", order.ModelName);
            Assert.Equal("CITSME9", order.ModelNumber);
            Assert.Equal("product-image.png", order.ProductImage);
            Assert.Equal(9999.99M, order.UnitCost);
            Assert.Equal(5, order.UnitsInStock);
        }
    }
}
