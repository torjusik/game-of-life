using System.Drawing;
using System.Threading;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private SolidBrush myBrush;
        private IGameOfLife gameOfLife;
        private Thread gameThread;
        private bool isRunning = true;
        private readonly object lockObject = new object();
        public bool Simulating { get; set; }

        public Form1(IGameOfLife gameOfLife)
        {
            InitializeComponent();
            myBrush = new SolidBrush(Color.White);
            OptimizeDrawing();
            this.gameOfLife = gameOfLife;
            Size = gameOfLife.GridSize;

            FormClosing += Form1_FormClosing;

            // Start the game update thread after the form is loaded
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Start the game update thread after the form is fully loaded
            StartGameThread();
        }

        private void StartGameThread()
        {
            gameThread = new Thread(GameLoop)
            {
                IsBackground = true
            };
            gameThread.Start();
        }

        private void GameLoop()
        {
            while (isRunning)
            {
                if (Simulating)
                {
                    lock (lockObject)
                    {
                        gameOfLife.UpdateGrid();
                    }

                    Invoke(new Action(() => Invalidate()));
                }

                Thread.Sleep(1); // ~60 FPS
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
            gameThread?.Join(1000); // Wait up to 1 second for thread to finish
        }

        private void OptimizeDrawing()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            lock (lockObject)
            {
                var graphics = e.Graphics;
                gameOfLife.DrawGrid(graphics, myBrush);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            Simulating = !Simulating;
        }
    }
}
