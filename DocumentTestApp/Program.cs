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

            cloudinteractive.document.Util.AzureComputerVision.Init(_azureEndpoint, _azureKey);
            Util.ConsolePrint(Util.PrintType.Info, "Microsoft Azure ComputerVision Service Init..");

            var document =
                //PdfDocument.ImportFromFile("C:\\Users\\Coppermine\\Downloads\\공군 전문특기병(IT개발관리병) 지원자격 및 선발기준.pdf").Result;
                //PdfDocument.ImportFromFile("X:\\Personal\\창원대학교\\2023\\2023 정보통신공학개론\\2020 기말고사 범위 내용 요약.pdf", new int[]{3}).Result;
                PdfDocument.ImportFromFile("X:\\ePub\\으뜸 파이썬.pdf", new int[] { 204 }).Result;
                //PdfDocument.ImportFromFile("X:\\ePub\\스튜어트 미분적분학 9E.pdf", new int[] { 455 }).Result;

            var texts = cloudinteractive.document.Util.AzureComputerVision.ExportTextFromDocument<PdfDocument>(document).Result;
            foreach (string text in texts)
            {
                Console.WriteLine(text);
            }
            
        }
    }
}