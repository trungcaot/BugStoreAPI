using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BugStoreDAL.EF;
using BugStoreDAL.EF.Initializers;
using BugStoreModels;
using BugStoreModels.ViewModels;
using Xunit;
using BugStoreAPI.Tests.Base;

namespace BugStoreAPI.Tests
{
    [Collection("Service Testing")]
    public class OrderControllerNoUpdateTests : BaseTestClass
    {
        private int _customerId = 1;
        private int _productId = 7;
        private readonly StoreContext _db;

        public OrderControllerNoUpdateTests()
        {
            RootAddress = "order";
            _db = new StoreContext();
            Initializer.InitializeData(_db);
            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<Order, Order>()
                        .ForMember(x => x.Product, opt => opt.Ignore());
                });

        }

        public override void Dispose()
        {
            _db.Dispose();
        }

        [Fact]
        public async void ShouldGetAllCartRecords()
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync($"{ServiceAddress}{RootAddress}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var records = JsonConvert.DeserializeObject<List<Order>>(jsonResponse);
                Assert.Equal(1, records.Count);
            }
        }

        [Fact]
        public async void ShouldReturnCustomersCart()
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync($"{ServiceAddress}{RootAddress}/{_customerId}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var records = JsonConvert.DeserializeObject<List<OrderProductInfo>>(jsonResponse);
                Assert.Equal(1, records.Count);
                Assert.Equal(_productId, records[0].ProductId);
                Assert.Equal("Samsung", records[0].CategoryName);
            }
        }

        [Fact]
        public async void ShouldNotFailIfBadCustomerId()
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync($"{ServiceAddress}{RootAddress}/7");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async void ShouldReturnCustomersCartItemByProductId()
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync($"{ServiceAddress}{RootAddress}/{_customerId}/{_productId}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var record = JsonConvert.DeserializeObject<OrderProductInfo>(jsonResponse);
                Assert.Equal(_productId, record.ProductId);
                Assert.Equal("Samsung", record.CategoryName);
            }
        }

        [Fact]
        public async void ShouldReturn404IfCustomersItemByProductIdNotFound()
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync($"{ServiceAddress}{RootAddress}/{_customerId}/99");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async void ShouldNotUpdateCartRecordOnAddIfNotEnoughInventory()
        {
            // Add to Cart: http://localhost:40001/api/shoppingcart/{customerId} HTTPPost
            // Content - Type:application / json
            using (var client = new HttpClient())
            {
                var record = _db.Orders.First();
                record.Quantity = 30;

                var json = JsonConvert.SerializeObject(
                    Mapper.Map<Order, Order>(record));
                var targetUrl = $"{ServiceAddress}{RootAddress}/{record.Id}";
                var response = await client.PutAsync(targetUrl,new StringContent(json, Encoding.UTF8, "application/json"));
                Assert.False(response.IsSuccessStatusCode);
                var message =await response.Content.ReadAsStringAsync();
                dynamic messageObject = JObject.Parse(message);
                Assert.Equal("Invalid quantity request.", messageObject.Error.ToString());
                Assert.Equal("Can't add more product than available in stock", messageObject.Message.ToString());
                Assert.True(!string.IsNullOrEmpty(messageObject.StackTrace.ToString()));
            }
        }

        [Fact]
        public async void ShouldNotAddRecordToTheCartIfNotEnoughStock()
        {
            using (var client = new HttpClient())
            {
                var record = new Order
                {
                    CustomerId = _customerId,
                    DateCreated = DateTime.Now,
                    ProductId = 2,
                    Quantity = 20
                };
                var json = JsonConvert.SerializeObject(record);
                var targetUrl = $"{ServiceAddress}{RootAddress}/";
                var response = await client.PostAsync(targetUrl,new StringContent(json, Encoding.UTF8, "application/json"));
                Assert.False(response.IsSuccessStatusCode);
                var message = await response.Content.ReadAsStringAsync();
                dynamic messageObject = JObject.Parse(message);
                Assert.Equal("Invalid quantity request.", messageObject.Error.ToString());
                Assert.Equal("Can't add more product than available in stock", messageObject.Message.ToString());
                Assert.True(!string.IsNullOrEmpty(messageObject.StackTrace.ToString()));
            }
        }
    }
}