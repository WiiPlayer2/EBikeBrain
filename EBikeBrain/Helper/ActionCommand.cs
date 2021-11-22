using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EBikeBrain.Helper
{
    internal class ActionCommand : ICommand
    {
        private readonly Action<object?> execute;

        private readonly Predicate<object?>? canExecute;

        public event EventHandler? CanExecuteChanged;

        private bool couldExecute;

        public ActionCommand(Action<object?> execute, Predicate<object?>? canExecute = default)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public ActionCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = default)
            : this((Action<object?>)(async obj => await execute(obj)), canExecute) { }

        public ActionCommand(Action execute, Func<bool>? canExecute = default)
            : this(_ => execute(), _ => canExecute?.Invoke() ?? true) { }

        public ActionCommand(Func<Task> execute, Func<bool>? canExecute = default)
            : this((Action)(async () => await execute()), canExecute) { }

        public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter)
        {
            Refresh(parameter);

            if (!couldExecute)
                return;

            execute(parameter);
        }

        public void Refresh(object? parameter = default)
        {
            var canExecuteNow = CanExecute(parameter);
            if (canExecuteNow != couldExecute)
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            couldExecute = canExecuteNow;
        }
    }
}
