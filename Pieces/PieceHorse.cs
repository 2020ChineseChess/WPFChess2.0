using System;
using System.Collections.Generic;
using System.Text;
using XIANG_QI_TRANSFER.GameBorads;

namespace XIANG_QI_TRANSFER.Pieces
{
    class PieceHorse : Piece
    {
        public PieceHorse(Team player, int x, int y) : base(player, x, y)
        {
            this.Name = '馬';


            Path = Player + "Horse.png";
        }

        public override bool ValidMoves(int x, int y, GameBoard gb)
        {
            int CurrentX = this.X;
            int CurrentY = this.Y;

            //if over river, the index should +1/-1; 
            if (CurrentX <= 5 && x >= 6)
            {
                x -= 1;
            }

            if (CurrentX >= 6 && x <= 5)
            {
                x += 1;
            }

            //to right
            if (y == CurrentY + 2 && (x == CurrentX + 1 || x == CurrentX - 1))
            {
                //if stuck
                if (gb.Board[CurrentX, CurrentY + 1] == null)
                    return true;
            }

            //to left
            if (y == CurrentY - 2 && (x == CurrentX + 1 || x == CurrentX - 1))
            {
                //if stuck
                if (gb.Board[CurrentX, CurrentY - 1] == null)
                    return true;
            }

            //to up
            if (x == CurrentX - 2 && (y == CurrentY + 1 || y == CurrentY - 1))
            {
                //if stuck
                if (gb.Board[CurrentX - 1, CurrentY] == null)
                    return true;
            }

            //to down
            if (x == CurrentX + 2 && (y == CurrentY + 1 || y == CurrentY - 1))
            {
                //if stuck
                if (gb.Board[CurrentX + 1, CurrentY] == null)
                    return true;
            }
            return false;
        }
    }
}
