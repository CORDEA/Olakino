using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Olakino;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private Timer? _timer;

    public ObservableCollection<ListItem> Items { get; } = new();

    private TimeSpan _selectedTime = TimeSpan.Parse("22:00:00");

    public TimeSpan SelectedTime
    {
        get => _selectedTime;
        set
        {
            if (_selectedTime == value)
            {
                return;
            }

            _selectedTime = value;
            TryStartTimer();
        }
    }

    private double _amount;

    public double Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            UpdateGram();
        }
    }

    private double _percent;

    public double Percent
    {
        get => _percent;
        set
        {
            _percent = value;
            UpdateGram();
        }
    }

    public PercentFormatter PercentFormatter => new()
    {
        IntegerDigits = 1,
        FractionDigits = 1,
        NumberRounder = new SignificantDigitsNumberRounder()
    };

    public double Calorie { get; set; }

    private double _gram;

    public double Gram
    {
        get => _gram;
        set
        {
            if (Math.Abs(_gram - value) < 0.01) return;
            _gram = value;
            OnPropertyChanged();
        }
    }

    private double _currentAmount;

    public string CurrentAmount => $"{_currentAmount:N}g";

    private double _totalCalories;

    public string TotalCalories => $"{_totalCalories:N0}kcal";

    private string _remainingTime = string.Empty;

    public string RemainingTime
    {
        get => _remainingTime;
        private set
        {
            if (_remainingTime == value) return;
            _remainingTime = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        TryStartTimer();
    }

    public void OnAddClick(object sender, RoutedEventArgs e)
    {
        Items.Add(new ListItem($"{Gram:N}g", $"{Amount:N} / {Percent:P1}"));
        _currentAmount += Gram;
        _totalCalories += Calorie;
        OnPropertyChanged(nameof(CurrentAmount));
        OnPropertyChanged(nameof(TotalCalories));
    }

    private void UpdateGram()
    {
        Gram = Amount * Math.Round(Percent, 2) * 0.789;
    }

    private async void OnTimerUpdated(object state)
    {
        var duration = SelectedTime.Subtract(DateTime.Now.TimeOfDay);
        var text = $"{duration.Hours:00}:{duration.Minutes:00}:{duration.Seconds:00}";
        if (duration <= TimeSpan.Zero)
        {
            text = "NaN";
            _timer?.Dispose();
            _timer = null;
        }

        await CoreApplication.MainView.Dispatcher.RunAsync(
            CoreDispatcherPriority.Normal,
            () => RemainingTime = text
        );
    }


    private void TryStartTimer()
    {
        var duration = SelectedTime.Subtract(DateTime.Now.TimeOfDay);
        if (duration <= TimeSpan.Zero || _timer != null) return;
        _timer = new Timer(OnTimerUpdated, null, 1000, 1000);
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
