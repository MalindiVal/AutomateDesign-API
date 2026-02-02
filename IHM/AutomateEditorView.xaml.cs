using IHM.Services;
using LogicLayer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ViewModels;
using static IHM.Helpers.VisualTreeHelpers;

namespace IHM
{
    /// <summary>
    /// Logique d’interaction pour AutomateEditorView.xaml.
    /// Gère l’interface utilisateur de l’éditeur d’automates, incluant :
    /// - Sélection des outils
    /// - Gestion du canvas
    /// - Interaction avec les états et transitions
    /// - Sauvegarde et export
    /// </summary>
    public partial class AutomateEditorView : UserControl
    {
        #region Attributs et Propriétés
        // Vue Modèle associée à cette vue
        private AutomateEditorViewModel? Vm => DataContext as AutomateEditorViewModel;
        private System.Windows.Point lastMousePosition;
        #endregion


        /// <summary>
        /// Constructeur
        /// </summary>
        public AutomateEditorView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gère le clic sur le bouton de fermeture de la notification.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseNotification_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AutomateEditorViewModel vm)
            {
                vm.NotificationVisible = false;
            }
        }
        #region Gestion Canvas

        /// <summary>
        /// Gère le clic gauche sur le canvas principal.
        /// Ajoute un état selon l’outil sélectionné.
        /// Désactive le mode de déplacement de la transition si un PATH n'est pas cliqué
        /// </summary>
        /// <param name="sender">Le canvas sur lequel l’événement a été déclenché.</param>
        /// <param name="e">Arguments de l’événement contenant la position du clic.</param>
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          if (!e.Handled)
            {
                DependencyObject source = e.OriginalSource as DependencyObject;
                bool clickedTransition = false;

                while (source != null && !clickedTransition)
                {
                    if (source is Path path && path.DataContext is TransitionVM)
                    {
                        clickedTransition = true;
                    }
                    else
                    {
                        source = VisualTreeHelper.GetParent(source);
                    }
                }

                if (!clickedTransition && Vm != null)
                {
                    foreach (TransitionVM t in Vm.Transitions)
                    {
                        t.IsSelected = false;
                    }

                    Vm.StopDraggingControlPoint();
                    Mouse.Capture(null);
                }

                Grid grid = FindParentOfType<Grid>(e.OriginalSource as DependencyObject);
                if (grid != null && grid.DataContext is EtatVM evm)
                {
                    System.Windows.Point clickPos = e.GetPosition(EditorCanvas);
                    LogicLayer.Point logicPoint = new LogicLayer.Point(clickPos.X, clickPos.Y);
                    bool isTransitionTool = Vm.CurrentTool == TypeEtat.Transition;

                    if (!isTransitionTool)
                    {
                        Vm.StartDraggingEtat();
                        lastMousePosition = clickPos;
                        grid.CaptureMouse();
                    }

                    Vm.OnEtatClicked(evm, logicPoint, isTransitionTool);
                    e.Handled = true;
                }
                else if (sender is Canvas canvas)
                {
                    System.Windows.Point position = e.GetPosition(canvas);
                    Vm.OnCanvasClicked(position.X, position.Y, canvas.ActualWidth, canvas.ActualHeight);
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Gère la pression d’une touche au niveau de la vue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                if (DataContext is AutomateEditorViewModel vm)
                {
                    vm.CancelTransition();
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Met à jour la taille du canvas lorsque la taille du canvas de l’éditeur change.
        /// </summary>
        /// <param name="sender">Le canvas de l’éditeur qui a été redimensionné.</param>
        /// <param name="e">Arguments de l’événement contenant les nouvelles dimensions.</param>
        private void EditorCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
             if (DataContext is AutomateEditorViewModel vm)
             {
                vm.CanvasWidth = e.NewSize.Width;
                vm.CanvasHeight = e.NewSize.Height;
             }
        }

        /// <summary>
        /// Gère le déplacement de la souris sur un état.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Etat_MouseMove(object sender, MouseEventArgs e)
        {
            if (Vm != null && Vm.IsDraggingEtat)
            {
                if (DataContext is AutomateEditorViewModel vm)
                {
                    System.Windows.Point pos = e.GetPosition(EditorCanvas);
                    vm.MoveEtat(pos.X, pos.Y);
                    lastMousePosition = pos;                    
                }
            }
        }

        /// <summary>
        /// Gère le relâchement du clic gauche sur un état.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Etat_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid grid && Vm != null)
            {
                if (Vm.IsDraggingEtat) 
                {
                    System.Windows.Point pos = e.GetPosition(EditorCanvas);
                    Vm.RealeaseEtat(pos.X, pos.Y);
                    Vm.StopDraggingEtat();

                    grid.ReleaseMouseCapture();
                    e.Handled = true;
                }
            }

        }

        #endregion

        #region Gestion Etats & Transitions

        /// <summary>
        /// Clic droit sur un état : ouvre le popup pour modifier ou supprimer l’état.
        /// </summary>
        /// <param name="sender">Le Grid représentant l’état cliqué.</param>
        /// <param name="e">Arguments de l’événement de souris.</param>
        private void Etat_RightClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is EtatVM evm && Vm != null)
            {
                if (DataContext is AutomateEditorViewModel vm)
                {
                    UpdateEtatViewModel updateVm = new UpdateEtatViewModel(evm);
                    UpdateEtatView popup = new UpdateEtatView(updateVm);
                    popup.Owner = Window.GetWindow(this);
                    popup.ShowDialog();

                    if (popup.Result == UpdateEtatViewModel.ActionResult.Supprimer)
                        vm.SupprimerEtat(evm);
                }
            }
        }

        /// <summary>
        /// Clic gauche sur un état : permet de créer une transition si l’outil transition est actif.
        /// </summary>
        /// <param name="sender">Le Grid représentant l’état cliqué.</param>
        /// <param name="e">Arguments de l’événement de souris.</param>
        private void Etat_LeftClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is EtatVM evm && Vm != null)
            {
                if (DataContext is AutomateEditorViewModel vm)
                {
                    vm.LinkEtats(evm);
                }
            }
        }


        /// <summary>
        /// Clic droit sur une transition : ouvre le popup pour modifier ou supprimer la transition.
        /// </summary>
        /// <param name="sender">Le FrameworkElement représentant la transition cliquée.</param>
        /// <param name="e">Arguments de l’événement de souris.</param>
        private void Transition_RightClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is TransitionVM tvm && Vm != null)
            {
                UpdateTransitionViewModel updateVM = new UpdateTransitionViewModel(tvm);
                UpdateTransitionView popup = new UpdateTransitionView(updateVM);
                popup.Owner = Window.GetWindow(this);
                popup.ShowDialog();

                if (popup.Result == UpdateTransitionViewModel.ActionResult.Supprimer)
                    if (DataContext is AutomateEditorViewModel vm)
                    {
                        vm.SupprimerTransition(tvm);
                    }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Débute le déplacement du point de contrôle.
        /// </summary>
        /// <param name="sender">Ellipse cliquée.</param>
        /// <param name="e">Événement de clic.</param>
        private void ControlPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse el && el.DataContext is TransitionVM tvm && Vm != null)
            {
                Vm.StartDraggingControlPoint(tvm); 
                el.CaptureMouse();
            }
        }

        /// <summary>
        /// Déplace le point de contrôle selon la position de la souris.
        /// </summary>
        /// <param name="sender">Ellipse déplacée.</param>
        /// <param name="e">Événement de mouvement.</param>
        private void ControlPoint_MouseMove(object sender, MouseEventArgs e)
        {
            if (Vm != null && Vm.IsDraggingControlPoint) 
            {
                System.Windows.Point pos = e.GetPosition(EditorCanvas);
                Vm.MoveControlPoint(pos.X, pos.Y); 
            }
        }

        /// <summary>
        /// Termine le déplacement du point de contrôle.
        /// </summary>
        /// <param name="sender">Ellipse relâchée.</param>
        /// <param name="e">Événement de relâchement.</param>
        private void ControlPoint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse el && Vm != null)
            {
                Vm.StopDraggingControlPoint(); 
                el.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Sélectionne une transition lors d’un clic gauche.
        /// </summary>
        /// <param name="sender">Chemin cliqué.</param>
        /// <param name="e">Événement de clic.</param>

        private void Transition_LeftClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Path path && path.DataContext is TransitionVM clicked && Vm != null)
            {
                foreach (TransitionVM t in Vm.Transitions)
                {
                    t.IsSelected = false;
                }

                clicked.IsSelected = true;
                e.Handled = true;
            }
        }
        #endregion

        #region Fichiers & Export

        /// <summary>
        /// Exporte l’automate courant via le réseau avec un nom unique.
        /// </summary>
        /// <param name="sender">Objet déclencheur de l’événement.</param>
        /// <param name="e">Arguments de l’événement.</param>
        private async void Export_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AutomateEditorViewModel vm)
            {
                Grid rootGrid = (Grid)this.Content;

                await vm.ExportAsync(rootGrid);
            }
        }

        #endregion
    }
}
