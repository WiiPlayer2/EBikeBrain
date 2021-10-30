using EBikeBrain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EBikeBrain
{
    internal class MainViewModel
    {
        public ICommand ConnectCommand { get; }

        public MainViewModel()
        {
            ConnectCommand = new AsyncActionCommand(Connect);
        }

        private Task Connect(object? arg)
        {
            return Task.CompletedTask;
        }
    }
}
