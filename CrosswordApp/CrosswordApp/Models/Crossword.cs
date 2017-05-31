using System.Collections.Generic;

namespace CrosswordApp.Models
{
    class Crossword
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public IEnumerable<WordInfo> Words { get; set; }
    }
}
