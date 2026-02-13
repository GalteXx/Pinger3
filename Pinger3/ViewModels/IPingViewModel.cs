using System;
using System.ComponentModel;

namespace Pinger3.ViewModels
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