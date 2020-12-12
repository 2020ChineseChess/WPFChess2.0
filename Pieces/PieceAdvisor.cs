using System;
using System.Collections.Generic;
using System.Text;
using XIANG_QI_TRANSFER.GameBorads;

namespace XIANG_QI_TRANSFER.Pieces
{
    class PieceAdvisor : Piece
    {
        public PieceAdvisor(Team Player, int intX, int intY) : base(Player, intX, intY)
        {
            if (this.Player == Team.black)
            {
                this.Name = '士';
            }
            else
            {
                this.Name = '仕';
            }
        }

        public override bool ValidMoves(int x, int y, GameBoard gb)
        {
            {
                int CurrentX = this.X;
                int CurrentY = this.Y;

                //judge the player is the black or red
                if (this.Player == Team.black)
                {
                    //judge the destination if is in the 3*3 grid
                    if (x <= 2 && x >= 0 && y <= 5 && y >= 3)
                    {
                        //judge the move if conforms to the diagonal rule
                        if ((x - CurrentX == 1 || x - CurrentX == -1) && (y - CurrentY == 1 || y - CurrentY == -1))
                        {
                            //judge is this a piece
                            if (gb.Board[x, y] != null)
                            {
                                //Whether the pieces are owner
                                if (gb.Board[x, y].Player == this.Player)
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }
                }
                else if (this.Player == Team.red)
                {
                    //  judge if the end point is in the meter grid
                    if (x <= 9 && x >= 7 && y <= 5 && y >= 3)
                    {
                        if ((x - CurrentX == 1 || x - CurrentX == -1) && (y - CurrentY == 1 || y - CurrentY == -1))
                        {
                            //judge is this a piece
                            if (gb.Board[x, y] != null)
                            {
                                //Whether the pieces are owner
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
}
