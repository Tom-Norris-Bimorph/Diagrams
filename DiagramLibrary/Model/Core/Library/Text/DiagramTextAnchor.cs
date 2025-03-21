using DiagramLibrary.Text;
using Rhino.Geometry;
using System.Drawing;

namespace DiagramLibrary
{
    /// <inheritdoc cref="IDiagramTextAnchor"/>
    public class DiagramTextAnchor : IDiagramTextAnchor
    {
        /// <inheritdoc />
        public TextJustification Anchor { get; }

        /// <summary>
        /// Constructs a new <see cref="IDiagramTextAnchor"/>.
        /// </summary>
        /// <param name="anchor"></param>
        public DiagramTextAnchor(TextJustification anchor)
        {
            this.Anchor = anchor;
        }

        /// <inheritdoc />
        public PointF GetAnchorCompensatedPoint(IDiagramLocation diagramLocation, SizeF size, float textHeight)
        {
            PointF newPoint;
            switch (this.Anchor)
            {
                case TextJustification.None:
                    newPoint = new PointF(diagramLocation.Point.X, textHeight);
                    break;
                case TextJustification.Left:
                    newPoint = new PointF(diagramLocation.Point.X, textHeight);
                    break;
                case TextJustification.Center:
                    newPoint = new PointF(diagramLocation.Point.X - (size.Width / 2), diagramLocation.Point.Y);
                    break;
                case TextJustification.Right:
                    newPoint = new PointF(diagramLocation.Point.X - size.Width, diagramLocation.Point.Y);
                    break;
                case TextJustification.Bottom:
                    newPoint = diagramLocation.Point;
                    break;
                case TextJustification.Middle:
                    newPoint = new PointF(diagramLocation.Point.X, diagramLocation.Point.Y - (size.Height / 2));
                    break;
                case TextJustification.Top:
                    newPoint = new PointF(diagramLocation.Point.X, diagramLocation.Point.Y - size.Height);
                    break;
                case TextJustification.BottomLeft:
                    newPoint = diagramLocation.Point;
                    break;
                case TextJustification.BottomCenter:
                    newPoint = new PointF(diagramLocation.Point.X - (size.Width / 2), diagramLocation.Point.Y);
                    break;
                case TextJustification.BottomRight:
                    newPoint = new PointF(diagramLocation.Point.X - size.Width, diagramLocation.Point.Y);
                    break;
                case TextJustification.MiddleLeft:
                    newPoint = new PointF(diagramLocation.Point.X, diagramLocation.Point.Y - (size.Height / 2));
                    break;
                case TextJustification.MiddleCenter:
                    newPoint = new PointF(diagramLocation.Point.X - (size.Width / 2), diagramLocation.Point.Y - (size.Height / 2));
                    break;
                case TextJustification.MiddleRight:
                    newPoint = new PointF(diagramLocation.Point.X - size.Width, diagramLocation.Point.Y - (size.Height / 2));
                    break;
                case TextJustification.TopLeft:
                    newPoint = new PointF(diagramLocation.Point.X, diagramLocation.Point.Y - size.Height);
                    break;
                case TextJustification.TopCenter:
                    newPoint = new PointF(diagramLocation.Point.X - (size.Width / 2), diagramLocation.Point.Y - size.Height);
                    break;
                case TextJustification.TopRight:
                    newPoint = new PointF(diagramLocation.Point.X - size.Width, diagramLocation.Point.Y - size.Height);
                    break;
                default:
                    newPoint = diagramLocation.Point;
                    break;
            }
            return newPoint;
        }
    }
}