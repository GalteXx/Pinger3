using Avalonia.Controls;
using Pinger3.ViewModels.PageViewModels;

namespace Pinger3.Views;

public partial class EndpointEditorWindow : Window
{
    public EndpointEditorWindow()
    {
        InitializeComponent();
    }

    public EndpointEditorWindow(IEndpointEditorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.OperationFinished += (_, _) => Close();
    }
}
