using System.Collections.Generic;

namespace Pinger3.Services;

public interface IPopOutEndpointsCache
{
    IEnumerable<string> GetAllIds();
    void AddId(string id);
    void RemoveId(string id);
    bool IsStored(string id);
}