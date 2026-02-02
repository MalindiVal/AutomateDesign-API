using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    /// <summary>
    /// Classe de base pour tous les ViewModels.
    /// <para>
    /// Implémente <see cref="INotifyPropertyChanged"/> pour permettre la notification
    /// automatique des changements de propriétés dans les interfaces WPF ou autres bindings.
    /// </para>
    /// <para>
    /// Cette classe simplifie la mise à jour des UI lorsqu'une propriété change dans le ViewModel.
    /// </para>
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Événement déclenché lorsqu'une propriété change.
        /// <para>
        /// Nécessaire pour la liaison de données (data binding) dans WPF et autres frameworks.
        /// </para>
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifie l'UI qu'une propriété a changé.
        /// </summary>
        /// <param name="propertyName">
        /// Nom de la propriété qui a changé.
        /// Utilise automatiquement le nom de la propriété appelante si aucun nom n'est fourni.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
