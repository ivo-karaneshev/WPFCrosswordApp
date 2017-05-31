using System;
using System.Windows.Input;

namespace CrosswordApp.Common
{
    class DelegateCommand : ICommand
    {
        private Action execute;

        public DelegateCommand(Action execute)
        {
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            execute();
        }
    }
}
