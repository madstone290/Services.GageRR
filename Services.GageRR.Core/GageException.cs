namespace Services.GageRR.Core
{
    /// <summary>
    /// Gage R&R 서비스에서 발생하는 예외
    /// </summary>
    public class GageException : Exception
    {
        public GageException(string? message) : base("[Gage R&R Error] " + message)
        {
        }
    }
}
