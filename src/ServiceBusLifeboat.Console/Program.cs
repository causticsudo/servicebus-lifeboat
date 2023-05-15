using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace ServiceBusPoc;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("[Namespace] Connection String:"); 
        var namespaceConnectionString = "Endpoint=sb://sb-credit-alias-staging.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=teX9nL0aIm/Ee+GpDuz/qV/yquZw18VI4gT5zGfbXT4=";

        var adminClient = new ServiceBusAdministrationClient(namespaceConnectionString);
        var client = new ServiceBusClient(namespaceConnectionString);
       

        
        Console.WriteLine("Queue Name:"); 
        var queueName = "credit-installment-commands-queue";

        var queue = await adminClient.GetQueueRuntimePropertiesAsync(queueName);

        Console.WriteLine(
            $"{queue.Value.Name} | total({queue.Value.TotalMessageCount}) | a({queue.Value.ActiveMessageCount}) | sch({queue.Value.ScheduledMessageCount} | dlq({queue.Value.DeadLetterMessageCount})) \n\n");
        
        
        ServiceBusReceiver receiver = client.CreateReceiver(queueName);
        int maxMessages = 10; // Número máximo de mensagens para receber
        var maxWaitTime = 5000; // Tempo máximo de espera para receber mensagens

        //IEnumerable<ServiceBusReceivedMessage> messages = await receiver.PeekMessagesAsync(maxMessages, maxWaitTime);
        
        var sender = client.CreateSender(queueName);
        await sender.CancelScheduledMessageAsync(16231814);
        
        // foreach (ServiceBusReceivedMessage message in messages)
        // {
        //     Console.WriteLine($"Removendo {message.SequenceNumber}" ); 
        //      await sender.CancelScheduledMessageAsync(message.SequenceNumber);
        //     Console.WriteLine($"removido");
        // }
    }
}