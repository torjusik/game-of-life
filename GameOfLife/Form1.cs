using System.Drawing;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer redrawTimer;
        SolidBrush myBrush;
        IGameOfLife gameOfLife;

        public Form1(IGameOfLife gameOfLife)
        {
            InitializeComponent();
            myBrush = new SolidBrush(Color.White);
            OptimizeDrawing();
            redrawTimer = new();
            InitializeRedrawTimer();
            this.gameOfLife = gameOfLife;
            Size = gameOfLife.GridSize;
        }

        private void InitializeRedrawTimer()
        {
            redrawTimer.Interval = 1; // ~60 FPS
            redrawTimer.Tick += (s, e) => gameOfLife.UpdateGrid();
            redrawTimer.Tick += (s, e) => Invalidate();
            redrawTimer.Start();
        }

        private void OptimizeDrawing()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            gameOfLife.DrawGrid(graphics, myBrush);

        }
    }
}
