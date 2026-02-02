using API.Data.Interfaces;
using API.Services.Interfaces;
using LogicLayer;

namespace API.Services.Realisations
{
    /// <summary>
    /// Implémentation de l'interface <see cref="IUtilisateurService"/>
    /// </summary>
    public class UtilisateurService : IUtilisateurService
    {

        private IUtilisateurDAO dao;
        private IHasherPassword hasherPassword;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="dao">Objet permettant d'accéder à la base de données</param>
        /// <param name="hasherPassword">Objet s'occupant du hashage de mot de passe</param>
        public UtilisateurService(IUtilisateurDAO dao, IHasherPassword hasherPassword)
        {
            this.dao = dao;
            this.hasherPassword = hasherPassword;
        }

        /// <inheritdoc/>
        public Utilisateur Login(Utilisateur user)
        {
            Utilisateur? current = this.dao.GetUserByLogin(user.Login);
            if (current != null)
            {
                bool res = this.hasherPassword.Verify(user.Mdp, current.Mdp);
                if (res)
                {
                    user = current;
                }
            }
            user.Mdp = "";
            return user;
        }

        /// <inheritdoc/>
        public Utilisateur Register(Utilisateur user)
        {
            user.Mdp = this.hasherPassword.Hash(user.Mdp);
            user = dao.Register(user);
            return user;
        }
    }
}
