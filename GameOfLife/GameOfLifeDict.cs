using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    internal class GameOfLifeDict : IGameOfLife
    {
        private HashSet<Point> currentCells;
        private readonly Point[] offsets;
        private readonly int[] dy;
        private HashSet<Point> previousCells;
        private int cellsPerRow;
        private int pixelsPerCell;
        public Size GridSize { get; set; }
        public GameOfLifeDict()
        {
            offsets = [new(-1, -1), new(-1, 0), new(-1, 1), new(0, 1), new(0, -1), new(1, -1), new(1, 0), new(1, 1)];
            currentCells = [new(20, 20), new(21, 20), new(22, 20), new(22, 21), new(21, 19)];
            previousCells = [];
            cellsPerRow = 200;
            GridSize = new(800, 800);
            pixelsPerCell = GridSize.Width / cellsPerRow;
        }
        public void DrawGrid(Graphics graphics, Brush brush)
        {
            foreach (var cell in currentCells)
            {
                graphics.FillRectangle(brush, new Rectangle(cell.Y * pixelsPerCell, cell.X * pixelsPerCell, pixelsPerCell, pixelsPerCell));
            }
        }
        public void UpdateGrid()
        {
            previousCells = [.. currentCells];
            currentCells.Clear();
            foreach (Point cell in previousCells)
            {
                int count = CountNeighbors(cell, previousCells);
                if (count == 2 || count == 3)
                {
                    currentCells.Add(cell);
                }
                foreach (Point offset in offsets)
                {
                    Point copiedCell = new(cell.X, cell.Y);
                    copiedCell.Offset(offset);
                    count = CountNeighbors(copiedCell, previousCells);
                    if (count == 3)
                    {
                        currentCells.Add(copiedCell);
                    }
                }
            }
        }
        private int CountNeighbors(Point cell, HashSet<Point> cells)
        {
            int count = 0;
            foreach (var offset in offsets)
            {
                Point copiedCell = new(cell.X, cell.Y);
                copiedCell.Offset(offset);
                if (previousCells.Contains(copiedCell))
                {
                    count += 1;
                }
            }
            return count;
        }
    }
}
