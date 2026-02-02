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
    /// Converter permettant de centrer une ellipse en ajustant sa position.
    /// </summary>
    public class CenterEllipseConverter : IValueConverter
    {
        /// <summary>
        /// Calcule une position centrée en soustrayant la moitié de la taille.
        /// </summary>
        /// <param name="value">La position initiale.</param>
        /// <param name="t">Le type de conversion.</param>
        /// <param name="parameter">La taille de l'ellipse.</param>
        /// <param name="culture">La culture utilisée.</param>
        /// <returns>La position ajustée pour centrer.</returns>
        public object Convert(object value, Type t, object parameter, CultureInfo culture)
        {
            double size = double.Parse((string)parameter);
            return (double)value - size / 2.0;
        }

        /// <summary>
        /// Conversion inverse non prise en charge.
        /// </summary>
        /// <param name="value">Valeur ignorée.</param>
        /// <param name="t">Type de conversion.</param>
        /// <param name="p">Paramètre ignoré.</param>
        /// <param name="c">Culture utilisée.</param>
        /// <returns>Toujours null.</returns>
        public object ConvertBack(object value, Type t, object p, CultureInfo c) => null;
    }
}
