using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModels
{
    /// <summary>
    /// Implémente ICommand pour relayer les appels aux délégués.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _exec;
        private readonly Func<T?, bool>? _can;

        public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _exec = execute;
            _can = canExecute;
        }

        public bool CanExecute(object? parameter) => _can?.Invoke((T?)parameter) ?? true;
        public void Execute(object? parameter) => _exec((T?)parameter);
        public event EventHandler? CanExecuteChanged;
        
        /// <summary>
        /// Déclenche l'événement CanExecuteChanged pour réévaluer l'état de la commande.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Implémente ICommand pour relayer les appels aux délégués sans paramètre.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _exec; private readonly Func<bool>? _can;
        public RelayCommand(Action exec, Func<bool>? can = null) { _exec = exec; _can = can; }
        public bool CanExecute(object? p) => _can?.Invoke() ?? true;
        public void Execute(object? p) => _exec();
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Déclenche l'événement CanExecuteChanged pour réévaluer l'état de la commande.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
