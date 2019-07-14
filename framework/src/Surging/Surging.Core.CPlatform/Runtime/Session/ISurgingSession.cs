
namespace Surging.Core.CPlatform.Runtime.Session
{
    public interface ISurgingSession
    {
        long? UserId { get; }

        string UserName { get; }

    }
}
