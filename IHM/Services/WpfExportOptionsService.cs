using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer;
using Service.Interfaces;

namespace IHM.Services
{
    /// <summary>
    /// Implémentation WPF du service d’options d’export.
    /// </summary>
    public class WpfExportOptionsService : IExportOptionsService
    {
        /// <inheritdoc/>
        public (bool confirmed, ExportAction? mode, string? automateName) ShowExportOptions(string currentName)
        {
            ExportOptionsWindow window = new ExportOptionsWindow(currentName);
            bool? result = window.ShowDialog();
            (bool, ExportAction?, string?) res = (false, null, null);

            if (result == true)
            {
                res = (true, window.SelectedOption, window.AutomateName);
            }

            return res;
        }
    }
}
