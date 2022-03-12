﻿using System;
using System.Globalization;
using System.Threading;
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

        public MainPage()
        {
            this.InitializeComponent();
            TryStartTimer();
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            var gram = GramTextBox.Text;
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

        private void OnTimerUpdated(object state)
        {
            var duration = _timeSpan.Subtract(DateTime.Now.TimeOfDay);
            if (duration <= TimeSpan.Zero)
            {
                _timer?.Dispose();
                _timer = null;
                return;
            }

            // TODO
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
