using System;
using System.ComponentModel;

namespace Pinger3.ViewModels.Controls
{
    public interface IPingingTargetViewModel
    {
        string DomainOrAddress { get; }
        string Name { get; }
        TimeSpan Ping { get; }
        TimeSpan TimeSinceLastRequest { get; }

        bool IsActive { get; set; }

        event PropertyChangedEventHandler? PropertyChanged;
    }
}