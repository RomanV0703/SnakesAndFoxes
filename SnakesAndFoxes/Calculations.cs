using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SnakesAndFoxes
{
    class Calculations
    {
        List<Point> movePositionsArray = new List<Point>();
        int playerMovesCount = 0;

        public List<Point> calcPossibleMoves(Rectangle selectedObject, Canvas gameCanvas, int[] rollResults, Point[,] gridCoordinatesArray, int gridCirclesCount)
        {
            for (int i = 0; i < rollResults.Length; i++)
            {
                if (rollResults[i] == 5 || rollResults[i] == 6)
                {
                    playerMovesCount++;
                }

            }

            Point currentPositionCoord = new Point();
            currentPositionCoord.X = Canvas.GetLeft(selectedObject) + selectedObject.Width / 2;
            currentPositionCoord.Y = Canvas.GetTop(selectedObject) + selectedObject.Height / 2;

            Point currentPositionIndex = new Point();

            for (int i = 0; i < gridCoordinatesArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridCoordinatesArray.GetLength(1); j++)
                {
                    if (Math.Abs(currentPositionCoord.X - gridCoordinatesArray[i, j].X) < 1 && Math.Abs(currentPositionCoord.Y - gridCoordinatesArray[i, j].Y) < 1)
                    {
                        currentPositionIndex.X = i;
                        currentPositionIndex.Y = j;

                        if (j == 0)
                        {
                            currentPositionIndex.X = 0;
                        }
                    }
                }
            }

            ((MainWindow)System.Windows.Application.Current.MainWindow).labelIJ.Content = string.Format("I,J: {0},{1}", currentPositionIndex.X, currentPositionIndex.Y);

            for (int i = 0; i < gridCoordinatesArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridCoordinatesArray.GetLength(1); j++)
                {
                    if ((Math.Abs(j - currentPositionIndex.Y) == 1 && (i - currentPositionIndex.X) == 0) ||
                       (((Math.Abs(i - currentPositionIndex.X) == 1) || (Math.Abs(i - currentPositionIndex.X) == 15)) && (j - currentPositionIndex.Y) == 0))
                    {
                        if (currentPositionIndex.Y % 2 == 0 && currentPositionIndex.X <= i)
                        {
                            movePositionsArray.Add(gridCoordinatesArray[i, j]);
                        }
                        if (currentPositionIndex.Y % 2 != 0 && currentPositionIndex.X >= i)
                        {
                            movePositionsArray.Add(gridCoordinatesArray[i, j]);
                        }
                        if (currentPositionIndex.Y == gridCirclesCount)
                        {
                            movePositionsArray.Add(gridCoordinatesArray[i, j]);
                        }
                    }
                }

                if (currentPositionIndex == new Point(0, 0))
                {
                    movePositionsArray.Add(gridCoordinatesArray[i, 1]);
                }
            }

            return movePositionsArray;
        }
    }
}
