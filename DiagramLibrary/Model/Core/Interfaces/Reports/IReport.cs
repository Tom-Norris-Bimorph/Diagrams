using Grasshopper.Kernel;
using System.Collections.Generic;

namespace DiagramLibrary.Core
{
    public interface IReport : IEnumerable<IReportItem>
    {
        /// <summary>
        /// Returns a string with all the warnings in the report.
        /// </summary>
        string GetWarnings();

        /// <summary>
        /// Returns a string with all the errors in the report.
        /// </summary>
        string GetErrors();

        /// <summary>
        /// Boolean value indicating if the report has any warnings.
        /// </summary>
        bool HasWarnings();

        /// <summary>
        /// Boolean value indicating if the report has any errors.
        /// </summary>
        bool HasErrors();

        /// <summary>
        /// Filters the report items by level which do match the input
        /// and returns the messages.
        /// </summary>
        List<string> FilterByLevel(GH_RuntimeMessageLevel level);

        /// <summary>
        /// Filters the report items by level which do not match the input
        /// and returns the messages.
        /// </summary>
        List<string> FilterExcludeByLevel(GH_RuntimeMessageLevel level);

        /// <summary>
        /// Adds all the report items from another <see cref="IReport"/> to this one.
        /// </summary>
        /// <param name="report"></param>
        void AddReport(IReport report);

        /// <summary>
        /// Adds a single report item to the report.
        /// </summary>
        void AddReportItem(IReportItem message);

        /// <summary>
        /// Adds a list of report items to the report.
        /// </summary>
        void AddReportItems(List<IReportItem> reportItems);

        /// <summary>
        /// Adds a single message to the report.
        /// </summary>
        void AddMessage(string message, GH_RuntimeMessageLevel level = GH_RuntimeMessageLevel.Remark);

        /// <summary>
        /// Adds a list of messages to the report.
        /// </summary>
        void AddMessages(List<string> messages);

    }
}