using System;

namespace TetroBlocks
{
    public struct Figure
    {
        //ячейки фигуры
        public int XC { get; private set; }
        public int YC { get; private set; }
		
        public int X1 { get; private set; }
        public int Y1 { get; private set; }
		
        public int X2 { get; private set; }
        public int Y2 { get; private set; }
		
        public int X3 { get; private set; }
        public int Y3 { get; private set; }
		
        public TileType Type;
		
        public static readonly Figure Zero=new Figure(TileType.Empty);
		
		
        public Figure(TileType type):this()
        {
            Type=type;
            XC=0;
            YC=0;
            switch(type)
            {
                //создаём форму фигуры согласно её цвету
                case TileType.IBlock: // I
                    X1=XC-1; X2=XC+1; X3=XC+2;
                    Y1=YC; Y2=YC; Y3=YC;
                    break;
                case TileType.LBlock: // L
                    X1=XC-1; X2=XC-1; X3=XC+1;
                    Y1=YC+1; Y2=YC; Y3=YC;
                    break;
                case TileType.ZBlock: // Z
                    X1=XC-1; X2=XC; X3=XC+1;
                    Y1=YC; Y2=YC+1; Y3=YC+1;
                    break;
                case TileType.ReverseLBlock: // Г
                    X1=XC-1; X2=XC+1; X3=XC+1;
                    Y1=YC; Y2=YC; Y3=YC+1;
                    break;
                case TileType.TBlock: // T
                    X1=XC-1; X2=XC; X3=XC+1;
                    Y1=YC; Y2=YC+1; Y3=YC;
                    break;
                case TileType.SBlock: // S
                    X1=XC-1; X2=XC; X3=XC+1;
                    Y1=YC+1; Y2=YC+1; Y3=YC;
                    break;
                case TileType.SquareBlock: // [ ]
                    X1=XC+1; X2=XC; X3=XC+1;
                    Y1=YC; Y2=YC+1; Y3=YC+1;
                    break;
                case TileType.Empty: // zero
                    X3=X2=X1=XC=0;
                    Y3=Y2=Y1=YC=0;
                    break;
                default:
                    X3=X2=X1=XC=0;
                    Y3=Y2=Y1=YC=0;
                    break;
            }
        }
		
        /// <summary>
        /// Смещает фигуру вниз
        /// </summary>
        /// <returns>Смещённую фигуру</returns>
        public Figure MoveDown()
        {
            return MoveTo(YC+1, XC);
        }
        /// <summary>
        /// Смещает фигуру вверх
        /// </summary>
        /// <returns>Смещённую фигуру</returns>
        public Figure MoveUp()
        {
            return MoveTo(YC-1, XC);
        }
		
        /// <summary>
        /// Смещает фигуру вправо
        /// </summary>
        /// <returns>Смещённую фигуру</returns>
        public Figure MoveRight()
        {
            return MoveTo(YC, XC+1);
        }
		
        /// <summary>
        /// Смещает фигуру влево
        /// </summary>
        /// <returns>Смещённую фигуру</returns>
        public Figure MoveLeft()
        {
            return MoveTo(YC, XC-1);
        }
		
        /// <summary>
        /// Перемещает фигуру в положение x=col, y=row
        /// </summary>
        /// <returns>Перемещённую фигуру</returns>
        public Figure MoveTo(int row, int col)
        {
            int dx=col-XC, dy=row-YC;
            Figure res=new Figure(this.Type);
            res.XC=col; res.YC=row;
            res.X1=X1+dx; res.Y1=Y1+dy;
            res.X2=X2+dx; res.Y2=Y2+dy;
            res.X3=X3+dx; res.Y3=Y3+dy;
            return res;
        }
		
//======[ Поворот ]===

        //формулы для поворота клеток относительно центра
        //(немного математики)
        private int RotateCol(int col)
        {
            return YC-XC+col;
        }
        private int RotateRow(int row)
        {
            return XC-row+YC;
        }

        /// <summary>
        /// Осуществляет поворот фигуры по часовой стрелке на 90 градусов
        /// </summary>
        /// <returns>Повёрнутую фигуру</returns>
        public Figure Rotate()
        {
            Figure res=Clone();
            res.X1=RotateRow(Y1); res.Y1=RotateCol(X1);
            res.X2=RotateRow(Y2); res.Y2=RotateCol(X2);
            res.X3=RotateRow(Y3); res.Y3=RotateCol(X3);
            return res;
        }
				
        public static bool operator ==(Figure f1, Figure f2)
        {
            return f1.Type==f2.Type && f1.XC==f2.XC && f1.YC==f2.YC &&
                   f1.X1==f2.X1 && f1.X2==f2.X2 && f1.X3==f2.X3 && 
                   f1.Y1==f2.Y1 && f1.Y2==f2.Y2 && f1.Y3==f2.Y3;
        }
        public static bool operator !=(Figure f1, Figure f2)
        {
            return !(f1==f2);
        }
		
        private Figure Clone()
        {
            Figure res=new Figure(this.Type);
            res.XC=XC; res.YC=YC; res.X1=X1; res.Y1=Y1;
            res.X2=X2; res.Y2=Y2; res.X3=X3; res.Y3=Y3;
            return res;
        }
		
		
        private static Random rnd=new Random();
        /// <summary>
        /// Возвращает случайную фигуру
        /// </summary>
        public static Figure RandomFigure()
        {
            return new Figure((TileType)rnd.Next(1, 8));
        }
    }
    
    public enum TileType { Empty, SBlock, ZBlock, IBlock, SquareBlock, ReverseLBlock, TBlock, LBlock, Wall }
}