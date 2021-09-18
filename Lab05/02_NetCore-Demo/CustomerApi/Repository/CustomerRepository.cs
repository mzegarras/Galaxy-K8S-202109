using System;
using Microsoft.Extensions.Options;
using CustomerApi.Config;
using CustomerApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
namespace CustomerApi.Repository
{

    public class CustomerRepository : ICustomerRepository
    {
    
     private readonly IMongoCollection<Models.Customer> _dbContext;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(ILogger<CustomerRepository> logger,IMongoConfig settings){
            
            _logger=logger;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _dbContext = database.GetCollection<Customer>(settings.CollectionName);
        }

        Customer ICustomerRepository.Create(Customer customer)
        {
            _logger.LogInformation("Create");
             _dbContext.InsertOne(customer);
             return customer;
        }

        List<Customer> ICustomerRepository.ListAll() =>_dbContext.Find(Customer => true).ToList();
      

        Customer ICustomerRepository.ListById(string id)=>_dbContext.Find<Customer>(customer => customer.Id == id).FirstOrDefault();
       

        void ICustomerRepository.Remove(string id)=>_dbContext.DeleteOne(book => book.Id == id);
        
    }
}