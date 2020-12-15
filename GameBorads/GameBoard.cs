using System;
using System.Collections.Generic;
using System.Text;
using XIANG_QI_TRANSFER.Pieces;
using static XIANG_QI_TRANSFER.Pieces.Piece;

namespace XIANG_QI_TRANSFER.GameBorads
{
    class GameBoard
    {
        Team player = Team.red;
        private Piece[,] board;
        public string river = "一一楚河一汉界一一"; //river
        public int currentRow, currentCol;
        public int futureRow, futureCol;
        public int lastRow, lastCol;
        public int selectedRow, selectedCol;
        public int step = 0;
        public bool isKilled;
        public Piece diedPiece;
        public int tempRow, tempCol;
        public bool[,] validMoves = new bool[11, 9];
        public bool isGameOver = false;
        public bool dangerous;

        public Team Player { get => player; set => player = value; }
        internal Piece[,] Board { get => board; set => board = value; }


        public GameBoard()
        {
            chessboardBuilding();
        }

        //交换玩家
        public void SwitchPlayer()
        {
            if (Player == Team.red)
            {
                Player = Team.black;
            }
            else
            {
                Player = Team.red;
            }
        }


        public bool SelectPiece(int row, int col)
        { 
            //if piece exist.
            if (board[row, col] != null)
            {
                //if the piece belong to player;
                if (board[row, col].Player == this.player)
                {
                    tempRow = row;
                    tempCol = col;

                    GetValidMovePath();
                    return true;
                }


                return false;
            }
            else
            {
                return false;
            }
        }


        public Boolean MovePiece(int row, int col)
        {
            dangerous = false;

            futureRow = row;
            futureCol = col;

            currentRow = tempRow;
            currentCol = tempCol;

            //store the died piece
            isKilled = false;
            diedPiece = null;

            //cancel the illegal move (nothing change
            if ((currentCol == futureCol) && (currentRow == futureRow))
            {
                return false;
            }

            //could not eating owner piece
            if (Board[futureRow, futureCol] != null)
                if (Board[futureRow, futureCol].Player == Board[currentRow, currentCol].Player)
                {
                    CleanValidMovePath();
                    tempRow = futureRow;
                    tempCol = futureCol;
                    selectedCol = futureCol;
                    selectedRow = futureRow;
                    GetValidMovePath();
                    return false;
                }
                else
                {
                    //is the move follow the chess rules
                    if (!(board[currentRow, currentCol].ValidMoves(futureRow, futureCol, this)))
                    {
                        return false;
                    }
                    else
                    {
                        isKilled = true;
                        diedPiece = Board[futureRow, futureCol];
                    }
                }


            //store the old position for undo   
            lastCol = currentCol;
            lastRow = currentRow;

            //new one sign to old one
            board[futureRow, futureCol] = board[currentRow, currentCol];

            //change the position of piece
            board[futureRow, futureCol].X = futureRow;
            board[futureRow, futureCol].Y = futureCol;

            //delete old one
            board[currentRow, currentCol] = null;

            //sign the step;
            currentRow = futureRow;
            currentCol = futureCol;

            selectedRow = -1;
            selectedCol = -1;

            isDangerous();

            step++;
            return true;
        }

        public void isDangerous()
        {
            for(int row = 0; row < 11; row++)
            {
                for(int col = 0; col< 9; col++)
                {
                    if(board[row,col] != null)
                    {
                        if (board[row,col].Player == Team.black)
                        {
                            for (int i = 8; i <= 10; i++)
                                for (int j = 3; j <= 5; j++)
                                    if (board[row, col].ValidMoves(i, j, this))
                                    {
                                        if (board[i, j] != null && ((board[row, col].Player != board[i, j].Player)))
                                            if (board[i, j].Name == '帥')
                                                dangerous = true;
                                    }
                        }
                        else
                        {
                            for (int i = 0; i <= 2; i++)
                                for (int j = 3; j <= 5; j++)
                                    if (board[row, col].ValidMoves(i, j, this))
                                    {
                                        if (board[i, j] != null && ((board[row, col].Player != board[i, j].Player)))
                                            if (board[i, j].Name == '將')
                                                dangerous = true;
                                    }
                        }
                    }
                }
            }

        }
        public void GetValidMovePath()
        {

            for (int i = 0; i < 11; i++)
                for (int j = 0; j < 9; j++)
                    //is the move follow the chess rules
                    if (board[tempRow, tempCol].ValidMoves(i, j, this))
                    {
                        if (board[i, j] == null)
                            validMoves[i, j] = true;
                        else
                            if (board[tempRow, tempCol].Player != board[i, j].Player)
                                validMoves[i, j] = true;

                    }
        }

        public void CleanValidMovePath()
        {
            for (int i = 0; i < 11; i++)
                for (int j = 0; j < 9; j++)
                {
                    validMoves[i, j] = false;
                }
        }

