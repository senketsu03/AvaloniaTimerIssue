using System;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;

namespace TimerIssue.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private DispatcherTimer Timer { get; set; }

    private TimeSpan _timerValue = TimeSpan.Zero;

    public string TimerValue
    {
        get => _timerValue.ToString("g", CultureInfo.InvariantCulture);
        set
        {
            var timer = TimeSpan.Parse(value, CultureInfo.InvariantCulture);

            this.RaiseAndSetIfChanged(ref _timerValue, timer);
        }
    }

    public MainWindowViewModel() => InitializeTimer();

    private void InitializeTimer() => Timer = new DispatcherTimer(TimeSpan.FromSeconds(1), default, TimerTick);

    private void TimerTick(object sender, EventArgs args)
    {
        var timer = (DispatcherTimer)sender;
        TimerValue = _timerValue.Add(timer.Interval).ToString();
    }

    public void StartTimer()
    {
        InitializeTimer();
        TimerValue = TimeSpan.Zero.ToString();
        Timer.Start();
    }

    public void StopTimer() => Timer?.Stop();

    public async Task StartButton()
    {
        StartTimer();

        int iterationCount = 1000;

        for (int i = 0; i <= iterationCount; i++)
        {
            await Task.Delay(1).ConfigureAwait(true);
        }

        StopTimer();
    }

    public Task CancelButton() => Task.Run(() => StopTimer());
}
