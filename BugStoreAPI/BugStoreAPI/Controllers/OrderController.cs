using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BugStoreDAL.Repositories.Interfaces;
using AutoMapper;
using BugStoreModels;
using System.Net.Http;
using Newtonsoft.Json;

namespace BugStoreAPI.Controllers
{
    [Route("api/order")]
    [Produces("application/json")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<Order, Order>()
                        .ForMember(x => x.Product, opt => opt.Ignore());
                });

        }

        //public IEnumerable<Order> GetOrder()
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            var Orders = _orderRepository.GetAll().ToList();
            return Mapper.Map<List<Order>, List<Order>>(Orders);
        }

        // GET: api/Order/1
        [HttpGet("{customerId:int}", Name = "GetForCustomer")]
        public IActionResult GetAllOrdersForACustomer(int customerId)
        {
            var orderViewModel = _orderRepository.GetOrders(customerId);
            if (orderViewModel?.Count == 0)
            {
                return NotFound();
            }
            return Ok(orderViewModel);
        }
        // GET: api/Order/1/7
        [HttpGet("{customerId:int}/{productId:int}", Name = "GetOneForCustomerAndProduct")]
        public IActionResult GetOneOrder(int customerId, int productId)
        {
            var orderViewModel = _orderRepository.GetOrder(customerId, productId);
            if (orderViewModel == null)
            {
                return NotFound();
            }
            return Ok(orderViewModel);
        }

        // POST: api/Order - add
        [HttpPost]
        public IActionResult PostOrder([FromBody]Order Order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _orderRepository.Add(Order);
            if (Order.Id == 0 || Order.Quantity <= 0)
            {
                return CreatedAtRoute("GetForCustomer", new { customerId = Order.CustomerId }, new StringContent("Order was removed"));
            }
            return CreatedAtRoute("GetOneForCustomerAndProduct", new { customerId = Order.CustomerId, productId = Order.ProductId },
            Mapper.Map<Order, Order>(Order));
        }
        // PUT: api/Order/5 - update
        [HttpPut("{id:int}")]
        public IActionResult PutOrder(int id, [FromBody]Order Order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Order.Id)
            {
                return BadRequest();
            }
            _orderRepository.Update(Order);
            return NoContent();
        }


        // DELETE: api/Order/delete/5/timestampstring
        [HttpDelete("delete/{id}/{timeStampString}")]
        public IActionResult Delete(int id, string timeStampString)
        {
            var timeStamp = JsonConvert.DeserializeObject<byte[]>($"\"{timeStampString}\"");
            _orderRepository.Delete(id, timeStamp);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

    }
}
