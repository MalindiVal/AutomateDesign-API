using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Représente un état d'un automate.
    /// </summary>
    public class Etat
    {
        #region Attributs
        private int id;
        private string nom = "Etat";
        private bool estFinal;
        private bool estInitial;
        private Position position = new Position(0, 0);
        #endregion

        #region Propriétés
        /// <summary>
        /// Nom de l'état
        /// </summary>
        public string Nom { get => nom; set => nom = value; }

        /// <summary>
        /// Est un etat final
        /// </summary>
        public bool EstFinal { get => estFinal; set => estFinal = value; }

        /// <summary>
        /// Est un etat final
        /// </summary>
        public bool EstInitial { get => estInitial; set => estInitial = value; }

        /// <summary>
        /// Propriété id
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// Position de l'état sur le canvas
        /// </summary>
        public Position Position { get => position; set => position = value; }
        #endregion

        /// inheritdoc/>
        public override bool Equals(object? obj)
        {
            bool res;
            if (obj is Etat other)
            {
                if (this.Id > 0 && other.Id > 0)
                    res = this.Id == other.Id;
                else
                {
                    res = string.Equals(Nom, other.Nom, StringComparison.Ordinal)
                        && EstFinal == other.EstFinal
                        && EstInitial == other.EstInitial;
                }
            } else {
                res = false;
            }
            return res;
        }

        /// inheritdoc/>
        public override int GetHashCode()
        {
            return Id > 0
                ? Id.GetHashCode()
                : HashCode.Combine(Nom, EstFinal, EstInitial);
        }


    }
}
