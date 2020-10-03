using DataAccess.Models;
using Shared.Dtos;

namespace Api.Analysers {
    public interface IAchievementAnalyser {
        void Analyse(ModelDataContext context, int userId, RideDto ride);
    }
}
