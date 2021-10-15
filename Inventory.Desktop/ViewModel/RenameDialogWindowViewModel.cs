using System;
using System.Windows.Input;
using Inventory.Desktop.Commands;

namespace Inventory.Desktop.ViewModel
{
    public class RenameDialogWindowViewModel : ViewModelBase
    {
        public string RenameString { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public Action<string> CallBackOnWindowClose { get; set; }
        public bool RenameStringEmpty => string.IsNullOrEmpty(RenameString);

        public Action CloseWindowAction { get; set; }

        public RenameDialogWindowViewModel()
        {
            CloseWindowCommand = new RelayCommand(() =>
            {
                if (!string.IsNullOrEmpty(RenameString))
                    CallBackOnWindowClose?.Invoke(RenameString);

                CloseWindowAction?.Invoke();

            });
        }


    }
}