        public Boolean judgeIsGameOver()
        {
            int generalCount = 0;
            int redGeneralRow = -1, redGeneralCol = -1;
            int blackGeneralRow = -1, blackGeneralCol = -1;

            for (int i = 8; i <= 10; i++)
                for (int j = 3; j <= 5; j++)
                    if (board[i, j] != null)
                        if (board[i, j].Name == '帥')
                        {
                            redGeneralRow = i;
                            redGeneralCol = j;
                            generalCount++;
                        }

            for (int i = 0; i <= 2; i++)
                for (int j = 3; j <= 5; j++)
                    if (board[i, j] != null)
                        if (board[i, j].Name == '將')
                        {
                            blackGeneralRow = i;
                            blackGeneralCol = j;
                            generalCount++;
                        }

            if (generalCount != 2)
            {
                isGameOver = true;
                return true;
            }

            if (blackGeneralCol == redGeneralCol)
            {
                for (int row = blackGeneralRow + 1; row < redGeneralRow; row++)
                {
                    if (board[row, blackGeneralCol] != null)
                    {
                        return false;
                    }
                }

                SwitchPlayer();

                isGameOver = true;
                return true;
            }

            return false;
        }

        void chessboardBuilding()
        {
            board = new Piece[11, 9];

            player = Team.red;
            currentRow = -1; currentCol = -1;
            futureRow = -1; futureCol = -1;
            lastRow = -1; lastCol = -1;
            selectedRow = -1; selectedCol = -1;
            step = 0;
            isGameOver = false;
            dangerous = false;

            //building the BlackChess
            Board[0, 0] = new PieceCar(Team.black, 0, 0); //车
            Board[0, 1] = new PieceHorse(Team.black, 0, 1); //马
            Board[0, 2] = new PieceElephant(Team.black, 0, 2); //相
            Board[0, 3] = new PieceAdvisor(Team.black, 0, 3); //士
            Board[0, 4] = new PieceGerneral(Team.black, 0, 4); //将
            Board[0, 5] = new PieceAdvisor(Team.black, 0, 5);
            Board[0, 6] = new PieceElephant(Team.black, 0, 6);
            Board[0, 7] = new PieceHorse(Team.black, 0, 7);
            Board[0, 8] = new PieceCar(Team.black, 0, 8);

            Board[2, 1] = new PieceCannon(Team.black, 2, 1);
            Board[2, 7] = new PieceCannon(Team.black, 2, 7);

            Board[3, 0] = new PieceSoldier(Team.black, 3, 0);
            Board[3, 2] = new PieceSoldier(Team.black, 3, 2);
            Board[3, 4] = new PieceSoldier(Team.black, 3, 4);
            Board[3, 6] = new PieceSoldier(Team.black, 3, 6);
            Board[3, 8] = new PieceSoldier(Team.black, 3, 8);


            //building the RedChess
            Board[10, 0] = new PieceCar(Team.red, 10, 0);
            Board[10, 1] = new PieceHorse(Team.red, 10, 1);
            Board[10, 2] = new PieceElephant(Team.red, 10, 2);
            Board[10, 3] = new PieceAdvisor(Team.red, 10, 3);
            Board[10, 4] = new PieceGerneral(Team.red, 10, 4);
            Board[10, 5] = new PieceAdvisor(Team.red, 10, 5);
            Board[10, 6] = new PieceElephant(Team.red, 10, 6);
            Board[10, 7] = new PieceHorse(Team.red, 10, 7);
            Board[10, 8] = new PieceCar(Team.red, 10, 8);

            Board[8, 1] = new PieceCannon(Team.red, 8, 1);
            Board[8, 7] = new PieceCannon(Team.red, 8, 7);

            Board[7, 0] = new PieceSoldier(Team.red, 7, 0);
            Board[7, 2] = new PieceSoldier(Team.red, 7, 2);
            Board[7, 4] = new PieceSoldier(Team.red, 7, 4);
            Board[7, 6] = new PieceSoldier(Team.red, 7, 6);
            Board[7, 8] = new PieceSoldier(Team.red, 7, 8);
        }

        internal void restart()
        {
            chessboardBuilding();
        }

        internal void undo()
        {

            //current one sign to old one
            board[lastRow, lastCol] = board[currentRow, currentCol];

            //change the position of piece
            board[lastRow, lastCol].X = lastRow;
            board[lastRow, lastCol].Y = lastCol;

            //if someone has been killed, recover;
            //if not, current -> null;
            if (isKilled)
                board[currentRow, currentCol] = diedPiece;
            else
                board[currentRow, currentCol] = null;

            //sign the last step;
            currentRow = lastRow;
            currentCol = lastCol;

            step = 0;
        }
    }
}

