using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using BugStoreModels;
using BugStoreModels.ViewModels;
using Xunit;

namespace BugStoreAPI.Tests
{
    public partial class OrderControllerTests
    {
        private async Task<OrderProductInfo> GetCartRecord()
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync($"{ServiceAddress}{RootAddress}/{_customerId}/{_productId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OrderProductInfo>(jsonResponse);
            }
        }

        [Fact]
        public async void ShouldUpdateRecordInTheCart()
        {
            var fullRecord = await GetCartRecord();
            var record = Mapper.Map<OrderProductInfo, Order>(fullRecord);
            using (var client = new HttpClient())
            {
                record.Quantity = 2;
                var json = JsonConvert.SerializeObject(record);
                var targetUrl = $"{ServiceAddress}{RootAddress}/{record.Id}";
                var response = await client.PutAsync(targetUrl,
                    new StringContent(json, Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode);
            }
            //Validate record was added
            var fullRecordUpdated = await GetCartRecord();
            var recordUpdated = Mapper.Map<OrderProductInfo, Order>(fullRecordUpdated);
            Assert.Equal(_productId, recordUpdated.ProductId);
            Assert.Equal(2, recordUpdated.Quantity);
        }

        [Fact]
        public async void ShouldRemoveRecordOnUpdateIfQuantityBecomesZero()
        {
            var fullRecord = await GetCartRecord();
            var record = Mapper.Map<OrderProductInfo, Order>(fullRecord);
            using (var client = new HttpClient())
            {
                record.Quantity = 0;
                var json = JsonConvert.SerializeObject(record);
                var targetUrl = $"{ServiceAddress}{RootAddress}/{record.Id}";
                var response = await client.PutAsync(targetUrl,
                    new StringContent(json, Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode);
            }
            //Validate record was deleted
            var fullRecordUpdated = await GetCartRecord();
            Assert.Null(fullRecordUpdated);
        }

        [Fact]
        public async void ShouldRemoveRecordOnUpdateIfQuantityBecomesLessThanZero()
        {
            var fullRecord = await GetCartRecord();
            var record = Mapper.Map<OrderProductInfo, Order>(fullRecord);
            using (var client = new HttpClient())
            {
                record.Quantity = -1;
                var json = JsonConvert.SerializeObject(record);
                var targetUrl = $"{ServiceAddress}{RootAddress}/{record.Id}";
                var response = await client.PutAsync(targetUrl,
                    new StringContent(json, Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode);
            }
            //Validate record was deleted
            var fullRecordUpdated = await GetCartRecord();
            Assert.Null(fullRecordUpdated);
        }
    }
}