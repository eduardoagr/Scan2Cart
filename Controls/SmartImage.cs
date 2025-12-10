namespace Scan2Cart.Controls;

public partial class SmartImage : Image {

    private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(10) };

    private static readonly Dictionary<string, byte[]> _cache = [];

    public event EventHandler? ImageLoaded;

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(ImageSource),
            typeof(SmartImage),
            default(ImageSource),
            propertyChanged: OnPlaceholderChanged);

    private static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue) {
        var control = ( SmartImage )bindable;
        control.Source ??= ( ImageSource )newValue;
    }

    public static readonly BindableProperty IsImageLoadedProperty =
    BindableProperty.Create(
        nameof(IsImageLoaded),
        typeof(bool),
        typeof(SmartImage),
        false,
        BindingMode.TwoWay);

    public bool IsImageLoaded {
        get => ( bool )GetValue(IsImageLoadedProperty);
        set => SetValue(IsImageLoadedProperty, value);
    }


    public ImageSource Placeholder {
        get => ( ImageSource )GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty ErrorProperty =
        BindableProperty.Create(
            nameof(Error),
            typeof(ImageSource),
            typeof(SmartImage),
            default(ImageSource));

    public ImageSource Error {
        get => ( ImageSource )GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }

    public static readonly BindableProperty DynamicSourceProperty =
        BindableProperty.Create(
            nameof(DynamicSource),
            typeof(object),
            typeof(SmartImage),
            default,
            propertyChanged: OnDynamicSourceChanged);

    public object DynamicSource {
        get => GetValue(DynamicSourceProperty);
        set => SetValue(DynamicSourceProperty, value);
    }

    private static void OnDynamicSourceChanged(BindableObject bindable, object oldValue, object newValue) {
        var control = ( SmartImage )bindable;
        // Use Fire-and-forget with proper async handling
        _ = control.SetImageAsync(newValue);
    }

    private async Task SetImageAsync(object newValue) {
        try {
            switch (newValue) {
                case ImageSource imgSource:
                    Source = imgSource;
                    break;

                case byte[] bytes when bytes.Length > 0:
                    Source = ImageSource.FromStream(() => new MemoryStream(bytes));
                    break;

                case string str when !string.IsNullOrWhiteSpace(str):
                    if (Uri.TryCreate(str, UriKind.Absolute, out var uri) &&
                        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)) {
                        // Remote image with caching
                        Source = await LoadRemoteImage(uri) ?? Error ?? Placeholder;
                    } else {
                        TryBase64(str);
                    }
                    break;

                default:
                    Source = Placeholder;
                    break;
            }

            if (Source != Placeholder)

                IsImageLoaded = true;
                ImageLoaded?.Invoke(this, EventArgs.Empty);

        } catch {
            Source = Error ?? Placeholder;
        }
    }

    private void TryBase64(string str) {
        try {
            var bytes = Convert.FromBase64String(str);
            Source = ImageSource.FromStream(() => new MemoryStream(bytes));
        } catch {
            Source = Error ?? Placeholder;
        }
    }

    private static async Task<ImageSource?> LoadRemoteImage(Uri uri) {
        try {
            var key = uri.ToString();

            if (_cache.TryGetValue(key, out var cachedBytes))
                return ImageSource.FromStream(() => new MemoryStream(cachedBytes, writable: false));

            var bytes = await DownloadImageAsync(uri);
            _cache[key] = bytes;

            return ImageSource.FromStream(() => new MemoryStream(bytes, writable: false));
        } catch {
            return null;
        }
    }

    private static async Task<byte[]> DownloadImageAsync(Uri uri) {
        using var stream = await _httpClient.GetStreamAsync(uri);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return ms.ToArray();
    }
}
