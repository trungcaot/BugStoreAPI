using System;
using System.Collections.Generic;
using BugStoreModels;
using BugStoreModels.ViewModels;

namespace BugStoreDAL.Repositories.Interfaces
{
    public interface IOrderRepository : IDisposable
    {
        int Count { get; }
        Order Find(int id);
        IEnumerable<Order> GetAll();
        Order Find(int customerId, int productId);
        Order FindUnTracked(int customerId, int productId);

        OrderProductInfo GetOrder(
            int customerId, int productId);

        IList<OrderProductInfo> GetOrders(int customerId);
        int Delete(Order entity, bool persist = true);
        int Delete(int id, byte[] timeStamp);
        int Update(Order entity);
        int Update(Order entity, int? quantityInStock);
        int Add(Order entity, bool persist = true);
        int Add(Order entity, int? quantityInStock, bool persist = true);
    }
}
