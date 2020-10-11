﻿using System.Linq;
using DataAccess.Models;
using Shared.Dtos;

namespace Api.Analysers {
    public class JumpAnalyser : IAchievementAnalyser {
        public void Analyse(ModelDataContext context, int userId, RideDto ride) {
            var jumpAchievements = context.JumpAchievement
                .Select(row => new MinJumpAchievement {
                    JumpAchievementId = row.JumpAchievementId,
                    MinAirtime = row.MinAirtime,
                    Name = row.Name,
                })
                .ToArray();

            foreach (var jumpAchievement in jumpAchievements) {
                if (jumpAchievement.Check(ride)) {
                    UserJumpAchievement userJumpAchievement = new UserJumpAchievement {
                        RideId = ride.RideId.Value,
                        JumpAchievementId = jumpAchievement.JumpAchievementId,
                        UserId = userId,
                    };

                    context.UserJumpAchievement.Add(userJumpAchievement);
                    context.SaveChanges();
                }
            }
        }
    }
}