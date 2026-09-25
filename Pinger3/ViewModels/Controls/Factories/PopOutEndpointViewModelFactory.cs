using Pinger3.Models;
using Pinger3.ViewModels.Controls;

namespace Pinger3.Services.PopOut.Factories;

public class PopOutEndpointViewModelFactory
{
    public IPopOutEndpointViewModel Create(EndpointModel model)
    {
        return new PopOutEndpointViewModel(model.Id, model.Name);
    }
}