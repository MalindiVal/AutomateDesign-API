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
    /// Logique d'interaction pour UpdateTransitionView.xaml
    /// </summary>
    public partial class UpdateTransitionView : Window
    {
        #region Attributs
        private UpdateTransitionViewModel.ActionResult result;
        private UpdateTransitionViewModel Vm => (UpdateTransitionViewModel)DataContext;
        #endregion

        #region Propriétés
        /// <summary>
        /// Résultat de l'action effectuée sur la fenêtre.
        /// </summary>
        public UpdateTransitionViewModel.ActionResult Result { get => result; set => result = value; }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Initialise une nouvelle instance de <see cref="UpdateTransitionView"/>.
        /// </summary>
        /// <param name="vm">Le ViewModel associé à cette fenêtre.</param>
        public UpdateTransitionView(UpdateTransitionViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            vm.CloseRequest = result =>
            {
                Result = result;
                Close();
            };
        }
        #endregion
    }
}
