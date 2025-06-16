using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class GameOfLifeDict : IGameOfLife
    {
        private HashSet<Point> currentCells;
        private readonly Point[] offsets;
        private HashSet<Point> previousCells;
        private HashSet<Point> checkedCells;
        private int cellsPerRow;
        private int pixelsPerCell;
        public Size GridSize { get; set; }
        public GameOfLifeDict()
        {
            offsets = [new(-1, -1), new(-1, 0), new(-1, 1), new(0, 1), new(0, -1), new(1, -1), new(1, 0), new(1, 1)];
            currentCells = [new(20, 20), new(21, 20), new(22, 20), new(22, 21), new(21, 19)]; //adds a r-pentomino polyomino
            previousCells = [];
            //makes another offset structure of the currentCells structure
            foreach (var cell in currentCells)
            {
                Point copiedCell = cell;
                copiedCell.Offset(100, 0);
                previousCells.Add(copiedCell);
            }
            currentCells = [.. currentCells, .. previousCells]; 
            previousCells = [];
            checkedCells = [];
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
            checkedCells.Clear();
            foreach (Point cell in previousCells)
            {
                int count = CountNeighbors(cell, previousCells);
                if (count == 2 || count == 3)
                {
                    currentCells.Add(cell);
                }
                checkedCells.Add(cell);
                foreach (Point offset in offsets)
                {
                    Point copiedCell = new(cell.X, cell.Y);
                    copiedCell.Offset(offset);
                    if (checkedCells.Contains(copiedCell))
                    {
                        //skip checking neighbors if checked already
                        continue;
                    }

                    count = CountNeighbors(copiedCell, previousCells);
                    if (count == 3)
                    {
                        currentCells.Add(copiedCell);
                    }
                    checkedCells.Add(copiedCell);
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
                    //case rarely happens so not sure if worth checking for
                    //if (count >= 4)
                    //{
                    //    return count;
                    //}
                }
            }
            return count;
        }
    }
}
