namespace DiagramLibrary
{
    public interface IDimensionAttributes
    {
        string Suffix { get; }
        int RoundTo { get; }
        float Offset { get; }
        string OverrideText { get; }
    }
}