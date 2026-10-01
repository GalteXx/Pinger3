using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public interface IEndpointRuntimeFactory
{
    EndpointRuntime Create(EndpointModel model);
}