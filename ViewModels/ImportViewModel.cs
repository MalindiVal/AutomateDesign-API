using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModels
{
 
    /// <summary>
    /// ViewModel pour la page ImportViewModel
    /// </summary>
    public class ImportViewModel : BaseViewModel
    {
        private readonly Action _openAutomate;
        private readonly Action _connect;
        private string errorMessage;

        /// <summary>
        /// Commande pour ouvrir un automate
        /// </summary>
        public ICommand OpenAutomateCommand { get; }

        /// <summary>
        /// Commande pour la connexion
        /// </summary>
        public ICommand ConnectCommand { get; }

        /// <summary>
        /// Message d'erreur à afficher
        /// </summary>
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        /// <param name="OpenAutomate"> Action correspondante à l'ouverture d'un nouvel automate. </param>
        public ImportViewModel(Action openAutomate,Action connect)
        {
            _openAutomate = openAutomate;
            _connect = connect;
            OpenAutomateCommand = new RelayCommand(() => _openAutomate());
            ConnectCommand = new RelayCommand(() => _connect());
        }
    }
}
