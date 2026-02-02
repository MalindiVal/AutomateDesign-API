using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Sert a faire les calcule géométrique des transition
    /// </summary>
    public partial class TransitionGeometry
    {
        #region Constantes
        const double TailleFleche = 10;
        #endregion

        #region Attributs
        private readonly Transition transition;
        #endregion

        #region Records
        /// <summary>
        /// Ligne
        /// </summary>
        /// <param name="X1">coordonnées X du début</param>
        /// <param name="Y1">coordonnées Y du début</param>
        /// <param name="X2">coordonnées X de la fin</param>
        /// <param name="Y2">coordonnées Y de la fin</param>
        public record LinePoints(double X1, double Y1, double X2, double Y2);

        /// <summary>
        /// Point avec des coordonées double
        /// </summary>
        /// <param name="X">abscisse</param>
        /// <param name="Y">ordonnée</param>
        public record PointD(double X, double Y);

        /// <summary>
        /// Cadre pour poser une boucle sans sortir du canvas
        /// </summary>
        /// <param name="MinX">X minimal</param>
        /// <param name="MinY">Y minimal</param>
        /// <param name="MaxX">X maximal</param>
        /// <param name="MaxY">Y maximale</param>
        public record BoundingBox(double MinX, double MinY, double MaxX, double MaxY);

        /// <summary>
        /// Différents points pour tracer la courbes 
        /// </summary>
        /// <param name="StartX">Coordonées X du point de départ</param>
        /// <param name="StartY">Coordonées Y du point de départ</param>
        /// <param name="ControlX">Coordonées X du point de controle</param>
        /// <param name="ControlY">Coordonées Y du point de controle</param>
        /// <param name="EndX">Coordonées X du point de fin</param>
        /// <param name="EndY">Coordonées Y du point de fin</param>
        public record BezierPoints(double StartX, double StartY, double ControlX, double ControlY, double EndX, double EndY);
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur de la géométrie de la transition
        /// </summary>
        /// <param name="transition">Transitions pour laquelles on calcule</param>
        public TransitionGeometry(Transition transition)
        {
            this.transition = transition;
        }
        #endregion
    }
}