using System;
using System.Collections.Generic;
using System.Text;
using XIANG_QI_TRANSFER.GameBorads;

namespace XIANG_QI_TRANSFER.Pieces
{
    abstract class Piece
    {
        private int x;//the x position of piece
        private int y;//the y position of piece
        private Team player;//the player of the chess
        private char name;//the expression of piece
        private string path;

        public enum Team
        {
            red,
            black
        }

        public Team Player { get => player; set => player = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public char Name { get => name; set => name = value; }
        public string Path { get => path; set => path = value; }

        public Piece(Team player, int intX, int intY)
        {
            this.Player = player;
            this.X = intX;
            this.Y = intY;
            //intX and intY are current position
        }
        public Piece(char name)
        {
            this.Name = name;
        }

        public abstract bool ValidMoves(int x, int y, GameBoard gameboard);

    }
}
