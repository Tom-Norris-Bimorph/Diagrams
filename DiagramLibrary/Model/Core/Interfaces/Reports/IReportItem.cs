using Grasshopper.Kernel;

namespace DiagramLibrary.Core
{

    /// <summary>
    /// An item in the <see cref="IReport"/>.
    /// </summary>
    public interface IReportItem
    {
        /// <summary>
        /// The Message of this <see cref="IReportItem"/>.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// The warning level of this <see cref="IReportItem"/>.
        /// </summary>
        GH_RuntimeMessageLevel Level { get; }
    }
}