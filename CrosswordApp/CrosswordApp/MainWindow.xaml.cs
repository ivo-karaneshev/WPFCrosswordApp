using CrosswordApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CrosswordApp
{
    public partial class MainWindow : Window
    {
        private CrosswordVM vm;
        private int cellSize;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                vm = new CrosswordVM();
                cellSize = 50;
                DataContext = vm;
                InitializeGridLayout();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                MessageBox.Show("Error loading the crossword. Check your file and restart the application.", "Error", MessageBoxButton.OK);
                Application.Current.Shutdown();
            }

        }

        private void InitializeGridLayout()
        {
            for (var j = 0; j < vm.Rows; j++)
            {
                var rowDef = new RowDefinition();
                rowDef.Height = new GridLength(cellSize);
                CrosswordGrid.RowDefinitions.Add(rowDef);

                for (var i = 0; i < vm.Columns; i++)
                {
                    var colDef = new ColumnDefinition();
                    colDef.Width = new GridLength(cellSize);
                    CrosswordGrid.ColumnDefinitions.Add(colDef);

                    var elements = new Stack<FrameworkElement>();
                    AddElements(i, j, elements);

                    while (elements.Count > 0)
                    {
                        var control = elements.Pop();
                        Grid.SetColumn(control, i);
                        Grid.SetRow(control, j);
                        CrosswordGrid.Children.Add(control);
                    }
                }
            }
        }

        private void AddElements(int i, int j, Stack<FrameworkElement> elements)
        {
            var data = vm.DataSource[i, j];

            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.Label))
                {
                    var label = new Label();
                    label.Content = data.Label;
                    label.IsHitTestVisible = false;
                    label.Padding = new Thickness(2);
                    elements.Push(label);
                }

                var textBox = new TextBox();
                textBox.SetBinding(TextBox.TextProperty, new Binding("DataSource[" + vm.DataSource.GetStringIndex(i, j) + "].Current"));
                textBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                textBox.VerticalContentAlignment = VerticalAlignment.Center;
                textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                textBox.VerticalAlignment = VerticalAlignment.Stretch;
                textBox.Height = cellSize;
                textBox.CharacterCasing = CharacterCasing.Upper;
                textBox.MaxLength = 1;
                textBox.PreviewTextInput += OnTextInput;
                elements.Push(textBox);
            }
            else
            {
                var rect = new Rectangle();
                rect.HorizontalAlignment = HorizontalAlignment.Stretch;
                rect.VerticalAlignment = VerticalAlignment.Stretch;
                rect.Fill = Brushes.Black;
                elements.Push(rect);
            }
        }

        private void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text.ElementAt(0)))
            {
                e.Handled = true;
            }
        }
    }
}
