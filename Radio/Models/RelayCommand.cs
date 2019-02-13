using System;
using System.Windows.Input;

namespace Radio.Models
{
    public class RelayCommand : ICommand
    {
         private Action<object> _action;
         private bool _canExecute;
         public event EventHandler CanExecuteChanged;
         
         public RelayCommand(Action execute, bool canExecute = true)
         {
             this._action = x => execute();
             this._canExecute = canExecute;
         }
         
         bool ICommand.CanExecute(object parameter)
         {
             return CanExecute;
         }
         
         public bool CanExecute
         {
             get { return _canExecute; }
             set
             {
                 if (_canExecute != value)
                 {
                     _canExecute = value;
                     OnCanExecuteChanged();
                 }
             }
         }
         
         public void Execute(object parameter)
         {
             this._action(parameter);
         }
         
         
         protected virtual void OnCanExecuteChanged()
         {
             CanExecuteChanged?.Invoke(this, EventArgs.Empty);
         }
    }
}
