using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class UploadBlobFunction
    {
        private readonly ILogger<UploadBlobFunction> _logger;

        public UploadBlobFunction(ILogger<UploadBlobFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(UploadBlobFunction))]
        public async Task Run([BlobTrigger("samples-workitems/{name}", Source = BlobTriggerSource.EventGrid, Connection = "")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob Trigger (using Event Grid) processed blob\n Name: {name} \n Data: {content}");
        }
    }
}
