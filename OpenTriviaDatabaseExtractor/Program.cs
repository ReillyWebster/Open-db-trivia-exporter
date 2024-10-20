using OpenTriviaSharp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OpenTriviaDatabaseExtractor
{
    internal class Program
    {
        static TriviaService service;
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            service = new TriviaService();

            await CreateJsonFiles();
        }

        private async static Task CreateJsonFiles()
        {
            string exportDirectory = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\OpenTriviaDatabaseExports\";
            if (!Directory.Exists(exportDirectory))
            {
                Directory.CreateDirectory(exportDirectory);
            }

            var categories = Enum.GetValues<Category>();
            foreach(Category category in categories)
            {
                if(category != Category.Any)
                {
                    Console.WriteLine($"Grabbing {category} questions...");
                    var categoryExportDirectory = $@"{exportDirectory}{category}Questions.json";
                    var questions = await GrabAllQuestions(category);
                    CscExporter.SaveFile(questions, exportDirectory);
                    JsonExporter.SaveJsonFile(questions, categoryExportDirectory);
                    Console.WriteLine($"{category} questions saved.");
                }
            }

            Console.WriteLine($"Export complete.");
        }

        static async Task<List<TriviaQuestion>> GrabAllQuestions(Category category)
        {
            List<TriviaQuestion> questions = new List<TriviaQuestion>();
            
            var count = await service.GetCategoryCount(category);

            while (questions.Count != count)
            {
                
                var tempQuestions = await service.LoadQuestions(category);

                foreach (TriviaQuestion question in tempQuestions)
                {
                    var questionToSave = questions.Where(q => q.Question == question.Question).ToList();
                    if (questionToSave.Count == 0)
                    {
                        Console.WriteLine($"{questions.Count} of {count}");
                        questions.Add(question);
                    }
                }
            }
            return questions.Where(q => q.Type == TriviaType.Multiple).ToList();
        }
    }
}
