namespace Scan2Cart.ViewModels;

public partial class HomePageViewModel(IPageService pageService, IDataProvider dataProvider) : BaseViewModel(pageService) {

    [ObservableProperty]
    public partial bool ShouldDetect { get; set; } = true;

    [ObservableProperty]
    public partial bool _IsProductInfoVisible { get; set; } = false;

    [ObservableProperty]
    public partial Product Product { get; set; }

    [ObservableProperty]
    public partial bool PopUpDetectedOpen { get; set; }

    public ObservableCollection<Product> Products { get; set; } = [];

    [RelayCommand]
    private async Task BarcodesDetected(IEnumerable<BarcodeResult> results) {

        var val = results.FirstOrDefault()?.Value;
        if (string.IsNullOrEmpty(val))
            return;

        var product = await dataProvider.GetProductByIdAsync(val);
        Product = product;
        ShouldDetect = false;
        PopUpDetectedOpen = true;
    }

    [RelayCommand]
    void Yes() {

        if (!Products.Any(x => x.Id == Product.Id)) {
            Products.Add(Product);
            PopUpDetectedOpen = false;
        }

        PopUpDetectedOpen = false;
    }

    [RelayCommand]
    void No() {

        PopUpDetectedOpen = false;
    }

    [RelayCommand]
    void Opening() {

        _IsProductInfoVisible = true;
    }

    [RelayCommand]
    void Closing() {

        _IsProductInfoVisible = false;
        PopUpDetectedOpen = true;
        ShouldDetect = true;
    }
}
