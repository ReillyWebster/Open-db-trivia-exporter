using OpenTriviaSharp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace OpenTriviaDatabaseExtractor
{
    public static class CsvExporter
    {
        private static string _separator = ",";
        private static string _multipleChoiceHeaders = $"Question{_separator}Difficulty{_separator}Answer1{_separator}Answer2{_separator}Answer3{_separator}Answer4{_separator}CorrectAnswer\n";
        private static string _trueFalseHeaders = $"Question{_separator}Difficulty{_separator}Answer1{_separator}Answer2{_separator}CorrectAnswer\n";
        public static void SaveCsvFile(List<TriviaQuestion> triviaQuestions, string exportDirectory)
        {
            var trueFalseQuestions = triviaQuestions.Where(tq => tq.Type == TriviaType.Boolean).ToList();
            var multipleChoiceQuestions = triviaQuestions.Where(tq => tq.Type == TriviaType.Multiple).ToList();

            var builder = ProcessTrueFalseQuestions(trueFalseQuestions);
            
            CreateCsvFile(builder, exportDirectory);

            builder = ProcessMultipleChoiceQuestions(multipleChoiceQuestions);

            CreateCsvFile(builder, exportDirectory);
        }

        private static StringBuilder ProcessMultipleChoiceQuestions(List<TriviaQuestion> multipleChoiceQuestions)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_multipleChoiceHeaders);

            foreach (var question in multipleChoiceQuestions)
            {
                builder.Append(
                    $"{question.Question}{_separator}"
                ); 
            }

            return builder;
        }

        private static StringBuilder ProcessTrueFalseQuestions(List<TriviaQuestion> trueFalseQuestions)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_trueFalseHeaders);

            foreach(var question in trueFalseQuestions)
            {

            }

            return builder;
        }

        private static void CreateCsvFile(StringBuilder builder, string filePath)
        {
            using (var file = File.Create(filePath))
            {
                file.Write(Encoding.ASCII.GetBytes(builder.ToString()));
                file.Close();
            }
        }
    }
}
