using Newtonsoft.Json;
using OpenTriviaSharp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace OpenTriviaDatabaseExtractor
{
    public static class JsonExporter
    {
        public static void SaveJsonFile(List<TriviaQuestion> triviaQuestions, string exportDirectory)
        {
            Debug.WriteLine(Directory.GetCurrentDirectory());
            using (StreamWriter file = File.CreateText(exportDirectory))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, triviaQuestions);
            }
        }
    }
}
