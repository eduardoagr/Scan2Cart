namespace Scan2Cart.Services;

class PageService : IPageService {

    public async Task DisplayAlertAsync(string title, string message, string cancel) => await Shell.Current.DisplayAlert(title, message, cancel);

    public async Task DisplayToastAsync(string message, ToastDuration toastDuration = ToastDuration.Short, double fontSize = 14) {
        var cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make(message, toastDuration, fontSize);
        await toast.Show(cancellationTokenSource.Token);
    }

    public async Task NavigateBackAsync() => await Shell.Current.GoToAsync("...", true);

    public async Task NavigateToAsync(string route) => await Shell.Current.GoToAsync(route, true);

    public async Task NavigateToAsync(string route, IDictionary<string, object> parameters) => await Shell.Current.GoToAsync(route, parameters);
}
