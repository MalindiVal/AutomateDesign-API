using LogicLayer;

namespace API.Data.Interfaces
{
    public interface ITransitionDAO
    {
        /// <summary>
        /// Insère les transitions liées à l'automate.
        /// </summary>
        /// <param name="automate"></param>
        /// <exception cref="DAOError"></exception>
        void InsertTransitions(Automate automate);

        /// <summary>
        /// Récupère les transitions liées à un automate à partir des lignes de la base de données.
        /// </summary>
        /// <param name="id">Automate avec ses propres informations mais sans ses transitions</param>
        /// <param name="etatDictionary">Dictionnaire d'états</param>
        List<Transition> GetTransitionsByAutomate(int id, Dictionary<int, Etat> etatDictionary);
    }
}
