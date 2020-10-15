using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interface;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;
        public OrdersProvider(OrdersDbContext dbContext,ILogger<OrdersProvider> logger,IMapper mapper )
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if(!dbContext.Orders.Any())
            {
                dbContext.OrderItems.Add(new Db.OrderItem { Id=1, ProductId=1, Quatity =10, UnitPrice =10});
                dbContext.OrderItems.Add(new Db.OrderItem { Id=2, ProductId=5, Quatity =50, UnitPrice =80});
                dbContext.OrderItems.Add(new Db.OrderItem { Id=3, ProductId=8, Quatity =60, UnitPrice =90});
                dbContext.OrderItems.Add(new Db.OrderItem { Id=4, ProductId=10, Quatity=70, UnitPrice =100});
                dbContext.SaveChanges();

                dbContext.Orders.Add(new Db.Order { Id = 1, CustomerId = 1, OrderDate = System.DateTime.Now, Total = 100 });
                dbContext.Orders.Add(new Db.Order { Id = 2, CustomerId = 5, OrderDate = System.DateTime.Now, Total = 200 });
                dbContext.Orders.Add(new Db.Order { Id = 3, CustomerId = 6, OrderDate = System.DateTime.Now, Total = 300 });
                dbContext.Orders.Add(new Db.Order { Id = 4, CustomerId = 7, OrderDate = System.DateTime.Now, Total = 400 });
                dbContext.SaveChanges();

            }
        }
       
       public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsc(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders.ToListAsync();

                if (orders != null)
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>((IEnumerable<Db.Order>)orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
