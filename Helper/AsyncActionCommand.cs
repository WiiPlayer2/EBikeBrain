using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EBikeBrain.Helper
{
    internal class AsyncActionCommand : ICommand
    {
        private readonly Func<object?, Task> execute;
        private readonly Predicate<object?>? canExecute;

        public AsyncActionCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = default)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => canExecute != null ? canExecute(parameter) : false;

        public async void Execute(object? parameter) => await execute(parameter);
    }
}
