using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using cloudinteractive.document;
namespace DocumentTestApp
{
    internal class Program
    {
        private static string _azureEndpoint;
        private static string _azureKey;
        private static string _openAIKey;

        private static string _secureEndpoint = "https://secure.cloudint.corp";
        private static string _azureEndpointPath = "endpoint/azure_cv";
        private static string _azureKeyPath = "key/azure_cv";
        private static string _openAIKeyPath = "key/openai";



        static void Main(string[] args)
        {
            Console.WriteLine("cloudinteractive.document - Test application\nCopyright (C) 2024 CloudInteractive Inc.\n\n");

            //자격 증명 받아오기
            bool usingAutoCredential =
                Util.ConsoleInputBool("Do you want to get credentials automatically via CloudInteractive Credential Server?");

            if (usingAutoCredential)
            {
                Util.ConsolePrint(Util.PrintType.Info, $"Get Credentials from {_secureEndpoint}...");
                try
                {
                    _azureEndpoint = Util.HttpGet(_secureEndpoint, _azureEndpointPath);
                    _azureKey = Util.HttpGet(_secureEndpoint, _azureKeyPath);
                    _openAIKey = Util.HttpGet(_secureEndpoint, _openAIKeyPath);
                    Util.ConsolePrint(Util.PrintType.Info, "Successfully got credentials from server.");
                }
                catch (Exception e)
                {
                    Util.ConsolePrint(Util.PrintType.Error, "Failed get credentials from server:\n" + e.ToString());
                    Util.ConsoleExit();
                }
            }
            else
            {
                _azureEndpoint = Util.ConsoleInput("Enter your Microsoft Azure Cognitive Services Endpoint URL");
                _azureKey = Util.ConsoleInput("Enter your Microsoft Azure Cognitive Services Key");
                _openAIKey = Util.ConsoleInput("Enter your OpenAI API Key");
            }

            //Azure Init
            cloudinteractive.document.Util.AzureComputerVision.Init(_azureEndpoint, _azureKey);
            Util.ConsolePrint(Util.PrintType.Info, "Microsoft Azure ComputerVision Service Init..");

            //대상 파일과 페이지 받아오기
            var location = Util.ConsoleInput("Enter a PDF document file location");
            int[]? pages = null;
            if (Util.ConsoleInputBool("Do you want to specify particular pages from the provided document"))
            {
                try
                {
                    string input = Util.ConsoleInput("Enter page numbers separated by spaces");
                    pages = Array.ConvertAll(input.Split(' '), int.Parse);
                }
                catch
                {
                    Util.ConsolePrint(Util.PrintType.Error, "Exception on parse page numbers.");
                    Util.ConsoleExit();
                }
            }

            //페이지 로드
            Util.ConsolePrint(Util.PrintType.Info, "Loading document file...");
            var document =
                PdfDocument.ImportFromFile(location, pages).Result;

            Util.ConsolePrint(Util.PrintType.Info, "Exporting text from document via Microsoft Azure Cognitive Services...");
            var texts = cloudinteractive.document.Util.AzureComputerVision.ExportTextFromDocument(document).Result;

            Util.ConsolePrint(Util.PrintType.Info, "Complete!");

            //OCR 결과 출력
            int idx = 0;
            foreach (string text in texts)
            {
                Util.ConsolePrint(Util.PrintType.Info, $"Page {idx}:");
                Console.WriteLine(text);
                idx++;
            }
            
        }
    }
}