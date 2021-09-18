using System;
using CustomerApi.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using CustomerApi.Repository;

namespace CustomerApi.Service
{

    public class CustomerService:ICustomerService{

        private readonly IBusService _busService;
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger,ICustomerRepository customerRepository,IBusService busService){
            
            _logger = logger;
            _repository = customerRepository;
            _busService = busService;

        }

        public Customer Create(Customer customer)
        {
            _repository.Create(customer);
            _busService.Send<Customer>(customer);
            return customer;
        }

        public List<Customer> ListAll() => _repository.ListAll();
        

        public Customer ListById(string id) => _repository.ListById(id);

        public void Remove(string id) => _repository.Remove(id);
    }
}