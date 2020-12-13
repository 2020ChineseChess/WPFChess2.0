using System;
using System.Collections.Generic;
using System.Text;
using XIANG_QI_TRANSFER.GameBorads;

namespace XIANG_QI_TRANSFER.Pieces
{
    class PieceElephant : Piece
    {
        public PieceElephant(Team player, int x, int y) : base(player, x, y)
        {
            if (this.Player == Team.black)
            {
                this.Name = '象';
            }
            else
            {
                this.Name = '相';
            }


            Path = Player + "Elephant.png";
        }

        public override bool ValidMoves(int x, int y, GameBoard gb)
        {
            int CurrentX = this.X;
            int CurrentY = this.Y;


            //the elephant could not over river;
            if (CurrentX <= 5)
                if (x > 5)
                    return false;

            if (CurrentX >= 6)
                if (x < 5)
                    return false;


            // judge if a rule is followed by a subrule
            if (x - CurrentX == 2 || x - CurrentX == -2)
            {
                if (y - CurrentY == 2 || y - CurrentY == -2)
                {
                    //  Whether there are pieces blocking elephant 
                    if (gb.Board[(x + CurrentX) / 2, (y + CurrentY) / 2] == null)
                    {
                        //judge is this a piece
                        if (gb.Board[x, y] != null)
                        {
                            //  Whether the pieces are owner
                            if (gb.Board[x, y].Player == this.Player)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
