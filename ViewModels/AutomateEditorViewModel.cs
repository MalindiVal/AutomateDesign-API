using System.Collections.ObjectModel;
using System.Windows.Input;
using LogicLayer;
using Service.Interfaces;

namespace ViewModels
{
    /// <summary>
    /// ViewModel pour l’éditeur d’automates.
    /// Gère les états, transitions, outils, canvas et export.
    /// </summary>
    public class AutomateEditorViewModel : BaseViewModel
    {
        #region Associations
        private IAutomateService service;
        private IFileDialogService fileDialogService;
        private IExportOptionsService exportOptionsService;
        private ICanvasExportService canvasExportService;
        private ICSharpService cSharpService;

        private TypeEtat currentTool = TypeEtat.None;

        private Automate metier = new Automate();
        private EtatVM? transitionStartEtat;
        private EtatVM? draggedEtat;
        private bool isDraggingEtat = false;
        private bool isDraggingControlPoint = false;
        private TransitionVM? draggedTransition = null;
        #endregion

        #region Attributes
        private double etatRadius = 75;       
        private double canvasWidth;
        private double canvasHeight;
        private string notification;
        private bool notificationVisible;
        private bool isError;
        private double initialX;
        private double initialY;
        private ICommand toolInitialState;
        private ICommand toolArrow;
        private ICommand toolState;
        private ICommand toolFinalState;
        private ICommand saveCommand;
        private readonly ObservableCollection<EtatVM> etats = new();
        private readonly ObservableCollection<TransitionVM> transitions = new();
        #endregion


        #region Properties
        #region Commands
        /// <summary>
        /// La commande pour selectionner la création d'un etat initial
        /// </summary>
        public ICommand ToolInitialState { get => toolInitialState; set => toolInitialState = value; }

        /// <summary>
        /// La commande pour sélection du créateur d'une transition
        /// </summary>
        public ICommand ToolArrow { get => toolArrow; set => toolArrow = value; }

        /// <summary>
        /// La commande pour la sélecion de l'outil de création d'un état
        /// </summary>
        public ICommand ToolState { get => toolState; set => toolState = value; }

        /// <summary>
        /// La commande pour la sélecion de l'outil de création d'un état FINAL
        /// </summary>
        public ICommand ToolFinalState { get => toolFinalState; set => toolFinalState = value; }

        /// <summary>
        /// La commande pour la sauvegarde de l'automade
        /// </summary>
        public ICommand SaveCommand { get => saveCommand; set => saveCommand = value; }

        #endregion


        /// <summary>
        /// Liste des etats contenus dans le canvas
        /// </summary>
        public ObservableCollection<EtatVM> Etats => etats;

        /// <summary>
        /// Liste des transitions
        /// </summary>
        public ObservableCollection<TransitionVM> Transitions => transitions;
        
