using LogicLayer;
using Service.Interfaces;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ViewModels;

namespace IHM.Services
{
    /// <summary>
    /// Implémentation de l'interface ICanvasExport. Cette classe utilise les classes de la couche WPF
    /// </summary>
    public class CanvasExportService : ICanvasExportService
    {
        public void SaveAutomatonAsImage(object rootElement, string filePath)
        {
            // Calcul du bounding box
            double minX = double.PositiveInfinity;
            double minY = double.PositiveInfinity;
            double maxX = double.NegativeInfinity;
            double maxY = double.NegativeInfinity;

            if (rootElement is FrameworkElement fe)
            {
                // Récupération des ItemsControls
                List<ItemsControl> itemsControls =
                    FindVisualChildren<ItemsControl>(fe).ToList();

                ItemsControl? etatsControl = itemsControls.FirstOrDefault(ic => ic.Items.SourceCollection?.OfType<EtatVM>().Any() == true);
                ItemsControl? etatsClone = null;
                if (etatsControl != null)
                {
                    etatsClone = CloneItemsControl(etatsControl);
                    foreach (EtatVM etat in etatsClone.Items.SourceCollection)
                    {
                        double r = etat.EstFinal ? etat.EtatFinalRadius : etat.EtatRadius;

                        double left = etat.X - r;
                        double right = etat.X + r;
                        double top = etat.Y - r;
                        double bottom = etat.Y + r;

                        minX = Math.Min(minX, left);
                        minY = Math.Min(minY, top);
                        maxX = Math.Max(maxX, right);
                        maxY = Math.Max(maxY, bottom);
                    }
                }


                ItemsControl? transitionsControl = itemsControls.FirstOrDefault(ic => ic.Items.SourceCollection?.OfType<TransitionVM>().Any() == true);
                ItemsControl? transitionsClone = null;
                if (transitionsControl != null)
                {
                    transitionsClone = CloneItemsControl(transitionsControl);

                    foreach (TransitionVM transition in transitionsClone.Items.SourceCollection)
                    {
                        double r = transition.Metier?.Condition != null ? transition.Metier.Condition.Count() * 10 : 10;

                        double left = transition.XTexte - r;
                        double right = transition.XTexte + r;
                        double top = transition.YTexte - r;
                        double bottom = transition.YTexte + r;

                        minX = Math.Min(minX, left);
                        minY = Math.Min(minY, top);
                        maxX = Math.Max(maxX, right);
                        maxY = Math.Max(maxY, bottom);
                    }
                }

                // Dimensions de l'image
                double offsetX = minX < 0 ? -minX : -minX;
                double offsetY = minY < 0 ? -minY : -minY;

                double width = Math.Max(1, maxX - minX);
                double height = Math.Max(1, maxY - minY);

                // Création du conteneur temporaire
                Grid container = new Grid
                {
                    Width = width,
                    Height = height,
                    Background = Brushes.White
                };
                if (etatsControl != null)
                {
                    ApplyTranslation(etatsClone, offsetX, offsetY);
                    container.Children.Add(etatsClone);
                }
                    

                if (transitionsControl != null)
                {
                    ApplyTranslation(transitionsClone, offsetX, offsetY);
                    container.Children.Add(transitionsClone);
                }

                // Mise en forme
                container.Measure(new Size(width, height));
                container.Arrange(new Rect(0, 0, width, height));
                container.UpdateLayout();

                Application.Current.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);

                // Export
                RenderAndSave(container, width, height, filePath);
            }

            
        }

        #region Helpers
        private void RenderAndSave(Visual visual, double width, double height, string filePath)
        {
            string? dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            RenderTargetBitmap rtb = new RenderTargetBitmap(
                (int)Math.Ceiling(width),
                (int)Math.Ceiling(height),
                96, 96,
                PixelFormats.Pbgra32);

            rtb.Render(visual);

            BitmapEncoder encoder = CreateEncoder(filePath);
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using FileStream fs = File.Create(filePath);
            encoder.Save(fs);
        }

        private BitmapEncoder CreateEncoder(string filePath)
        {
            return Path.GetExtension(filePath).ToLower() switch
            {
                ".jpg" or ".jpeg" => new JpegBitmapEncoder { QualityLevel = 95 },
                ".png" => new PngBitmapEncoder(),
                ".bmp" => new BmpBitmapEncoder(),
                _ => throw new NotSupportedException($"Format non supporté : {filePath}")
            };
        }

        private ItemsControl CloneItemsControl(ItemsControl original) =>
            new ItemsControl
            {
                ItemsSource = original.ItemsSource,
                ItemTemplate = original.ItemTemplate,
                ItemsPanel = original.ItemsPanel,
                ItemContainerStyle = original.ItemContainerStyle
            };

        private IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T t)
                    yield return t;

                foreach (T result in FindVisualChildren<T>(child))
                    yield return result;
            }
        }

        private void ApplyTranslation(UIElement element, double dx, double dy)
        {
            element.RenderTransform = new TranslateTransform(dx, dy);
        }
        #endregion
    }
}
