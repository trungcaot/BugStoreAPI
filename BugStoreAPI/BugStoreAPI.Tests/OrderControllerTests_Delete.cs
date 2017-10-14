using System.Net.Http;
using System.Text;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BugStoreModels;
using BugStoreModels.ViewModels;
using Xunit;

namespace BugStoreAPI.Tests
{
    public partial class OrderControllerTests 
    {
        [Fact]
        public async void ShouldDeleteRecordInTheCart()
        {
            var fullRecord = await GetCartRecord();
            var record = Mapper.Map<OrderProductInfo, Order>(fullRecord);
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(record);
                var timeStampString = JsonConvert.SerializeObject(record.TimeStamp).Replace("\"", "");
                var targetUrl = $"{ServiceAddress}{RootAddress}/delete/{record.Id}/{timeStampString}";
                var response = await client.DeleteAsync(targetUrl);
                Assert.True(response.IsSuccessStatusCode);
            }
            //Validate record was deleted
            var fullRecordUpdated = await GetCartRecord();
            Assert.Null(fullRecordUpdated);
        }

        [Fact]
        public async void ShouldNotDeleteMissingRecord()
        {
            var fullRecord = await GetCartRecord();
            var record = Mapper.Map<OrderProductInfo, Order>(fullRecord);
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(record);
                var timeStampString = JsonConvert.SerializeObject(record.TimeStamp).Replace("\"","");
                var targetUrl = $"{ServiceAddress}{RootAddress}/delete/100/{timeStampString}";
                var response = await client.DeleteAsync(targetUrl);
                Assert.False(response.IsSuccessStatusCode);
                var message = await response.Content.ReadAsStringAsync();
                dynamic messageObject = JObject.Parse(message);
                Assert.Equal("Concurency Issue", messageObject.Error.ToString());
            }
        }
    }
}
