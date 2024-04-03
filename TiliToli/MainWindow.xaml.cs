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
                    foreach (UIElement element in gridMain.Children)
                    {
                        if (element is Button btn && btn != clickedBtn)
                        {
                            int rowClicked = Grid.GetRow(clickedBtn);
                            int colClicked = Grid.GetColumn(clickedBtn);
                            int rowBtn = Grid.GetRow(btn);
                            int colBtn = Grid.GetColumn(btn);

                            if ((rowBtn == rowClicked && Math.Abs(colBtn - colClicked) == 1) ||
                                (colBtn == colClicked && Math.Abs(rowBtn - rowClicked) == 1))
                            {
                                string btnContent = btn.Content.ToString();
                                if (string.IsNullOrEmpty(btnContent))
                                {
                                    Grid.SetRow(clickedBtn, rowBtn);
                                    Grid.SetColumn(clickedBtn, colBtn);
                                    Grid.SetRow(btn, rowClicked);
                                    Grid.SetColumn(btn, colClicked);

                                    string tempContent = clickedBtn.Content.ToString();
                                    clickedBtn.Content = btn.Content;
                                    btn.Content = tempContent;

                                    CheckGameCompletion();
                                    return;
                                }
                            }
                        }
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


    
