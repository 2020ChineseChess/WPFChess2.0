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

        enum State
        {
            SelectPiece,
            SelectMove
        }

        public MainWindow()
        {
            InitializeComponent();
        }



 
        public void DrawGrid(Grid grid)
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
                    else
                    {
         
                        //btn.Background = Brushes.Transparent;
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

                    if(row == gb.selectedRow && col == gb.selectedCol)
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
                "\n - Column = " + btnCol );   */


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
                        ChangeState(State.SelectMove);
                    }
                    else
                        operateTips.Text = "last move State:\nillegal";

                    DrawGrid(grid);

                    break;

                case State.SelectMove:

                    if (gb.MovePiece(BtnRow, BtnCol))
                    {
                        operateTips.Text = "last move State:\nlegal";
                        ChangeState(State.SelectPiece);
                        gb.step++;
                    }
                    else
                        operateTips.Text = "last move State:\nillegal";


                    if (gb.judgeIsGameOver())
                        MessageBox.Show(gb.Player + " win");
                    else
                        gb.SwitchPlayer();

                    DrawGrid(grid);


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
            DrawGrid(grid);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //restart
            operateTips.Text = "last move State:\nlegal";
            gb.restart();
            ChangeState(State.SelectPiece);
            DrawGrid(grid);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //undo
            gb.selectedRow = -1;
            gb.selectedCol = -1;
            operateTips.Text = "last move State:\nlegal";

            switch (GameState)
            {
                case State.SelectMove:
                    GameState = State.SelectPiece;
                    MessageBox.Show("undo successful, please select piece again");
                    DrawGrid(grid);
                    break;
                case State.SelectPiece:

                    if (gb.step != 0)
                    {
                        gb.SwitchPlayer();
                        gb.undo();
                        DrawGrid(grid);
                    }
                    else
                    {
                        MessageBox.Show("undo failure, has not piece been moved");
                    }
                    break;
            }
        }


        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            int btnRow = (int)((Button)sender).GetValue(XQRowProperty);
            int btnCol = (int)((Button)sender).GetValue(XQColProperty);

            MessageBox.Show("Button is: " +
            ((Button)sender).Name +
            "\n - Row       = " + btnRow +
            "\n - Column = " + btnCol );
        }

        public static readonly DependencyProperty XQColProperty =
            DependencyProperty.Register("XQCol",
                typeof(int), typeof(Button),
                new PropertyMetadata(default(int)));
    }
}