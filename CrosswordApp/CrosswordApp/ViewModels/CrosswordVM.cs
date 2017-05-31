using CrosswordApp.Common;
using CrosswordApp.Models;
using CrosswordApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace CrosswordApp.ViewModels
{
    class CrosswordVM : INotifyPropertyChanged
    {
        private CrosswordService crosswordService;
        private Crossword crossword;
        public event PropertyChangedEventHandler PropertyChanged;

        public Bindable2DArray<GridCell> DataSource { get; set; }
        public List<string> CluesAcross { get; set; }
        public List<string> CluesDown { get; set; }
        public int Rows { get { return crossword.Height; } }
        public int Columns { get { return crossword.Width; } }
        public ICommand CheckCrosswordCommand { get; private set; }
        public ICommand SolveCrosswordCommand { get; private set; }
        public string Output { get; set; }

        public CrosswordVM()
        {
            crosswordService = new CrosswordService();
            CluesAcross = new List<string>();
            CluesDown = new List<string>();
            CheckCrosswordCommand = new DelegateCommand(() => CheckCrossword());
            SolveCrosswordCommand = new DelegateCommand(() => SolveCrossword());

            crossword = crosswordService.ReadFromFile();
            InitializeGrid();
        }

        private void Notify(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void CheckCrossword()
        {
            var success = true;
            for (var i = 0; i < Columns; i++)
            {
                for (var j = 0; j < Rows; j++)
                {
                    var data = DataSource[i, j];
                    if (data != null && !data.Current.Equals(data.Target, StringComparison.InvariantCultureIgnoreCase))
                    {
                        success = false;
                    }
                }
            }

            Output = success ? "Well done!" : "Try again!";
            Notify(nameof(Output));
        }

        private void SolveCrossword()
        {
            for (var i = 0; i < Columns; i++)
            {
                for (var j = 0; j < Rows; j++)
                {
                    var data = DataSource[i, j];
                    if (data != null)
                    {
                        data.Current = data.Target;
                    }
                }
            }

            Notify(nameof(DataSource));
        }

        private void InitializeGrid()
        {
            var grid = new Bindable2DArray<GridCell>(crossword.Width, crossword.Height);

            var wordIndex = 1;
            foreach (var word in crossword.Words)
            {
                var positions = word.Position.Split(',');
                var i = int.Parse(positions[0]);
                var j = int.Parse(positions[1]);

                for (var k = 0; k < word.Answer.Length; k++)
                {
                    var cell = grid[i, j];
                    if (cell == null)
                    {
                        cell = new GridCell();
                        cell.Current = string.Empty;
                        cell.Target = word.Answer.ElementAt(k).ToString().ToUpper();
                        grid[i, j] = cell;
                    }

                    var direction = (Direction)Enum.Parse(typeof(Direction), word.Direction, true);
                    if (direction == Direction.Across)
                    {
                        i++;
                        if (k == 0)
                        {
                            cell.Label = cell.Label != null ? cell.Label + $" {wordIndex.ToString()}" : wordIndex.ToString();
                            CluesAcross.Add($"{wordIndex}. {word.Clue}");
                        }
                    }
                    else
                    {
                        j++;
                        if (k == 0)
                        {
                            cell.Label = cell.Label != null ? cell.Label + $" {wordIndex.ToString()}" : wordIndex.ToString();
                            CluesDown.Add($"{wordIndex}. {word.Clue}");
                        }
                    }
                }

                wordIndex++;
            }

            DataSource = grid;
        }
    }
}
