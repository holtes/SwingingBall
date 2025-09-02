using System.Collections.Generic;
using UnityEngine;

namespace Core.Tools
{
    public static class CombinationChecker
    {
        public static List<Match> FindMatches(int[,] grid, int matchLength = 3)
        {
            var result = new List<Match>();
            if (grid == null) return result;

            int cols = grid.GetLength(0);
            int rows = grid.GetLength(1);
            if (cols <= 0 || rows <= 0 || matchLength <= 1) return result;

            var directions = new (int dx, int dy)[]
            {
            (1, 0),
            (0, 1),
            (1, 1),
            (-1, 1)
            };

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int type = grid[x, y];
                    if (type == 0) continue;

                    foreach (var (dx, dy) in directions)
                    {
                        int prevX = x - dx;
                        int prevY = y - dy;
                        if (IsInside(prevX, prevY, cols, rows) && grid[prevX, prevY] == type)
                            continue;

                        var match = new Match { TypeId = type };
                        match.Cells.Add(new Vector2Int(x, y));

                        int nx = x + dx;
                        int ny = y + dy;

                        while (IsInside(nx, ny, cols, rows) && grid[nx, ny] == type)
                        {
                            match.Cells.Add(new Vector2Int(nx, ny));
                            nx += dx;
                            ny += dy;
                        }

                        if (match.Cells.Count >= matchLength)
                            result.Add(match);
                    }
                }
            }

            return result;
        }

        private static bool IsInside(int x, int y, int cols, int rows)
        {
            return x >= 0 && x < cols && y >= 0 && y < rows;
        }
    }
}