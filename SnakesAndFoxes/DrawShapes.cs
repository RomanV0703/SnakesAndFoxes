using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace SnakesAndFoxes
{
    class DrawShapes
    {
        public List<Line> gridLines = new List<Line>();
        public List<Ellipse> gridCircles = new List<Ellipse>();
        public List<Rectangle> tokens = new List<Rectangle>();
        public List<Rectangle> tokensFoxes = new List<Rectangle>();
        public List<Ellipse> tokenPlayerMarks = new List<Ellipse>();
        public List<Rectangle> dices = new List<Rectangle>();
        public List<Label> diceLabels = new List<Label>();
        public List<Polyline> snakes = new List<Polyline>();
        public List<Polyline> foxes = new List<Polyline>();
        public List<Ellipse> players = new List<Ellipse>();

        public void drawGridCircles(Canvas gameCanvas, int gridStep, int gridCirclesCount)
        {
            for (int i = 0; i < gridCirclesCount; i++)
            {
                Ellipse gridCircle = new Ellipse()
                {
                    Name = "gridCircle" + i.ToString(),
                    Width = gridStep * (i + 1),
                    Height = gridStep * (i + 1),
                    StrokeThickness = 2,
                    SnapsToDevicePixels = true
                };

                if (i % 2 == 0)
                {
                    gridCircle.Stroke = Brushes.Tomato;
                }
                if (i % 2 != 0)
                {
                    gridCircle.Stroke = Brushes.Green;
                }
                if (i + 1 == gridCirclesCount)
                {
                    gridCircle.Stroke = Brushes.RoyalBlue;
                }

                gridCircles.Add(gridCircle);
            }

            foreach (Ellipse gridCircle in gridCircles)
            {
                gameCanvas.Children.Add(gridCircle);
                Canvas.SetTop(gridCircle, -gridCircle.Height / 2);
                Canvas.SetLeft(gridCircle, -gridCircle.Width / 2);
            }

            //gridCircles.Clear();
        }

        public void drawCentralCircle(Canvas gameCanvas, int gridStep)
        {
            Ellipse centralCircle = new Ellipse()
            {
                Name = "centralCircle",
                Width = gridStep / 3,
                Height = gridStep / 3,
                Stroke = Brushes.RoyalBlue,
                StrokeThickness = 2,
                Fill = Brushes.White
            };

            gameCanvas.Children.Add(centralCircle);
            Canvas.SetTop(centralCircle, -centralCircle.Height / 2);
            Canvas.SetLeft(centralCircle, -centralCircle.Width / 2);
        }

        public void drawGridLines(Canvas gameCanvas, int gridStep, int gridLinesCount, int gridCirclesCount)
        {
            for (int i = 0; i < (gridLinesCount * 2); i++)
            {
                double angle = Math.PI / gridLinesCount * i;
                double length = gridStep * gridCirclesCount / 2;

                double X1 = 0;
                double Y1 = 0;
                double X2 = X1 - length * Math.Round(Math.Cos(angle), 2);
                double Y2 = Y1 - length * Math.Round(Math.Sin(angle), 2);

                Line gridLine = new Line()
                {
                    Name = "gridLine" + i.ToString(),
                    Stroke = Brushes.RoyalBlue,
                    StrokeThickness = 2,
                    X1 = X1,
                    Y1 = Y1,
                    X2 = X2,
                    Y2 = Y2
                };

                gridLines.Add(gridLine);
            }

            foreach (Line gridLine in gridLines)
            {
                gameCanvas.Children.Add(gridLine);
            }
        }

        public void addLabels(Canvas gameCanvas)
        {
            int i = 0;

            foreach (Line gridLine in gridLines)
            {
                Label gridLineLabel = new Label()
                {
                    Name = "gridLineLabel" + i.ToString(),
                    Content = i.ToString(),
                    Effect = new DropShadowEffect()
                };

                gameCanvas.Children.Add(gridLineLabel);
                Canvas.SetTop(gridLineLabel, gridLines[i].X2);
                Canvas.SetLeft(gridLineLabel, gridLines[i].Y2);

                i++;
            }

            gridLines.Clear();
        }

        public void drawTokens(Canvas gameCanvas, int gridStep, int gridLinesCount, int gridCirclesCount, Point[,] gridCoordinatesArray)
        {
            #region PlayerTokens
            Rectangle token = new Rectangle()
            {
                Name = "tokenPlayer",
                Width = gridStep / 3.5,
                Height = gridStep / 3.5,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = Brushes.Beige,
                RadiusX = 2,
                RadiusY = 2
            };

            tokens.Add(token);

            gameCanvas.Children.Add(token);
            Canvas.SetTop(token, -token.Height / 2);
            Canvas.SetLeft(token, -token.Width / 2);
            Canvas.SetZIndex(token, 2);

            Ellipse tokenPlayerMark = new Ellipse()
            {
                Name = "tokenPlayerMark",
                Width = token.Width / 2,
                Height = token.Width / 2,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
            };

            tokenPlayerMarks.Add(tokenPlayerMark);

            gameCanvas.Children.Add(tokenPlayerMark);
            Canvas.SetTop(tokenPlayerMark, -token.Height / 2 + tokenPlayerMark.Height / 2);
            Canvas.SetLeft(tokenPlayerMark, -token.Width / 2 + tokenPlayerMark.Width / 2);
            Canvas.SetZIndex(tokenPlayerMark, 3);
            #endregion


            for (int i = 0; i < gridLinesCount * 2; i++)
            {
                token = new Rectangle()
                {
                    Name = "tokenFox" + i.ToString(),
                    Width = gridStep / 3.5,
                    Height = gridStep / 3.5,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Fill = Brushes.Beige,
                    RadiusX = 2,
                    RadiusY = 2
                };

                tokensFoxes.Add(token);

                gameCanvas.Children.Add(token);
                //Canvas.SetTop(token, -token.Height / 2);
                //Canvas.SetLeft(token, -token.Width / 2);
                Canvas.SetTop(token, gridCoordinatesArray[i, gridCirclesCount].Y - token.Height / 2);
                Canvas.SetLeft(token, gridCoordinatesArray[i, gridCirclesCount].X - token.Width / 2);
                Canvas.SetZIndex(token, 2);
            }
        }

        public void drawMovePositions(Canvas gameCanvas, List<Point> movePositionsArray)
        {
            var movePositions = gameCanvas.Children.OfType<Ellipse>().ToList();
            foreach (var movePosition in movePositions)
            {
                if (movePosition.Name.Contains("movePositionCircle"))
                {
                    gameCanvas.Children.Remove(movePosition);
                }
            }

            int i = 0;

            foreach (Point point in movePositionsArray)
            {
                Ellipse movePositionCircle = new Ellipse()
                {
                    Name = "movePositionCircle" + i.ToString(),
                    Width = 10,
                    Height = 10,
                    Stroke = Brushes.Green,
                    StrokeThickness = 0,
                    Fill = Brushes.Green,
                    Opacity = 0.8
                };

                i++;

                movePositions.Add(movePositionCircle);

                gameCanvas.Children.Add(movePositionCircle);

                Canvas.SetTop(movePositionCircle, point.Y - movePositionCircle.Height / 2);
                Canvas.SetLeft(movePositionCircle, point.X - movePositionCircle.Width / 2);
                Canvas.SetZIndex(movePositionCircle, 1);
            }

            //foreach (Ellipse movePositionCircle in movePositions)
            //{
            //    gameCanvas.Children.Add(movePositionCircle);
            //    Canvas.SetTop(movePositionCircle, (-movePositionCircle.Height / 2));
            //    Canvas.SetLeft(movePositionCircle, (-movePositionCircle.Width / 2));
            //}
        }

        public void drawDices(Canvas diceCanvas, int[] rollResults)
        {
            for (int i = 0; i < 6; i++)
            {
                Rectangle dice = new Rectangle()
                {
                    Name = "dice" + i.ToString(),
                    Width = diceCanvas.ActualWidth / 1.5,
                    Height = diceCanvas.ActualWidth / 1.5,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Fill = Brushes.Beige,
                    RadiusX = 2,
                    RadiusY = 2
                };

                dices.Add(dice);

                Label diceLabel = new Label()
                {
                    Name = "diceLabel" + i.ToString(),
                    Content = rollResults[i].ToString()
                };

                diceLabels.Add(diceLabel);
            }

            double j = 0;

            //foreach (Rectangle dice in dices)
            for (int k = 0; k < dices.Count; k++)
            {
                Rectangle dice = dices[k];
                Label diceLabel = diceLabels[k];

                diceCanvas.Children.Add(dice);
                Canvas.SetTop(dice, diceCanvas.ActualWidth / 2 - dice.Height / 2 + j);
                Canvas.SetLeft(dice, diceCanvas.ActualWidth / 2 - dice.Width / 2);
                Canvas.SetZIndex(dice, 1);

                diceCanvas.Children.Add(diceLabel);
                Canvas.SetTop(diceLabel, diceCanvas.ActualWidth / 2 - dice.Height / 2 + j);
                Canvas.SetLeft(diceLabel, diceCanvas.ActualWidth / 2 - dice.Width / 2);
                Canvas.SetZIndex(diceLabel, 3);

                #region Snake
                if (rollResults[k] == 1 || rollResults[k] == 2) // 1 or 2 equals Snake
                                                                // 3 or 4 equals Fox
                                                                // 5 or 6 equals Player
                {
                    PointCollection polylinePoints = new PointCollection();

                    int n = 13;

                    Point point1 = new Point(0 * n, 0 * n);
                    Point point2 = new Point(1 * n, 1 * n);
                    Point point3 = new Point(0 * n, 2 * n);
                    Point point4 = new Point(1 * n, 3 * n);

                    polylinePoints.Add(point1);
                    polylinePoints.Add(point2);
                    polylinePoints.Add(point3);
                    polylinePoints.Add(point4);

                    Polyline snake = new Polyline()
                    {
                        Name = "snake" + k.ToString(),
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                        Points = polylinePoints
                    };

                    snakes.Add(snake);
                    diceCanvas.Children.Add(snake);
                    Canvas.SetTop(snake, diceCanvas.ActualWidth / 1.8 - dice.Height / 2 + j);
                    Canvas.SetLeft(snake, diceCanvas.ActualWidth / 1.4 - dice.Width / 2);
                    Canvas.SetZIndex(snake, 2);
                } 
                #endregion

                #region Fox
                if (rollResults[k] == 3 || rollResults[k] == 4) // 1 or 2 equals Snake
                                                                // 3 or 4 equals Fox
                                                                // 5 or 6 equals Player
                {
                    PointCollection polylinePoints = new PointCollection();

                    int n = 13;

                    Point point1 = new Point(1 * n, 2 * n);
                    Point point2 = new Point(2 * n, 0 * n);
                    Point point3 = new Point(0 * n, 0 * n);
                    Point point4 = point1;

                    polylinePoints.Add(point1);
                    polylinePoints.Add(point2);
                    polylinePoints.Add(point3);
                    polylinePoints.Add(point4);

                    Polyline fox = new Polyline()
                    {
                        Name = "fox" + k.ToString(),
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                        Points = polylinePoints
                    };

                    foxes.Add(fox);
                    diceCanvas.Children.Add(fox);
                    Canvas.SetTop(fox, diceCanvas.ActualWidth / 1.5 - dice.Height / 2 + j);
                    Canvas.SetLeft(fox, diceCanvas.ActualWidth / 1.5 - dice.Width / 2);
                    Canvas.SetZIndex(fox, 2);
                }
                #endregion

                #region Player
                if (rollResults[k] == 5 || rollResults[k] == 6) // 1 or 2 equals Snake
                                                                // 3 or 4 equals Fox
                                                                // 5 or 6 equals Player
                {
                    Ellipse player = new Ellipse()
                    {
                        Name = "player" + k.ToString(),
                        Width = 10,
                        Height = 10,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                    };

                    players.Add(player);
                    diceCanvas.Children.Add(player);
                    Canvas.SetTop(player, diceCanvas.ActualWidth / 2 - player.Height / 2 + j);
                    Canvas.SetLeft(player, diceCanvas.ActualWidth / 2 - player.Width / 2);
                    Canvas.SetZIndex(player, 2);
                } 
                #endregion

                j += dice.Height * 1.2;
            }

            dices.Clear();
            diceLabels.Clear();
            snakes.Clear();
        }

        public void drawArrows(Canvas gameCanvas, int gridStep, int gridCirclesCount)
        {
            Point startPoint = new Point(Canvas.GetLeft(gridCircles[0]) + gridStep * 0.25, Canvas.GetTop(gridCircles[0]) - gridStep * 0.07);
            Point endPoint = new Point(Canvas.GetLeft(gridCircles[0]) + gridStep * 0.75, Canvas.GetTop(gridCircles[0]) - gridStep * 0.07);

            for (int i = 0; i < gridCirclesCount - 1; i++)
            {

                PointCollection polylinePoints = new PointCollection();

                Point point1, point2, point3, point4;

                int n = 2;

                if (i % 2 != 0)
                {
                    point1 = new Point(0 * n, 2 * n);
                    point2 = new Point(2 * n, 1 * n);
                    point3 = new Point(0 * n, 0 * n);
                    point4 = point1;
                }
                else //(i % 2 == 0)
                {
                    point1 = new Point(0 * n, 2 * n);
                    point2 = new Point(-2 * n, 1 * n);
                    point3 = new Point(0 * n, 0 * n);
                    point4 = point1;
                }

                polylinePoints.Add(point1);
                polylinePoints.Add(point2);
                polylinePoints.Add(point3);
                polylinePoints.Add(point4);

                Polyline arrowhead = new Polyline()
                {
                    Name = "arrowhead" + i.ToString(),
                    StrokeThickness = 2,
                    Points = polylinePoints
                };

                Line arrow = new Line()
                {
                    Name = "arrow" + i.ToString(),
                    StrokeThickness = 2,
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = endPoint.X,
                    Y2 = endPoint.Y
                };

                if (i % 2 == 0)
                {
                    arrow.Stroke = Brushes.Tomato;
                    arrowhead.Stroke = Brushes.Tomato;
                    Canvas.SetLeft(arrowhead, startPoint.X + 4);
                }
                else //if (i % 2 != 0)
                {
                    arrow.Stroke = Brushes.Green;
                    arrowhead.Stroke = Brushes.Green;
                    Canvas.SetLeft(arrowhead, endPoint.X - 4);
                }

                gameCanvas.Children.Add(arrow);
                gameCanvas.Children.Add(arrowhead);
                Canvas.SetTop(arrowhead, startPoint.Y - 2);

                startPoint = new Point(startPoint.X, startPoint.Y - gridStep / 2);
                endPoint = new Point(endPoint.X, endPoint.Y - gridStep / 2);
            }

        }

    }
}
