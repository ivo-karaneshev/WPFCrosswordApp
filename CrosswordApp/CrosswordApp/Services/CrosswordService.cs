using CrosswordApp.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CrosswordApp.Services
{
    class CrosswordService
    {
        public Crossword ReadFromFile()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"Data\Crossword.json");

            using (var reader = File.OpenText(filePath))
            {
                var serializer = new JsonSerializer();
                return JsonConvert.DeserializeObject<Crossword>(reader.ReadToEnd());
            }
        }
    }
}
