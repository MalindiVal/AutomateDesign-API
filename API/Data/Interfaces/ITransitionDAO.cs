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
    }
}
