namespace GameOfLife
{
    public interface IGameOfLife
    {
        public Size GridSize { get; set; }
        internal void DrawGrid(Graphics graphics, Brush brush);
        internal void UpdateGrid();

    }
}