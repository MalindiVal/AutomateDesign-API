using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    /// <summary>
    /// Interface de service pour la génération de fichier C#
    /// </summary>
    public interface ICSharpService
    {
        /// <summary>
        /// Permet d'exporter un automate en plussieurs fichiers c#
        /// </summary>
        /// <param name="automate">Automate</param>
        /// <param name="filepath">Chemin du dossier</param>
        public void ExportAutomate(Automate automate, string filepath);
    }
}
