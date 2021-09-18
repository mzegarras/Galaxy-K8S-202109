using System;
using Microsoft.Extensions.Options;
using CustomerApi.Config;
using CustomerApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace CustomerApi.Service
{
    public interface IBusService{

         void Send<T>(T message);

         IConnection CreateConnection();
    }

    public class BusService : IBusService
    {
        private readonly IMqConfig _mqConfig;
         private readonly ILogger<CustomerService> _logger;
        private  IConnection _connection;

        public BusService(ILogger<CustomerService> logger,IMqConfig mqConfig){
            this._mqConfig = mqConfig;
            this._logger=logger;

            CreateConnection();
        }
         
         public IConnection CreateConnection()
        {

            this._logger.LogInformation("try connect:{0}/user:{1}/password:{2}",_mqConfig.HostName,
                                                                   _mqConfig.UserName,
                                                                   _mqConfig.Password);
            try
            {
                var factory = new ConnectionFactory() { HostName = _mqConfig.HostName, UserName = _mqConfig.UserName, Password = _mqConfig.UserName };
                _connection = factory.CreateConnection();
                return _connection; 
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateConnection",ex);
                throw;
            }
        }


        public void Send<T>(T message)
        {
            using (var channel = _connection.CreateModel()) {

                string jsonString = JsonSerializer.Serialize(message);
                   
                var body = Encoding.UTF8.GetBytes(jsonString);
                channel.BasicPublish(exchange: "customers",
                                    routingKey: "clientes.created.putted",
                                    basicProperties: null,
                                    body: body);

            }
                 
        }

        

    }
}