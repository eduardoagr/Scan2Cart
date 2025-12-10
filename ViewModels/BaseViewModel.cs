namespace Scan2Cart.ViewModels;

public abstract partial class BaseViewModel(IPageService pageService) : ObservableObject {

    // Shell Helpers...
    protected async Task DisplayAlertAsync(string title, string message, string cancel) => await pageService.DisplayAlertAsync(title, message, cancel);

    protected async Task DisplayToastAsync(string message, ToastDuration toastDuration = ToastDuration.Short, double fontSize = 14) {
        var cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make(message, toastDuration, fontSize);
        await toast.Show(cancellationTokenSource.Token);
    }

    // Navigation helpers...
    protected async Task NavigateAsync(string route) => await pageService.NavigateToAsync(route);
    protected async Task NavigateAsync(string route, IDictionary<string, object> parameters) => await pageService.NavigateToAsync(route, parameters);
    protected async Task NavigateBackAsync() => await pageService.NavigateBackAsync();
}
