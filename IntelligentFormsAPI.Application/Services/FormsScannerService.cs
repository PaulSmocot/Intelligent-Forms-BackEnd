using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using IntelligentFormsAPI.Application.Interfaces;

namespace IntelligentFormsAPI.Application.Services
{
    public class FormsScannerService : IFormsScannerService
    {
        private readonly string apiKey = "403bba90b1f84c31b9a8f585d67ea691";
        private readonly string endpoint = "https://formrecognizer137d.cognitiveservices.azure.com/";

        public async Task<Dictionary<string, string>> ScanIdentityCardAsync(string base64Image)
        {
            var client = new DocumentAnalysisClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            byte[] imageBytes = Convert.FromBase64String(base64Image);
            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-document", stream);

                var fieldMap = new Dictionary<string, string>();

                foreach (var kvp in operation.Value.KeyValuePairs)
                {
                    if (kvp.Key is not null && kvp.Value is not null)
                    {
                        try
                        {
                            if (kvp.Key.Content.Equals("Nume/Nom/Last name"))
                            {

                                var split = kvp.Value.Content.Split("\n");
                                fieldMap.TryAdd("CNP", split[0]);
                                fieldMap.TryAdd(kvp.Key.Content, split[1]);
                            }
                            else if (kvp.Key.Content.Equals("SERIA RB NR"))
                            {
                                var split = kvp.Key.Content.Split(" ");
                                fieldMap.TryAdd(split[0], split[1]);
                                fieldMap.TryAdd(split[2], kvp.Value.Content);
                            }
                            else
                            {
                                fieldMap.TryAdd(kvp.Key.Content, kvp.Value.Content);
                            }
                        }
                        catch
                        {
                            fieldMap.TryAdd(kvp.Key.Content, kvp.Value.Content);
                        }
                    }
                }
            

                return fieldMap;
            }
        }

        public async Task<Dictionary<string, string>> ScanPassportAsync(string base64Image)
        {
            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

            byte[] imageBytes = Convert.FromBase64String(base64Image);
            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-idDocument", stream);

                AnalyzeResult identityDocuments = operation.Value;

                AnalyzedDocument identityDocument = identityDocuments.Documents.Single();

                var fieldMap = new Dictionary<string, string>();

                foreach (var item in identityDocument.Fields)
                {
                    fieldMap.TryAdd(item.Key, item.Value.Content);
                }

                return fieldMap;
            }
        }

        public async Task<Dictionary<string, string>> ScanVehicleIdentityCardAsync(string base64Image)
        {
            var client = new DocumentAnalysisClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            byte[] imageBytes = Convert.FromBase64String(base64Image);
            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-document", stream);

                var fieldMap = new Dictionary<string, string>();

                foreach (var kvp in operation.Value.KeyValuePairs)
                {
                    if (kvp.Key is not null && kvp.Value is not null)
                    {
                        fieldMap.Add(kvp.Key.Content, kvp.Value.Content);
                    }
                }

                return fieldMap;
            }
        }

        public async Task<string> ScanAnyDocumentAsync(string base64Image)
        {
            var client = new DocumentAnalysisClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            byte[] imageBytes = Convert.FromBase64String(base64Image);
            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-document", stream);

                return operation.Value.Content;
            }
        }
    }
}
