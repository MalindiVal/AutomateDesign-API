using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Représente une transition entre deux états dans un automate.
    /// </summary>
    public class Transition
    {
        #region Attributs
        private Etat etat1;
        private Etat etat2;
        private string condition = "Condition";
        private double? manualControlX;
        private double? manualControlY;
        #endregion

        #region Propriétés       
        /// <summary>
        /// Indique si un point de contrôle manuel a été défini.
        /// </summary>
        public bool HasManualControl => ManualControlX.HasValue && ManualControlY.HasValue;

        /// <summary>
        /// Etat de départ
        /// </summary>
        public Etat EtatDebut { get => etat1; set => etat1 = value; }

        /// <summary>
        /// Etat de destination
        /// </summary>
        public Etat EtatFinal { get => etat2; set => etat2 = value; }

        /// <summary>
        /// Nom de la transition
        /// </summary>
        public string Condition { get => condition; set => condition = value; }

        #endregion

        #region Propriétés calculées

        /// <summary>
        /// Coordonnée X du point de contrôle manuel (nullable).
        /// </summary>
        public double? ManualControlX { get => manualControlX; set => manualControlX = value; }

        /// <summary>
        /// Coordonnée Y du point de contrôle manuel (nullable).
        /// </summary>
        public double? ManualControlY { get => manualControlY; set => manualControlY = value; }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur d'une transition
        /// </summary>
        /// <param name="debut">Etat de début</param>
        /// <param name="fin">Etat final</param>
        public Transition(Etat debut,Etat fin) 
        { 
            this.etat1 = debut;
            this.etat1.EstFinal = false;
            this.etat2 = fin;
        }

        #endregion
    }
}
