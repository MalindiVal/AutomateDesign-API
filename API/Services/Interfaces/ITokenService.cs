using LogicLayer;

namespace API.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Génère un token JWT pour un utilisateur authentifié
        /// </summary>
        /// <param name="user">Utilisateur authentifié</param>
        /// <returns>Token JWT</returns>
        string GenerateToken(Utilisateur user);
    }
}
