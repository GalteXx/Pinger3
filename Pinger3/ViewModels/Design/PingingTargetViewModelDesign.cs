using CommunityToolkit.Mvvm.Input;
using Pinger3.ViewModels.Controls;
using System;
using System.ComponentModel;

namespace Pinger3.ViewModels.Design;

public class PingingTargetViewModelDesign : IPingingTargetViewModel
{
    public static PingingTargetViewModelDesign Instance => new();

    public string DomainOrAddress => "192.168.2.1";

    public string Name => "Mockup Server";

    public TimeSpan Ping { get; } = TimeSpan.FromMilliseconds(Random.Shared.Next(1, 120));
    public TimeSpan TimeSinceLastRequest { get; } = TimeSpan.FromMilliseconds(Random.Shared.Next(1, 100));

    public bool IsActive
    {
        get => Random.Shared.Next(0, 2) == 0;
        set { }
    }
    
    public TimeSpan DelayBetweenRequests =>
        TimeSpan.FromMilliseconds(Random.Shared.Next(30, (int)TimeSpan.FromHours(2).TotalMilliseconds));

#pragma warning disable CS0067 // The event is never used
    public event PropertyChangedEventHandler? PropertyChanged;
}