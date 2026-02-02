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
    /// Convertisseur WPF pour transformer un <see cref="bool"/> en <see cref="Visibility"/>.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convertit un booléen en <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">Valeur booléenne à convertir.</param>
        /// <param name="targetType">Type cible (ignored).</param>
        /// <param name="parameter">Paramètre optionnel. Si "Invert", inverse la logique.</param>
        /// <param name="culture">Culture (ignored).</param>
        /// <returns>
        /// <see cref="Visibility.Visible"/> si <c>true</c>, sinon <see cref="Visibility.Collapsed"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Convertit un <see cref="Visibility"/> en booléen.
        /// </summary>
        /// <param name="value">Valeur de visibilité.</param>
        /// <param name="targetType">Type cible (ignored).</param>
        /// <param name="parameter">Paramètre optionnel. Si "Invert", inverse la logique.</param>
        /// <param name="culture">Culture (ignored).</param>
        /// <returns>True si visible, false sinon.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility v && v == Visibility.Visible;
        }
    }
}
