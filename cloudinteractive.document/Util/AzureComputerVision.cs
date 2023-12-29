using System;
using System.Net.Security;
using System.Text;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json.Linq;

namespace cloudinteractive.document.Util
{
    public static class AzureComputerVision
    {
        private static string? _endpoint;
        private static string? _key;
        private static bool _isInited = false;

        public static void Init(string endpoint, string key)
        {
            if (_isInited) return;
            _endpoint = endpoint;
            _key = key;
            _isInited = true;
        }

        public static async Task<string[]> ExportTextFromDocument(IExportableDocument document)
        {
            using var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_key))
            {
                Endpoint = _endpoint
            };

            List<string> texts = new List<string>();
            foreach (var image in document.Images)
            {
                using var stream = new MemoryStream(image);
                var header = await client.ReadInStreamAsync(stream);
                var operationLocation = header.OperationLocation;
                Thread.Sleep(2000);

                const int numberOfCharsInOperationId = 36;
                string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);
                ReadOperationResult results;

                do
                {
                    results = await client.GetReadResultAsync(Guid.Parse(operationId));
                } while ((results.Status == OperationStatusCodes.Running ||
                          results.Status == OperationStatusCodes.NotStarted));

                var textResults = results.AnalyzeResult.ReadResults;
                var builder = new StringBuilder();

                foreach (var page in textResults)
                {
                    foreach (var line in page.Lines)
                    {
                        builder.AppendLine(line.Text);
                    }
                }
                texts.Add(builder.ToString());
            }

            return texts.ToArray();
        }

    }
}
