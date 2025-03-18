using DiagramLibrary.Core;
using Grasshopper.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DiagramLibrary
{
    /// <inheritdoc cref="IReport"/>
    public class Report : IReport
    {
        private readonly IList<IReportItem> _reportItems = new List<IReportItem>();

        /// <summary>
        /// Constructs a new empty <see cref="IReport"/>
        /// </summary>
        public Report()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="IReport"/>
        /// </summary>
        public Report(string message, GH_RuntimeMessageLevel level = GH_RuntimeMessageLevel.Remark)
        {
            this.AddMessage(message, level);
        }

        /// <inheritdoc />
        public string GetWarnings()
        {
            var messages = this.FilterByLevel(GH_RuntimeMessageLevel.Warning);
            return String.Join(", ", messages);
        }

        /// <inheritdoc />
        public string GetErrors()
        {
            var messages = this.FilterByLevel(GH_RuntimeMessageLevel.Error);
            return String.Join(", ", messages);
        }

        /// <inheritdoc />
        public bool HasWarnings()
        {
            foreach (var reportItem in _reportItems)
            {
                if (reportItem.Level == GH_RuntimeMessageLevel.Warning)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public bool HasErrors()
        {
            foreach (var reportItem in _reportItems)
            {
                if (reportItem.Level == GH_RuntimeMessageLevel.Error)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public List<string> FilterByLevel(GH_RuntimeMessageLevel level)
        {
            var output = new List<string>();
            for (var i = 0; i < _reportItems.Count; i++)
            {
                if (_reportItems[i].Level == level)
                {
                    output.Add(_reportItems[i].Message);
                }
            }

            return output;
        }

        /// <inheritdoc />
        public List<string> FilterExcludeByLevel(GH_RuntimeMessageLevel level)
        {
            var output = new List<string>();
            for (var i = 0; i < _reportItems.Count; i++)
            {
                if (_reportItems[i].Level != level)
                {
                    output.Add(_reportItems[i].Message);
                }
            }

            return output;
        }

        /// <inheritdoc />
        public void AddReport(IReport report)
        {
            foreach (var reportItem in report)
            {
                _reportItems.Add(reportItem);
            }
        }

        /// <inheritdoc />
        public void AddReportItem(IReportItem message)
        {
            _reportItems.Add(message);
        }

        /// <inheritdoc />
        public void AddReportItems(List<IReportItem> reportItems)
        {
            foreach (var reportItem in reportItems)
            {
                this.AddReportItem(reportItem);
            }
        }

        /// <inheritdoc />
        public void AddMessage(string message, GH_RuntimeMessageLevel level = GH_RuntimeMessageLevel.Remark)
        {
            _reportItems.Add(new ReportItem(message, level));
        }

        /// <inheritdoc />
        public void AddMessages(List<string> messages)
        {
            foreach (var message in messages)
            {
                this.AddMessage(message);
            }
        }

        /// <inheritdoc />
        public IEnumerator<IReportItem> GetEnumerator() => _reportItems.GetEnumerator();

        /// <inheritdoc />
        public override string ToString()
        {
            var messages = this.FilterExcludeByLevel(GH_RuntimeMessageLevel.Remark);
            return String.Join(", ", messages);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}