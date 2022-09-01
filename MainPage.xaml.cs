namespace Phoneword;

public partial class MainPage : ContentPage
{
    private string? _translatedNumber;

    public MainPage() => InitializeComponent();

    private void TranslateButton_Clicked(object sender, EventArgs e)
    {
        string enteredNumber = PhoneNumberText.Text;
        _translatedNumber = PhonewordTranslator.ToNumber(enteredNumber);
        if (!string.IsNullOrEmpty(enteredNumber))
        {
            CallButton.IsEnabled = true;
            CallButton.Text = $"Call {_translatedNumber}";
        }
        else
        {
            CallButton.IsEnabled = false;
            CallButton.Text = "Call";
        }
    }

    private async void CallButton_Clicked(object sender, EventArgs e)
    {
        if (await DisplayAlert(title: "Dial a Number",
                               message: $"Would you like to call {_translatedNumber}?",
                               accept: "Yes",
                               cancel: "No"))
        {
            try
            {
                if (PhoneDialer.Default.IsSupported)
                {
                    System.Diagnostics.Debug.Assert(_translatedNumber is not null);
                    PhoneDialer.Default.Open(_translatedNumber);
                }
            }
            catch (ArgumentNullException)
            {
                await DisplayAlert(title: "Unable to dial",
                                   message: "Phone nunber was not valid.",
                                   cancel: "OK");
                throw;
            }
            catch (Exception)
            {
                await DisplayAlert(title: "Unable to dial", message: "Phone dialing failed.", cancel: "OK");
            }
        }
    }
}

