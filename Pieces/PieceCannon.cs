using System;
using System.Collections.Generic;
using System.Text;
using XIANG_QI_TRANSFER.GameBorads;

namespace XIANG_QI_TRANSFER.Pieces
{
    class PieceCannon : Piece
    {

        public PieceCannon(Team player, int x, int y) : base(player, x, y)
        {
            if (this.Player == Team.black)
            {
                this.Name = '炮';
            }
            else
            {
                this.Name = '砲';
            }

            Path = Player + "Cannon.png";
        }

        public override bool ValidMoves(int x, int y, GameBoard gb)
        {
            int CurrentX = this.X;
            int CurrentY = this.Y;
            int count = -1;


            //move horizontally
            if (x == CurrentX && y != CurrentY)
            {
                if (y > CurrentY)
                {
                    //to right
                    count = 0;
                    for (int i = CurrentY + 1; i < y; i++)
                        if (gb.Board[x, i] != null)
                            count++;
                }
                if (y < CurrentY)
                {
                    //to left
                    count = 0;
                    for (int i = CurrentY - 1; i > y; i--)
                    {
                        if (gb.Board[x, i] != null)
                            count++;
                    }
                }
            }

            //move vertically
            if (y == CurrentY && x != CurrentX)
            {
                //up
                if (x < CurrentX)
                {
                    count = 0;
                    for (int i = CurrentX - 1; i > x; i--)
                        if (gb.Board[i, y] != null)
                            count++;
                }
                //down
                if (x > CurrentX)
                {
                    count = 0;
                    for (int i = CurrentX + 1; i < x; i++)
                        if (gb.Board[i, y] != null)
                            count++;
                }
            }

            //move and eat the piece
            if (count == 1 && gb.Board[x, y] != null)
                return true;
            //just move the Cannon
            if (count == 0 && gb.Board[x, y] == null)
                return true;

            return false;
        }
    }
}
