public interface ILogUpdateService{
    Task SegmentProcessAsync(int id, ReadOnlySequence<byte> segment);   
}