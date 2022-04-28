using OpenTriviaSharp;
using OpenTriviaSharp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenTriviaDatabaseExtractor
{
    public class TriviaService
    {
        private OpenTriviaClient client;
        private string token;
        private TriviaQuestion[] questions;

        public TriviaService()
        {
            client = new OpenTriviaClient();
        }

        public async void AsyncClient()
        {
            client = await InitializeClient();
        }

        private async Task<OpenTriviaClient> InitializeClient()
        {
            var client = new OpenTriviaClient();

            token = await client.RequestTokenAsync();

            await client.ResetTokenAsync(token);

            return client;
        }

        public async Task<TriviaQuestion[]> LoadQuestions(Category category)
        {
            var amount = await client.CountQuestionAsync(category);

            token = await client.RequestTokenAsync();

            questions = await client.GetQuestionAsync(
                amount: (byte)amount,
                type: TriviaType.Any,
                difficulty: Difficulty.Any,
                category: category,
                sessionToken: token);

            return questions;
        }

        public async Task<int> GetCategoryCount(Category category)
        {
            var count = await client.CountQuestionAsync(category);

            return (int)count;
        }

        public List<TriviaQuestion> FilterQuestionsByType(TriviaQuestion[] questions,TriviaType triviaType = TriviaType.Multiple)
        {
            return questions.Where(q => q.Type == triviaType).ToList();
        }
    }
}
