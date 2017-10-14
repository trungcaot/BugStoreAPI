
using BugStoreDAL.Repositories.Interfaces;
using BugStoreModels;
using BugStoreModels.ViewModels;
using BugStoreDAL.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using BugStoreDAL.Exceptions;

namespace BugStoreDAL.Repositories
{
    public class OrderRepositoty : IOrderRepository
    {
        private readonly StoreContext _db;
        private readonly DbSet<Order> _table;
        private readonly IProductRepositoty _productRepo;
        private readonly bool _dispose;
        public OrderRepositoty(IProductRepositoty productRepo, StoreContext db) 
            : this(productRepo, db, false)
        {
        }

        public OrderRepositoty(IProductRepositoty productRepo, StoreContext db, bool dispose)
        {
            _db = db;
            _table = _db.Orders;
            _productRepo = productRepo;
            _dispose = dispose;
        }

        public int Count => _table.Count();
        public Order Find(int id) => _table.Find(id);

        public IEnumerable<Order> GetAll()
            => _table.OrderByDescending(x => x.DateCreated);

        public Order Find(int customerId, int productId) => _table.FirstOrDefault(
            x => x.CustomerId == customerId && x.ProductId == productId);
        public Order FindUnTracked(int customerId, int productId) => _table.Where(
            x => x.CustomerId == customerId && x.ProductId == productId).AsNoTracking().FirstOrDefault();

        public OrderProductInfo GetOrder(
            int customerId, int productId)
        {
            return _db.ViewModels.FromSql($@"SELECT scr.Id,
            scr.CustomerId,
            scr.DateCreated,
            p.CurrentPrice * scr.Quantity AS LineItemTotal,
                scr.ProductId,
            scr.Quantity,
            scr.TimeStamp,
            p.CategoryId,
            c.Name AS CategoryName,
                p.CurrentPrice,
            p.Description,
            p.IsFeatured,
            p.ModelName,
            p.ModelNumber,
            p.ProductImage,
            p.UnitCost,
            p.UnitsInStock
                FROM Store.Orders scr
            INNER JOIN Store.Products p ON p.Id = scr.ProductId
            INNER JOIN Store.Categories c ON c.Id = p.CategoryId
            WHERE scr.CustomerId = {customerId}
            AND scr.ProductId = {productId}
            ").FirstOrDefault();
        }

        public IList<OrderProductInfo> GetOrders(int customerId)
        {
            return _db.ViewModels.FromSql($@"SELECT scr.Id,
       scr.CustomerId,
       scr.DateCreated,
       p.CurrentPrice * scr.Quantity AS LineItemTotal,
       scr.ProductId,
       scr.Quantity,
       scr.TimeStamp,
       p.CategoryId,
       c.Name AS CategoryName,
       p.CurrentPrice,
       p.Description,
       p.IsFeatured,
       p.ModelName,
       p.ModelNumber,
       p.ProductImage,
       p.UnitCost,
       p.UnitsInStock
FROM Store.Orders scr
INNER JOIN Store.Products p ON p.Id = scr.ProductId
INNER JOIN Store.Categories c ON c.Id = p.CategoryId
WHERE scr.CustomerId = {customerId}").ToList();
        }

        public int Delete(Order entity, bool persist = true)
        {
            _table.Remove(entity);
            return SaveChanges();
        }

        public int Delete(int id, byte[] timeStamp)
        {
            var entry = _db.ChangeTracker.Entries<Order>()
            .Select((EntityEntry e) => (Order)e.Entity)
            .FirstOrDefault(x => x.Id == id);
            if (entry != null)
            {
                if (timeStamp != null && entry.TimeStamp.SequenceEqual(timeStamp))
                {
                    return Delete(entry);
                }
                throw new Exception("Unable to delete due to concurrency violation.");
            }
            _db.Entry(new Order { Id = id, TimeStamp = timeStamp }).State = EntityState.Deleted;
            return SaveChanges();
        }

        public int Update(Order entity)
        {
            return Update(entity, _productRepo.Find(entity.ProductId)?.UnitsInStock);
        }

        public int Update(Order entity, int? quantityInStock)
        {
            if (entity.Quantity <= 0)
            {
                return Delete(entity.Id, entity.TimeStamp);
            }
            if (entity.Quantity > quantityInStock)
            {
                throw new InvalidQuantityException("Can't add more product than available in stock");
            }
            _db.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }

        public int Add(Order entity, bool persist = true)
        {
            return Add(entity, _productRepo.Find(entity.ProductId)?.UnitsInStock, persist);
        }

        public int Add(Order entity, int? quantityInStock, bool persist = true)
        {
            var item = Find(entity.CustomerId, entity.ProductId);
            if (item == null)
            {
                if (quantityInStock != null && entity.Quantity > quantityInStock.Value)
                {
                    throw new InvalidQuantityException("Can't add more product than available in stock");
                }
                _table.Add(entity);
                return SaveChanges();
            }
            item.Quantity += entity.Quantity;
            if (item.Quantity <= 0)
            {
                //entity.Id will still be zero
                return Delete(item, persist);
            }
            var recordsAffected = Update(item, quantityInStock);
            //Update the passed in entity to preserve the entity values on return
            entity.Quantity = item.Quantity;
            entity.Id = item.Id;
            entity.DateCreated = DateTime.Now;
            entity.TimeStamp = item.TimeStamp;
            return recordsAffected;
        }

        public int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //Thrown when there is a concurrency errors
                //If Entries propery is null, no records were modified
                //entities in Entries threw error due to timestamp/conncurrency
                //for now, just rethrow the exception
                throw;
            }
            catch (DbUpdateException ex)
            {
                //Thrown when database update fails
                //Examine the inner execption(s) for additional 
                //details and affected objects
                //for now, just rethrow the exception
                throw;
            }
            catch (Exception ex)
            {
                //some other exception happened and should be handled
                throw;
            }
        }

        public void Dispose()
        {
            if (_dispose)
            {
                _db?.Dispose();
            }
        }
    }
}
