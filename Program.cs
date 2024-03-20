
using ConsoleAppOpenAI;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace ChatGptDemo
{
    internal class Program
    {        
        static async Task Main(string[] args)
        {

            var msg = "what is factorial of number 10";


            var apiKey = "";
            OpenAIProxy chatOpenAI = new OpenAIProxy(apiKey);

            for (int i = 0; i < 3; i++)
            {
                chatOpenAI.StackMessages(new Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions.ChatCompletionMessage
                {
                    Content = msg,
                    Role = "user"
                });
            }
            var results = await chatOpenAI.SendChatMessage(msg);

            foreach (var item in results)
            {
                Console.WriteLine($"{item.Role}: {item.Content}");
            }

        }
    }
}