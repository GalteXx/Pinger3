using Pinger3.Models;

namespace Pinger3.ViewModels.Controls.Factories;

public sealed class PingTargetViewModelFactory : IPingTargetViewModelFactory
{
    public IPingingTargetViewModel Create(EndpointModel endpoint)
    {
        return new PingingTargetViewModel(endpoint);
    }
}