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
using System.Windows.Shapes;
using ViewModels;


namespace IHM
{
    /// <summary>
    /// Logique d'interaction pour UpdateEtatView.xaml
    /// </summary>
    public partial class UpdateEtatView : Window
    {
        #region Attributs
        private UpdateEtatViewModel.ActionResult result;
        private UpdateEtatViewModel Vm => (UpdateEtatViewModel)DataContext;
        #endregion

        #region Constructeurs
        /// <summary>
        /// Résultat de l'action effectuée sur la fenêtre.
        /// </summary>
        public UpdateEtatViewModel.ActionResult Result 
        { 
            get => result; 
            private set => result = value; 
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Initialise une nouvelle instance de <see cref="UpdateEtatView"/>.
        /// </summary>
        /// <param name="vm">Le ViewModel associé à cette fenêtre.</param>
        public UpdateEtatView(UpdateEtatViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            // Abonnement à l'événement de demande de fermeture
            vm.CloseRequest = HandleCloseRequest;
        }

        /// <summary>
        /// Gère la demande de fermeture de la fenêtre.
        /// </summary>
        /// <param name="actionResult"></param>
        private void HandleCloseRequest(UpdateEtatViewModel.ActionResult actionResult)
        {
            if (actionResult == UpdateEtatViewModel.ActionResult.None &&
                string.IsNullOrWhiteSpace(Vm.NomTemp))
            {
                MessageBox.Show(
                    "Le nom ne peut pas être vide.",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            Result = actionResult;
            Close();
        }
        #endregion
    }
}
