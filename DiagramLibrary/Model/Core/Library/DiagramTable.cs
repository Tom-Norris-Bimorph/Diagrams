using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramLibrary
{
    public class DiagramTable : DiagramCachedClass
    {
        GH_Structure<GH_String> m_Data = new GH_Structure<GH_String>();
        List<double> m_CellWidths = new List<double>();
        List<double> m_CellHeights = new List<double>();

        private PointF m_Location;
        private string m_FontName = Diagram.DefaultFontName;
        private float m_Padding = 0f;
        private float m_TextSize = 1f;
        private TextJustification m_Justification = TextJustification.None;
        private bool m_flip = false;
        private Color m_HeaderColor;



        public static DiagramTable Create(GH_Structure<GH_String> data, List<double> cellWidths, List<double> cellHeights, float textSize, PointF Location, Color Colour, float LineWeight, string fontName, float padding, TextJustification justification, Color headerColor, bool flip)
        {
            var diagramTable = new DiagramTable();
            diagramTable._colour = Colour;
            diagramTable._lineWeight = LineWeight;
            diagramTable.m_Data = data;
            diagramTable.m_CellWidths = cellWidths;
            diagramTable.m_CellHeights = cellHeights;
            diagramTable.m_TextSize = textSize;
            diagramTable.m_Location = Location;
            diagramTable.m_FontName = fontName;
            diagramTable.m_Padding = padding;
            diagramTable.m_Justification = justification;
            diagramTable.m_flip = flip;
            diagramTable.m_HeaderColor = headerColor;
            diagramTable.UpdateCache();
            return diagramTable;
        }

        public override DiagramObject Duplicate()
        {
            var diagramTable = new DiagramTable();
            diagramTable.m_Location = m_Location;
            diagramTable._colour = _colour;
            diagramTable._lineWeight = _lineWeight;
            diagramTable.m_Data = m_Data;
            diagramTable.m_CellWidths = m_CellWidths;
            diagramTable.m_CellHeights = m_CellHeights;
            diagramTable.m_TextSize = m_TextSize;
            diagramTable.m_FontName = m_FontName;
            diagramTable.m_flip = m_flip;
            diagramTable.m_HeaderColor = m_HeaderColor;
            diagramTable.m_Padding = m_Padding;
            diagramTable.m_Justification = m_Justification;
            diagramTable.UpdateCache();

            return diagramTable;
        }

        public string ToCSV()
        {
            var returnString = "";
            for (var i = 0; i < m_Data.Branches.Count; i++)
            {

                for (var j = 0; j < m_Data.Branches[i].Count; j++)
                {
                    returnString += m_Data.Branches[i][j].Value + ",";
                }
                returnString += "\n";

            }
            return returnString;
        }



        public void GetSizes(out SizeF[] sizes, out Rectangle3d[] recs)
        {

            var numberOfCols = 0;
            var numberOfRows = 0;

            //calculate sizes
            for (var i = 0; i < m_Data.Branches.Count; i++)
            {
                numberOfRows++;
                for (var j = numberOfCols; j < m_Data.Branches[i].Count; j++)
                {
                    numberOfCols++;
                }
            }

            if (m_flip)
            {
                var temp = numberOfCols;
                numberOfCols = numberOfRows;
                numberOfRows = temp;
            }

            sizes = new SizeF[numberOfCols * numberOfRows];
            recs = new Rectangle3d[numberOfCols * numberOfRows];

            double currentX = 0;
            double currnetY = 0;

            for (var rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
            {
                float width = 100; //Absolute Defaults
                float height = 50; //Absolute Defaults

                currentX = 0;
                for (var colIndex = 0; colIndex < numberOfCols; colIndex++)
                {

                    if (m_CellWidths.Count > colIndex)
                    {
                        //Try and Find Width
                        width = (float)m_CellWidths[colIndex];
                    }
                    else
                    {
                        //Fall back on the first width given
                        if (m_CellWidths.Count > 0)
                        {
                            width = (float)m_CellWidths[0];
                        }
                    }

                    if (m_CellHeights.Count > rowIndex)
                    {
                        //Try and Find hieght
                        height = (float)m_CellHeights[rowIndex];
                    }
                    else
                    {
                        //Fall back on the first height given
                        if (m_CellHeights.Count > 0)
                        {
                            height = (float)m_CellHeights[0];
                        }
                    }





                    if (m_flip)
                    {
                        recs[colIndex * numberOfRows + rowIndex] = new Rectangle3d(new Plane(new Point3d(m_Location.X + currentX, m_Location.Y + currnetY, 0), Plane.WorldXY.ZAxis), width, height);
                        sizes[colIndex * numberOfRows + rowIndex] = new SizeF(width, height);
                    }
                    else
                    {
                        recs[rowIndex * numberOfCols + colIndex] = new Rectangle3d(new Plane(new Point3d(m_Location.X + currentX, m_Location.Y + currnetY, 0), Plane.WorldXY.ZAxis), width, height);
                        sizes[rowIndex * numberOfCols + colIndex] = new SizeF(width, height);
                    }
                    currentX += width;

                }
                currnetY += height;


            }
        }



        public override List<DiagramObject> GenerateObjects()
        {
            var diagramObjects = new List<DiagramObject>();

            //calculate sizes
            this.GetSizes(out var sizes, out var recs);

            //draw text

            if (m_flip)
            {
                var currentIndex = 0;
                for (var i = 0; i < m_Data.Branches.Count; i++)
                {

                    for (var j = m_Data.Branches[i].Count - 1; j >= 0; j--)
                    {

                        var clr = Color.Transparent;
                        if (j == 0)
                        {
                            clr = m_HeaderColor;
                        }

                        diagramObjects.Add(DiagramText.Create(m_Data.Branches[i][j].Value, Diagram.ConvertPoint(recs[currentIndex].Plane.Origin), _colour, m_TextSize, TextJustification.BottomLeft, clr, Color.Transparent, -1, m_FontName, sizes[currentIndex], m_Padding, m_Justification));

                        currentIndex++;
                    }
                }
            }
            else
            {
                var currentIndex = 0;
                for (var i = m_Data.Branches.Count - 1; i >= 0; i--)
                {

                    for (var j = 0; j < m_Data.Branches[i].Count; j++)
                    {

                        var clr = Color.Transparent;
                        if (i == 0)
                        {
                            clr = m_HeaderColor;
                        }

                        diagramObjects.Add(DiagramText.Create(m_Data.Branches[i][j].Value, Diagram.ConvertPoint(recs[currentIndex].Plane.Origin), _colour, m_TextSize, TextJustification.BottomLeft, clr, Color.Transparent, -1, m_FontName, sizes[currentIndex], m_Padding, m_Justification));
                        currentIndex++;
                    }
                }
            }

            //draw lines
            for (var i = 0; i < recs.Length; i++)
            {
                diagramObjects.Add(DiagramCurve.Create(recs[i].ToNurbsCurve(), _colour, _lineWeight));

            }

            return diagramObjects;
        }
    }
}
