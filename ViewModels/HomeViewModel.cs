using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModels
{
    /// <summary>
    /// ViewModel de la page d'accueil.
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Commande pour créer un nouvel automate
        /// </summary>
        public ICommand NewAutomateCommand { get; }  

        /// <summary>
        /// Commande pour importer un automate
        /// </summary>
        public ICommand ImportAutomateCommand { get; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="newAutomateAction">Action pour créer un nouvel automate</param>
        /// <param name="importAutomateAction">Action pour importer un automate</param>
        public HomeViewModel(Action newAutomateAction, Action importAutomateAction)
        {
            NewAutomateCommand = new RelayCommand(newAutomateAction);
            ImportAutomateCommand = new RelayCommand(importAutomateAction);
        }
    }
}
