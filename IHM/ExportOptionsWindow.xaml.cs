using LogicLayer;
using System.Windows;

namespace IHM
{
    /// <summary>
    /// Fenêtre permettant à l'utilisateur de choisir le type d'export pour un automate.
    /// Permet l'export sur le réseau ou en image.
    /// Le champ de nom peut être pré-rempli si un nom d'automate existant est fourni.
    /// </summary>
    public partial class ExportOptionsWindow : Window
    {
        #region Attributs
        private ExportAction selectedOption;
        private string automateName;
        #endregion

        #region Propriétés
        /// <summary>
        /// Option sélectionnée par l'utilisateur ("Network" ou "Image").
        /// </summary>
        public ExportAction SelectedOption => selectedOption;

        /// <summary>
        /// Nom de l'automate saisi par l'utilisateur (utilisé uniquement pour l'export réseau).
        /// </summary>
        public string AutomateName => automateName;
        #endregion

        #region constructeur
        /// <summary>
        /// Initialise une nouvelle instance de la fenêtre d'export.
        /// </summary>
        /// <param name="existingName">Nom de l'automate existant à pré-remplir dans le champ (optionnel).</param>
        public ExportOptionsWindow(string existingName = "")
        {
            InitializeComponent();
            NameTextBox.Text = existingName; // Pré-remplissage du nom de l'automate
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Confirme l'export réseau.
        /// Vérifie que le nom est renseigné avant de fermer la fenêtre avec DialogResult = true.
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement (bouton Confirmer).</param>
        /// <param name="e">Arguments de l'événement RoutedEventArgs.</param>
        private void NetworkButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Veuillez entrer un nom avant de confirmer.",
                                "Nom requis",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
            else
            {
                selectedOption = ExportAction.Reseau;
                automateName = name;

                DialogResult = true;
                Close();
            }

            
        }

        /// <summary>
        /// Confirme l'export en tant qu'image et ferme la fenêtre.
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement (bouton Image).</param>
        /// <param name="e">Arguments de l'événement RoutedEventArgs.</param>
        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();

            selectedOption = ExportAction.Image;

            if (string.IsNullOrWhiteSpace(name))
            {
                automateName = "Automate";
            } else
            {
                automateName = name;
            }
                

            DialogResult = true;
            Close();
        }

        private void CsharpButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();

            selectedOption = ExportAction.CSharp;

            if (string.IsNullOrWhiteSpace(name))
            {
                automateName = "Automate";
            }
            else
            {
                automateName = name;
            }


            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Annule l'export et ferme la fenêtre.
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement (bouton Annuler).</param>
        /// <param name="e">Arguments de l'événement RoutedEventArgs.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        #endregion
    }
}
