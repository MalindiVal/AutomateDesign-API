using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Représente une position dans un plan 2D.
    /// </summary>
    public class Position
    {
        #region Attributs
        private double x;
        private double y;
        #endregion

        #region Propriétés
        /// <summary>
        /// Propriété X
        /// </summary>
        public double X { get => x; set => x = value; }

        /// <summary>
        /// Propriété Y
        /// </summary>
        public double Y { get => y; set => y = value; }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="x">Coordonnées en X</param>
        /// <param name="y">Coordonnées en Y</param>
        public Position(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        #endregion
    }
}
