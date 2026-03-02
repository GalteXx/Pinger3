namespace Pinger3.ViewModels.Controls
{
    public interface IPingingTargetWrapperViewModel
    {
        bool IsMarkedForPopout { get; set; }
        IPingingTargetViewModel TargetViewModel { get; }
    }
}