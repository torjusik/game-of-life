using System.Drawing;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        SolidBrush myBrush;
        bool[,] grid;
        bool[,] gridOld;
        System.Windows.Forms.Timer redrawTimer;
        int cellsPerRow;
        int pixelsPerCell;
        Size GridSize;
        public Form1()
        {
            InitializeComponent();
            myBrush = new SolidBrush(Color.White);
            OptimizeDrawing();
            InitializeRedrawTimer();
            cellsPerRow = 100;
            GridSize = new(600, 600);
            Size = GridSize;
            grid = new bool[cellsPerRow, cellsPerRow];
            gridOld = new bool[cellsPerRow, cellsPerRow];
            pixelsPerCell = GridSize.Width / cellsPerRow;
            grid[20, 20] = true;
            grid[21, 20] = true;
            grid[22, 20] = true;
            grid[22, 21] = true;
            grid[21, 19] = true;


        }

        private void InitializeRedrawTimer()
        {
            redrawTimer = new();
            redrawTimer.Interval = 100; // ~60 FPS
            redrawTimer.Tick += (s, e) => UpdateGrid();
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
            DrawGrid(graphics);

        }

        private void DrawGrid(Graphics graphics)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j])
                    {
                        graphics.FillRectangle(myBrush, new Rectangle(j*pixelsPerCell, i*pixelsPerCell, pixelsPerCell, pixelsPerCell));
                    }
                }
            }
        }

        private void UpdateGrid()
        {
            Buffer.BlockCopy(grid, 0, gridOld, 0, grid.Length);
            grid = new bool[cellsPerRow, cellsPerRow];

            for (int i = 0; i < gridOld.GetLength(0); i++)
            {
                for (int j = 0; j < gridOld.GetLength(1); j++)
                {
                    int count = CountNeighbours(gridOld, new Point(i, j));
                    if (gridOld[i, j])
                    {
                        if (count < 2 || count > 3)
                        {
                        }
                        else
                        {
                            grid[i, j] = true;
                        }
                    }
                    else if (count == 3)
                    {
                        grid[i, j] = true;
                    }
                }
            }
        }

        private int CountNeighbours(bool[,] gridOld, Point coords)
        {
            int count = 0;
            int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                Point neighbor = new Point(
                    (coords.X + dx[i] + cellsPerRow) % cellsPerRow,
                    (coords.Y + dy[i] + cellsPerRow) % cellsPerRow
                );
                count += gridOld[neighbor.X, neighbor.Y] ? 1 : 0;
            }
            return count;
        }
    }
}