        /// <summary>
        /// L'outil actuellement utilisé
        /// </summary>
        public TypeEtat CurrentTool
        {
            get => currentTool;
            set { currentTool = value; OnPropertyChanged(); }
        }

        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Le titre de l'éditeur
        /// </summary>
        public string Title 
        {
            get
            {
                return this.metier.Nom;
            }
            set
            {
                this.metier.Nom = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Diametre de l'état
        /// </summary>
        public double EtatRadius
        {
            get {
                return etatRadius;
            }
            set { 
                etatRadius = value; 
                OnPropertyChanged(); 
            }
        }

        /// <summary>
        /// La longuer du canvas
        /// </summary>
        public double CanvasWidth
        {
            get
            {
                return canvasWidth;
            }
            set { canvasWidth = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// La hauteur du Canvas
        /// </summary>
        public double CanvasHeight
        {
            get => canvasHeight;
            set { canvasHeight = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// L'objet metier de  l'editeur
        /// </summary>
        public Automate Metier { get => metier; set => metier = value; }
        
        /// <summary>
        /// Phrase de notification lors d'une action
        /// </summary>
        public string NotificationMessage { 
            get => notification;
            set {
                notification = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Si la notification est visible
        /// </summary>
        public bool NotificationVisible
        {
            get => notificationVisible;
            set
            {
                notificationVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Si la notification est une erreur
        /// </summary>
        public bool IsError
        {
            get => isError;
            set
            {
                isError = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indique si un état est en cours de déplacement.
        /// </summary>
        public bool IsDraggingEtat
        {
            get => isDraggingEtat;
            set
            {
                isDraggingEtat = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indique si un point de contrôle de transition est en cours de déplacement.
        /// </summary>
        public bool IsDraggingControlPoint
        {
            get => isDraggingControlPoint;
            set
            {
                isDraggingControlPoint = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// La transition dont le point de contrôle est en cours de déplacement.
        /// </summary>
        public TransitionVM? DraggedTransition
        {
            get => draggedTransition;
            set
            {
                draggedTransition = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructeur de la vue modèle de l'éditeur d'automate
        /// </summary>
        /// <param name="service">Service dédié à la gestion de l'automate.</param>
        /// <param name="fileDialogService">Systeme de dialog de fihicer.</param>
        /// <param name="exportOptionsService">Service dédié aux options d'exports de l'automate.</param>
        /// <param name="canvasExportService">Service dédié à l'export du canvas.</param>
        public AutomateEditorViewModel(IAutomateService service, IFileDialogService fileDialogService, IExportOptionsService exportOptionsService, ICanvasExportService? canvasExportService, ICSharpService cSharpService)
        {
            this.service = service;
            this.fileDialogService = fileDialogService;
            this.exportOptionsService = exportOptionsService;
            this.canvasExportService = canvasExportService;
            this.cSharpService = cSharpService;

            this.metier = new Automate();

            InitializeCommands();
        }

        /// <summary>
        /// Initialisation des commandes
        /// </summary>
        private void InitializeCommands()
        {
            ToolInitialState = new RelayCommand(() => SwitchTools(TypeEtat.Initial));
            ToolArrow = new RelayCommand(() => SwitchTools(TypeEtat.Transition));
            ToolState = new RelayCommand(() => SwitchTools(TypeEtat.Normal));
            ToolFinalState = new RelayCommand(() => SwitchTools(TypeEtat.Final));
            SaveCommand = new RelayCommand(SauvegardeAutomate);
        }
        #endregion

        #region Methods

        #region Gestion des interactions
        /// <summary>
        /// Gère un clic sur le canvas et applique l’action correspondant à l’outil actif.
        /// </summary>
        /// <param name="x">Coordonnée X du clic</param>
        /// <param name="y">Coordonnée Y du clic</param>
        public void HandleCanvasClick(double x, double y)
        {
            switch (CurrentTool)
            {
                case TypeEtat.Initial:
                case TypeEtat.Final:
                case TypeEtat.Normal:
                    AjouterEtat(x, y);
                    break;

            }
        }

        /// <summary>
        /// Changement de l'outil actuel
        /// </summary>
        /// <param name="t">Le nouvelle outil</param>
        public void SwitchTools(TypeEtat t)
        {
            this.CancelTransition();
            this.CurrentTool = t;
        }

        /// <summary>
        /// Permet d'annuler la création d'une transition
        /// </summary>
        public void CancelTransition()
        {
            if (this.CurrentTool == TypeEtat.Transition)
            {
                if (transitionStartEtat != null)
                {
                    transitionStartEtat.IsHighlighted = false;
                    transitionStartEtat = null;
                }
            }
        }

        /// <summary>
        /// Permet de lier deux Etats
        /// </summary>
        /// <param name="evm">L'état à lier à une transition</param>
        public void LinkEtats(EtatVM evm)
        {
            if (this.CurrentTool == TypeEtat.Transition)
            {
                if (transitionStartEtat == null)
                {
                    transitionStartEtat = evm;
                    evm.IsHighlighted = true;
                }
                else
                {
                    this.AjouterTransition(transitionStartEtat, evm);
                    this.CancelTransition();
                }
            }
        }

        /// <summary>
        /// Maitient du click sur un etat
        /// </summary>
        /// <param name="evm">EtatVM concerné</param>
        public void HoldEtat(EtatVM evm)
        {
            if (CurrentTool != TypeEtat.Transition)
            {
                draggedEtat = evm;
                initialX = evm.X;
                initialY = evm.Y;
                evm.IsDragged = true;
            }
        }

        /// <summary>
        /// Lorsque le click est relaché
        /// </summary>
        /// <param name="x">position x du laché</param>
        /// <param name="y">position y du laché</param>
        public void RealeaseEtat(double x, double y)
        {
            if (CurrentTool != TypeEtat.Transition)
            {

                if (CheckOverlap(x, y))
                {
                    draggedEtat.X = initialX;
                    draggedEtat.Y = initialY;
                }
                else
                {
                    draggedEtat.X = x;
                    draggedEtat.Y = y;
                }

                draggedEtat.IsDragged = false;
                draggedEtat = null;
                initialX = 0;
                initialY = 0;

            }
        }

        /// <summary>
        /// Permet de bouger l'etat en fonction de la position de la souris
        /// </summary>
        /// <param name="x">position x de la souris</param>
        /// <param name="y">position y de la souris</param>
        public void MoveEtat(double x, double y)
        {
            if (draggedEtat != null)
            {
                draggedEtat.X = x;
                draggedEtat.Y = y;
            }
        }

        /// <summary>
        /// Démarre le déplacement d'un point de contrôle.
        /// </summary>
        /// <param name="transition">Transition à déplacer.</param>
        public void StartDraggingControlPoint(TransitionVM transition)
        {
            DraggedTransition = transition;
            IsDraggingControlPoint = true;
        }

        /// <summary>
        /// Arrête le déplacement d'un point de contrôle de transition.
        /// </summary>
        public void StopDraggingControlPoint()
        {
            IsDraggingControlPoint = false;
            DraggedTransition = null;
        }

        /// <summary>
        /// Déplace le point de contrôle de la transition en cours vers (x, y).
        /// </summary>
        /// <param name="x">Coordonnée X.</param>
        /// <param name="y">Coordonnée Y.</param>
        public void MoveControlPoint(double x, double y)
        {
            if (IsDraggingControlPoint && DraggedTransition != null)
            {
                DraggedTransition.TryMoveControlPoint(x, y, CanvasWidth, CanvasHeight);
            }
        }

        /// <summary>
        /// Démarre le déplacement d'un état.
        /// </summary>
        public void StartDraggingEtat()
        {
            IsDraggingEtat = true;
        }

        /// <summary>
        /// Arrête le déplacement d'un état.
        /// </summary>
        public void StopDraggingEtat()
        {
            IsDraggingEtat = false;
        }

        #endregion

        #region Gestion Etats

        /// <summary>
        /// Ajoute un état à l’automate.
        /// </summary>
        /// <param name="x">Coordonnée X du centre de l’état</param>
        /// <param name="y">Coordonnée Y du centre de l’état</param>
        public void AjouterEtat(double x, double y)
        {
            if (!(CurrentTool == TypeEtat.None || CheckOverlap(x, y)))
            {
                bool estInitial = CurrentTool == TypeEtat.Initial;
                bool estFinal = CurrentTool == TypeEtat.Final;
                if (estInitial)
                {
                    EtatVM? ancienInitial = Etats.FirstOrDefault(e => e.EstInitial);
                    if (ancienInitial != null)
                        ancienInitial.EstInitial = false;
                }               
                HashSet<int> indicesUtilises = new HashSet<int>();
                foreach (EtatVM e in Etats)
                {
                    if (e.Nom.StartsWith("Etat "))
                    {
                        string nombreTexte = e.Nom.Substring("Etat ".Length);
                        if (int.TryParse(nombreTexte, out int n))
                            indicesUtilises.Add(n);
                    }
                }
                int indexLibre = 0;
                while (indicesUtilises.Contains(indexLibre))
                    indexLibre++;               
                Etat etat = new Etat
                {
                    Nom = $"Etat {indexLibre}",
                    Position = new Position(x, y),
                    EstInitial = estInitial,
                    EstFinal = estFinal
                };
                Etats.Add(new EtatVM(etat) { EtatRadius = EtatRadius });

            }
        }

        /// <summary>
        /// Vérifie si une position chevauche un autre état ou sort du canvas.
        /// </summary>
        /// <param name="x">Coordonnée X</param>
        /// <param name="y">Coordonnée Y</param>
        /// <returns>True si chevauchement ou hors limites, false sinon</returns>
        public bool CheckOverlap(double x, double y)
        {
            bool res = false;
            double radius = EtatRadius / 2.0;

            if (CurrentTool == TypeEtat.Final)
            {
                EtatVM tempEtat = new EtatVM(new Etat()) { EtatRadius = EtatRadius };
                radius = tempEtat.EtatFinalRadius / 2.0;
            }

            bool outOfBounds = x - radius < 0 || y - radius < 0 || x + radius > CanvasWidth || y + radius > CanvasHeight;
            if (outOfBounds) {
                res = true;
            } else
            {
                foreach (EtatVM evm in Etats)
                {
                    res = evm.CheckOverlap(x, y);
                    if (res)
                    {
                        break;
                    }
                }
            }   

            return res;
        }

        /// <summary>
        /// Supprime un état et toutes ses transitions associées.
        /// </summary>
        /// <param name="evm">Etat à supprimer</param>
        public void SupprimerEtat(EtatVM evm)
        {
            List<TransitionVM> transitionsASupprimer = new List<TransitionVM>();
            transitionsASupprimer = Transitions.Where(t => t.EtatDepart == evm || t.EtatArrivee == evm).ToList();


            foreach (TransitionVM t in transitionsASupprimer)
                Transitions.Remove(t);

            Etats.Remove(evm);
        }


        #endregion

        #region Gestion des Transitions

        /// <summary>
        /// Ajoute une transition entre deux états.
        /// </summary>
        /// <param name="start">Etat de départ</param>
        /// <param name="end">Etat d’arrivée</param>
        public void AjouterTransition(EtatVM start, EtatVM end)
        {
            TransitionVM nouvelleTransition = new TransitionVM(start, end);

            // Calculer index et total AVANT d'ajouter
            int futureIndex = start.TransitionsOut.Count(t => t.EtatArrivee == end);
            int futureTotal = futureIndex + 1; 

            if (nouvelleTransition.TransitionInsideCanvas(nouvelleTransition.IsLoop,futureIndex,futureTotal,CanvasWidth,CanvasHeight))
            {
                // Ajout aux listes
                start.TransitionsOut.Add(nouvelleTransition);
                end.TransitionsIn.Add(nouvelleTransition);

                // previousTransition
                TransitionVM? previous = start.TransitionsOut
                    .Where(t => t.EtatArrivee == end && t != nouvelleTransition)
                    .LastOrDefault();
                nouvelleTransition.PreviousTransition = previous;

                // Génération condition
                HashSet<int> indicesUtilises = new HashSet<int>();
                foreach (TransitionVM t in Transitions)
                {
                    if (t.Condition.StartsWith("Condition "))
                    {
                        string nombreTexte = t.Condition.Substring("Condition ".Length);
                        if (int.TryParse(nombreTexte, out int n))
                            indicesUtilises.Add(n);
                    }
                }

                int indexLibre = 0;
                while (indicesUtilises.Contains(indexLibre)) indexLibre++;
                nouvelleTransition.Condition = "Condition " + indexLibre;

                // Ajout final
                Transitions.Add(nouvelleTransition);
                nouvelleTransition.RefreshGeometry();
            }
        }

        /// <summary>
        /// Supprime une transition.
        /// </summary>
        /// <param name="tvm">Transition à supprimer</param>
        public void SupprimerTransition(TransitionVM tvm)
        {
            tvm.EtatDepart.TransitionsOut.Remove(tvm);
            tvm.EtatArrivee.TransitionsIn.Remove(tvm);
            Transitions.Remove(tvm);
        }

        #endregion

        #region Persistance
        /// <summary>
        /// Construit un objet Automate à partir des ViewModels.
        /// </summary>
        public void ConstruireAutomateDepuisVM()
        {
            this.metier.Etats.Clear();
            foreach (EtatVM etat in this.Etats)
            {
                this.metier.Etats.Add(etat.Metier);
            }
            this.metier.Transitions.Clear();
            foreach (TransitionVM transition in this.Transitions)
            {
                this.metier.Transitions.Add(transition.Metier);
            }
        }

        /// <summary>
        /// Sauvegarde l'automate courant dans un fichier local.
        /// </summary>
        public void SauvegardeAutomate()
        {
            string notificationMessage = string.Empty;
            try
            {
                ConstruireAutomateDepuisVM();
                string? filename = fileDialogService.SaveFile("JSON Files (*.json)|*.json|All Files (*.*)|*.*", "json", this.metier.Nom.Replace(" ", "_"));
                if (filename != null)
                {
                    this.service.SauvegardeAutomate(metier, filename);
                    notificationMessage = "Sauvegarde Locale réussi sur " + filename;
                    ShowNotification(notificationMessage, false);
                }
            }
            catch (Exception ex)
            {
                notificationMessage = "Échec de la sauvegarde locale : " + ex.Message;
                ShowNotification(notificationMessage, true);
            }
        }

        /// <summary>
        /// Exporte l’automate courant vers un endpoint réseau avec un nom personnalisé.
        /// </summary>
        /// <returns>Task représentant l’opération asynchrone</returns>
        /// <exception cref="Exception">Si l’export échoue</exception>
        private async Task ExporterReseau()
        {
            string notificationMessage = string.Empty;
            
            
            if (this.metier.Id == null)
            {
                this.metier = await this.service.AddAutomate(this.metier);
                notificationMessage = "L'automate a été enregistré en réseau sous le nom : " + this.metier.Nom;
            }
            else
            {
                this.metier = await this.service.UpdateAutomate(this.metier);
                notificationMessage = "L'automate a été mise à jour sous le nom : " + this.metier.Nom;
            }
            RecupAutomate();
            if (!string.IsNullOrEmpty(notificationMessage))
            {
                ShowNotification(notificationMessage, false);
            }
        }

        private void RecupAutomate()
        {
            Title = this.metier.Nom;
            // D'abord créer tous les EtatVM
            Dictionary<Etat, EtatVM> mapEtats = new Dictionary<Etat, EtatVM>();
            Etats.Clear();

            foreach (Etat etat in this.metier.Etats)
            {
                EtatVM etatVM = new EtatVM(etat) { EtatRadius = EtatRadius };
                Etats.Add(etatVM);
                mapEtats[etat] = etatVM;
            }

            Transitions.Clear();
            // Ensuite créer les TransitionVM avec les bonnes références
            foreach (Transition t in this.metier.Transitions)
            {
                if (mapEtats.TryGetValue(t.EtatDebut, out EtatVM debut) &&
                    mapEtats.TryGetValue(t.EtatFinal, out EtatVM fin))
                {
                    TransitionVM transitionVM = new TransitionVM(debut, fin,t);
                    transitionVM.Condition = t.Condition;
                    Transitions.Add(transitionVM);
                    debut.TransitionsOut.Add(transitionVM);
                    fin.TransitionsIn.Add(transitionVM);
                }
            }
        }

        #endregion
        #endregion

        /// <summary>
        /// Gère un clic sur un état en fonction de l’outil actif.
        /// </summary>
        /// <param name="etat">Etat clické.</param>
        /// <param name="position">Position du click.</param>
        /// <param name="isTransitionTool">Booléen définissant si l'outil de transition est actif ou non.</param>
        public void OnEtatClicked(EtatVM etat, Point position, bool isTransitionTool)
        {
            if (isTransitionTool)
            {
                LinkEtats(etat);
            }
            else
            {
                HoldEtat(etat);
            }
        }

        /// <summary>
        /// Gère un clic sur le canvas principal.
        /// </summary>
        /// <param name="x">Valeur en X du click.</param>
        /// <param name="y">Valeur eh Y du click.</param>
        /// <param name="canvasWidth">Largeur du Canva.</param>
        /// <param name="canvasHeight">Hauteur du Canva.</param>
        public void OnCanvasClicked(double x, double y, double canvasWidth, double canvasHeight)
        {
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            HandleCanvasClick(x, y);
        }

        /// <summary>
        /// Affiche une notification avec un message et un statut d'erreur.
        /// </summary>
        /// <param name="message">Message à afficher.</param>
        /// <param name="isError">Booléen déterminant si le message est une erreur.</param>
        public void ShowNotification(string message, bool isError)
        {
            NotificationMessage = message;
            IsError = isError;
            NotificationVisible = true;
        }

        /// <summary>
        /// Exporte l’automate courant selon les options choisies par l’utilisateur.
        /// </summary>
        /// <param name="rootElement">objet de type grid utilisé pour l'export en image</param>
        /// <returns></returns>
        public async Task ExportAsync(object rootElement)
        {
            string notificationMessage = string.Empty;
            
            var (confirmed, mode, automateName) = exportOptionsService.ShowExportOptions(Title);

            if (confirmed && !string.IsNullOrWhiteSpace(automateName))
            {
               this.metier.Nom = automateName;
                ConstruireAutomateDepuisVM();
                try
                {
                    if (mode == ExportAction.Reseau)
                    {
                        await ExporterReseau();
                    }
                    else if (mode == ExportAction.Image)
                    {
                        ExportImage(rootElement);
                    } else if (mode == ExportAction.CSharp)
                    {
                        ExportCSharp();
                    }
                }
                catch (Exception ex)
                {
                    notificationMessage = "Échec de l'enregistrement : " + ex.Message;
                    ShowNotification(notificationMessage, true);
                }
                RecupAutomate();
            }
        }

        private void ExportImage(object rootElement)
        {
            string notificationMessage = string.Empty;

            string? filePath = fileDialogService.SaveFile(
                "PNG Files (*.png)|*.png|JPEG Files (*.jpg)|*.jpg|BMP Files (*.bmp)|*.bmp",
                "png",
                 this.metier.Nom);

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                canvasExportService.SaveAutomatonAsImage(rootElement, filePath);
                notificationMessage = "L'automate a été enregistré en image sur " + filePath;
                ShowNotification(notificationMessage, false);
            }
        }

        private void ExportCSharp()
        {
            string notificationMessage = string.Empty;
            string? folderPath = fileDialogService.OpenFolder();
            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                cSharpService.ExportAutomate(this.metier, folderPath);
                notificationMessage = "Les fichiers C# ont pu etre enregistré dans le dossier  " + folderPath;
                ShowNotification(notificationMessage, false);
            }
        }

        /// <summary>
        /// Charge un automate dans l’éditeur.
        /// </summary>
        /// <param name="automate">Automate à charger.</param>
        public void LoadAutomate(Automate? automate)
        {
            metier = automate ?? new Automate();

            RecupAutomate();
        }
    }
}
