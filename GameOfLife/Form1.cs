namespace GameOfLife
{
    public partial class Form1 : Form
    {
        SolidBrush myBrush;
        Dictionary<Point, bool> grid;
        Dictionary<Point, bool> gridOld;
        System.Windows.Forms.Timer redrawTimer;
        int cellsPerRow;
        int pixelsPerCell;
        Size GridSize;
        public Form1()
        {
            InitializeComponent();
            myBrush = new SolidBrush(Color.Black);
            OptimizeDrawing();
            InitializeRedrawTimer();
            grid = new();
            gridOld = new();
            cellsPerRow = 100;
            GridSize = new(600, 600);
            Size = GridSize;
            pixelsPerCell = GridSize.Width / cellsPerRow;
            grid.Add(new Point(5, 5), true);
            grid.Add(new Point(5, 6), true);
            grid.Add(new Point(5, 7), true);
            grid.Add(new Point(6, 5), true);
            grid.Add(new Point(7, 5), true);
            grid.Add(new Point(8, 5), true);
        }

        private void InitializeRedrawTimer()
        {
            redrawTimer = new();
            redrawTimer.Interval = 1000; // ~60 FPS
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
            foreach (var keyValuePair in gridOld)
            {
                Point coords = keyValuePair.Key;
                graphics.FillRectangle(myBrush, new Rectangle(coords.X, coords.Y, pixelsPerCell, pixelsPerCell));
            }
        }

        private void UpdateGrid()
        {
            gridOld = new Dictionary<Point, bool>(grid);
            grid.Clear();

            foreach (var keyValuePair in gridOld)
            {
                Point coords = keyValuePair.Key;
                int count = CountNeighbours(gridOld, coords);
                if (count > 0) {
            }
        }

        private int CountNeighbours(Dictionary<Point, bool> gridOld, Point coords)
        {
            int count = 0;
            int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                Point neighbor = new Point(coords.X + dx[i], coords.Y + dy[i]);
                count += gridOld.GetValueOrDefault(neighbor) ? 1 : 0;
            }
            return count;
        }
    }
}
