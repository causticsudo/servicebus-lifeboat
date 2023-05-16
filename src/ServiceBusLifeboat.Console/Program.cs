using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using ServiceBusLifeboat.Domain.Exceptions;
using static System.Console;

namespace ServiceBusLifeboat.Console;

internal class Program
{
    private static string _namespaceConnectionString; 
    private static ServiceBusAdministrationClient _adminClient;
    private static ServiceBusClient _client;
    private static List<QueueProperties> _inMemoryQueues;
    private static QueueProperties _selectedQueueInMemory;

    public static Task Main(string[] args)
    {
        WriteLine("[Namespace] Connection String:");

        _namespaceConnectionString = ReadLine() ?? throw new ArgumentException("value cannot be null");

        TryConnectToServiceBus();

        ShowNamespaceQueuesSummary();

        SelectQueueToManage();

//IEnumerable<ServiceBusReceivedMe// foreach (ServiceBusReceivedMessage message in messages)
        // {
        //     Console.WriteLine($"Removendo {message.SequenceNumber}" ); 
        //      await sender.CancelScheduledMessageAsync(message.SequenceNumber);
        //     Console.WriteLine($"removido");
        // }ssage> messages = await receiver.PeekMessagesAsync(maxMessages, maxWaitTime);

        // var sender = _client.CreateSender(selectedIndex);
        //
        // await sender.CancelScheduledMessageAsync(16231814);

        return Task.CompletedTask;
    }

    private static void SelectQueueToManage()
    {
        var inMemoryQueuesCount = _inMemoryQueues.Count;

        WriteLine("Select Queue:");
        var selectedIndex = Convert.ToInt32(ReadLine());

        if (selectedIndex > inMemoryQueuesCount)
        {
            throw new ArgumentOutOfRangeException($"Invalid queue range, use a value between (0-{inMemoryQueuesCount--})");
        }

        _selectedQueueInMemory = _inMemoryQueues[selectedIndex];
    }

    private async static void ShowNamespaceQueuesSummary()
    {
        int queueIndex = 0;
        var queues = _adminClient.GetQueuesAsync();

        await foreach (QueueProperties queue in queues)
        {
            var queueName = queue.Name;

            var runtimeQueueSumary =  _adminClient.GetQueueRuntimePropertiesAsync(queueName).Result.Value;

            var totalActiveMessagesCount = runtimeQueueSumary.ActiveMessageCount;
            var totalScheduledMessagesCount = runtimeQueueSumary.ScheduledMessageCount;
            var totalDeadLetterMessagesCount = runtimeQueueSumary.DeadLetterMessageCount;

            WriteLine($"({queueIndex}){queue.Name} - | A({totalActiveMessagesCount}) | S({totalScheduledMessagesCount} | DLQ({totalDeadLetterMessagesCount})) \n\n");

            queueIndex++;
        }

        _inMemoryQueues =  queues.ToBlockingEnumerable().ToList();
    }

    private static void TryConnectToServiceBus()
    {
        try
        {
            _adminClient = new(_namespaceConnectionString);
            _client = new(_namespaceConnectionString);
            _inMemoryQueues = null;

            WriteLine("Connected !");
        }
        catch (Exception e)
        {
            throw new InvalidConnectionStringException();
        }
    }
}