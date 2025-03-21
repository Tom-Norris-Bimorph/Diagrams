namespace DiagramLibrary
{
    public class DimensionAttributes : IDimensionAttributes
    {

        public string Suffix { get; }

        public int RoundTo { get; }

        public float Offset { get; }

        public string OverrideText { get; }

        public DimensionAttributes(float offset, int roundTo, string suffix, string overrideText)
        {
            this.Offset = offset;
            this.RoundTo = roundTo;
            this.Suffix = suffix;
            this.OverrideText = overrideText;
        }
    }
}