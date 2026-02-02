using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IHM.Converters
{
    /// <summary>
    /// Inverse un booléen
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        /// <summary>
        /// Convertit une valeur booléenne en son contraire.
        /// </summary>
        /// <param name="value">Valeur source provenant du binding.</param>
        /// <param name="targetType">Type attendu par la cible du binding.</param>
        /// <param name="parameter">Paramètre optionnel non utilisé.</param>
        /// <param name="culture">Culture utilisée pour la conversion.</param>
        /// <returns>
        /// <c>true</c> si <paramref name="value"/> est <c>false</c>,
        /// <c>false</c> si <paramref name="value"/> est <c>true</c>,
        /// sinon <c>true</c>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool res = true;
            if (value is bool boolValue)
            {
                res = !boolValue;
            }
            return res;
        }

        /// <summary>
        /// Réalise l'inversion inverse du Booléen, utilisée lors du binding bidirectionnel.
        /// </summary>
        /// <param name="value">Valeur provenant de la cible (UI).</param>
        /// <param name="targetType">Type attendu par la source du binding.</param>
        /// <param name="parameter">Paramètre optionnel non utilisé.</param>
        /// <param name="culture">Culture utilisée pour la conversion.</param>
        /// <returns>
        /// <c>true</c> si <paramref name="value"/> est <c>false</c>,
        /// <c>false</c> si <paramref name="value"/> est <c>true</c>,
        /// sinon <c>false</c>.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool res = false;
            if (value is bool boolValue)
            {
                res = !boolValue;
            }
            return res;
        }
    }
}
