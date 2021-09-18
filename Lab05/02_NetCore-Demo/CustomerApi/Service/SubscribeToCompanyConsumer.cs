using System;
using CustomerApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerApi.Service
{
    public class SubscribeToCompanyConsumer : BackgroundService
    {
        private readonly IBusService _busService;
        private readonly ILogger<SubscribeToCompanyConsumer> _logger;

        public SubscribeToCompanyConsumer(IBusService busService,ILogger<SubscribeToCompanyConsumer> logger){
            this._busService=busService;
            this._logger=logger;
            

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
           stoppingToken.ThrowIfCancellationRequested();
           
            _logger.LogInformation("CreateConsumer");
            IConnection _connection= this._busService.CreateConnection();
            var channel = _connection.CreateModel();
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (ch, message) =>
            {
                var body = message.Body.ToArray();
                // copy or deserialise the payload
                // and process the message
                // ...
                _logger.LogInformation("Processing");
                _logger.LogInformation("Delivery: " + message.DeliveryTag);
                await Task.Yield();
            };

            channel.BasicConsume("customers.crear", true, consumer);
            
           return Task.CompletedTask;
        }
    }
}


            
           