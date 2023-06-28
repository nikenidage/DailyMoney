using DailyMoneyUi.Views;
using ReactiveUI;
using System;
using System.Windows.Input;
using DailyMoneyUi.Models;
using System.IO;

namespace DailyMoneyUi.ViewModels;

public class SettingWindowViewModel : ViewModelBase
{
    public ICommand SaveCommand { get; }

    public SettingWindowViewModel()
    {
        SaveCommand = ReactiveCommand.Create(MoneyCalculateSettings.Save);
    }
}

