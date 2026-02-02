using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using ViewModels;

namespace ViewModels
{
    /// <summary>
    /// VueModel de la fenetre UpdateEtatView
    /// </summary>
    public class UpdateEtatViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region Attributes
        private const int MaxNomLength = 25;
        private string nomTemp;
        private readonly Dictionary<string, List<string>> errors = new();
        private EtatVM etat;
        private bool isNomTooLong;
        private Action<ActionResult> closeRequest;
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
        /// Verifie si le nom n'est pas vide et ne dépasse pas la limite
        /// </summary>
        public bool IsNomTooLong => !string.IsNullOrEmpty(NomTemp) && NomTemp.Length >= MaxNomLength;

        /// <summary>
        /// Le nom temporaire
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

        /// <summary>
        /// Si il y a des erreurs
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return errors.Count > 0;
            }
        }

        /// <summary>
        /// L'etat qui est concernée par cette fenetre
        /// </summary>
        public EtatVM Etat { get => etat; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="etat">L'etat qui va etre modifié</param>
        public UpdateEtatViewModel(EtatVM etat)
        {
            this.etat = etat;
            NomTemp = etat.Nom;
            CancelCommand = new RelayCommand(Annuler);
            DeleteCommand = new RelayCommand(Supprimer);
            ConfirmCommand = new RelayCommand(Confirmer);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Evenement qui verifie si les erreurs repérées sont corrigées
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Récupère les erreurs dans une property
        /// </summary>
        /// <param name="propertyName">La property à analyser</param>
        /// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            IEnumerable res = null;
            if (propertyName != null && errors.ContainsKey(propertyName))
            {
                res = errors[propertyName];
            }
            return res;
        }

        /// <summary>
        /// Confirme les modifications réaliser
        /// </summary>
        private void Confirmer()
        {
            ValidateNom();

            if (HasErrors)
                return;

            Etat.Nom = NomTemp;
            CloseRequest?.Invoke(ActionResult.Confirmer);
        }

        /// <summary>
        /// Supprime l'état qui se fait modifier
        /// </summary>
        private void Supprimer() 
        { 
            CloseRequest?.Invoke(ActionResult.Supprimer);
        }

        /// <summary>
        /// Annule les modification réalisées
        /// </summary>
        private void Annuler()
        {
            CloseRequest?.Invoke(ActionResult.None);
        }
        #endregion

        /// <summary>
        /// Les action que va faire la fenetre lors de sa fermeture
        /// </summary>
        public Action<ActionResult> CloseRequest { 
            get => closeRequest; 
            set => closeRequest = value; 
        }

        /// <summary>
        /// Les actions possibles
        /// </summary>
        public enum ActionResult
        {
            None,
            Supprimer,
            Confirmer
        }

        private void ValidateNom()
        {
            ClearErrors(nameof(NomTemp));

            if (string.IsNullOrWhiteSpace(NomTemp))
                AddError(nameof(NomTemp), "Le nom ne peut pas être vide.");

            else if (NomTemp.Length > MaxNomLength)
                AddError(nameof(NomTemp), $"Le nom ne doit pas dépasser {MaxNomLength} caractères.");
        }

        private void AddError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
            {
                errors[propertyName].Add(error);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (errors.Remove(propertyName))
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
