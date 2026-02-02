namespace Service.Interfaces
{
    /// <summary>
    /// Définit un contrat pour les services capables de convertir un <see cref="Canvas"/> WPF en image.
    /// </summary>
    public interface ICanvasExportService
    {
        /// <summary>
        /// Sauvegarde le contenu graphique d’un <see cref="Canvas"/> dans un fichier image sur le disque.
        /// </summary>
        /// <param name="rootElement">Elément visuel WPF contenant l'automate.</param>
        /// <param name="filePath">Chemin d'enregistrement de l'image.</param>
        void SaveAutomatonAsImage(object rootElement, string filePath);
    }
}
