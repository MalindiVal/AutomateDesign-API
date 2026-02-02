using System;
using System.Windows.Input;

namespace ViewModels
{
    /// <summary>
    /// ViewModel responsable de la mise à jour d’une transition dans l’interface.
    /// Permet de modifier, confirmer ou supprimer une transition existante.
    /// </summary>
    public class UpdateTransitionViewModel : BaseViewModel
    {
        #region Attributes
        private const int MaxNomLength = 25;
        private string nomTemp;
        private ICommand cancelCommand;
        private ICommand deleteCommand;
        private ICommand confirmCommand;
        #endregion

        #region Properties

        /// <summary>
        /// Commande pour Annuler les modifications
        /// </summary>
        public ICommand CancelCommand
        {
            get => cancelCommand;
            set => cancelCommand = value;
        }

        /// <summary>
        /// Commande pour la suppresion de l'etat
        /// </summary>
        public ICommand DeleteCommand
        {
            get => deleteCommand;
            set => deleteCommand = value;
        }

        /// <summary>
        /// Commande pour la confirmation de modification
        /// </summary>
        public ICommand ConfirmCommand
        {
            get => confirmCommand;
            set => confirmCommand = value;
        }
        /// <summary>
        /// Obtient la transition actuellement modifiée.
        /// </summary>
        public TransitionVM Transition { get; }

        /// <summary>
        /// Indique si le nom temporaire dépasse la longueur maximale.
        /// </summary>
        public bool IsNomTooLong => !string.IsNullOrEmpty(NomTemp) && NomTemp.Length >= MaxNomLength;

        /// <summary>
        /// Obtient ou définit temporairement le nom (condition) de la transition,
        /// avant validation ou annulation des modifications.
        /// </summary>
        public string NomTemp
        {
            get => nomTemp;
            set
            {
                if (string.IsNullOrEmpty(value))
                    nomTemp = string.Empty;
                else if (value.Length > MaxNomLength)
                    nomTemp = value.Substring(0, MaxNomLength);
                else
                    nomTemp = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNomTooLong));
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initialise une nouvelle instance du ViewModel de mise à jour de transition.
        /// </summary>
        /// <param name="transition">Transition à modifier.</param>
        public UpdateTransitionViewModel(TransitionVM transition)
        {
            Transition = transition;
            NomTemp = transition.Condition;
            CancelCommand = new RelayCommand(Annuler);
            DeleteCommand = new RelayCommand(Supprimer);
            ConfirmCommand = new RelayCommand(Confirmer);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Confirme les modifications apportées à la transition et ferme la fenêtre.
        /// </summary>
        private void Confirmer()
        {
            Transition.Condition = NomTemp;
            CloseRequest?.Invoke(ActionResult.Confirmer);
        }

        /// <summary>
        /// Supprime la transition et ferme la fenêtre.
        /// </summary>
        private void Supprimer() => CloseRequest?.Invoke(ActionResult.Supprimer);

        /// <summary>
        /// Annule les modifications et ferme la fenêtre sans action.
        /// </summary>
        private void Annuler() => CloseRequest?.Invoke(ActionResult.None);
        #endregion

        /// <summary>
        /// Représente les différentes actions possibles lors de la fermeture de la fenêtre.
        /// </summary>
        public enum ActionResult
        {
            None,
            Supprimer,
            Confirmer
        }

        /// <summary>
        /// Action à invoquer pour fermer la fenêtre avec un résultat donné.
        /// </summary>
        public Action<ActionResult> CloseRequest { get; set; }
    }
}
