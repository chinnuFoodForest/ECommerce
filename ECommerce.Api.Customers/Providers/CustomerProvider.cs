using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interface;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomerProvider : ICustomerProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomerProvider> logger;
        private readonly IMapper mapper;

        public CustomerProvider(CustomersDbContext dbContext, ILogger<CustomerProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }
        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer { Id = 1, Name = "Chinnu", Address = "Punnavelil" });
                dbContext.Customers.Add(new Db.Customer { Id = 2, Name = "Blessen", Address = "Vanchikkapuzha" });
                dbContext.Customers.Add(new Db.Customer { Id = 3, Name = "Meenu", Address = "yz" });
                dbContext.Customers.Add(new Db.Customer { Id = 4, Name = "Thankam", Address = "abcd" });
                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers,
            string ErrorMessage)> GetCustomersAsc()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();
                if (customers != null & customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsc(int id)
        {
            try
            {
                var customerrepo = await dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);

                if (customerrepo != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customerrepo);
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
