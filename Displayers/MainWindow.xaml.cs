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
            //清理掉上一次产生的控件
            boardGrid.Children.Clear();

            //循环以内是棋盘棋子 以外是功能模块（选中的棋子
            for (int row = 0; row < 11; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Image img = new Image();

                    if (gb.Board[row, col] != null)
                    {
                        //打印棋子
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
                        //如果这个区域，是某棋子的可移动区域，打印框，否则打印空棋盘。
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

 
                    //给控件赋值并添加事件  
                    img.SetValue(XQRowProperty, row);
                    img.SetValue(XQColProperty, col);
                    img.MouseDown += new MouseButtonEventHandler(this.Image_MouseDown);

                    //将新控价添加到指定行列并放到grid中
                    Grid.SetRow(img, row);
                    Grid.SetColumn(img, col);
                    boardGrid.Children.Add(img);

                }
            }


            //如果值非法（没有选中任何棋子 不显示 否则显示
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

                    //判断游戏是否结束
                    if (gb.isGameOver)
                    {
                        MessageBox.Show("Gameover, " + gb.Player + " player wins!\n" +
                            "Please start a new game");
                        break;
                    }

                    //成功选中则替换右边显示区域
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
                        //若操作合法，清理上一个棋子的可移动轨迹。
                        gb.CleanValidMovePath();
                        operateTips.Text = "last move State:\nlegal";

                        //如果游戏结束，则宣布游戏结束，否则交换玩家。
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

            HandleClick(imgRow, imgCol);
        }
    }
}