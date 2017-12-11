using System;

namespace TetroBlocks
{
    public class TetrisGame
    {
        public Figure NextFigure;
		
        private int score;
        public int Score
        {
            get { return score; }
            set
            {
                score=value;
                OnStateChanged();
            }
        }
		
        private int figDropped;
        public int FiguresDropped
        {
            get { return figDropped; }
            set
            {
                figDropped=value;
                OnStateChanged();
            }
        }
		
        private bool gameOver, paused, figChanged;
        public bool GameOver
        {
            get { return gameOver; }
            set
            {
                gameOver=value;
                OnStateChanged();
            }
        }
        public bool Paused
        {
            get { return paused; }
            set
            {
                if(!paused && value)
                {
                    GamePaused=DateTime.Now;
                    paused=value;
                    OnStateChanged();
                }
                if(paused && !value)
                {
                    GameStarted=GameStarted+(DateTime.Now-GamePaused);
                    paused=value;
                    OnStateChanged();
                }
            }
        }
        public bool FigureChanged
        {
            get { return figChanged; }
            set
            {
                figChanged=value;
                OnStateChanged();
            }
        }
		
        public DateTime GameStarted, GamePaused;
		
		
        /// <summary>
        /// Создаёт новый экземпляр TetrisGame, готовый к началу игры
        /// </summary>
        public TetrisGame()
        {
            Score=0; FiguresDropped=0;
            NextFigure=Figure.RandomFigure();
            GameOver=false; Paused=false; FigureChanged=false;
            GameStarted=DateTime.Now;
        }
		
        /// <summary>
        /// Завершает игру
        /// </summary>
        public void Over()
        {
            GameOver=true;
        }
		
		
        public event EventHandler StateChanged;
        protected virtual void OnStateChanged()
        {
            if (StateChanged != null)
            {
                StateChanged(this, new EventArgs());
            }
        }
    }
}