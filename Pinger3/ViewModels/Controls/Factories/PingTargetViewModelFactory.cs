using Pinger3.Models;
using Pinger3.Services.Pinging;
using Pinger3.Services.PopOut;

namespace Pinger3.ViewModels.Controls.Factories;

public sealed class PingTargetViewModelFactory(IPingManager pingManager, IPopOutCatalog popout)
    : IPingTargetViewModelFactory
{
    public IPingingTargetViewModel Create(EndpointModel endpoint)
    {
        return new PingingTargetViewModel(endpoint,
            pingManager.IsRunning(endpoint.Id),
            popout.IsPopOut(endpoint.Id));
    }
}