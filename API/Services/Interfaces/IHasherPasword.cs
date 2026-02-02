namespace API.Services.Interfaces
{

    /// <summary>
    /// Interface pour le service de hachage et de vérification des mots de passe.
    /// Cette interface définit les méthodes nécessaires pour hacher un mot de passe
    /// et pour vérifier si un mot de passe en texte clair correspond à un hash stocké.
    /// </summary>
    public interface IHasherPassword
    {
        /// <summary>
        /// Hache un mot de passe
        /// Le mot de passe est transformé en un hash sécurisé avec un sel généré automatiquement.
        /// </summary>
        /// <param name="password">Le mot de passe en texte clair à hacher.</param>
        /// <returns>Une chaîne de caractères représentant le hash du mot de passe, incluant le sel et le facteur de travail.</returns>
        public string Hash(string password);

        /// <summary>
        /// Vérifie si un mot de passe en texte clair correspond à un hash stocké.
        /// </summary>
        /// <param name="password">Le mot de passe en texte clair à vérifier.</param>
        /// <param name="storedHash">Le hash stocké du mot de passe à comparer.</param>
        /// <returns>Vrai si le mot de passe correspond au hash, sinon faux.</returns>
        public bool Verify(string password, string storedHash);
    }
}
