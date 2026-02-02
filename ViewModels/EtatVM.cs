using LogicLayer;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ViewModels
{
    /// <summary>
    /// ViewModel représentant un état dans un automate.
    /// <para>
    /// Fournit une couche intermédiaire entre la logique métier (<see cref="Etat"/>) 
    /// et l'affichage (UI), notamment pour la position, les transitions et la taille des états.
    /// </para>
    /// </summary>
    public class EtatVM : BaseViewModel
    {
        #region Constants
        private const double ArrowStartXOffset = -30.0;
        private const double ArrowEndXOffset = -10.0;
        private const double ArrowYDelta = 5.0;
        private const double DeuxiemeCercleRayon = 8.0;
        private bool _isHighlighted;
        private bool isDragged;
        #endregion

        #region Attributes
        private Etat etat;
        #endregion

        #region Properties
        /// <summary>
        /// Logic for the ViewModel
        /// </summary>
        public Etat Metier => etat;

        /// <summary>
        /// Property ID
        /// </summary>
        public int Id { get => etat.Id; set { if (etat.Id != value) { etat.Id = value; OnPropertyChanged(); } } }

        /// <summary>
        /// Property Name
        /// </summary>
        public string Nom { get => etat.Nom; set { if (etat.Nom != value) { etat.Nom = value; OnPropertyChanged(); } } }

        /// <summary>
        /// Property IsFinal
        /// </summary>
        public bool EstFinal { get => etat.EstFinal; set { if (etat.EstFinal != value) { etat.EstFinal = value; OnPropertyChanged(); } } }

        /// <summary>
        /// Property IsInitial
        /// </summary>
        public bool EstInitial { get => etat.EstInitial; set { if (etat.EstInitial != value) { etat.EstInitial = value; OnPropertyChanged(); } } }

        /// <summary>
        /// Property X
        /// </summary>
        public double X { 
            get => etat.Position.X;
            set { 
                if (etat.Position.X != value) 
                {
                    etat.Position.X = value; 
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(XOffset));
                }
            }
        }

        /// <summary>
        /// Property Y
        /// </summary>
        public double Y {
            get => etat.Position.Y; 
            set { 
                if (etat.Position.Y != value) 
                { 
                    etat.Position.Y = value; 
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(YOffset));
                } 
            }
        }

        /// <summary>
        /// State's Radius
        /// </summary>
        public double EtatRadius { get; set; }

        /// <summary>
        /// Final State's Radius 
        /// </summary>
        public double EtatFinalRadius => EtatRadius + DeuxiemeCercleRayon;

        /// <summary>
        /// Collection of the transition coming in  
        /// </summary>
        public ObservableCollection<TransitionVM> TransitionsOut { get; }

        /// <summary>
        /// Collection of the transition coming out the state
        /// </summary>
        public ObservableCollection<TransitionVM> TransitionsIn { get; }
        #endregion

        
        
        /// <summary>
        /// Si un état est selectionnée pour la création d'une transition
        /// </summary>
        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Si un etat est attrapée par la souris
        /// </summary>
        public bool IsDragged
        {
            get => isDragged;
            set
            {
                if (isDragged != value)
                {
                    isDragged = value;
                    OnPropertyChanged();
                }
            }
        }

        #region Binding
        /// <summary>
        /// Offset on the X axis
        /// </summary>
        public double XOffset => X - EtatRadius / 2.0;

        /// <summary>
        /// OffSet on the Y axis
        /// </summary>
        public double YOffset => Y - EtatRadius / 2.0;

        /// <summary>
        /// X coordinates for the tip/end of the arrow
        /// </summary>
        public double ArrowEndX => 0.0;

        /// <summary>
        /// Y coordinates for the tip/end of the arrow
        /// </summary>
        public double ArrowEndY => EtatRadius / 2.0;

        /// <summary>
        /// X coordinates for the start of the arrow
        /// </summary>
        public double ArrowStartX => ArrowStartXOffset;

        /// <summary>
        /// y coordinates for the start of the arrow
        /// </summary>
        public double ArrowStartY => EtatRadius / 2.0;

        /// <summary>
        /// 
        /// </summary>
        public double ArrowEndXMinus10 => ArrowEndXOffset;

        /// <summary>
        /// 
        /// </summary>
        public double ArrowEndYMinus5 => EtatRadius / 2.0 - ArrowYDelta;

        /// <summary>
        /// 
        /// </summary>
        public double ArrowEndYPlus5 => EtatRadius / 2.0 + ArrowYDelta;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="etat">state</param>
        public EtatVM(Etat etat)
        {
            this.etat = etat;
            TransitionsOut = new ObservableCollection<TransitionVM>();
            TransitionsIn = new ObservableCollection<TransitionVM>();
            TransitionsOut.CollectionChanged += OnTransitionsChanged;
            TransitionsIn.CollectionChanged += OnTransitionsChanged;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifie qu'il y ait pas de chevauchement sur un EtatVM
        /// </summary>
        /// <param name="x">position X</param>
        /// <param name="y">position Y</param>
        /// <returns></returns>
        public bool CheckOverlap(double x, double y)
        {
            bool res = false;
            double radius = EtatRadius / 2.0;
            if (this.EstFinal)
            {
                radius = this.EtatFinalRadius / 2.0;
            } 
            double dx = x - this.X;
            double dy = y - this.Y;
            double distanceSquared = dx * dx + dy * dy;
            double otherRadius = this.EstFinal ? this.EtatFinalRadius / 2.0 : this.EtatRadius / 2.0;

            if (!this.IsDragged)
            {
                if (distanceSquared < Math.Pow(radius + otherRadius, 2))
                    res = true;
            }  

            return res;
        }

        /// <summary>
        /// Sert a activer la réindexation lors d'un changement dans la liste
        /// </summary>
        /// <param name="sender">objet qui a déclencher l'évenement</param>
        /// <param name="e">Les arguments de l'événement contenant le nom de la propriété modifiée.</param>
        private void OnTransitionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TransitionVM t in e.OldItems)
                {                    
                    t.Dispose();
                }
            }
            if (e.NewItems != null)
            {
                foreach (TransitionVM t in e.NewItems)
                {                   
                    if (t.EtatDepart != this && t.EtatArrivee != this)
                        continue;
                }
            }           
            ReindexerTransitionsMultiples();
        }


        
            public TransitionVM CreerTransitionVers(EtatVM arrivee)
        {
            // 1) index temporaire = nombre d'existantes dans le même sens
            int index = this.TransitionsOut.Count(t => t.EtatArrivee == arrivee);
            int total = index + 1;

            // 2) instanciation de la VM (elle créera son Transition interne)
            var vm = new TransitionVM(this, arrivee);

            // 3) calculer le point de contrôle initial via une TransitionGeometry temporaire
            //    (on ne dépend pas d'une propriété 'Geometry' sur Transition qui n'existe pas)
            var tempTransition = new LogicLayer.Transition(this.Metier, arrivee.Metier);
            var tempGeom = new LogicLayer.TransitionGeometry(tempTransition);
            var bez = tempGeom.GetBezierPoints(index, total, arrivee.EtatRadius);

            // 4) appliquer ce point de contrôle manuellement sur l'objet métier de la VM
            vm.Metier.ManualControlX = bez.ControlX;
            vm.Metier.ManualControlY = bez.ControlY;

            // 5) ajouter proprement dans les collections
            this.TransitionsOut.Add(vm);
            arrivee.TransitionsIn.Add(vm);

            // 6) forcer le rafraîchissement visuel
            vm.RefreshGeometry();

            return vm;
        
        }


        /// <summary>
        /// Sert a réindexer en fonction de leur position dans la liste des transition lors de la suppression de l'une d'en entre elles
        /// </summary>
        public void ReindexerTransitionsMultiples()
        {
            List<IGrouping<EtatVM,TransitionVM>> groupes = TransitionsOut
                .GroupBy(t => t.EtatArrivee)
                .ToList();
            foreach (IGrouping<EtatVM,TransitionVM> groupe in groupes)
            {
                foreach (TransitionVM transition in groupe)
                {                   
                    transition.RefreshGeometry();
                }
            }          
            List<IGrouping<EtatVM,TransitionVM>> groupesIn = TransitionsIn
                .GroupBy(t => t.EtatDepart)
                .ToList();
            foreach (IGrouping < EtatVM, TransitionVM > groupe in groupesIn)
            {
                foreach (TransitionVM transition in groupe)
                {
                    transition.RefreshGeometry();
                }
            }
        }
        #endregion
    }
}
