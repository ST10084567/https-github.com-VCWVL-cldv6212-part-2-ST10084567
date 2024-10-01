using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class CreateJacket
    {
        private readonly ILogger<CreateJacket> _logger;

        public CreateJacket(ILogger<CreateJacket> logger)
        {
            _logger = logger;
        }

        [Function(nameof(CreateJacket))]
        public async Task Run(
            [ServiceBusTrigger("myqueue", Connection = "")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
