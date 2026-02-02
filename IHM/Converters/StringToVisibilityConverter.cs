using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace IHM.Converters
{
    /// <summary>
    /// Convertit une chaîne en Visibility (visible si non vide)
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convertit une chaîne de caractères en visibilité.
        /// </summary>
        /// <param name="value">La valeur provenant du binding, attendue comme <see cref="string"/>.</param>
        /// <param name="targetType">Type attendu par la cible du binding (doit être <see cref="Visibility"/>).</param>
        /// <param name="parameter">Paramètre optionnel non utilisé.</param>
        /// <param name="culture">Culture utilisée pour la conversion.</param>
        /// <returns>
        /// <see cref="Visibility.Visible"/> si la chaîne n'est ni vide ni composée uniquement d'espaces ;
        /// sinon <see cref="Visibility.Collapsed"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Non implémenté
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
