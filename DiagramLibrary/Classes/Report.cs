using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace DiagramLibrary
{
    public class Report
    {
        List<ReportItem> m_Messages = new List<ReportItem>();


        public List<ReportItem> ReportItems
        {
            get { return m_Messages; }

        }

        public override string ToString()
        {
            List<string> msgs = this.FilterExcludeByLevel(GH_RuntimeMessageLevel.Remark);
            return String.Join(", ", msgs);
        }

        public static Report OneLiner(string message) {
            Report output = new Report();
            output.AddMessage(message);
            return output;
        }


        public static Report OneLiner(string message, GH_RuntimeMessageLevel level)
        {
            Report output = new Report();
            output.AddMessage(message, level);
            return output;
        }


        public void AddReport(Report report) {
            this.AddMessages(report.ReportItems);
        }

        public string GetWarnings() {
            List<string> msgs = this.FilterByLevel(GH_RuntimeMessageLevel.Warning);
            return String.Join(", ", msgs);
        }

        public string GetErrors()
        {
            List<string> msgs = this.FilterByLevel(GH_RuntimeMessageLevel.Error);
            return String.Join(", ", msgs);
        }


        public bool HasWarnings()
        {
          
            for (int i = 0; i < m_Messages.Count; i++)
            {
                if (m_Messages[i].Level == GH_RuntimeMessageLevel.Warning)
                {
                    return true;
                }
            }
            return false;
        }


        public bool HasErrors()
        {

            for (int i = 0; i < m_Messages.Count; i++)
            {
                if (m_Messages[i].Level == GH_RuntimeMessageLevel.Error)
                {
                    return true;
                }
            }
            return false;
        }




        public List<string> FilterByLevel(GH_RuntimeMessageLevel level) {
            List<string> output = new List<string>();
            for (int i = 0; i < m_Messages.Count; i++)
            {
                if (m_Messages[i].Level == level) {
                    output.Add(m_Messages[i].Message);
                }
            }
            return output;
        }

        public List<string> FilterExcludeByLevel(GH_RuntimeMessageLevel level)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < m_Messages.Count; i++)
            {
                if (m_Messages[i].Level != level)
                {
                    output.Add(m_Messages[i].Message);
                }
            }
            return output;
        }

        public List<string> Messages
        {
            get { return m_Messages.Select(x => x.Message).ToList(); }

        }


        public void AddMessage(string message) {
            m_Messages.Add(new ReportItem(message));
        }

        public void AddMessage(string message, GH_RuntimeMessageLevel level)
        {
            m_Messages.Add(new ReportItem(message, level));
        }

        public void AddMessage(ReportItem message)
        {
            m_Messages.Add(message);
        }


        public void AddMessages(List<string> message)
        {
            for (int i = 0; i < message.Count; i++)
            {
                this.AddMessage(message[i]);
            }
           
        }

        public void AddMessages(List<ReportItem> message)
        {
            for (int i = 0; i < message.Count; i++)
            {
                this.AddMessage(message[i]);
            }

        }

        public virtual Report Duplicate() {
            Report output = new Report();
            output.m_Messages = m_Messages;
                      return output;
        }


    }


    public class ReportItem
    {
        string m_Message = null;
        GH_RuntimeMessageLevel m_Level;

        public string Message
        {
            get { return m_Message; }

        }


        public GH_RuntimeMessageLevel Level
        {
            get { return m_Level; }

        }

       public ReportItem(string message) {
            m_Message = message;
            m_Level = GH_RuntimeMessageLevel.Remark;
        }

        public ReportItem(string message, GH_RuntimeMessageLevel level)
        {
            m_Message = message;
            m_Level = level;
        }




    }



}