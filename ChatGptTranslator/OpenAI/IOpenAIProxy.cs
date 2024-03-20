using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace ChatGptTranslator.OpenAI
{
    public interface IOpenAIProxy
    {
        Task<ChatCompletionMessage[]> SendChatMessage(string message);
    }
}
