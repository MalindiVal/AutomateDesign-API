using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace IHM.Helpers
{
    /// <summary>
    /// Fournit des méthodes d'aide pour la manipulation de l'arbre visuel WPF.
    /// </summary>
    public static class VisualTreeHelpers
    {
        /// <summary>
        /// Recherche le premier parent d'un type spécifique dans l'arbre visuel WPF.
        /// </summary>
        /// <typeparam name="T">Type du parent recherché.</typeparam>
        /// <param name="child">Elément enfant à partir duquel débuter la recherche.</param>
        /// <returns>Le premier parent du type donné.</returns>
        public static T? FindParentOfType<T>(DependencyObject? child) where T : DependencyObject
        {
            T? result = null;

            while (child != null && result == null)
            {
                var parent = VisualTreeHelper.GetParent(child);

                if (parent is T typedParent)
                {
                    result = typedParent;
                }

                child = parent;
            }

            return result;
        }
    }
}
