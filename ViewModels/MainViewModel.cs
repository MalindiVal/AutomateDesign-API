using ClientData.Interfaces;
using ClientData.Realisations;
using LogicLayer;
using Service.Implementaions;
using Service.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ViewModels
{
    /// <summary>
    /// ViewModel principal de l'application.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private object? _currentViewModel;
        private AutomateEditorViewModel? currenteditor;
        private int _counter = 1;
        private readonly Func<AutomateEditorViewModel> editorFactory;
        private readonly IFileDialogService fileDialogService;
        private readonly IExportOptionsService exportOptionService;
        private readonly ICanvasExportService canvasExportService;
        private readonly IAutomateService automateService;
        private readonly IUtilisateurService userService;

        private readonly ICommand login;
        private readonly ICommand logout;
        private readonly ICommand navigateHome;
        private readonly ICommand openNewAutomateCommand;
        private readonly ICommand activateEditorCommand;
        private readonly ICommand closeEditorCommand;
        private readonly ICommand navigateAutomates;
        private readonly ICommand importAutomateCommand;
        private bool connected = false;
        private bool notconnected = true;
        private string loginmessage;
        private Utilisateur? curentuser;

        /// <summary>
        /// ViewModel actuellement affiché.
        /// </summary>
        public object? CurrentViewModel
        {
            get => _currentViewModel;
            private set { _currentViewModel = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Liste des éditeurs d’automates ouverts.
        /// </summary>
        public ObservableCollection<AutomateEditorViewModel> OpenEditors { get; } = new();

        /// <summary>
        /// Commande pour la déconnexion
        /// </summary>
        public ICommand Logout
        {
            get
            {
                return logout;
            }
        }

        /// <summary>
        /// Commande pour la connexion
        /// </summary>
        public ICommand Login
        {
            get
            {
                return login;
            }
        }
        /// <summary>
        /// Commande pour naviguer vers la page d'accueil.
        /// </summary>
        public ICommand NavigateHome {
            get
            {
                return navigateHome;
            }
        }
        /// <summary>
        /// Commande pour ouvrir un nouvel automate.
        /// </summary>
        public ICommand OpenNewAutomateCommand
        {
            get
            {
                return openNewAutomateCommand;
            }
        }

        /// <summary>
        /// Commande pour activer un éditeur d'automate existant.
        /// </summary>
        public ICommand ActivateEditorCommand
        {
            get
            {
                return activateEditorCommand;
            }
        }

        /// <summary>
        /// Commande pour fermer un éditeur d'automate.
        /// </summary>
        public ICommand CloseEditorCommand
        {
            get
            {
                return closeEditorCommand;
            }
        }

        /// <summary>
        /// Commande pour naviguer dans 
        /// </summary>
        public ICommand NavigateAutomates
        {
            get
            {
                return navigateAutomates;
            }
        }

        /// <summary>
        /// Commande pour ouvrir la page importation
        /// </summary>
        public ICommand ImportAutomateCommand
        {
            get
            {
                return importAutomateCommand;
            }
        }
        /// <summary>
        /// Si un utilisateur est connecté
        /// </summary>
        public bool Connected 
        { 
            get => connected;
            set {
                connected = value;
                NotConnected = !connected;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Message à afficher quand l'utilisateur est connecté
        /// </summary>
        public string LoginMessage
        {
            get
            {
                return loginmessage;
            }
            set
            {
                loginmessage = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Si l'utilisateur n'est pas connectée
        /// </summary>
        public bool NotConnected {
            get {
                bool res = true;
                if(Connected) res = false;
                return res;
            }
            set
            {
                notconnected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public MainViewModel(IFileDialogService service, IExportOptionsService exportOptionService, ICanvasExportService canvasExportService, Func<AutomateEditorViewModel> editorFactory,IAutomateService automateService, IUtilisateurService userService)
        {
            fileDialogService = service;
            this.exportOptionService = exportOptionService;
            this.canvasExportService = canvasExportService;
            this.curentuser = null;

            navigateHome = new RelayCommand(ShowHome);
            openNewAutomateCommand = new RelayCommand(OpenNewAutomate);
            activateEditorCommand = new RelayCommand<AutomateEditorViewModel?>((vm) => OpenCurrentEditor(vm));
            closeEditorCommand = new RelayCommand<AutomateEditorViewModel?>(CloseEditor);
            navigateAutomates = new RelayCommand(() => ShowConnect(ShowHome));
            importAutomateCommand = new RelayCommand(ShowImportPage);
            logout = new RelayCommand(Disconnect);
            login = new RelayCommand(() => ShowConnect(ShowHome));

            ShowHome();
            this.exportOptionService = exportOptionService;
            this.canvasExportService = canvasExportService;
            this.editorFactory = editorFactory;
            this.automateService = automateService;
            this.userService = userService;
        }

        private AutomateService CreateAutomateService()
        {
            return new AutomateService(new AutomateDAO(), new JsonAutomateSerializer());
        }

        /// <summary>
        /// Affiche la page d'accueil.
        /// </summary>
        private void ShowHome()
        {
            CurrentViewModel = new HomeViewModel(OpenNewAutomate, ShowImportPage);
        }

        private void ShowImportPage()
        {
            CurrentViewModel = new ImportViewModel(OpenFichier,() => ShowConnect(ShowImportPage));
        }



        /// <summary>
        /// Créé un nouvel éditeur et l'active.
        /// </summary>
        private void OpenNewAutomate()
        {
            string title = $"Automate {_counter++}";

            // Service unique pour ce nouvel automate
            AutomateService service = CreateAutomateService();

            // On crée un automate vierge
            Automate automate = new Automate { Nom = title , Utilisateur = this.curentuser };

            // On demande un éditeur via DI
            AutomateEditorViewModel editor = editorFactory();
            currenteditor = editor;

            editor.LoadAutomate(automate);

            // On ouvre l’éditeur
            OpenEditors.Add(editor);
            CurrentViewModel = editor;
        }

        private void OpenImportAutomateWindow()
        {
            AutomateService service = CreateAutomateService();

            CurrentViewModel = new ImportAutomateViewModel(
                service,
                openEditor: (automate) =>
                {
                    // On demande un éditeur via DI
                    AutomateEditorViewModel editor = editorFactory();

                    // On injecte le service utilisé pour le chargement
                    automate.Utilisateur = this.curentuser;
                    editor.LoadAutomate(automate);

                    // On l'ajoute dans les onglets
                    OpenEditors.Add(editor);
                    CurrentViewModel = editor;
                },
                cancel: ShowHome,
                this.curentuser
            );
        }

        private void ShowConnect(Action BackAction)
        {
            if (this.curentuser != null)
            {
                OpenImportAutomateWindow();
            }
            else
            {
                CurrentViewModel = new ConnexionViewModel(async () => await Connect(OpenImportAutomateWindow), BackAction);
            }
        }

        private async Task Connect(Action succes)
        {
            if (CurrentViewModel is ConnexionViewModel vm)
            {
                try
                {
                    Utilisateur user = new Utilisateur
                    {
                        Login = vm.Identifiant,
                        Mdp = vm.Password
                    };

                    user = await this.userService.Login(user);

                    vm.ClearForm();

                    if (user?.Id != null)
                    {
                        this.curentuser = user;
                        this.Connected = true;
                        this.LoginMessage = "Bienvenue, " + this.curentuser.Login;
                        foreach(AutomateEditorViewModel editor in this.OpenEditors)
                        {
                            editor.Metier.Utilisateur = this.curentuser;
                        }
                        succes();
                    }
                    else
                    {
                        vm.ErrorMessage = "Login ou Mot de passe incorrect";
                    }
                }
                catch (Exception ex)
                {
                    vm.ErrorMessage = "Une erreur inattendue s'est produit : " + ex.Message;
                }
                
                
            }
        }

        private void Disconnect()
        {
            this.curentuser = null;
            foreach (AutomateEditorViewModel editor in this.OpenEditors)
            {
                editor.Metier.Utilisateur = this.curentuser;
            }
            this.Connected = false;
            ShowHome();
        }



        private void OpenFichier()
        {
            if (CurrentViewModel is ImportViewModel vm)
            {
                string? filePath = fileDialogService.OpenFile("JSON Files (*.json)|*.json|All Files (*.*)|*.*","json");

                if (!string.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        AutomateService service = CreateAutomateService();
                        Automate automate = service.ChargementAutomate(filePath);

                        // On demande un nouvel éditeur à la factory (DI)
                        AutomateEditorViewModel editor = editorFactory();

                        currenteditor = editor;

                        // On lui injecte l'automate chargé
                        automate.Utilisateur = this.curentuser;
                        editor.LoadAutomate(automate);

                        // On l’ouvre
                        OpenEditors.Add(editor);
                        CurrentViewModel = editor;
                    }
                    catch (Exception ex)
                    {
                        vm.ErrorMessage = "Le fichier choisi a un format inadapté : " + ex.Message;
                    }
                }
            }    
        }

        public async Task ExporterAutomateReseau(string nom)
        {
            if (CurrentViewModel is AutomateEditorViewModel viewModel)
            {
                try
                {
                    if (this.curentuser != null)
                    {
                        viewModel.ConstruireAutomateDepuisVM();

                        if (nom != null)
                            viewModel.Metier.Nom = nom;

                        AutomateService service = CreateAutomateService();
                        viewModel.Metier = await service.AddAutomate(viewModel.Metier);
                        viewModel.NotificationMessage = "L'automate a été enregistré en réseau sous le nom : " + nom;
                        viewModel.IsError = false;
                    }
                    else
                    {
                        CurrentViewModel = new ConnexionViewModel(async () => await Connect(() => OpenCurrentEditor(viewModel)), () => OpenCurrentEditor(viewModel));
                    }
                    
                }
                catch (Exception ex)
                {
                    viewModel.NotificationMessage = "Échec de l'enregistrement : " + ex.Message;
                    viewModel.IsError = true;
                }
                finally
                {
                    viewModel.NotificationVisible = true;
                }
                
                
            }
        }

        private void OpenCurrentEditor(AutomateEditorViewModel vm)
        {
            if (vm != null)CurrentViewModel = vm;
            currenteditor = vm;
        }



        /// <summary>
        /// Ferme un éditeur donné.
        /// </summary>
        /// <param name="vm"></param>
        private void CloseEditor(AutomateEditorViewModel? vm)
        {
            if (vm is not null)
            {
                int idx = OpenEditors.IndexOf(vm);
                if (idx >= 0)
                {
                    bool wasActive = ReferenceEquals(CurrentViewModel, vm);
                    OpenEditors.RemoveAt(idx);

                    // Si l’éditeur fermé était actif, bascule vers un autre onglet
                    if (wasActive)
                    {
                        if (OpenEditors.Count > 0)
                        {
                            // Active l’onglet précédent si possible, sinon le premier
                            int next = Math.Clamp(idx - 1, 0, OpenEditors.Count - 1);
                            CurrentViewModel = OpenEditors[next];
                        }
                        else
                        {
                            ShowHome();
                        }
                    }
                }
            }
        }
    }
}