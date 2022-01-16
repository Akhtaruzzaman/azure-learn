using Azure.Core;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SBSender.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration configuration;

        public QueueService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendMessageAsync<T>(T serviceBusMessage, string queueName)
        {
            ServiceBusClient queueClient = new ServiceBusClient(configuration.GetConnectionString("AzureServiceBus"));
            ServiceBusSender serviceBusSender = queueClient.CreateSender(queueName);


            string messageBody = JsonSerializer.Serialize(serviceBusMessage);
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));
            await serviceBusSender.SendMessageAsync(message);
            await serviceBusSender.DisposeAsync();
            await queueClient.DisposeAsync();

            //ServiceBusMessageBatch messageBatch = await serviceBusSender.CreateMessageBatchAsync();
            //for (int i = 0; i < 10; i++)
            //{
            //    messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}"));
            //}
            //try
            //{
            //    await serviceBusSender.SendMessagesAsync(messageBatch);
            //}
            //finally
            //{
            //    await serviceBusSender.DisposeAsync();
            //    await queueClient.DisposeAsync();
            //}
        }
    }
}
