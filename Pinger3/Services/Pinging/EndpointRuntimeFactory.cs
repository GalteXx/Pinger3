using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public class EndpointRuntimeFactory
{
    public EndpointRuntime Create(EndpointModel model)
    {
        return new EndpointRuntime { Model = model };
    }
}