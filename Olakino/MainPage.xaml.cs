using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Olakino
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            var gram = GramTextBox.Text;
            var amount = AmountTextBox.Text;
            var percent = PercentTextBox.Text;
        }

        private void OnAmountTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGram();
        }

        private void OnPercentTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGram();
        }

        private void UpdateGram()
        {
            if (!int.TryParse(AmountTextBox.Text, out var amount) ||
                !double.TryParse(PercentTextBox.Text, out var percent))
            {
                return;
            }

            GramTextBox.Text = (amount * percent).ToString(CultureInfo.CurrentCulture);
        }
    }
}
