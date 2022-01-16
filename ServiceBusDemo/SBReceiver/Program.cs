using Azure.Messaging.ServiceBus;
using SBShared.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SBReceiver
{
    class Program
    {
        const string connectionCtring = "Endpoint=sb://azure-service-raju-001.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=iUATsENUm+/rUGFdIVWwGpP5bVARo68qXAmPwc0ICuQ=";
        const string queueName = "personqueue";
        static ServiceBusClient client;
        static ServiceBusProcessor processor;

        static async Task Main(string[] args)
        {
            client = new ServiceBusClient(connectionCtring);

            // create a processor that we can use to process the messages
            processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                await client.DisposeAsync();
                await processor.DisposeAsync();
            }
        }
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            var person = JsonSerializer.Deserialize<PersonModel>(body);
            Console.WriteLine($"First Name: {person.FirstName}, Last Name: {person.LastName}");

            // complete the message. messages is deleted from the subscription. 
            await args.CompleteMessageAsync(args.Message);
        }
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
