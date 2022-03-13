using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using Windows.UI.Core;
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
        private Timer? _timer;
        private TimeSpan _timeSpan;
        private double _currentAmount = 0;

        public ObservableCollection<ListItem> Items { get; } = new ObservableCollection<ListItem>();

        public MainPage()
        {
            this.InitializeComponent();
            TryStartTimer();
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(GramTextBox.Text, out var gram) ||
                !int.TryParse(AmountTextBox.Text, out var amount) ||
                !double.TryParse(PercentTextBox.Text, out var percent))
            {
                return;
            }

            _currentAmount += gram;
            CurrentAmountText.Text = _currentAmount.ToString(CultureInfo.CurrentCulture);
            Items.Add(new ListItem($"{gram:N}", $"{amount:N} / {percent:P1}"));
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

            GramTextBox.Text = $"{amount * percent * 0.01 * 0.789:N}";
        }

        private async void OnTimerUpdated(object state)
        {
            var duration = _timeSpan.Subtract(DateTime.Now.TimeOfDay);
            var text = duration.ToString();
            if (duration <= TimeSpan.Zero)
            {
                text = "NaN";
                _timer?.Dispose();
                _timer = null;
            }

            await TimerText.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                TimerText.Text = text
            );
        }

        private void OnSelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
        {
            var time = args.NewTime;
            if (time == null) return;
            _timeSpan = time.Value;
            TryStartTimer();
        }

        private void TryStartTimer()
        {
            var duration = _timeSpan.Subtract(DateTime.Now.TimeOfDay);
            if (duration <= TimeSpan.Zero || _timer != null) return;
            _timer = new Timer(OnTimerUpdated, null, 1000, 1000);
        }
    }
}
