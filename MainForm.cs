/*
 * Created by SharpDevelop.
 * User: user
 * Date: 08.10.2013
 * Time: 2:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using TetroBlocks;

namespace Tetris
{
	/// <summary>
	/// Главная форма
	/// </summary>
	public partial class MainForm : Form
	{
		private GameField GF;
		private GameField Preview;
		private TetrisGame Game;
		public static Bitmap Red, Green, Blue, Yellow, Orange, Purple, LightBlue;
		
		private Image PausedImage, GameOverImage;
		
		public MainForm()
		{
			Game=new TetrisGame();
			Game.Score=0; Game.GameOver=true;
			
			Saver.Load();
			
			GF=new GameField(18, 12);
			
			Preview=new GameField(4, 4);
			Preview.BorderColor=Preview.BackColor;
			
			Random rnd=new Random();
			
			// Типа сплэш
			for(int row=0; row<GF.TilesHeight; row++)
			{
				for(int col=0; col<GF.TilesWidth; col++)
				{
					TileType t=(TileType)rnd.Next(0, 7);
					GF.SetCell(row, col, t);
				}
			}
			
			Game.StateChanged+=new EventHandler(Game_StateChanged);
			
			InitializeComponent();
			
		}

		void Game_StateChanged(object sender, EventArgs e)
		{
			Refresh();
		}
		
		private void SetScore(int nscore)
		{
			Game.Score=nscore;
			//ScoreLabel.Text=Score.ToString();
		}
		
		private void NewGame()
		{
			Game=new TetrisGame();
			Game.StateChanged+= new EventHandler(Game_StateChanged);
			SetScore(0);
			
			GameTimer.Interval=1000;
			GameTimer.Enabled=true;
			
			Game.NextFigure=Figure.RandomFigure();
			
			GF.Clear();
			
			Refresh();
		}
		
		private void SetPause(bool enable)
		{
			if(Game.GameOver) return;
			Game.Paused=enable;
			
			GameTimer.Enabled=!enable;
			
			//Refresh();
		}
		
		// Игровой цикл
		void GameTimerTick(object sender, EventArgs e)
		{
			if(Game.Paused) return;
			
			GF.DoStep();
			
			if(!GF.IsFigureFalling) 
			{
				//нужно поместить новую фигуру на поле и скрыть полные ряды
				SetScore(Game.Score+GF.RemoveFullRows()*10);
				
				if(!GF.PlaceFigure(Game.NextFigure))
				{
					//игра окончена
					OnGameOver();
				}
				else
				{
					Game.NextFigure=Figure.RandomFigure();
					Game.FiguresDropped++;
					Preview.Clear();
					Preview.SetFigure(Game.NextFigure.MoveTo(1, 1), false);
					
					if(Game.FigureChanged && Game.FiguresDropped%5==0) Game.FigureChanged=false;
					//ускоряем игру при росте количества очков
					if(Game.FiguresDropped%15==0 && Game.Score!=0)
					{
						if(GameTimer.Interval>300)
						{
							//GameTimer.Enabled=false;
							GameTimer.Interval-=100;
							//GameTimer.Enabled=true;
						}
					}
				}
			}
			
			Refresh();
		}
		
		private void OnGameOver()
		{
			Game.Over();
			GameTimer.Enabled=false;
			
			TetrisSave test=new TetrisSave("---", Game.Score, DateTime.Now-Game.GameStarted);
			
			if(Saver.HighScores.CanAdd(test) || Saver.HighTimes.CanAdd(test))
			{
				Saver.Save(test);
			}
		}
		// Обработка ввода
		void MainFormKeyDown(object sender, KeyEventArgs e)
		{
			e.SuppressKeyPress=true;
			
			if(Game.GameOver || Game.Paused) return;
			
			if(e.KeyData==Keys.Left || e.KeyData==Keys.A)
			{
				GF.MoveLeft();
			}
			if(e.KeyData==Keys.Right || e.KeyData==Keys.D)
			{
				GF.MoveRight();
			}
			if(e.KeyData == Keys.Space)
			{
				if(GF.Drop())
					SetScore(Game.Score+5);
			}
			if(e.KeyData==Keys.Down || e.KeyData==Keys.S)
			{
				if(GF.MoveDown())
					SetScore(Game.Score+1);
			}
			if(e.KeyData==Keys.W || e.KeyData == Keys.Up)
			{
				GF.RotateFigure();
			}
			
			if(e.KeyData==Keys.Q)
			{
				if(!Game.FigureChanged && GF.IsFigureFalling)
				{
					Game.NextFigure=new Figure(GF.ChangeFigure(Game.NextFigure).Type);
					Preview.Clear();
					Preview.SetFigure(Game.NextFigure.MoveTo(1, 1), false);
					Game.FigureChanged=true;
					if(Game.NextFigure==Figure.Zero)
						OnGameOver();
				}
			}
			Refresh();
		}

		
		void GameFieldPictureBoxPaint(object sender, PaintEventArgs e)
		{
			GF.Paint(e.Graphics);
			
			if(Game.Paused)
			{
				Rectangle img=new Rectangle((GameFieldPictureBox.Width-PausedImage.Width)/2,
				                            (GameFieldPictureBox.Height-PausedImage.Height)/2,
				                            PausedImage.Width, PausedImage.Height);
				e.Graphics.DrawImage(PausedImage, img);
				return;
			}
			if(Game.GameOver)
			{
				Rectangle img=new Rectangle((GameFieldPictureBox.Width-GameOverImage.Width)/2,
				                            (GameFieldPictureBox.Height-GameOverImage.Height)/2,
				                            GameOverImage.Width, GameOverImage.Height);
				e.Graphics.DrawImage(GameOverImage, img);
			}
		}
		
		void НоваяИграToolStripMenuItemClick(object sender, EventArgs e)
		{
			NewGame();
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			//string[] res = GetType().Assembly.GetManifestResourceNames();
			
			try
			{
				Blue=new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream("ImBLUE"));
				Red=new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream("ImRED"));
				Green=new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream("ImGREEN"));
				LightBlue=new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream("ImLBLUE"));
				Purple=new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream("ImPURPLE"));
				Yellow=new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream("ImYELLOW"));
				Orange=new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream("ImORANGE"));
				
				PausedImage=new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream("ImPAUSE"));
				GameOverImage=new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream("ImGAMEOVER"));
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка при загрузке изображений!");
			}
			
			//MessageBox.Show(string.Join(", ", res));
		}		
		
		void ПаузапродолжитьToolStripMenuItemClick(object sender, EventArgs e)
		{
			SetPause(!Game.Paused);
		}
		
		void ВыходToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
	}
}
