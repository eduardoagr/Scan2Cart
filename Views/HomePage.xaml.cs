namespace Scan2Cart.Views;

public partial class HomePage : ContentPage {
    public HomePage(HomePageViewModel pageViewModel) {
        InitializeComponent();

        BindingContext = pageViewModel;

        ProductPopupControl.BindingContext = pageViewModel;

        CameraView.Options = new BarcodeReaderOptions {

            Formats = BarcodeFormat.QrCode,
            TryHarder = false,
            Multiple = false,
        };

    }


    private void CameraView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e) {

        if (CameraView != null && BindingContext is HomePageViewModel viewModel) {

            viewModel.BarcodesDetectedCommand.Execute(e.Results);
        }
    }
}