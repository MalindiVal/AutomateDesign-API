using LogicLayer;

namespace API.Data.Interfaces
{
    public interface IEtatDAO
    {
        void InsertEtats(Automate automate);

        public List<Etat> GetEtatsByAutomate(int id);


    }
}
