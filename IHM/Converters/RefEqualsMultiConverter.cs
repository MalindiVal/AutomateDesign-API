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
    /// Convertisseur qui vérifie si deux références sont égales.
    /// </summary>
    public class RefEqualsMultiConverter : IMultiValueConverter
    {
        /// <summary>
        /// Convertit deux références en un booléen indiquant si elles sont égales.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            => values is { Length: 2 } && ReferenceEquals(values[0], values[1]);

        /// <summary>
        /// Non supporté.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
