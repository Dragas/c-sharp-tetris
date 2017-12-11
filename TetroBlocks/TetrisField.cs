using System;
using System.Collections.Generic;

namespace TetroBlocks
{
    public class TetrisField
	{
		public int TilesWidth { get; private set; }
		public int TilesHeight { get; private set; }
		
		protected TileType[,] Tiles;
		
		public TileType this[int row, int col]
		{
			get
			{
				try
				{
					return Tiles[row, col];
				}
				catch(IndexOutOfRangeException)
				{
					return TileType.Wall;
				}
			}
		}
		
		
		public TetrisField(int height, int width)
		{
			TilesWidth=width;
			TilesHeight=height;
			Tiles=new TileType[height, width];
			for(int row=0; row<TilesHeight; row++)
			{
				for(int col=0; col<TilesWidth; col++)
				{
					Tiles[row, col]=TileType.Empty;
				}
			}
		}
		
		public bool SetCell(int row, int col, TileType type)
		{
			try
			{
				Tiles[row, col]=type;
				return true;
			}
			catch(IndexOutOfRangeException)
			{
				return false;
			}
		}
		
		public int SetFigure(Figure f, bool rewrite)
		{
			
			int res=4;
			try
			{
				if(Tiles[f.YC, f.XC]==TileType.Empty || rewrite)
				{
					Tiles[f.YC, f.XC]=f.Type;
				}
				else --res;
			}
			catch(IndexOutOfRangeException)
			{ --res; }
			
			
			try
			{
				if(Tiles[f.Y1, f.X1]==TileType.Empty || rewrite)
				{
					Tiles[f.Y1, f.X1]=f.Type;
				}
				else --res;
			}
			catch(IndexOutOfRangeException)
			{ --res; }
			
			try
			{
				if(Tiles[f.Y2, f.X2]==TileType.Empty || rewrite)
				{
					Tiles[f.Y2, f.X2]=f.Type;
				}
				else --res;
			}
			catch(IndexOutOfRangeException)
			{ --res; }
			
			try
			{
				if(Tiles[f.Y3, f.X3]==TileType.Empty || rewrite)
				{
					Tiles[f.Y3, f.X3]=f.Type;
				}
				else --res;
			}
			catch(IndexOutOfRangeException)
			{ --res; }
			
			return res;
		}
		public bool IsEmpty(Figure f)
		{
			if(this[f.YC, f.XC]!=TileType.Empty) return false;
			if(this[f.Y1, f.X1]!=TileType.Empty) return false;
			if(this[f.Y2, f.X2]!=TileType.Empty) return false;
			if(this[f.Y3, f.X3]!=TileType.Empty) return false;
			return true;
		}
		public bool IsEmpty(int row, int col)
		{
			if(this[row, col]!=TileType.Empty) return false;
			return true;
		}
		protected void EraseFigure(Figure f)
		{
			f.Type=TileType.Empty;
			SetFigure(f, true);
		}
		protected bool MoveDown(int row, int col)
		{
			if(Tiles[row, col]!=TileType.Empty)
			{
				TileType below=Tiles[row+1, col];
				if(below==TileType.Empty)
				{
					Tiles[row+1, col]=Tiles[row, col];
					Tiles[row, col]=TileType.Empty;
				}
				return Tiles[row+2, col]==TileType.Empty; //Still can move it down
			}
			return false;
		}
		protected bool MoveDown(Figure f)
		{
			Figure lower=f.MoveDown();
			
			f.Type=TileType.Empty;
			SetFigure(f, true);
			
			if(IsEmpty(lower))
			{
				// свободно, двигаем вниз
				SetFigure(lower, false);
				return true;
			}
			else
			{
				f.Type=lower.Type;
				SetFigure(f, false);
				return false;
			}
			
			
			/*bool able=CanMoveDown(f);
			if(able)
			{
				MoveDown(f.YC, f.XC); MoveDown(f.Y1, f.X1);
				MoveDown(f.Y2, f.X2); MoveDown(f.Y3, f.X3);
				able=CanMoveDown(f.MoveDown());
			}
			return able;*/
		}
		
		protected bool CanMoveDown(int row, int col)
		{
			return Tiles[row+1, col]==TileType.Empty;
		}
		
