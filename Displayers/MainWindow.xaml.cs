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
        GameBoard gameboard = new GameBoard();
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

                    if (gameboard.Board[row, col] != null)
                    {
                        //打印棋子
                        String path;
                        
                        if (gameboard.validMoves[row, col])
                            path = "Resource\\aim\\" + gameboard.Board[row, col].Path;
                        else
                            path = "Resource\\" + gameboard.Board[row, col].Path;

                        img.Source = new BitmapImage(new Uri(
                            path, UriKind.Relative));
                    }
                    else
                    {
                        //如果这个区域，是某棋子的可移动区域，打印框，否则打印空棋盘。
                        if (gameboard.validMoves[row, col])
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
            if (!(gameboard.selectedRow == -1 && gameboard.selectedCol == -1))
            {
                String path = "Resource\\" + gameboard.Board[gameboard.selectedRow, gameboard.selectedCol].Path;

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
                    if (gameboard.isGameOver)
                    {
                        MessageBox.Show("Gameover, " + gameboard.Player + " player wins!\n" +
                            "Please start a new game");
                        break;
                    }

                    //判断选择是否合法
                    if (gameboard.SelectPiece(imgRow, imgCol))
                    {
                        //合法则执行以下操作 提示上一次操作合法
                        operateTips.Text = "Operation State:\nlegal";
                        
                        //下面两行代码的作用是在屏幕右边显示选中棋子
                        gameboard.selectedRow = imgRow;
                        gameboard.selectedCol = imgCol;

                        //更改状态
                        ChangeState(State.SelectMove);
                    }
                    else //操作不合法 提示非法
                        operateTips.Text = "Operation State:\nillegal";

                    DrawGrid(boardGrid);

                    break;

                case State.SelectMove:

                    if (gameboard.MovePiece(imgRow, imgCol))
                    {
                        //if move 如果移动了
                        if(gameboard.currentRow == gameboard.futureRow && gameboard.currentCol == gameboard.futureCol)
                        {
                            //如果游戏结束，则宣布游戏结束，否则交换玩家。
                            if (gameboard.CalculateisGameOver())
                                MessageBox.Show("Gameover, " + gameboard.Player + " player wins!\n" +
                                    "Please start a new game");
                            else
                                gameboard.SwitchPlayer();


                            //若移动合法，清理可移动轨迹。
                            gameboard.CleanValidMovePath();

                            ChangeState(State.SelectPiece);
                        }

                        operateTips.Text = "Operation State:\nlegal";

                    }
                    else
                        operateTips.Text = "Operation State:\nillegal";

                    DrawGrid(boardGrid);

                    //如果被将则提示危险
                    if (gameboard.check)
                    {
                        MessageBox.Show("Check!");
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
                    tips.Text = "Player: " + gameboard.Player +
                        "\n\nState:\n" + GameState;
                    break;
                case State.SelectPiece:
                    tips.Text = "Player: " + gameboard.Player +
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
            gameboard.restart();

            ChangeState(State.SelectPiece);

            DrawGrid(boardGrid);
        }

        private void Button_Click_undo(object sender, RoutedEventArgs e)
        {
            if (gameboard.step != 0)
            {
                ChangeState(State.SelectPiece);
                
                gameboard.undo();

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