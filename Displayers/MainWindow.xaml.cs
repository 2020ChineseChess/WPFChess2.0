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

        public static readonly DependencyProperty XQColProperty =
            DependencyProperty.Register("XQCol",
        typeof(int), typeof(Image),
        new PropertyMetadata(default(int)));

        public static readonly DependencyProperty XQRowProperty =
             DependencyProperty.Register("XQRow",
                typeof(int), typeof(Image),
                new PropertyMetadata(default(int)));

        enum State
        {
            SelectPiece,
            SelectMove
        }

        public MainWindow()
        {
            InitializeComponent();
        }



        public void DrawGrid(Grid boardGrid)
        {
            boardGrid.Children.Clear();

            for (int row = 0; row < 11; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Image img = new Image();
                    img.Height = 50;
                    img.Width = 50;

                    if (gb.Board[row, col] != null)
                    {
                        String path;

                        if (gb.validMoves[row, col])
                            path = "Resource\\aim\\" + gb.Board[row, col].Path;
                        else
                            path = "Resource\\" + gb.Board[row, col].Path;

                        img.Source = new BitmapImage(new Uri(
                            path, UriKind.Relative));
                    }
                    else
                    {
                        if (gb.validMoves[row, col])
                        {
                            img.Source = new BitmapImage(new Uri(
                                "Resource\\box.png", UriKind.Relative));
                        }
                        else
                        {
                            img.Source = new BitmapImage(new Uri(
                                "Resource\\null.png", UriKind.Relative));

                        }
                    }

 
                    if (row != 5)
                    {
                        img.SetValue(XQRowProperty, row);
                        img.SetValue(XQColProperty, col);

                        img.MouseDown += new MouseButtonEventHandler(this.Image_MouseDown);
                    }


                    Grid.SetRow(img, row);
                    Grid.SetColumn(img, col);
                    boardGrid.Children.Add(img);

                }
            }


            if (!(gb.selectedRow == -1 && gb.selectedCol == -1))
            {
                String path = "Resource\\" + gb.Board[gb.selectedRow, gb.selectedCol].Path;

                SelecetedPiece.Source = new BitmapImage(new Uri(
                               path, UriKind.Relative));
            }
            else
            {
                SelecetedPiece.Source = new BitmapImage(new Uri(
                               "Resource\\null.png", UriKind.Relative));
            }
        }

        public void HandleClick(int imgRow, int imgCol)
        {
            switch (GameState)
            {
                case State.SelectPiece:

                    if (gb.isGameOver)
                    {
                        MessageBox.Show("Gameover, " + gb.Player + " player wins!\n" +
                            "Please start a new game");
                        break;
                    }

                    if (gb.SelectPiece(imgRow, imgCol))
                    {
                        operateTips.Text = "last move State:\nlegal";

                        gb.selectedRow = imgRow;
                        gb.selectedCol = imgCol;

                        ChangeState(State.SelectMove);
                    }
                    else
                        operateTips.Text = "last move State:\nillegal";

                    DrawGrid(boardGrid);

                    break;

                case State.SelectMove:


                    if (gb.MovePiece(imgRow, imgCol))
                    {
                        gb.CleanValidMovePath();
                        operateTips.Text = "last move State:\nlegal";

                        if (gb.judgeIsGameOver())
                            MessageBox.Show("Gameover, " + gb.Player + " player wins!\n" +
                                "Please start a new game");
                        else
                            gb.SwitchPlayer();

                        ChangeState(State.SelectPiece);
                    }
                    else
                        operateTips.Text = "last move State:\nillegal";

                    DrawGrid(boardGrid);

                    if (gb.dangerous)
                    {
                        MessageBox.Show("Dangerous!");
                    }

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



        private void Button_Click_start(object sender, RoutedEventArgs e)
        {
            DrawGrid(boardGrid);
        }

        private void Button_Click_restart(object sender, RoutedEventArgs e)
        {
            operateTips.Text = "last move State:\nlegal";
            gb.restart();
            ChangeState(State.SelectPiece);
            DrawGrid(boardGrid);
        }

        private void Button_Click_undo(object sender, RoutedEventArgs e)
        {
            if (gb.step != 0)
            {
                ChangeState(State.SelectPiece);
                
                gb.undo();

                DrawGrid(boardGrid);
            }
            else
            {
                MessageBox.Show("undo failure, has not piece been moved");
            }

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int imgRow = (int)((Image)sender).GetValue(XQRowProperty);
            int imgCol = (int)((Image)sender).GetValue(XQColProperty);

            /*
            MessageBox.Show("Image is: " +
                ((Image)sender).Name +
                "\n - Row       = " + imgRow +
                "\n - Column = " + imgCol);*/

            HandleClick(imgRow, imgCol);
        }
    }
}