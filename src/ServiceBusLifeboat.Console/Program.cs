using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using ServiceBusLifeboat.Domain.Exceptions;
using ServiceBusLifeboat.Domain.Extensions;
using static System.Console;

namespace ServiceBusLifeboat.Console;

internal class Program
{
    private static string _namespaceConnectionString; 
    private static ServiceBusAdministrationClient _adminClient;
    private static ServiceBusClient _client;
    private static List<QueueProperties> _inMemoryQueues;
    private static ServiceBusSender _queueSender;
    private static List<ulong> _selectedSequenceNumbersInMemory;
    private static List<ulong> _sequenceNumbersFail;

    public Program()
    {
        WriteLine("[Namespace] Connection String:");

        _namespaceConnectionString = ReadLine() ?? throw new ArgumentException("value cannot be null");

        TryConnectToServiceBus();
    }

    public static async Task Main(string[] args)
    {
        _selectedSequenceNumbersInMemory = new List<ulong>();
        _sequenceNumbersFail = new List<ulong>();
        bool isEndExecution;
        do
        {
            ShowNamespaceQueuesSummary();
            SelectQueueToManage();

            bool isUnscheduleConfirmed;
            do
            {
                InsertSequencesNumber();
                ShowSelectedSequencesNumber();
                ShowConfirmationOptions();
                isUnscheduleConfirmed = Convert.ToBoolean(ReadLine());
            } while (!isUnscheduleConfirmed);

            await UnscheduleMessages();

            ShowFailedMessages();

            ShowEndExecutionOptions();
            isEndExecution =  Convert.ToBoolean(ReadLine());
        } while (isEndExecution);
    }

    private static async Task UnscheduleMessages()
    {
        foreach (var sequenceNumber in _selectedSequenceNumbersInMemory)
        {
            WriteLine($"Unscheduling {sequenceNumber}" );
            try
            {
                await _queueSender.CancelScheduledMessageAsync((long)sequenceNumber);
                WriteLine($"Message Unscheduled");
            }
            catch
            {
                WriteLine($"Fail to unschedule message");
                _sequenceNumbersFail.Add(sequenceNumber);
            }
        }
    }

    private static void InsertSequencesNumber()
    {
        WriteLine("Enter a comma-separated list of sequence numbers:");
        WriteLine("Ex: 127531,2375183,321753");
        var sequenceNumberInput = ReadLine() ?? throw new ArgumentException("value cannot be null");
        _selectedSequenceNumbersInMemory = sequenceNumberInput.ConvertToLongList();
    }

    private static void ShowConfirmationOptions()
    {
        WriteLine("0 - yes");
        WriteLine("1 - no");
    }

    private static void ShowSelectedSequencesNumber()
    {
        WriteLine("Confirm that you want to delete all these sequence numbers");
        foreach (var sequenceNumber in _selectedSequenceNumbersInMemory)
        {
            WriteLine($"{sequenceNumber}");
        }
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

        _queueSender = _client.CreateSender(_inMemoryQueues[selectedIndex].Name);
    }

    private static async void ShowNamespaceQueuesSummary()
    {
        var queueIndex = 0;
        var queues = _adminClient.GetQueuesAsync();

        await foreach (var queue in queues)
        {
            var queueName = queue.Name;

            var runtimeQueueSummary =  _adminClient.GetQueueRuntimePropertiesAsync(queueName).Result.Value;

            var totalActiveMessagesCount = runtimeQueueSummary.ActiveMessageCount;
            var totalScheduledMessagesCount = runtimeQueueSummary.ScheduledMessageCount;
            var totalDeadLetterMessagesCount = runtimeQueueSummary.DeadLetterMessageCount;

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
        catch
        {
            throw new InvalidConnectionStringException();
        }
    }

    private static void ShowFailedMessages()
    {
        WriteLine("The messages below could not be unscheduled");
        foreach (var sequenceNumber in _sequenceNumbersFail)
        {
            WriteLine($"{sequenceNumber}");
        }
    }

    private static void ShowEndExecutionOptions()
    {
        WriteLine("0 - exit");
        WriteLine("1 - back to menu");
    }
}