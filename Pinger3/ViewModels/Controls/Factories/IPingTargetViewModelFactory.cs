using Pinger3.Models;

namespace Pinger3.ViewModels.Controls.Factories;

public interface IPingTargetViewModelFactory
{
    IPingingTargetViewModel Create(EndpointModel endpoint);
}