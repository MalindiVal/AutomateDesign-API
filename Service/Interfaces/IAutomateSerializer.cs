using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    /// <summary>
    /// Interface pour la sérialisation et la désérialisation des automates.
    /// </summary>
    public interface IAutomateSerializer
    {
        /// <summary>
        /// Sauvegarde d'un automate dans un fichier local
        /// </summary>
        /// <param name="automate">Automate à sauvegarder</param>
        /// <param name="filename">Chemin du fichier à sauvegarder</param>
        void SauvegardeAutomate(Automate automate, string filename);

        /// <summary>
        /// Chargement d'un automate à partir d'un fichier
        /// </summary>
        /// <param name="filename">Chemin du fichier à ouvrir</param>
        /// <returns>Automate rçcupérer à partir</returns>
        Automate ChargementAutomate(string filename);
    }

}
