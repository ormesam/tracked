using DataAccess.Models;
using Shared.Dtos;

namespace Api.Analysers {
    public interface IRideAnalyser {
        void Analyse(ModelDataContext context, int userId, RideDto ride);
    }
}