		protected bool CanMoveDown(Figure f)
		{
			Figure lower=f.MoveDown();
			
			f.Type=TileType.Empty;
			SetFigure(f, true);
			
			bool able=IsEmpty(lower);
			
			f.Type=lower.Type;
			SetFigure(f, false);
			
			return able;
			
		}
		protected bool MoveRight(int row, int col)
		{
			if(Tiles[row, col]!=TileType.Empty)
			{
				TileType below=Tiles[row, col+1];
				if(below==TileType.Empty)
				{
					Tiles[row, col+1]=Tiles[row, col];
					Tiles[row, col]=TileType.Empty;
				}
				return Tiles[row, col+1]==TileType.Empty; //Still can move it right
			}
			return false;
		}
		protected bool MoveRight(Figure f)
		{
			Figure moved=f.MoveRight();
			f.Type=TileType.Empty;
			SetFigure(f, true);
			if(IsEmpty(moved))
			{
				SetFigure(moved, false);
				return true;
			}
			f.Type=moved.Type;
			SetFigure(f, false);
			return false;
		}
		
		protected bool CanMoveRight(int row, int col)
		{
			return Tiles[row, col+1]==TileType.Empty;
		}
		protected bool CanMoveRight(Figure f)
		{
			return CanMoveRight(f.YC, f.XC) && CanMoveRight(f.Y1, f.X1)
				 && CanMoveRight(f.Y2, f.X2) && CanMoveRight(f.Y3, f.X3);
		}

		protected bool MoveLeft(int row, int col)
		{
			if(Tiles[row, col]!=TileType.Empty)
			{
				TileType below=Tiles[row, col-1];
				if(below==TileType.Empty)
				{
					Tiles[row, col-1]=Tiles[row, col];
					Tiles[row, col]=TileType.Empty;
				}
				return Tiles[row, col-1]==TileType.Empty; //Still can move it left
			}
			return false;
		}
		
		protected bool MoveLeft(Figure f)
		{
			Figure moved=f.MoveLeft();
			f.Type=TileType.Empty;
			SetFigure(f, true);
			if(IsEmpty(moved))
			{
				SetFigure(moved, false);
				return true;
			}
			f.Type=moved.Type;
			SetFigure(f, false);
			return false;
		}
		
		public bool CanMoveLeft(int row, int col)
		{
			return Tiles[row, col+1]==TileType.Empty;
		}
		public bool CanMoveLeft(Figure f)
		{
			return CanMoveLeft(f.YC, f.XC) && CanMoveLeft(f.Y1, f.X1)
				 && CanMoveLeft(f.Y2, f.X2) && CanMoveLeft(f.Y3, f.X3);
		}
		
		protected Figure RotateFigure(Figure f)
		{
			Figure rotated=f.Rotate(), rotated2;
			f.Type=TileType.Empty;
			SetFigure(f, true);
			f.Type=rotated.Type;
			
			if(IsEmpty(rotated))
			{
				SetFigure(rotated, false);
				return rotated;
			}
			//неудача, фигура наткнулась на препятствие, нужно сместить её
			//вниз
			rotated2=rotated.MoveDown();
			if(IsEmpty(rotated2))
			{
				SetFigure(rotated2, false);
				return rotated2;
			}
			//вправо
			rotated2=rotated.MoveRight();
			if(IsEmpty(rotated2))
			{
				SetFigure(rotated2, false);
				return rotated2;
			}
			//влево
			rotated2=rotated.MoveLeft();
			if(IsEmpty(rotated2))
			{
				SetFigure(rotated2, false);
				return rotated2;
			}
			//тотальная неудача, я сдаюсь
			SetFigure(f, false);
			return Figure.Zero;
		}
		
		public int RemoveFullRows()
		{
			//Список заполненных рядов к удалению
			List<int> FullRows=new List<int>();
			
			for(int row=0; row<TilesHeight; row++)
			{
				bool fullrow=true;
				for(int col=0; col<TilesWidth; col++)
				{
					if(Tiles[row, col]==TileType.Empty)
					{
						fullrow=false;
						break;
					}
				}
				if(fullrow)
				{
					FullRows.Add(row);
				}
			}
			
			foreach(int frow in FullRows)
			{
				for(int row=frow-1; row>0; row--)
				{
					for(int col=0; col<TilesWidth; col++)
					{
						Tiles[row+1, col]=Tiles[row, col];
						if(IsRowEmpty(row+1)) 
							break;
					}
				}
			}
			
			return TilesWidth*FullRows.Count;
		}
		
		private bool IsRowEmpty(int row)
		{
			for(int col=0; col<TilesWidth; col++)
			{
				if(Tiles[row, col]!=TileType.Empty)
					return false;
			}
			return true;
		}
		
		public virtual void Clear()
		{
			for(int row=0; row<TilesHeight; row++)
			{
				for(int col=0; col<TilesWidth; col++)
				{
					SetCell(row, col, TileType.Empty);
				}
			}
		}
	}
}