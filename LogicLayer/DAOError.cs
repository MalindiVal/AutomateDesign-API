using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Exception levée lorsqu'une erreur survient dans la couche DAO.
    /// </summary>
    public class DAOError : Exception
    {
        /// <summary>
        /// Initialise une nouvelle instance de l'exception <see cref="DAOError"/> avec un message d'erreur.
        /// </summary>
        /// <param name="msg">Message décrivant l'erreur.</param>
        public DAOError(string msg) : base(msg){ }

        /// <summary>
        /// Initialise une nouvelle instance de l'exception <see cref="DAOError"/> avec un message d'erreur et une exception interne.
        /// </summary>
        /// <param name="msg">Message décrivant l'erreur.</param>
        /// <param name="inner">Exception interne.</param>
        public DAOError(string msg, Exception inner) : base(msg, inner) { }
    }
}
