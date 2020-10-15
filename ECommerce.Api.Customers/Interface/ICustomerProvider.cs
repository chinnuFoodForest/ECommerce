using ECommerce.Api.Customers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Interface
{
   public interface ICustomerProvider
    {
        Task<(bool IsSuccess, IEnumerable<Customer> Customers, string ErrorMessage)> GetCustomersAsc();
        Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> GetCustomerAsc(int id);
    }
}
