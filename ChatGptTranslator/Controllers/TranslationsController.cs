using ChatGptTranslator.Models;
using ChatGptTranslator.OpenAI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatGptTranslator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationsController : ControllerBase
    {

        public TranslationsController()
        {
        }

        [HttpPost("/Translate")]
        public async Task<ActionResult<TranslationResponse>> Translate([FromBody] TranslationRequest request)
        {
            var msg = $"Hey ChatGPT, please, give me just the translation of the following text into {request.Language}: \n{request.Query}";


            var apiKey = "";
            OpenAIProxy chatOpenAI = new OpenAIProxy(apiKey);

            chatOpenAI.StackMessages(new Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions.ChatCompletionMessage
            {
                Content = msg,
                Role = "user"
            });
            var results = await chatOpenAI.SendChatMessage(msg);

            return new  TranslationResponse() { Result = results[0].Content };
        }

    }
}
