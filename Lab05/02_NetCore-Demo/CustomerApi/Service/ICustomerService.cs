using System;
using CustomerApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomerApi.Service
{
    public interface ICustomerService{
        List<Customer> ListAll();

        Customer ListById(string id);
        Customer Create(Customer customer);

         void Remove(string id);
    }
}