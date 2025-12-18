namespace Scan2Cart.Models;

public partial class Product : ObservableObject {

    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required string ImageUrl { get; set; }

    [ObservableProperty] public partial decimal Price { get; set; }

    public required int Quantity { get; set; }

    [ObservableProperty] public partial int SelectedQuantity { get; set; } = 1;

    [ObservableProperty] public partial decimal Total { get; set; }

    public IEnumerable<int> QuantityRange => Enumerable.Range(1, Quantity);

    partial void OnSelectedQuantityChanged(int value) {
        Total = Price * value;
        OnPropertyChanged(nameof(Total));
    }

    partial void OnPriceChanged(decimal value) {
        Total = value * SelectedQuantity;
        OnPropertyChanged(nameof(Total));
    }
}