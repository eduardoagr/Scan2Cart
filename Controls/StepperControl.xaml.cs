using System.Windows.Input;

namespace Scan2Cart.Controls;

public partial class StepperControl : ContentView {
    public StepperControl() {
        InitializeComponent();
    }


    public static readonly BindableProperty PlusCommandProperty = BindableProperty.Create(
        nameof(PlusCommand), typeof(ICommand), typeof(StepperControl));

    public ICommand PlusCommand {
        get => (ICommand)GetValue(PlusCommandProperty);
        set => SetValue(PlusCommandProperty, value);
    }


    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value), typeof(int), typeof(StepperControl));

    public int Value {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }


    public static readonly BindableProperty MinusCommandProperty = BindableProperty.Create(
        nameof(MinusCommand), typeof(ICommand), typeof(StepperControl));

    public ICommand MinusCommand {
        get => (ICommand)GetValue(MinusCommandProperty);
        set => SetValue(MinusCommandProperty, value);
    }


}