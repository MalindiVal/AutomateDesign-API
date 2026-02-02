using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Exceptions
{
    /// <summary>
    /// Exception levée lorsqu'un identifiant négatif est assigné à un automate.
    /// </summary>
    public class NoNegatifIdError : Exception
    {
        /// <summary>
        /// Constructeur par défaut de l'exception NoNegatifIdError.
        /// Se lève si l'identifiant est négatif.
        /// </summary>
        public NoNegatifIdError() : base("Pas de valeur négatif pour l'identifiant") { }
    }
}
