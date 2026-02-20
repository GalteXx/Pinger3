using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Pinger3.ViewModels.Controls
{
    public interface IPingViewModel
    {
        string DomainOrAddress { get; }
        string Name { get; }
        TimeSpan Ping { get; }
        TimeSpan TimeSinceLastRequest { get; }

        event PropertyChangedEventHandler? PropertyChanged;
    }
}