using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TetroBlocks;

namespace Tetris
{
    public class GameField : TetrisField
    {
        public Color BackColor, BorderColor;
        public Figure Current;

        public bool IsFigureFalling { get; private set; }

        public bool ShowTips;


        public GameField(int height, int width) : base(height, width)
        {
            BorderColor = Color.FromArgb(192, 192, 192);
            BackColor = Color.FromArgb(240, 240, 240);
            IsFigureFalling = false;
            ShowTips = true;
            Current = Figure.Zero;
        }
        public bool PlaceFigure(Figure f)
        {
            f = f.MoveTo(0, TilesWidth / 2 - 1);
            int scs = SetFigure(f, false);
            Current = f;
            if (scs != 4) //game over!
                return false;
            IsFigureFalling = true;
            return true;
        }

        public Figure ChangeFigure(Figure nfig)
        {
            if (Current == Figure.Zero) return Current;
            Figure old = Current;
            EraseFigure(old);
            if (!PlaceFigure(nfig))
                return Figure.Zero;
            return old;
        }
        public bool RotateFigure()
        {
            if (Current == Figure.Zero) return false;
            Figure t = RotateFigure(Current);
            if (t != Figure.Zero)
            {
                Current = t;
                return true;
            }
            return false;
        }
        public bool MoveLeft()
        {
            if (Current == Figure.Zero) return false;
            if (MoveLeft(Current))
            {
                Current = Current.MoveLeft();
                return true;
            }
            return false;
        }

        public bool MoveRight()
        {
            if (Current == Figure.Zero) return false;
            if (MoveRight(Current))
            {
                Current = Current.MoveRight();
                return true;
            }
            return false;
        }
        public bool MoveDown()
        {
            if (Current == Figure.Zero) return false;
            if (MoveDown(Current))
            {
                Current = Current.MoveDown();
                return true;
            }
            return false;
        }
        public bool Drop()
        {
            if (Current == Figure.Zero) return false;
            while (Current != Figure.Zero)
                DoStep();
            return true;
        }

        public void DoStep()
        {
            if (Current != Figure.Zero)
            {
                IsFigureFalling = MoveDown(Current); 
                if (IsFigureFalling) 
                {
                    Current = Current.MoveDown(); 
                }
                else
                    Current = Figure.Zero;
            }
            else
                IsFigureFalling = false;
        }

        public override void Clear()
        {
            base.Clear();

            Current = Figure.Zero;
            IsFigureFalling = false;
        }

        public void Paint(Graphics g)
        {
            Pen border = new Pen(BorderColor, 2F);
            SolidBrush fone = new SolidBrush(BackColor);

            g.DrawRectangle(border, 4, 4, TilesWidth * TileSide + 2, TilesHeight * TileSide + 2);
            g.FillRectangle(fone, 5, 5, TilesWidth * TileSide, TilesHeight * TileSide);

            for (int row = 0; row < TilesHeight; row++)
            {
                for (int col = 0; col < TilesWidth; col++)
                {
                    Rectangle tile = new Rectangle(5 + col * TileSide, 5 + row * TileSide, TileSide, TileSide);
                    switch (Tiles[row, col])
                    {
                        case TileType.IBlock:
                            if (MainForm.Blue == null) g.FillRectangle(Brushes.Blue, tile);
                            else g.DrawImage(MainForm.Blue, tile);
                            break;
                        case TileType.ZBlock:
                            if (MainForm.Green == null) g.FillRectangle(Brushes.Green, tile);
                            else g.DrawImage(MainForm.Green, tile);
                            break;
                        case TileType.SquareBlock:
                            if (MainForm.Yellow == null) g.FillRectangle(Brushes.Yellow, tile);
                            else g.DrawImage(MainForm.Yellow, tile);
                            break;
                        case TileType.TBlock:
                            if (MainForm.Purple == null) g.FillRectangle(Brushes.Purple, tile);
                            else g.DrawImage(MainForm.Purple, tile);
                            break;
                        case TileType.ReverseLBlock:
                            if (MainForm.Orange == null) g.FillRectangle(Brushes.Orange, tile);
                            else g.DrawImage(MainForm.Orange, tile);
                            break;
                        case TileType.SBlock:
                            if (MainForm.Red == null) g.FillRectangle(Brushes.Red, tile);
                            else g.DrawImage(MainForm.Red, tile);
                            break;
                        case TileType.LBlock:
                            if (MainForm.LightBlue == null) g.FillRectangle(Brushes.LightBlue, tile);
                            else g.DrawImage(MainForm.LightBlue, tile);
                            break;
                    }
                }
            }

            if (ShowTips && IsFigureFalling)
            {
                Figure tip = Current;
                EraseFigure(Current);

                while (IsEmpty(tip)) 
                {
                    tip = tip.MoveDown();
                }
                tip = tip.MoveUp();

                SetFigure(Current, false);

                Point[] cells = new Point[]
                {
                    new Point(tip.XC, tip.YC), new Point(tip.X1, tip.Y1),
                    new Point(tip.X2, tip.Y2), new Point(tip.X3, tip.Y3)
                };

                SolidBrush b = new SolidBrush(Color.FromArgb(32, 192, 0, 0));
                Pen p = new Pen(Color.FromArgb(128, 192, 0, 0), 1);

                foreach (Point cell in cells)
                {
                    if (!IsEmpty(cell.Y, cell.X)) continue;
                    Rectangle tile = new Rectangle(6 + cell.X * TileSide, 6 + cell.Y * TileSide, TileSide - 2,
                        TileSide - 2);
                    g.FillRectangle(b, tile);
                }
            }
        }       
        public const int TileSide=20;
    }
}