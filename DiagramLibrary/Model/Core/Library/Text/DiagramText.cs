using DiagramLibrary.Core;
using DiagramLibrary.Text;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    /// <inheritdoc cref="IDiagramText"/>
    public class DiagramText : IDiagramText
    {
        /// <inheritdoc />
        public IDiagramTextAttributes TextAttributes { get; }

        /// <inheritdoc />
        public IDiagramFilledRectangle Mask { get; }

        /// <inheritdoc />
        public IDiagramLocation Location { get; }

        /// <inheritdoc />
        public IDiagramCurveAttributes CurveAttributes => this.TextAttributes.CurveAttributes;

        /// <inheritdoc />
        public string Text { get; }

        /// <summary>
        /// Constructs a new <see cref="IDiagramText"/>.
        /// </summary>
        public DiagramText(string text, IDiagramLocation location, IDiagramTextAttributes diagramTextAttributes, IDiagramFilledRectangle mask)
        {
            this.Text = text;
            this.Location = location;
            this.TextAttributes = diagramTextAttributes;
            this.Mask = mask;

        }

        /// <inheritdoc />
        public IDiagramObject Duplicate()
        {
            var maskCopy = this.Mask.Duplicate();

            var diagramText = new DiagramText(this.Text, this.Location, this.TextAttributes, maskCopy);

            return diagramText;
        }

        /// <inheritdoc />
        public BoundingBox GetBoundingBox()
        {
            SizeF totalSize;

            //Create a dummy graphics, for the measure text
            using (var g = Graphics.FromImage(new Bitmap(10, 10)))
            {
                var tempSize = this.CalculateTextSize(g, out totalSize, out var lines, out var rowSizes);

            }
            var anchorCompensatedPoint = this.TextAttributes.Anchor.GetAnchorCompensatedPoint(this.Location, totalSize, this.TextAttributes.TextSize);

            return new BoundingBox(anchorCompensatedPoint.X, anchorCompensatedPoint.Y, 0, anchorCompensatedPoint.X + totalSize.Width, anchorCompensatedPoint.Y + totalSize.Height, 0);
        }

        /// <inheritdoc />
        public List<string> CalculateTextLines(Graphics g, Font font, SizeF maxSize, StringFormat format, out SizeF totalSize, out List<SizeF> rowSizes)
        {
            var outputStrings = new List<string>();
            rowSizes = new List<SizeF>();

            var words = this.Text.Split(' ');

            var currentLine = "";
            float currentHeight = 0;
            var lineSpacing = font.FontFamily.GetLineSpacing(FontStyle.Regular);
            var lineSpacingPixel = font.Size * lineSpacing / font.FontFamily.GetEmHeight(FontStyle.Regular);
            float maxWidth = 0;
            var lastLineSize = SizeF.Empty;

            for (var i = 0; i < words.Length; i++)
            {
                var tempLine = currentLine + ' ' + words[i]; // Add each word
                var textSize = g.MeasureString(tempLine, font, new SizeF(maxSize.Width, this.TextAttributes.TextSize), format, out var charsFitted, out var linesFilled);

                if (maxSize.Height > 0 && currentHeight + textSize.Height >= maxSize.Height) break;

                if (charsFitted != tempLine.Length)
                {
                    outputStrings.Add(currentLine);
                    rowSizes.Add(lastLineSize);
                    currentHeight += lineSpacingPixel;

                    currentLine = words[i];
                    var isWordTooWide = true;

                    while (isWordTooWide)
                    {
                        var textSize2 = g.MeasureString(currentLine, font, maxSize, format, out var charsFitted2, out var linesFilled2);

                        if (charsFitted2 == 0)
                        {
                            isWordTooWide = false;
                        }

                        if (charsFitted2 != currentLine.Length)
                        {
                            outputStrings.Add(currentLine.Substring(0, charsFitted2));
                            rowSizes.Add(textSize2);
                            currentHeight += lineSpacingPixel;

                            currentLine = currentLine.Substring(charsFitted2 + 1);
                        }
                        else
                        {
                            isWordTooWide = false;

                            if (textSize2.Width > maxWidth)
                            {
                                maxWidth = textSize2.Width;
                            }
                        }
                    }
                }
                else
                {
                    if (textSize.Width > maxWidth)
                    {
                        maxWidth = textSize.Width;
                    }

                    currentLine = tempLine;
                    lastLineSize = textSize;
                }
            }

            outputStrings.Add(currentLine);
            rowSizes.Add(g.MeasureString(currentLine, font, maxSize, format));
            currentHeight += lineSpacingPixel;

            totalSize = new SizeF(maxWidth, currentHeight);

            return outputStrings;
        }

        /// <inheritdoc />
        public SizeF CalculateTextSize(Graphics g, out SizeF maskSize, out List<string> lines, out List<SizeF> rowSizes)
        {
            if (this.TextAttributes.TextSize <= 0)
            {
                maskSize = SizeF.Empty;
                lines = new List<string>();
                rowSizes = new List<SizeF>();
                return SizeF.Empty;
            }

            var allowedTextSize = SizeF.Empty;
            var font = new System.Drawing.Font(this.TextAttributes.FontName, this.TextAttributes.TextSize, GraphicsUnit.Pixel);

            var format = new StringFormat();

            if (this.WrapSize.Width < 0)
            {
                //Item is Not rapped, in this case we want the allowed text to be be as big as it needs and the padding is added on top,

                var textSize_NotConstrained = g.MeasureString(this.Text, font);
                maskSize = new SizeF(textSize_NotConstrained.Width + this.Padding + this.Padding, textSize_NotConstrained.Height + this.Padding + this.Padding);
                lines = new List<string> { this.Text };
                rowSizes = new List<SizeF> { textSize_NotConstrained };
                return textSize_NotConstrained;

            }
            //Item is Wrapped

            if (this.WrapSize.Height < 0)
            {

                //In this case we want the padding to be added to the Height but subracted from the width

                lines = this.CalculateTextLines(g, font, new SizeF(this.WrapSize.Width - this.Padding - this.Padding, -1), format, out var measuredSize, out rowSizes);
                maskSize = new SizeF(this.WrapSize.Width, measuredSize.Height + this.Padding + this.Padding);
                return measuredSize;
            }

            //In this case we want the padding to be subracted from the Height but subracted from the width
            maskSize = this.WrapSize;
            lines = this.CalculateTextLines(g, font, new SizeF(this.WrapSize.Width - this.Padding - this.Padding, this.WrapSize.Height - this.Padding - this.Padding), format, out var textSize_ConstrainedWidthAndHeight, out rowSizes);
            return textSize_ConstrainedWidthAndHeight;

        }

        /// <inheritdoc />
        public void DrawBitmap(Graphics g)
        {

            var font = new System.Drawing.Font(this.TextAttributes.FontName, this.TextAttributes.TextSize, GraphicsUnit.Pixel);

            var actualTotalTextSize = this.CalculateTextSize(g, out var maskSize, out var lines, out var rowSizes);

            var anchorCompensatedPoint = this.TextAttributes.Anchor.GetAnchorCompensatedPoint(this.Location, maskSize, this.TextAttributes.TextSize);

            this.Mask.UpdateRectangle(anchorCompensatedPoint, maskSize);
            this.Mask.DrawBitmap(g);

            var lineSpacing = font.FontFamily.GetLineSpacing(FontStyle.Regular);
            var lineSpacingPixel = font.Size * lineSpacing / font.FontFamily.GetEmHeight(FontStyle.Regular);

            var allowedTextSize = maskSize.Height - this.TextAttributes.Padding - this.TextAttributes.Padding;
            var heightSizeDiffence = allowedTextSize - actualTotalTextSize.Height;
            float verticalFustificationCompensation = 0;

            if (this.TextAttributes.Justification.IsMiddle)
            {
                verticalFustificationCompensation += heightSizeDiffence / 2;
            }

            if (this.TextAttributes.Justification.IsTop)
            {
                verticalFustificationCompensation += heightSizeDiffence;
            }

            //We have to draw this upside down as Bitmap is top to bottom but Rhino is bottom to top, to compensate we mirror-y the image for the canvas, but we have to draw the text upside down so it is correct when flipped.
            var tempTransform = g.Transform;
            g.ScaleTransform(1, -1);// Begin Upside Down,

            for (var i = 0; i < lines.Count; i++)
            {
                var rowSize = rowSizes[i];
                var allowedTextSizeWidth = maskSize.Width - this.TextAttributes.Padding - this.TextAttributes.Padding;

                var widthSizeDiffernce = allowedTextSizeWidth - rowSize.Width;

                float justificationCompensation = 0;

                if (this.TextAttributes.Justification.IsCentre)
                {
                    justificationCompensation += widthSizeDiffernce / 2;
                }

                if (this.TextAttributes.Justification.IsRight)
                {
                    justificationCompensation += widthSizeDiffernce;
                }

                //Add Padding and Justification Compensation, note Y is negative and signes are reverse as we drawing this upside down
                var pt = new PointF(anchorCompensatedPoint.X + this.TextAttributes.Padding + justificationCompensation, -anchorCompensatedPoint.Y - this.TextAttributes.Padding - actualTotalTextSize.Height - verticalFustificationCompensation + (lineSpacingPixel * i));

                //  g.DrawRectangle(this.GetPen(), pt.X, pt.Y,actualTotalTextSize.Width, this.TextSize);//good for debugging text
                g.DrawString(lines[i], font, this.CurveAttributes.GetBrush(), pt);
            }

            g.Transform = tempTransform;// End Upside Down

        }

        public void DrawRhinoPreview(Rhino.Display.DisplayPipeline pipeline, double tolerance, Transform xform, DrawState state)
        {

            var texts = this.GeneratePreviewGeometry(state, xform, out var clr, out var combinedXforms);

            this.Mask.DrawRhinoPreview(pipeline, tolerance, xform, state);

            for (var i = 0; i < texts.Count; i++)
            {
                pipeline.DrawText(texts[i], clr, combinedXforms[i]); //scale*trans order matters

            }
        }

        public List<Guid> BakeRhinoPreview(double tolerance, Transform transform, DrawState state, Rhino.RhinoDoc doc, Rhino.DocObjects.ObjectAttributes attr)
        {

            var outList = new List<Guid>();

            var texts = this.GeneratePreviewGeometry(state, transform, out var clr, out var combinedXforms);
            outList.AddRange(this.Mask.BakeRhinoPreview(tolerance, transform, state, doc, attr));

            for (var i = 0; i < texts.Count; i++)
            {
                attr.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;
                attr.ObjectColor = clr;
                texts[i].Transform(combinedXforms[i]);
                texts[i].Transform(Transform.Translation(0, -this.TextAttributes.TextSize, 0));// Corrects for rhino draw positioning
                var guid = doc.Objects.AddText(texts[i], attr);

                outList.Add(guid);
            }

            return outList;
        }

        private List<TextEntity> GeneratePreviewGeometry(DrawState state, Transform xform, out Color clr, out List<Transform> CombinedXforms)
        {

            var outList = new List<TextEntity>();
            CombinedXforms = new List<Transform>();

            var actualTotalTextSize = SizeF.Empty;
            var maskSize = SizeF.Empty;

            clr = this.CurveAttributes.Colour;
            var drawLines = this.Mask.LineWeight > 0;

            switch (state)
            {
                case DrawState.Normal:
                    break;
                case DrawState.Selected:
                    clr = Diagram.SelectedColor;
                    drawLines = true;
                    break;
                case DrawState.NoFills:
                    clr = Color.Transparent;
                    break;

            }

            List<string> lines;
            List<SizeF> rowSizes;

            //Create a dummy graphics, for the measure text
            using (var g = Graphics.FromImage(new Bitmap(10, 10)))
            {
                actualTotalTextSize = this.CalculateTextSize(g, out maskSize, out lines, out rowSizes);

            }

            var font = new System.Drawing.Font(this.TextAttributes.FontName, this.TextAttributes.TextSize, GraphicsUnit.Pixel);
            var lineSpacing = font.FontFamily.GetLineSpacing(FontStyle.Regular);
            var lineSpacingPixel = font.Size * lineSpacing / font.FontFamily.GetEmHeight(FontStyle.Regular);

            var allowedTextSizeHeight = maskSize.Height - this.TextAttributes.Padding - this.TextAttributes.Padding;
            var heightSizeDiffence = allowedTextSizeHeight - actualTotalTextSize.Height;
            float verticalFustificationCompensation = 0;

            if (this.TextAttributes.Justification.IsMiddle)
            {
                verticalFustificationCompensation += heightSizeDiffence / 2;
            }

            if (this.TextAttributes.Justification.IsTop)
            {
                verticalFustificationCompensation += heightSizeDiffence;
            }

            var anchorCompensatedPoint = this.TextAttributes.Anchor.GetAnchorCompensatedPoint(this.Location, maskSize, this.TextAttributes.TextSize);

            this.Mask.UpdateRectangle(anchorCompensatedPoint, maskSize);

            for (var i = 0; i < lines.Count; i++)
            {
                var txt = new TextEntity();
                txt.PlainText = lines[i];

                txt.Font = new Rhino.DocObjects.Font(this.TextAttributes.FontName, Rhino.DocObjects.Font.FontWeight.Normal, Rhino.DocObjects.Font.FontStyle.Upright, false, false);
                var pln = Plane.WorldXY;
                txt.Plane = pln;
                txt.Justification = this.TextAttributes.Anchor;
                txt.TextHeight = this.TextAttributes.TextSize;

                var rowSize = rowSizes[i];
                var allowedTextSizeWidth = maskSize.Width - this.TextAttributes.Padding - this.TextAttributes.Padding;

                var widthSizeDiffernce = allowedTextSizeWidth - rowSize.Width;
                float justificationCompensation = 0;

                if (this.TextAttributes.Justification.IsCentre)
                {
                    justificationCompensation += widthSizeDiffernce / 2;
                }

                if (this.TextAttributes.Justification.IsRight)
                {
                    justificationCompensation += widthSizeDiffernce;
                }

                //Add Padding and Justification Compensation, note Y is negative and signes are reverse as we drawing this upside down

                var pt = new Point3d(anchorCompensatedPoint.X + this.TextAttributes.Padding + justificationCompensation, anchorCompensatedPoint.Y + this.TextAttributes.Padding + verticalFustificationCompensation + (lineSpacingPixel * (lines.Count - i)), 0);
                var scale = Transform.Scale(new Point3d(pt.X, pt.Y, 0), this.TextAttributes.TextSize * (0.77)); //0.77 seems to be a good value to match the system drawing font size
                var trans = Transform.Translation(new Vector3d(pt.X, pt.Y, 0));
                var localXform = scale * trans;
                var combinedXform = localXform;
                if (xform != Transform.ZeroTransformation)
                {
                    combinedXform = Transform.Multiply(xform, localXform);
                }

                CombinedXforms.Add(combinedXform);
                outList.Add(txt);
            }

            return outList;
        }
    }
}
