using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnakesAndFoxes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Point cursorPosition;
        int gridStep = 75;
        static int gridCirclesCount = 8;
        static int gridLinesCount = 8;
        Point[,] gridCoordinatesArray = new Point[gridLinesCount * 2, gridCirclesCount + 1];
        List<Point> movePositionsArray;
        int[] rollResults;
        Rectangle selectedToken;

        DrawShapes drawShapes = new DrawShapes();
        //Calculations calculation = new Calculations();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBox_gridStep.Text = gridStep.ToString();
            textBox_gridCirclesCount.Text = gridCirclesCount.ToString();
            textBox_gridLinesCount.Text = gridLinesCount.ToString();

            drawGrid(gameCanvas);
            drawTokens(gameCanvas);
            rollTheDice();
        }

        private void drawGrid(Canvas gameCanvas)
        {
            gridStep = int.Parse(textBox_gridStep.Text);
            gridCirclesCount = int.Parse(textBox_gridCirclesCount.Text);
            gridLinesCount = int.Parse(textBox_gridLinesCount.Text);

            createGridCoordinatesArray();

            drawShapes.drawGridCircles(gameCanvas, gridStep, gridCirclesCount);
            drawShapes.drawGridLines(gameCanvas, gridStep, gridLinesCount, gridCirclesCount);
            drawShapes.drawCentralCircle(gameCanvas, gridStep);
            drawShapes.addLabels(gameCanvas);
            drawShapes.drawArrows(gameCanvas, gridStep, gridCirclesCount);
        }

        private void createGridCoordinatesArray()
        {
            //gridCoordinatesArray[0, 0] = new Point(0, 0);

            for (int i = 0; i < (gridLinesCount * 2); i++)
            {
                double angle = Math.PI / gridLinesCount * i;
                double length = 0;
                double X1 = 0;
                double Y1 = 0;

                for (int j = 1; j < gridCirclesCount + 1; j++)
                {
                    length = (double)gridStep / 2;

                    double X2 = X1 - length * Math.Round(Math.Cos(angle), 2);
                    double Y2 = Y1 - length * Math.Round(Math.Sin(angle), 2);

                    X1 = X2;
                    Y1 = Y2;

                    gridCoordinatesArray[i, j] = new Point(X2, Y2);
                }
            }
        }

        private void drawTokens(Canvas gameCanvas)
        {
            int gridStep = int.Parse(textBox_gridStep.Text);

            drawShapes.drawTokens(gameCanvas, gridStep, gridLinesCount, gridCirclesCount, gridCoordinatesArray);
        }

        private void rollTheDice()
        {
            diceCanvas.Children.Clear();
            RollTheDice rollDices = new RollTheDice();
            rollResults = rollDices.roll(6);
            drawShapes.drawDices(diceCanvas, rollResults);
        }

        private void gameCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            cursorPosition = Mouse.GetPosition(gameCanvas);

            labelXY.Content = string.Format("X,Y: {0},{1}", cursorPosition.X, cursorPosition.Y);
        }

        private void button_drawGrid_Click(object sender, RoutedEventArgs e)
        {
            gameCanvas.Children.Clear();
            drawGrid(gameCanvas);
        }

        private void button_drawToken_Click(object sender, RoutedEventArgs e)
        {
            //drawToken(gameCanvas);
        }

        private void drawMovePositions()
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(gameCanvas);

            foreach(Line line in drawShapes.gridLines)
            {
                GridLineAdorner gridLineAdorner = new GridLineAdorner(line);
                gridLineAdorner.removeAllAdorners(adornerLayer, line);
                adornerLayer.Add(gridLineAdorner);
            }
        }

        private void gameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point cursorPosition = e.GetPosition(gameCanvas);

            if (e.OriginalSource is Rectangle)
            {
                selectedToken = (Rectangle)e.OriginalSource;

                //if (selectedObject.Name.Contains("tokenPlayer"))
                if (selectedToken.Name.Contains("token"))
                {
                    Calculations calculation = new Calculations();
                    movePositionsArray = calculation.calcPossibleMoves(selectedToken, gameCanvas, rollResults, gridCoordinatesArray, gridCirclesCount);
                    drawShapes.drawMovePositions(gameCanvas, movePositionsArray);
                }
            }

            if (e.OriginalSource is Ellipse)
            {
                Ellipse source = (Ellipse)e.OriginalSource;

                if(source.Name.Contains("movePosition"))
                {
                    double X = Canvas.GetTop(source);
                    double Y = Canvas.GetLeft(source);
                    
                    Rectangle currentToken = selectedToken;

                    if (selectedToken.Name.Contains("Player"))
                    {
                        Ellipse currentMark = drawShapes.tokenPlayerMarks[0];

                        Canvas.SetTop(currentMark, X - currentToken.Height / 4 + currentMark.Height / 2);
                        Canvas.SetLeft(currentMark, Y - currentToken.Width / 4 + currentMark.Width / 2);

                        drawShapes.tokenPlayerMarks[0] = currentMark;
                    }

                    Canvas.SetTop(currentToken, X - currentToken.Height / 4);
                    Canvas.SetLeft(currentToken, Y - currentToken.Width / 4);

                    selectedToken = currentToken;


                    Calculations calculation = new Calculations();
                    movePositionsArray = calculation.calcPossibleMoves(currentToken, gameCanvas, rollResults, gridCoordinatesArray, gridCirclesCount);
                    drawShapes.drawMovePositions(gameCanvas, movePositionsArray);
                }

            }
        }

        private void button_diceRoll_Click(object sender, RoutedEventArgs e)
        {
            rollTheDice();
        }
    }
}
