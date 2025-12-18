namespace Scan2Cart.ViewModels;

public partial class HomePageViewModel(IPageService pageService, IDataProvider dataProvider) : BaseViewModel(pageService) {

    private bool _isPopupOpening;

    private DateTime _lastAlertTime = DateTime.MinValue;

    #region ObservableProperties

    [ObservableProperty]
    public partial bool IsSelectedQtyPopopOpened { get; set; }

    [ObservableProperty]
    public partial bool IsCartPopopOpen { get; set; }

    [ObservableProperty]
    public partial bool ShouldDetect { get; set; } = true;

    [ObservableProperty]
    public partial bool IsProductInfoVisible { get; set; } = false;

    [ObservableProperty]
    public partial Product? Product { get; set; }

    [ObservableProperty]
    public partial bool IsPopUpDetectedOpen { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FormattedCartTotal))]
    public partial decimal CartTotal { get; set; }

    public ObservableCollection<Product> Products { get; set; } = [];

    #endregion

    public string FormattedCartTotal => CartTotal.ToString("C", CultureInfo.CurrentCulture);

    private void RecalculateCartTotal() {
        CartTotal = Products.Sum(p => p.Total);
    }

    private void AddProduct(Product p) {

        // Listen for changes in SelectedQuantity or Price
        p.PropertyChanged += (s, e) => {
            if(e.PropertyName == nameof(Product.SelectedQuantity) ||
                e.PropertyName == nameof(Product.Price)) {
                RecalculateCartTotal();
            }
        };

        Products.Add(p);
        RecalculateCartTotal();
    }

    [RelayCommand]
    private async Task BarcodesDetected(IEnumerable<BarcodeResult> results) {

        if(_isPopupOpening) return;

        var val = results.FirstOrDefault()?.Value;
        if(string.IsNullOrEmpty(val)) return;

        _isPopupOpening = true;

        Product = await dataProvider.GetProductByIdAsync(val);

        if(Product is null) {

            await ShowProductNotFoundAlertAsync();
            _isPopupOpening = false;
            return;
        }

        await Task.Delay(100);

        MainThread.BeginInvokeOnMainThread(() => {
            IsProductInfoVisible = true;
            IsPopUpDetectedOpen = true;
        });

        _isPopupOpening = false;
    }

    [RelayCommand]
    void Yes() {

        // Add product if not already in the list
        if(Product is not null && !Products.Any(x => x.Id == Product.Id))
            AddProduct(Product);

        // Close popup and reset flags
        IsPopUpDetectedOpen = false;
        ShouldDetect = true;
    }

    [RelayCommand]
    void No() {

        IsPopUpDetectedOpen = false;
        ShouldDetect = true;
    }

    [RelayCommand]
    async Task OpenProductPopop() {

        ShouldDetect = false;
    }

    [RelayCommand]
    void CloseProductPopop() {

        IsProductInfoVisible = false;
        IsPopUpDetectedOpen = false;
        ShouldDetect = true;
    }
    [RelayCommand]
    async Task ShowCart() {

        ShouldDetect = false;
        IsCartPopopOpen = true;
    }

    [RelayCommand]
    async Task HideCart() {

        IsCartPopopOpen = false;
        ShouldDetect = true;
    }

    [RelayCommand]
    void OpenQuantityPopUp(Product p) {

        IsSelectedQtyPopopOpened = true;
    }

    [RelayCommand]
    void SwipeToDelete(Product p) {

        Products.Remove(p);
        RecalculateCartTotal();
    }

    [RelayCommand]
    void ClearAll() {

        Products.Clear();
        RecalculateCartTotal();
    }
    private async Task ShowProductNotFoundAlertAsync() {
        var now = DateTime.UtcNow;
        if((now - _lastAlertTime).TotalSeconds < 3) return;
        _lastAlertTime = now;

        await MainThread.InvokeOnMainThreadAsync(async () => {
            await DisplayAlertAsync("Error", "It looks like this item does not exist in your database", "OK");
        });
    }

}
