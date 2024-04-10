using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TiliToli
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            // Létrehozunk egy 3x3-as rácsot
            for (int i = 0; i < 3; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                ColumnDefinition colDef = new ColumnDefinition();

                gridMain.RowDefinitions.Add(rowDef);
                gridMain.ColumnDefinitions.Add(colDef);
            }

            // A számok 1-től 8-ig
            int[] numbers = Enumerable.Range(1, 8).ToArray();

            // Az utolsó gomb számnélküli
            int? lastNumber = null;

            // A számok véletlenszerű keverése
            Random rand = new Random();
            numbers = numbers.OrderBy(x => rand.Next()).ToArray();

            int index = 0;
            // A rács elemeinek beállítása a számokkal
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (index < 8)
                    {
                        Button btn = new Button();
                        btn.Content = numbers[index].ToString();
                        btn.Margin = new Thickness(5);
                        btn.Click += Btn_Click;

                        Grid.SetRow(btn, i);
                        Grid.SetColumn(btn, j);

                        gridMain.Children.Add(btn);

                        index++;
                    }
                    else
                    {
                        Button btn = new Button();
                        btn.Content = "";
                        btn.Margin = new Thickness(5);
                        btn.Click += Btn_Click;

                        Grid.SetRow(btn, i);
                        Grid.SetColumn(btn, j);

                        gridMain.Children.Add(btn);
                    }
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;
            if (clickedBtn != null)
            {
                int clickedNumber;
                if (int.TryParse(clickedBtn.Content.ToString(), out clickedNumber))
                {
                    int rowClicked = Grid.GetRow(clickedBtn);
                    int colClicked = Grid.GetColumn(clickedBtn);

                    // Felfele mozgatás
                    if (rowClicked > 0 && IsButtonEmpty(rowClicked - 1, colClicked))
                    {
                        SwapButtons(clickedBtn, rowClicked - 1, colClicked);
                        return;
                    }

                    // Lefele mozgatás
                    if (rowClicked < 2 && IsButtonEmpty(rowClicked + 1, colClicked))
                    {
                        SwapButtons(clickedBtn, rowClicked + 1, colClicked);
                        return;
                    }

                    // Balra mozgatás
                    if (colClicked > 0 && IsButtonEmpty(rowClicked, colClicked - 1))
                    {
                        SwapButtons(clickedBtn, rowClicked, colClicked - 1);
                        return;
                    }

                    // Jobbra mozgatás
                    if (colClicked < 2 && IsButtonEmpty(rowClicked, colClicked + 1))
                    {
                        SwapButtons(clickedBtn, rowClicked, colClicked + 1);
                        return;
                    }
                }
            }
        }

        private bool IsButtonEmpty(int row, int col)
        {
            foreach (UIElement element in gridMain.Children)
            {
                if (element is Button btn)
                {
                    int btnRow = Grid.GetRow(btn);
                    int btnCol = Grid.GetColumn(btn);
                    if (btnRow == row && btnCol == col && string.IsNullOrEmpty(btn.Content.ToString()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void SwapButtons(Button btn1, int row2, int col2)
        {
            int row1 = Grid.GetRow(btn1);
            int col1 = Grid.GetColumn(btn1);

            foreach (UIElement element in gridMain.Children)
            {
                if (element is Button btn2)
                {
                    int btnRow = Grid.GetRow(btn2);
                    int btnCol = Grid.GetColumn(btn2);
                    if (btnRow == row2 && btnCol == col2)
                    {
                        Grid.SetRow(btn1, row2);
                        Grid.SetColumn(btn1, col2);
                        Grid.SetRow(btn2, row1);
                        Grid.SetColumn(btn2, col1);

                        string tempContent = btn1.Content.ToString();
                        btn1.Content = btn2.Content;
                        btn2.Content = tempContent;

                        CheckGameCompletion();
                        break;
                    }
                }
            }
        }

        private void CheckGameCompletion()
        {
            bool isCompleted = true;
            int expectedNumber = 1;

            foreach (UIElement element in gridMain.Children)
            {
                if (element is Button btn && !string.IsNullOrEmpty(btn.Content.ToString()))
                {
                    int number;
                    if (int.TryParse(btn.Content.ToString(), out number))
                    {
                        if (number != expectedNumber)
                        {
                            isCompleted = false;
                            break;
                        }
                        expectedNumber++;
                    }
                }
            }

            if (isCompleted)
            {
                MessageBox.Show("Gratulálok, teljesítetted a játékot!");
            }
        }
    }
}


    
