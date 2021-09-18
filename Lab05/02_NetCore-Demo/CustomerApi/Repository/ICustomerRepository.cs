
using CustomerApi.Models;
using System.Collections.Generic;
namespace CustomerApi.Repository
{
    public interface ICustomerRepository{
        Customer Create(Customer customer);
        List<Customer> ListAll();

        Customer ListById(string id) ;

        void Remove(string id);
    }
    
}