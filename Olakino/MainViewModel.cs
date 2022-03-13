using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.UI.Xaml;

namespace Olakino;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private Timer? _timer;

    public ObservableCollection<ListItem> Items { get; } = new();

    private TimeSpan _selectedTime;

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

    public double Gram { get; set; }

    public double CurrentAmount { get; private set; }
    public string RemainingTime { get; private set; } = string.Empty;

    public MainViewModel()
    {
        TryStartTimer();
    }

    public void OnAddClick(object sender, RoutedEventArgs e)
    {
        CurrentAmount += Gram;
        // CurrentAmountText.Text = $"{_currentAmount:N}";
        Items.Add(new ListItem($"{Gram:N}", $"{Amount:N} / {Percent:P1}"));
    }

    private void UpdateGram()
    {
        // TODO
        Gram = Amount * Percent * 0.01 * 0.789;
    }

    private void OnTimerUpdated(object state)
    {
        var duration = SelectedTime.Subtract(DateTime.Now.TimeOfDay);
        var text = duration.ToString();
        if (duration <= TimeSpan.Zero)
        {
            text = "NaN";
            _timer?.Dispose();
            _timer = null;
        }

        RemainingTime = text;
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
