using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LogicLayer;
using Service.Interfaces;

namespace ViewModels
{
    /// <summary>
    /// ViewModel pour l'importation et la gestion des automates.
    /// </summary>
    public class ImportAutomateViewModel : BaseViewModel
    {
        private readonly IAutomateService service;
        private Utilisateur user;
        private readonly Action<Automate> openEditor;
        private readonly Action cancel;
        private string errorMessage;

        private ICommand refreshCommand;
        private ICommand importCommand;
        private ICommand cancelCommand;

        /// <summary>
        /// Collection des automates disponibles.
        /// </summary>
        public ObservableCollection<Automate> Automates { get; } = new ObservableCollection<Automate>();

        private Automate? selectedAutomate;

        /// <summary>
        /// Automate sélectionné dans l'interface.
        /// </summary>
        public Automate? SelectedAutomate
        {
            get => selectedAutomate;
            set
            {
                selectedAutomate = value;
                OnPropertyChanged();
                (ImportCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Commande pour rafraîchir la liste des automates.
        /// </summary>
        public ICommand RefreshCommand 
        { 
            get => refreshCommand;
        }

        /// <summary>
        /// Commande pour importer l'automate sélectionné.
        /// </summary>
        public ICommand ImportCommand { 
            get => importCommand;
        }

        /// <summary>
        /// Commande pour annuler l'opération.
        /// </summary>
        public ICommand CancelCommand 
        { 
            get => cancelCommand;
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
        /// Constructeur du ViewModel.
        /// </summary>
        /// <param name="service">Service pour accéder aux automates.</param>
        /// <param name="openEditor">Action pour ouvrir l'éditeur d'automate.</param>
        /// <param name="cancel">Action pour annuler l'importation.</param>
        public ImportAutomateViewModel(IAutomateService service, Action<Automate> openEditor, Action cancel, Utilisateur user)
        {
            this.service = service;
            this.openEditor = openEditor;
            this.cancel = cancel;
            this.user = user;

            refreshCommand = new RelayCommand(async () => await LoadAutomates());
            importCommand = new RelayCommand(async () => await ImportSelectedAutomate(),
                () => SelectedAutomate != null && SelectedAutomate.Id.HasValue);
            cancelCommand = new RelayCommand(cancel);

            this.LoadAutomates();
        }

        /// <summary>
        /// Importe l'automate actuellement sélectionné et ouvre l'éditeur.
        /// </summary>
        /// <returns>Une tâche asynchrone représentant l'opération.</returns>
        /// <exception cref="InvalidOperationException">Si aucun automate n'est sélectionné ou si son ID est null.</exception>
        private async Task ImportSelectedAutomate()
        {
            try
            {
                if (SelectedAutomate != null)
                {
                    Automate fullAutomate = await service.ImporterAutomate(SelectedAutomate.Id.Value);

                    if (fullAutomate != null)
                    {
                        openEditor(fullAutomate);
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorMessage = "Une erreur innatendu s'est produit : " + ex.Message;
            }
        }

        /// <summary>
        /// Charge tous les automates depuis le service.
        /// </summary>
        /// <returns>Une tâche asynchrone représentant l'opération.</returns>
        private async Task LoadAutomates()
        {
            List<Automate> automatesList = await service.GetAllAutomatesByUser(this.user);

            Automates.Clear();
            foreach (Automate automate in automatesList)
            {
                Automates.Add(automate);
            }
        }
    }
}
