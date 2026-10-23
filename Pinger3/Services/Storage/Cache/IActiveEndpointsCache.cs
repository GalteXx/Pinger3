using System.Collections.Generic;

namespace Pinger3.Services;

public interface IActiveEndpointsCache
{
    void AddId(string id);
    void RemoveId(string id);
    bool IsStored(string id);
    IEnumerable<string> GetAllIds();
}