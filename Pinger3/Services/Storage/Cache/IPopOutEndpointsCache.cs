namespace Pinger3.Services;

public interface IPopOutEndpointsCache
{
    void AddId(string id);
    void RemoveId(string id);
    bool IsStored(string id);
}