using BCrypt.Net;
using API.Services.Interfaces;

namespace API.Services
{
    /// <summary>
    /// Implémentation de l'interface IHasherPassword utilisant l'algorithme de hachage BCrypt.
    /// Cette classe permet de hacher un mot de passe et de vérifier sa validité en le comparant à un hash stocké.
    /// </summary>
    public class BCryptPasswordHasher : IHasherPassword
    {
        private const int WorkFactor = 12;

        /// <inheritdoc/>
        public string Hash(string password)
        {
           return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WorkFactor);
        }

        /// <inheritdoc/>
        public bool Verify(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
