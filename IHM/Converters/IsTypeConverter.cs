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
    /// Convertisseur qui vérifie si une valeur est d'un type donné.
    /// </summary>
    public class IsTypeConverter : IValueConverter
    {
        /// <summary>
        /// Convertit une valeur en un booléen indiquant si elle est d'un type donné.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter is Type t && value != null && t.IsInstanceOfType(value);
        }

        /// <summary>
        /// Non supporté.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
