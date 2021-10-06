using DataAccess;

namespace Api.Analysers {
    public interface IRideAnalyser {
        void Analyse(Transaction transaction, int userId, int rideId);
    }
}
