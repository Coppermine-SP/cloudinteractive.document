# cloudinteractive.document
**cloudinteractive.document is a simple C# document (Image, PDF) OCR and ChatGPT integration library project.**

Export text from PDF documents via Microsoft Azure Cognitive Service and process text via OpenAI ChatGPT. 

(A C# fork of [cloudinteractive-ai-insights](https://github.com/Coppermine-SP/cloudinteractive-ai-insights))

### Table of Contents
- [Responsible use of Generative AI](#responsible-use-of-generative-ai)
- [How to Use](#how-to-use)
- [Demo Application](#demo-app)
- [Dependencies](#dependencies)
- [Showcase](#showcase)

## Responsible use of Generative AI

[책임감 있는 AI 사용이란 무엇입니까? (국문)](https://github.com/Coppermine-SP/Coppermine-SP/blob/main/ResponsibleUseOfAI_KR.md)

**WARNING:**
This project should only be used as an auxiliary tool. Generative AI is not a solution provider for your assignment. Relying on this tool to fully complete your assignments is a clear act of cheating. 

**Please agree to the responsible use of Generative AI before utilizing this tool.**

## How to Use
**Currently, this library only works in Windows.** 
> System.Drawing is not supported in non-Windows platforms.
> 
> Alternatively, you can change project target framework to .NET 6 and enable System.Drawing support for non-Windows platforms by setting `System.Drawing.EnableUnixSupport` runtime configuration switch to `true` in the runtimeconfig.json file.
> 
> It is untested and not recommended.
>
> For more information, please check out this [Microsoft documentation](https://aka.ms/systemdrawingnonwindows).

## Dependencies
* [Ghostscript.NET](https://www.nuget.org/packages/Ghostscript.NET/1.2.3.1?_src=template) - AGPL 3.0 License
* [Microsoft.Azure.CongitiveServices.Vision.ComputerVision](https://www.nuget.org/packages/Microsoft.Azure.CognitiveServices.Vision.ComputerVision/7.0.1?_src=template) - MIT License
* [OpenAI](https://www.nuget.org/packages/OpenAI/1.10.0?_src=template) - CC0-1.0 License
* [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common/8.0.0?_src=template) - MIT License
