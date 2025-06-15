using System.Drawing;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        SolidBrush myBrush;

        System.Windows.Forms.Timer redrawTimer;
        GameOfLifeGrid gameOfLifeGrid;

        public Form1()
        {
            InitializeComponent();
            myBrush = new SolidBrush(Color.White);
            OptimizeDrawing();
            InitializeRedrawTimer();
            gameOfLifeGrid = new GameOfLifeGrid();
            Size = gameOfLifeGrid.GridSize;
        }

        private void InitializeRedrawTimer()
        {
            redrawTimer = new();
            redrawTimer.Interval = 1; // ~60 FPS
            redrawTimer.Tick += (s, e) => gameOfLifeGrid.UpdateGrid();
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
            gameOfLifeGrid.DrawGrid(graphics, myBrush);

        }
    }
}
