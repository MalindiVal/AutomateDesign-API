    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModels;

namespace IHM
{
    /// <summary>
    /// Logique d'interaction pour ConnexionView.xaml
    /// </summary>
    public partial class ConnexionView : UserControl
    {
        #region Contructeur
        /// <summary>
        /// Constructeur
        /// </summary>
        public ConnexionView()
        {
            InitializeComponent();

            // Focus automatique sur le champ identifiant au chargement
            Loaded += (s, e) => IdentifiantTextBox.Focus();
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Gère le changement de mot de passe pour mettre à jour le ViewModel
        /// </summary>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConnexionViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }

        /// <summary>
        /// Permet de réinitialiser le PasswordBox depuis le ViewModel
        /// </summary>
        public void ClearPassword()
        {
            PasswordBox.Clear();
        }
        #endregion
    }
}
