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
        #region Constantes     
        private const double DecalagePremiere = 5;
        private const double DecalageMultiple = 20;        
        #endregion

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
        /// Booléen qui renvoie true si on doit faire une boucle
        /// </summary>
        public bool IsLoop => EtatDebut == EtatFinal || (EtatDebut.Position.X == EtatFinal.Position.X && EtatDebut.Position.Y == EtatFinal.Position.Y);

        /// <summary>
        /// Détermine l'angle de la transition
        /// </summary>
        private double Angle => Math.Atan2(EtatFinal.Position.Y - EtatDebut.Position.Y, EtatFinal.Position.X - EtatDebut.Position.X);

        /// <summary>
        /// Détermine la distance entre les deux états
        /// </summary>
        private double Distance => Math.Sqrt(Math.Pow(EtatFinal.Position.X - EtatDebut.Position.X, 2) + Math.Pow(EtatFinal.Position.Y - EtatDebut.Position.Y, 2));

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

        /// <summary>
        /// Constructeur vide pour sérialisation ou initialisation ultérieure.
        /// </summary>
        public Transition(){}
        #endregion

        #region Méthodes
        /// <summary>
        /// Calcule la position géométrique X de l'état de départ liées a la structure logique de la transition 
        /// </summary>
        /// <param name="index">Nombre de transitions dans le même sens</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">rayon de l'état</param>
        /// <returns>renvoit la position X du départ de la transition</returns>
        public double GetXDepartDecale(int index, int total, double rayon)
        {
            double result;
            if (IsLoop)
            {
                result = EtatDebut.Position.X;
            }
            else
            {
                double dx = EtatFinal.Position.X - EtatDebut.Position.X;
                double dy = EtatFinal.Position.Y - EtatDebut.Position.Y;
                double longueur = Math.Sqrt(dx * dx + dy * dy);
                if (longueur == 0) longueur = 1;

                double offset = (index - (total - 1) / 2.0) * DecalageMultiple + DecalagePremiere;
                result = EtatDebut.Position.X + (rayon / 2.0) * Math.Cos(Angle) - dy / longueur * offset;
            }
            return result;        
        }

        /// <summary>
        /// Calcule la position géométrique Y de l'état de départ liées a la structure logique de la transition 
        /// </summary>
        /// <param name="index">Nombre de transitions dans le même sens</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">rayon de l'état</param>
        /// <returns>renvoit la position X du départ de la transition</returns>
        public double GetYDepartDecale(int index, int total, double rayon)
        {
            double result;
            if (IsLoop)
            {
                result = EtatDebut.Position.Y;
            }
            else
            {
                double dx = EtatFinal.Position.X - EtatDebut.Position.X;
                double dy = EtatFinal.Position.Y - EtatDebut.Position.Y;
                double longueur = Math.Sqrt(dx * dx + dy * dy);
                if (longueur == 0) longueur = 1;

                double offset = (index - (total - 1) / 2.0) * DecalageMultiple + DecalagePremiere;
                result = EtatDebut.Position.Y + (rayon / 2.0) * Math.Sin(Angle) + dx / longueur * offset;
            }
            return result;
                
        }

        /// <summary>
        /// Calcule la position géométrique X de l'état d'arrivée liées a la structure logique de la transition 
        /// </summary>
        /// <param name="index">Nombre de transitions dans le même sens</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">rayon de l'état</param>
        /// <returns>renvoit la position X d'arrivée de la transition</returns>
        public double GetXArriveeDecale(int index, int total, double rayon)
        {
            double result;
            if (IsLoop)
            {
                result = EtatFinal.Position.X;
            }
            else 
            {
                double dx = EtatFinal.Position.X - EtatDebut.Position.X;
                double dy = EtatFinal.Position.Y - EtatDebut.Position.Y;
                double longueur = Math.Sqrt(dx * dx + dy * dy);
                if (longueur == 0) longueur = 1;

                double offset = (index - (total - 1) / 2.0) * DecalageMultiple + DecalagePremiere;
                result = EtatFinal.Position.X - (rayon / 2.0) * Math.Cos(Angle) - dy / longueur * offset;
            }
            return result;             
        }

        /// <summary>
        /// Calcule la position géométrique Y de l'état d'arrivée liées a la structure logique de la transition 
        /// </summary>
        /// <param name="index">Nombre de transitions dans le même sens</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">rayon de l'état</param>
        /// <returns>renvoit la position Y d'arrivée de la transition</returns>
        public double GetYArriveeDecale(int index, int total, double rayon)
        {
            double result;
            if (IsLoop)
            {
                result = EtatFinal.Position.Y;
            }
            else
            {
                double dx = EtatFinal.Position.X - EtatDebut.Position.X;
                double dy = EtatFinal.Position.Y - EtatDebut.Position.Y;
                double longueur = Math.Sqrt(dx * dx + dy * dy);
                if (longueur == 0) longueur = 1;

                double offset = (index - (total - 1) / 2.0) * DecalageMultiple + DecalagePremiere;
                result = EtatFinal.Position.Y - (rayon / 2.0) * Math.Sin(Angle) + dx / longueur * offset;
            }
            return result;           
        }
        #endregion
    }
}
