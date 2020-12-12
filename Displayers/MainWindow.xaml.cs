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
using System.Windows.Shapes;
using XIANG_QI_TRANSFER.GameBorads;



namespace XIANG_QI_TRANSFER.Displayers
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        GameBoard gb = new GameBoard();
        State GameState;
        int SelectedRow = -1, SelectedCol = -1;

        enum State
        {
            SelectPiece,
            SelectMove
        }

        public MainWindow()
        {
            InitializeComponent();
            createGrid(grid);
        }



 
        public void createGrid(Grid grid)
        {
            for (int row = 0; row < 11; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Button btn = new Button();
                    btn.Height = 40;
                    btn.Width = 80;

                    if (gb.Board[row, col] != null)
                    {
                        btn.Content = gb.Board[row, col].Name;
                        if (gb.Board[row, col].Player != Pieces.Piece.Team.black)
                            btn.Foreground = Brushes.Red;
                    }


                    if (row != 5)
                    {
                        btn.SetValue(XQRowProperty, row);
                        btn.SetValue(XQColProperty, col);
                        btn.Click += new RoutedEventHandler(this.Button_Click);
                    }
                    else
                    {
                        btn.Background = Brushes.LightSeaGreen;
                    }

                    if(row == SelectedRow && col == SelectedCol)
                    {
                        btn.Background = Brushes.LightCyan;
                    }

                    Grid.SetRow(btn, row);
                    Grid.SetColumn(btn, col);
                    grid.Children.Add(btn);
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int btnRow = (int)((Button)sender).GetValue(XQRowProperty);
            int btnCol = (int)((Button)sender).GetValue(XQColProperty);

            /*
            MessageBox.Show("Button is: " +
                ((Button)sender).Name +
                "\n - Row       = " + btnRow +
                "\n - Column = " + btnCol );*/

            HandleClick(btnRow,btnCol);
        }

        public void HandleClick(int BtnRow, int BtnCol)
        {

            switch (GameState)
            {
                case State.SelectPiece:


                    if (gb.SelectPiece(BtnRow, BtnCol))
                    {
                        operateTips.Text = "last move State:\nlegal";
                        SelectedRow = BtnRow;
                        SelectedCol = BtnCol;
                        ChangeState(State.SelectMove);
                    }
                    else
                        operateTips.Text = "last move State:\nillegal";

                    createGrid(grid);

                    break;

                case State.SelectMove:

                    if (gb.MovePiece(BtnRow, BtnCol))
                    {
                        operateTips.Text = "last move State:\nlegal";
                        SelectedRow = BtnRow;
                        SelectedCol = BtnCol;
                        ChangeState(State.SelectPiece);
                    }
                    else
                        operateTips.Text = "last move State:\nillegal";


                    createGrid(grid);

                    if (gb.judgeIsGameOver())
                        MessageBox.Show(gb.Player + " win");
                    else
                        gb.SwitchPlayer();

                    break;
            }

        }

        
        private void ChangeState(State newState)
        {
            GameState = newState;

            switch (newState)
            {
                case State.SelectMove:
                    tips.Text = "Player: " + gb.Player +
                        "\n\nState:\n" + GameState;
                    break;
                case State.SelectPiece:
                    tips.Text = "Player: " + gb.Player +
                        "\n\nState:\n" + GameState;
                    break;
            }

        }
        
        public static readonly DependencyProperty XQRowProperty =
            DependencyProperty.Register("XQRow",
                typeof(int), typeof(Button),
                new PropertyMetadata(default(int)));

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //start
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //restart
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //undo
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("123");
        }

        public static readonly DependencyProperty XQColProperty =
            DependencyProperty.Register("XQCol",
                typeof(int), typeof(Button),
                new PropertyMetadata(default(int)));
    }
}



