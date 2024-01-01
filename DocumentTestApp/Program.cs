using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using cloudinteractive.document;
using cloudinteractive.document.Util;
using OpenAI_API.Models;

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
            cloudinteractive.document.Util.OpenAI.Init(_openAIKey);
            Util.ConsolePrint(Util.PrintType.Info, "Microsoft Azure ComputerVision Service Init..");

            var usingPdf = Util.ConsoleInputBool("Do you want to using PDF file?");

            string location;
            IExportableDocument document;

            if (usingPdf)
            {
                //대상 파일과 페이지 받아오기
                location = Util.ConsoleInput("Enter a PDF document file location");
                int[]? pages = null;
                if (Util.ConsoleInputBool("Do you want to specify particular pages from the provided document"))
                {
                    try
                    {
                        string input = Util.ConsoleInput("Enter pages separated by spaces");
                        pages = Array.ConvertAll(input.Split(' '), int.Parse);
                    }
                    catch
                    {
                        Util.ConsolePrint(Util.PrintType.Error, "Exception on parse page numbers.");
                        Util.ConsoleExit();
                    }
                }
                Util.ConsolePrint(Util.PrintType.Info, "Loading document file...");
                document = PdfDocument.ImportFromFile(location, pages).Result;

            }
            else
            {
                location = Util.ConsoleInput("Enter a image file location");

                Util.ConsolePrint(Util.PrintType.Info, "Loading document file...");
                document = ImageDocument.ImportFromFile(location).Result;
            }

            try
            {
                //문서 OCR
                Util.ConsolePrint(Util.PrintType.Info,
                    "Exporting text from document via Microsoft Azure Cognitive Services...");
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

                var prompt = Util.ConsoleInput("Enter a prompt for process the document via ChatGPT");
                Util.ConsolePrint(Util.PrintType.Info, "Processing the document with the presented prompt via ChatGPT : Waiting for OpenAI API response..");

                var response = OpenAI.GetChatCompletion(prompt, texts, Model.GPT4_Turbo).Result;
                Util.ConsolePrint(Util.PrintType.Info, "Complete!");
                Util.ConsolePrint(Util.PrintType.Info, "Response from ChatGPT: ");
                Console.WriteLine(response);
                Util.ConsoleExit();

            }
            catch (Exception e)
            {
                Util.ConsolePrint(Util.PrintType.Error, "Exception! : " + e.ToString());
                Util.ConsoleExit();
            }

        }
    }
}