using Core.Enums;
using Core.Tools;
using Data.Configs;
using System.Collections.Generic;

namespace Domain.Game.Models
{
    public class CollectorModel
    {
        private readonly BallType[,] _grid;

        public int Columns { get; private set; }
        public int Rows { get; private set; }

        public CollectorModel(GameConfig config)
        {
            Columns = config.GridSize.x;
            Rows = config.GridSize.y;
            _grid = new BallType[Columns, Rows];

            ClearGrid();
        }
        public void SetBall(int row, int col, BallType type)
        {
            _grid[row, col] = type;
        }

        public void ClearBall(int row, int col)
        {
            _grid[row, col] = BallType.None;
        }

        public void ClearGrid()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                    _grid[x, y] = BallType.None;
            }
        }

        public bool IsFull()
        {
            for (int col = 0; col < Columns; col++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    if (_grid[col, row] == BallType.None)
                        return false;
                }
            }
            return true;
        }

        public bool TryFindMatches(out List<Match> matches)
        {
            matches = CombinationChecker.FindMatches(CastToIntGrid(_grid));
            return matches.Count > 0;
        }

        private int[,] CastToIntGrid(BallType[,] ballTypesGrid)
        {
            int cols = ballTypesGrid.GetLength(0);
            int rows = ballTypesGrid.GetLength(1);
            int[,] intGrid = new int[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                    intGrid[x, y] = (int)ballTypesGrid[x, y];
            }
            return intGrid;
        }
    }
}