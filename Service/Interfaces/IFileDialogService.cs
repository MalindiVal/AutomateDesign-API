namespace Service.Interfaces
{
    /// <summary>
    /// Service pour gérer les dialogues de fichiers.
    /// </summary>
    public interface IFileDialogService
    {
        /// <summary>
        /// Ouvre une boîte de dialogue pour sélectionner un fichier.
        /// </summary>
        /// <param name="filter">Filtre des types de fichiers</param>
        /// <param name="defaultExt">Extension par défaut</param>
        /// <returns>Le chemin du fichier sélectionné ou null si annulé</returns>
        string? OpenFile(string filter, string defaultExt);

        /// <summary>
        /// Ouvre une fenetre de dialogue pour y enregistrer un fichier
        /// </summary>
        /// <param name="filter">Filtre des types de fichiers</param>
        /// <param name="defaultExt">L'extension par défaut</param>
        /// <param name="defaultName">Le nom par défaut du fichier à enregistrer</param>
        /// <returns>le chemin du fichier à créer</returns>
        string? SaveFile(string filter, string defaultExt,string defaultName);

        /// <summary>
        /// Ouvre une fenetre de dialog pour ouvrir un dossier
        /// </summary>
        /// <returns>Le chemin du dossier choisi</returns>
        string? OpenFolder();
    }
}