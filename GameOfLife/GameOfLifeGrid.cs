using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    internal class GameOfLifeGrid
    {
        bool[,] grid;
        bool[,] gridOld;
        private int cellsPerRow;
        private int pixelsPerCell;
        public Size GridSize;
        private int[] dx;
        private int[] dy;
        public GameOfLifeGrid()
        {
            cellsPerRow = 200;
            GridSize = new(800, 800);
            pixelsPerCell = GridSize.Width / cellsPerRow;

            grid = new bool[cellsPerRow, cellsPerRow];
            gridOld = new bool[cellsPerRow, cellsPerRow];

            grid[20, 20] = true;
            grid[21, 20] = true;
            grid[22, 20] = true;
            grid[22, 21] = true;
            grid[21, 19] = true;

            dx = [-1, 0, 1, -1, 1, -1, 0, 1 ];
            dy = [-1, -1, -1, 0, 0, 1, 1, 1 ];
        }
        internal void DrawGrid(Graphics graphics, Brush brush)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j])
                    {
                        graphics.FillRectangle(brush, new Rectangle(j * pixelsPerCell, i * pixelsPerCell, pixelsPerCell, pixelsPerCell));
                    }
                }
            }
        }
        internal void UpdateGrid()
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
                        if (count == 2 || count == 3)
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
        internal int CountNeighbours(bool[,] gridOld, Point coords)
        {
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                count += gridOld[(coords.X + dx[i] + cellsPerRow) % cellsPerRow, (coords.Y + dy[i] + cellsPerRow) % cellsPerRow] ? 1 : 0;
                if (count > 3 || (count < 1 && i >= 6))
                {
                    return count;
                }
            }
            return count;
        }
    }
}
