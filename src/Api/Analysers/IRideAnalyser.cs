using DataAccess.Models;

namespace Api.Analysers {
    public interface IRideAnalyser {
        void Analyse(ModelDataContext context, int userId, int rideId);
    }
}
