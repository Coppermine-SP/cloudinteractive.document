using System;
using System.Text;
using System.Text.Json.Nodes;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace cloudinteractive.document.Util
{
    public static class OpenAI
    {
        private static string? _key;
        private static int? _maxLength;
        public static void Init(string key, ushort maxLength=2000)
        {
            _key = key;
            _maxLength = maxLength;
        }

        public static async Task<string> GetChatCompletionAsync(string prompt, string[] document, Model model)
        {
            if (_key == null) throw new InvalidOperationException("OpenAI module is not initialized.");
            var builder = new StringBuilder();
            foreach (string page in document)
            {
                if (builder.Length < _maxLength)
                {
                    builder.Append(page);
                }
            }

            ChatRequest request = new ChatRequest()
            {
                Model = model,
                Temperature = 0.0,
                ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
                Messages = new List<ChatMessage>()
                {
                    new ChatMessage(ChatMessageRole.System, prompt),
                    new ChatMessage(ChatMessageRole.User, builder.ToString())
                }
            };
            OpenAIAPI api = new OpenAIAPI(_key);

            var response = await api.Chat.CreateChatCompletionAsync(request);

            return response.ToString();
        }
    }
}
