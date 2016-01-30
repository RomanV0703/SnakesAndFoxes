using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakesAndFoxes
{
    class GridLineAdorner : Adorner
    {
        public GridLineAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            Rectangle movePosition = new Rectangle()
            {
                Name = "movePosition",
                Width = Height = 3,
                Stroke = Brushes.DarkKhaki,
                Fill = Brushes.DarkKhaki
            };
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            // Some arbitrary drawing implements.
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            double renderRadius = 5.0;

            Point centerPoint = new Point();

            drawingContext.DrawEllipse(renderBrush, renderPen, centerPoint, renderRadius, renderRadius);
        }

        public void removeAllAdorners(AdornerLayer adornerLayer, UIElement adornedElement)
        {
            var adorners = adornerLayer.GetAdorners(adornedElement);

            if (adorners != null)
            {
                foreach (var adorner in adorners)
                {
                    adornerLayer.Remove(adorner);
                }
            }
        }
    }
}
