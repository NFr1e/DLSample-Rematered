namespace DLSample.Facility.Input
{
    public interface IInputLayer
    {
        string Name { get; }
        int Priority { get; }
        bool BlockLowerLayers { get; }
    }
}
