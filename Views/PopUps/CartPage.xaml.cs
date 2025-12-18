namespace Scan2Cart.Views.PopUps;

public partial class CartPage:SfPopup {
    public CartPage() {
        InitializeComponent();
    }

    private void SwipeItem_Invoked(object sender,EventArgs e) {

        if(sender is SwipeItem swipe && swipe.BindingContext is Product p) {
            (BindingContext as HomePageViewModel)?.SwipeToDeleteCommand.Execute(p);
        }
    }
}