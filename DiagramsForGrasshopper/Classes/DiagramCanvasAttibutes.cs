using System;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

namespace DiagramsForGrasshopper
{
    public class DiagramCanvasAttibutes : GH_ComponentAttributes
    {

        
        private float m_CapsuleHeight = 20;


        public DiagramCanvasAttibutes(GH_Component owner) : base(owner) { }

        protected override void Layout()
        {

            Pivot = GH_Convert.ToPoint(Pivot);


            float width = 125;
            float height = m_CapsuleHeight;


            CanvasDiagram owner = this.Owner as CanvasDiagram;


            DiagramLibrary.Diagram diagram = null;
            var diagramOutput = owner.Params.Output[0].VolatileData;

           
            for (int i = 0; i < diagramOutput.PathCount; i++)
            {
                var diagramGooList = diagramOutput.get_Branch(diagramOutput.Paths[i]);

                for (int j = 0; j < diagramGooList.Count; j++)
                {
                    var diagramGoo = diagramGooList[j] as Grasshopper.Kernel.Types.IGH_Goo;
                    diagramGoo.CastTo(out  diagram);
                    break;
                }
                break;
            }

            if (diagram != null) {
                Size size = diagram.GetBoundingSize(owner.Scale);
                if (size.Width > 10)
                {

                    width = size.Width + 60;

                }
                owner.Update = true;
            }




            m_innerBounds = new RectangleF(Pivot.X, Pivot.Y, width, height);
            LayoutInputParams(Owner, m_innerBounds);
            LayoutOutputParams(Owner, m_innerBounds);
            Bounds = LayoutBounds(Owner, m_innerBounds);



        }

        protected override void Render(Grasshopper.GUI.Canvas.GH_Canvas canvas, Graphics graphics, Grasshopper.GUI.Canvas.GH_CanvasChannel channel)
        {
            if (channel != Grasshopper.GUI.Canvas.GH_CanvasChannel.Objects)
            {
                base.Render(canvas, graphics, channel);
                return;
            }

            //RenderComponentCapsule(canvas, graphics, true, false, false, true, true, true);
            RenderComponentCapsule(canvas, graphics, true, false, false, true, true, true);


            CanvasDiagram canvasDiagram = this.Owner as CanvasDiagram;


            DiagramLibrary.Diagram diagram = null;
            var diagramOutput = canvasDiagram.Params.Output[0].VolatileData;
            for (int i = 0; i < diagramOutput.PathCount; i++)
            {
                var diagramGooList = diagramOutput.get_Branch(diagramOutput.Paths[i]);

                for (int j = 0; j < diagramGooList.Count; j++)
                {
                    var diagramGoo = diagramGooList[j] as Grasshopper.Kernel.Types.IGH_Goo;
                    diagramGoo.CastTo(out diagram);
                    break;
                }
                break;
            }






            if (diagram != null)
            {
              
                Size size = diagram.GetBoundingSize(canvasDiagram.Scale);
                if (size.Width < 1) {
                    size.Width = 1;
                    }

                if (size.Height < 1)
                {
                    size.Height = 1;
                }

                graphics.DrawLine(Pens.DarkGray, m_innerBounds.Location, new Point((int)m_innerBounds.X, (int)(m_innerBounds.Y + m_CapsuleHeight)));
                graphics.DrawLine(Pens.DarkGray, new Point((int)(m_innerBounds.X+m_innerBounds.Width), (int)(m_innerBounds.Y + m_CapsuleHeight)), 
                    new Point((int)(m_innerBounds.X + m_innerBounds.Width), (int)(m_innerBounds.Y + m_CapsuleHeight)));

                if (diagram.Title != null)
                {
                    string text = diagram.Title.Text;
                    graphics.DrawString(text, new Font(diagram.Title.FontName,8f), new SolidBrush(canvas.ForeColor), new Point((int)(m_innerBounds.X + 3), (int)(m_innerBounds.Y + 3)));
                }


                //Rectangle rec = new Rectangle((int)this.Bounds.X+4, (int)(this.Bounds.Y + m_CapsuleHeight)+4, (int)this.Bounds.Width-8, (int)(this.Bounds.Width / size.Width * size.Height) -8);
              
                double aspectRatio = (this.Bounds.Width / size.Width) * size.Height;
                Rectangle rec = new Rectangle((int)this.Bounds.X + 4, (int)(this.Bounds.Y + m_CapsuleHeight) + 4, (int)this.Bounds.Width - 8, (int)(aspectRatio) - 8);
                graphics.FillRectangle(Brushes.White, rec);
                graphics.FillRectangle(Brushes.White, rec);
                graphics.DrawRectangle(Pens.Black, rec);

                if (Grasshopper.GUI.Canvas.GH_Canvas.ZoomFadeLow > 0 )
                {

                    if (canvasDiagram.Update || canvasDiagram.Bitmap == null)
                    {
                        canvasDiagram.Bitmap = diagram.DrawBitmap(canvasDiagram.Scale);
                        canvasDiagram.Update = false;
                    }
                    
                    graphics.DrawImage(canvasDiagram.Bitmap, rec.X+1, rec.Y+1, rec.Width-2, rec.Height-2);
                    
                }

            }
        }
       
    }
}
