using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    /// <summary>
    /// Service d’IHM permettant de demander à l’utilisateur comment exporter un automate.
    /// </summary>
    public interface IExportOptionsService
    {
        /// <summary>
        /// Affiche la fenêtre d’options d’export et retourne la décision.
        /// </summary>
        /// <param name="currentName">Nom courant proposé pour l’automate.</param>
        /// <returns>
        /// confirmed : l’utilisateur a validé l’action<br/>
        /// mode : "Reseau" ou "Image" ou "CSharp"<br/>
        /// automateName : nom choisi pour l’automate
        /// </returns>
        (bool confirmed, ExportAction? mode, string? automateName) ShowExportOptions(string currentName);
    }
}
