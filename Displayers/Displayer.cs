using System;
using System.Collections.Generic;
using System.Text;
using XIANG_QI_TRANSFER.GameBorads;

namespace XIANG_QI_TRANSFER.Displayers
{
    class Displayer
    {
        public void DisplayBoard(GameBoard gb)
        {
            //清屏 改变颜色和创建图案
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  |ChineseChess|    ");
            Console.WriteLine("                    ");
            int count = 0;

            //打印棋子或格子
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    //判断目标点是否存在棋子
                    if (gb.Board[i, j] != null)
                    {
                        //如果存在棋子 根据队伍打印不同颜色
                        if (gb.Board[i, j].Player == Pieces.Piece.Team.red)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (gb.Board[i, j].Player == Pieces.Piece.Team.black)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }

                        //打印棋子并初始化颜色设定
                        Console.Write(gb.Board[i, j].Name);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        //若为第五行 打印楚河汉界 否则打印棋盘
                        if (i == 5)
                        {
                            //river = "一一楚河一汉界一一";
                            Console.Write(gb.river[j]);
                        }
                        else
                        {
                            Console.Write('十');
                        }
                    }


                }

                //如果是河 则不计数
                if (i != 5)
                {
                    Console.Write(" " + count++);
                }
                else
                {
                    //美观
                    Console.Write("  ");
                }

                //换行 = \n
                Console.WriteLine("");

            }

            //标识列数
            Console.WriteLine(" A B C D E F G H I  \n");
            Console.BackgroundColor = ConsoleColor.Black;

            //给玩家设定背景颜色及初始化颜色设定
            Console.Write("CurrentPlayer:");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(gb.Player);

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void AskSelectPiece()
        {
            Console.WriteLine("Which piece do you want to move?");
        }
        public void selectError()
        {
            Console.WriteLine("illegal request!You can only move your pieces!");
        }
        public void moveError()
        {
            Console.WriteLine("illegal request!You can't move to this place!");
        }

        public void AskMovePiece()
        {
            Console.WriteLine("Where do you want move it to?");
        }

        public void GameOver()
        {
            Console.WriteLine("GameOver! ");
        }
    }
}
