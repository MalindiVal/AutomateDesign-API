using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Représente un point dans un espace 2D.
    /// </summary>
    public struct Point
    {
        #region Attributs
        private double x;
        private double y;
        #endregion

        #region Propriétés
        /// <summary>
        /// Coordonnée X du point.
        /// </summary>
        public double X { get { return x; } }
        /// <summary>
        /// Coordonnée Y du point.
        /// </summary>
        public double Y { get { return y; } }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur de Point.
        /// </summary>
        /// <param name="x">Coordonnée X du point.</param>
        /// <param name="y">Coordonnée Y du point.</param>
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion
    }
}
