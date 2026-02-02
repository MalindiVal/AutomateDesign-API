using ClientData.Interfaces;
using ClientData.Realisations;
using LogicLayer;
using Service;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModels
{
    /// <summary>
    /// ViewModel pour la page de connexion
    /// </summary>
    public class ConnexionViewModel : BaseViewModel
    {
        private string identifiant;
        private string password;
        private string errorMessage;
        private bool isLoading;

        /// <summary>
        /// Action pour la commande de connexion
        /// </summary>
        public ICommand ConnectCommand { get; }

        /// <summary>
        /// Action pour la commande de retour
        /// </summary>
        public ICommand ReturnCommand { get; }

        /// <summary>
        /// Identifiant de l'utilisateur 
        /// </summary>
        public string Identifiant { 
            get{
                return identifiant;
            }
            set{
                identifiant = value;
                OnPropertyChanged();
            } 
        }

        /// <summary>
        /// Mot de passe
        /// </summary>
        public string Password { 
            get{
                return password;
            } 
            set{
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                    (ConnectCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            } 
        }

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
        /// Indique si une opération de connexion est en cours
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    OnPropertyChanged();
                    (ConnectCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Connexion">Action à faire avec le bouton Connnexion</param>
        public ConnexionViewModel(Action Connexion, Action BackAction)
        {
            ConnectCommand = new RelayCommand(Connexion,CanConnect);
            ReturnCommand = new RelayCommand(BackAction);
        }

        /// <summary>
        /// Détermine si la commande de connexion peut être exécutée
        /// </summary>
        private bool CanConnect()
        {
            return !string.IsNullOrWhiteSpace(Identifiant)
                && !string.IsNullOrWhiteSpace(Password)
                && !IsLoading;
        }

        /// <summary>
        /// Réinitialise les champs du formulaire
        /// </summary>
        public void ClearForm()
        {
            Identifiant = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}
