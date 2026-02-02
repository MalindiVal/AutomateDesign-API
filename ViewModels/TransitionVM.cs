using LogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using static LogicLayer.TransitionGeometry;

namespace ViewModels
{
    /// <summary>
    /// ViewModel représentant une transition entre deux états dans un automate. 
    /// Fournit une abstraction graphique et métier pour le rendu des transitions,
    /// y compris les boucles, le positionnement des flèches et la gestion des transitions multiples. 
    /// </summary>
    public class TransitionVM : BaseViewModel
    {
        #region Attributes
        private Transition transition;
        private TransitionGeometry geometry;
        private TransitionVM previousTransition;
        private EtatVM etatDepart;
        private EtatVM etatArrivee;
        private int transitionIndex;
        private double mouseControlX;
        private double mouseControlY;
        private bool isSelected;
        #endregion

        #region Properties
        /// <summary>
        /// Indique si la transition est sélectionnée.
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ShowControlPoint));
                }
            }
        }

        /// <summary>
        /// Affiche le point de contrôle lorsque la transition est sélectionnée.
        /// </summary>
        public bool ShowControlPoint => IsSelected;

        /// <summary>
        /// Coordonnée X du point de contrôle (manuelle ou calculée).
        /// </summary>
        public double ControlX
        {
            get => transition.ManualControlX
                ?? geometry.GetBezierPoints(GetIndex(), GetTotal(), EtatArrivee.EtatRadius).ControlX;
            set
            {
                transition.ManualControlX = value;
                OnPropertyChanged();
                RefreshGeometry();
            }
        }

        /// <summary>
        /// Coordonnée Y du point de contrôle (manuelle ou calculée).
        /// </summary>
        public double ControlY
        {
            get => transition.ManualControlY
                ?? geometry.GetBezierPoints(GetIndex(), GetTotal(), EtatArrivee.EtatRadius).ControlY;
            set
            {
                transition.ManualControlY = value;
                OnPropertyChanged();
                RefreshGeometry();
            }
        }

        /// <summary>
        /// Position X de la poignée d’édition du point de contrôle.
        /// </summary>
        public double HandleX
        {
            get
            {
                double result;
                if (transition.ManualControlX.HasValue)
                {
                    result = mouseControlX;
                }
                else 
                {
                    PointD p = geometry.GetCurvePoint(IsLoop, GetIndex(), GetTotal(), IsLoop ? EtatDepart.EtatRadius : EtatArrivee.EtatRadius);
                    result = p.X;
                }                    
                return result;
            }
            }


        /// <summary>
        /// Position Y de la poignée d’édition du point de contrôle.
        /// </summary>
        public double HandleY
        {
            get
            {
                double result;
                if (transition.ManualControlX.HasValue)
                {
                    result = mouseControlY;
                }
                else
                {
                    PointD p = geometry.GetCurvePoint(IsLoop, GetIndex(), GetTotal(), IsLoop ? EtatDepart.EtatRadius : EtatArrivee.EtatRadius);
                    result = p.Y;
                }
                return result;
            }
        }

        /// <summary>
        /// Met à jour la position du point de contrôle selon la souris.
        /// </summary>
        /// <param name="mouseX">Position X de la souris.</param>
        /// <param name="mouseY">Position Y de la souris.</param>
        public void UpdateControlForMouse(double mouseX, double mouseY)
        {
            mouseControlX = mouseX;
            mouseControlY = mouseY;
            double r = EtatArrivee.EtatRadius;
            geometry.UpdateControlForMouse(mouseX, mouseY, r);
            RefreshGeometry();
        }


        /// <summary>
        /// Accès direct à l'objet métier associé.
        /// </summary>
        public Transition Metier { get => transition; set => transition = value; }

        /// <summary>
        /// Référence à la transition précédente reliant les mêmes états.
        /// Permet le rendu décalé des transitions multiples.
        /// </summary>
        public TransitionVM PreviousTransition { get=> previousTransition; set => previousTransition = value; }

        /// <summary>
        /// État de départ de la transition.
        /// </summary>
        public EtatVM EtatDepart { get => etatDepart; set => etatDepart = value; }

        /// <summary>
        /// État d'arrivée de la transition.
        /// </summary>
        public EtatVM EtatArrivee { get => etatArrivee; set => etatArrivee = value; }
    
        /// <summary>
        /// Condition associée à la transition.
        /// </summary>
        public string Condition
        {
            get => transition.Condition;
            set
            {
                if (transition.Condition != value)
                {
                    transition.Condition = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Indique si la transition est une boucle (self-loop).
        /// </summary>
        public bool IsLoop => transition.IsLoop;

        /// <summary>
        /// Indique si la transition n'est pas une boucle.
        /// </summary>
        public bool IsNotLoop => !IsLoop;       

        /// <summary>
        /// Coordonnée X de départ décalée pour dessiner la transition (hors boucle).
        /// </summary>
        public double XDepartDecale
        {
            get
            {
                return this.transition.GetXDepartDecale(GetIndex(), GetTotal(), EtatArrivee.EtatRadius);
            }
        }

        /// <summary>
        /// Coordonnée Y de départ décalée pour dessiner la transition (hors boucle).
        /// </summary>
        public double YDepartDecale
        {
            get
            {               
                return this.transition.GetYDepartDecale(GetIndex(), GetTotal(), EtatArrivee.EtatRadius);
            }
        }

        /// <summary>
        /// Coordonnée X d'arrivée décalée pour dessiner la transition (hors boucle).
        /// </summary>
        public double XArriveeDecale
        {
            get
            {
                return this.transition.GetXArriveeDecale(GetIndex(), GetTotal(), EtatArrivee.EtatRadius);
            }
        }

        /// <summary>
        /// Coordonnée Y d'arrivée décalée pour dessiner la transition (hors boucle).
        /// </summary>
        public double YArriveeDecale
        {
            get
            {
                return this.transition.GetYArriveeDecale(GetIndex(), GetTotal(), EtatArrivee.EtatRadius);               
            }
        }
        
        /// <summary>
        /// Tracé de la courbe de bézier entre deux état A et B
        /// </summary>
        public string PathData => geometry.GetBezierPathData(GetIndex(), GetTotal(), EtatArrivee.EtatRadius);

        /// <summary>
        /// Tracé de la courbe pour une boucle
        /// </summary>
        public string LoopPathData => geometry.GetLoopPathData(GetIndex(), GetTotal(), EtatDepart.EtatRadius);

        /// <summary>
        /// Met un angle sur le texte pour qu'elle suivent l'orientation de la courbe
        /// </summary>
        public double AngleTexte => geometry.GetTextAngle(GetIndex(), GetTotal(), EtatArrivee.EtatRadius);

        ///// <summary>
        ///// Coordonnées de la flèche pour le rendu graphique.
        ///// </summary>
        public string FlechePoints => geometry.GetArrowPoints(IsLoop, GetIndex(), GetTotal(), IsLoop ? EtatDepart.EtatRadius : EtatArrivee.EtatRadius);


        ///// <summary>
        ///// Position X du texte associé à la transition.
        ///// </summary>
        public double XTexte => geometry.GetTextPosition(IsLoop, GetIndex(), GetTotal(), IsLoop ? EtatDepart.EtatRadius : EtatArrivee.EtatRadius).X;

        ///// <summary>
        ///// Position Y du texte associé à la transition.
        ///// </summary>
        public double YTexte => geometry.GetTextPosition(IsLoop, GetIndex(), GetTotal(), IsLoop ? EtatDepart.EtatRadius : EtatArrivee.EtatRadius).Y;

      
        #endregion

        #region Constructors
        /// <summary>
        /// Initialise une transition entre deux états.
        /// </summary>
        /// <param name="depart">État de départ.</param>
        /// <param name="arrivee">État d'arrivée.</param>
        /// <param name="transition">Transition de base</param>
        public TransitionVM(EtatVM depart, EtatVM arrivee, Transition transition = null)
        {
            etatDepart = depart;
            etatArrivee = arrivee;
            if (transition != null)
            {
                this.transition = transition;
            } else
            {
                this.transition = new Transition(depart.Metier, arrivee.Metier);
            }
                
            this.geometry = new TransitionGeometry(this.transition);
            etatDepart.PropertyChanged += OnEtatPositionChanged;
            etatArrivee.PropertyChanged += OnEtatPositionChanged;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Permet de récupérer les transitions sortantes qui vont sur le même état
        /// </summary>
        /// <returns>le nobre de transitions dans ce sens</returns>
        private int GetIndex()
        {
            List<TransitionVM> list = EtatDepart.TransitionsOut.Where(t => t.EtatArrivee == EtatArrivee).ToList();
            return list.IndexOf(this);
        }  

        /// <summary>
        /// Permet de récupérer tout les transitions sortantes
        /// </summary>
        /// <returns>le nombre total de transition</returns>
        private int GetTotal()
        {
            return EtatDepart.TransitionsOut.Count(t => t.EtatArrivee == EtatArrivee);
        }       

        /// <summary>
        ///  Vérifie si un point est proche de la transition ou état (utile pour la sélection dans l'UI).
        /// </summary>
        /// <param name="p">Point cliqué</param>
        /// <param name="tolerance">Tolérance du click</param>
        /// <returns>Renvoie true si on est sur ou dans la zone de tolérance</returns>
        public bool Contient(LogicLayer.Point p, double tolerance = 7.0)
        {
            return geometry.Contains(p, tolerance, GetIndex(), GetTotal(), EtatArrivee.EtatRadius);
        }

        /// <summary>
        /// Met a jour toute les propriété lié a la transition
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement</param>
        /// <param name="e">Les arguments de l'événement contenant le nom de la propriété modifiée.</param>
        private void OnEtatPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EtatVM.X) || e.PropertyName == nameof(EtatVM.Y))
            {               
               RefreshGeometry();             
            }
        }

        /// <summary>
        /// Sert a recharger tout les propriété géométrique  
        /// </summary>
        public void RefreshGeometry()
        {
            OnPropertyChanged(nameof(XDepartDecale));
            OnPropertyChanged(nameof(YDepartDecale));
            OnPropertyChanged(nameof(XArriveeDecale));
            OnPropertyChanged(nameof(YArriveeDecale));
            OnPropertyChanged(nameof(PathData));
            OnPropertyChanged(nameof(LoopPathData));
            OnPropertyChanged(nameof(FlechePoints));
            OnPropertyChanged(nameof(XTexte));
            OnPropertyChanged(nameof(YTexte));
            OnPropertyChanged(nameof(AngleTexte));
            OnPropertyChanged(nameof(HandleX));
            OnPropertyChanged(nameof(HandleY));
        }

        /// <summary>
        /// Libère les ressources
        /// </summary>
        public void Dispose()
        {
            if (etatDepart != null)
                etatDepart.PropertyChanged -= OnEtatPositionChanged;

            if (etatArrivee != null)
                etatArrivee.PropertyChanged -= OnEtatPositionChanged;
        }
        #endregion

        /// <summary>
        /// Tente de déplacer le point de contrôle vers une nouvelle position.
        /// Annule le déplacement si la transition sort du canvas.
        /// </summary>
        /// <param name="targetX">Position X cible.</param>
        /// <param name="targetY">Position Y cible.</param>
        /// <param name="canvasWidth">Largeur du canvas.</param>
        /// <param name="canvasHeight">Hauteur du canvas.</param>
        public void TryMoveControlPoint(double targetX, double targetY, double canvasWidth, double canvasHeight)
        {           
            double oldControlX = Metier.ManualControlX ?? ControlX;
            double oldControlY = Metier.ManualControlY ?? ControlY;
            bool wasManualNull = Metier.ManualControlX == null && Metier.ManualControlY == null;            
            UpdateControlForMouse(targetX, targetY);           
            if (!TransitionInsideCanvas(IsLoop, GetIndex(), GetTotal(), canvasWidth, canvasHeight, isMove: true))
            {               
                if (wasManualNull)
                {
                    Metier.ManualControlX = null;
                    Metier.ManualControlY = null;
                }
                else
                {
                    Metier.ManualControlX = oldControlX;
                    Metier.ManualControlY = oldControlY;
                }
                RefreshGeometry();
            }
            else
            {               
                RefreshGeometry();
            }
        }

        /// <summary>
        /// Vérifie si la transition reste dans les limites du canvas.
        /// </summary>
        /// <param name="loop">True si c'est une boucle.</param>
        /// <param name="index">Index de la transition.</param>
        /// <param name="total">Nombre total de transitions entre ces états.</param>
        /// <param name="canvasWidth">Largeur du canvas.</param>
        /// <param name="canvasHeight">Hauteur du canvas.</param>
        /// <param name="isMove">True pour mode déplacement, false pour mode création.</param>
        /// <returns>True si la transition reste dans le canvas.</returns>
        public bool TransitionInsideCanvas(bool loop, int index, int total, double canvasWidth, double canvasHeight, bool isMove = false)
        {
            BoundingBox bbox;

            if (!isMove)
            {
                // Mode création
                if (loop)
                {
                    bbox = geometry.GetLoopBoundingBox(index, total, EtatDepart.EtatRadius);
                }
                else
                {
                    bbox = geometry.GetBezierBoundingBox(index, total, EtatArrivee.EtatRadius);
                }
            }
            else
            {
                double x1 = EtatDepart.X;
                double y1 = EtatDepart.Y;
                double x2 = EtatArrivee.X;
                double y2 = EtatArrivee.Y;
                double r = EtatArrivee.EtatRadius / 2.0;

                double controlX = Metier.ManualControlX ?? ControlX;
                double controlY = Metier.ManualControlY ?? ControlY;

                
                PointD pStart = geometry.ProjectOnCircle(x1, y1, r, controlX, controlY);
                PointD pEnd = geometry.ProjectOnCircle(x2, y2, r, controlX, controlY);

                BezierPoints points = new BezierPoints(
                    pStart.X,
                    pStart.Y,
                    controlX,
                    controlY,
                    pEnd.X,
                    pEnd.Y
                );
                bbox = geometry.CalculExtrema(points);

            }

            bool dansBords =
                bbox.MinX >= 0 &&
                bbox.MinY >= 0 &&
                bbox.MaxX <= canvasWidth &&
                bbox.MaxY <= canvasHeight;

            return dansBords;
        }
    }
}
