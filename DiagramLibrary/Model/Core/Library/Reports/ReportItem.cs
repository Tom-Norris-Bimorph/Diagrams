using DiagramLibrary.Core;
using Grasshopper.Kernel;

namespace DiagramLibrary
{

    /// <inheritdoc cref="IReportItem"/>
    public class ReportItem : IReportItem
    {
        /// <inheritdoc />
        public string Message { get; }

        /// <inheritdoc />
        public GH_RuntimeMessageLevel Level { get; }

        /// <summary>
        /// Constructs a new <see cref="IReportItem"/>
        /// </summary>
        public ReportItem(string message,
            GH_RuntimeMessageLevel level = GH_RuntimeMessageLevel.Remark)
        {
            this.Message = message;
            this.Level = level;
        }
    }
}