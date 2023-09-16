using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DistanceAchievements",
                columns: table => new
                {
                    DistanceAchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MinDistanceMiles = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistanceAchievements", x => x.DistanceAchievementId);
                });

            migrationBuilder.CreateTable(
                name: "JumpAchievements",
                columns: table => new
                {
                    JumpAchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MinAirtime = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JumpAchievements", x => x.JumpAchievementId);
                });

            migrationBuilder.CreateTable(
                name: "SpeedAchievements",
                columns: table => new
                {
                    SpeedAchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MinMph = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeedAchievements", x => x.SpeedAchievementId);
                });

            migrationBuilder.CreateTable(
                name: "TraceMessages",
                columns: table => new
                {
                    TraceMessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraceMessages", x => x.TraceMessageId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoogleUserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    RideId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StartUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DistanceMiles = table.Column<double>(type: "float", nullable: false),
                    MaxSpeedMph = table.Column<double>(type: "float", nullable: false),
                    AverageSpeedMph = table.Column<double>(type: "float", nullable: false),
                    RouteSvgPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnalyserVersion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.RideId);
                    table.ForeignKey(
                        name: "FK_Rides_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trails",
                columns: table => new
                {
                    TrailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trails", x => x.TrailId);
                    table.ForeignKey(
                        name: "FK_Trails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBlocks",
                columns: table => new
                {
                    UserBlockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BlockUserId = table.Column<int>(type: "int", nullable: false),
                    BlockedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlocks", x => x.UserBlockId);
                    table.ForeignKey(
                        name: "FK_UserBlocks_Users_BlockUserId",
                        column: x => x.BlockUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBlocks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFollows",
                columns: table => new
                {
                    UserFollowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FollowUserId = table.Column<int>(type: "int", nullable: false),
                    FollowedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollows", x => x.UserFollowId);
                    table.ForeignKey(
                        name: "FK_UserFollows_Users_FollowUserId",
                        column: x => x.FollowUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFollows_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccelerometerReadings",
                columns: table => new
                {
                    AccelerometerReadingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RideId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false),
                    Z = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccelerometerReadings", x => x.AccelerometerReadingId);
                    table.ForeignKey(
                        name: "FK_AccelerometerReadings_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "RideId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jumps",
                columns: table => new
                {
                    JumpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RideId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Airtime = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jumps", x => x.JumpId);
                    table.ForeignKey(
                        name: "FK_Jumps_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "RideId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RideLocations",
                columns: table => new
                {
                    RideLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RideId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    AccuracyInMetres = table.Column<double>(type: "float", nullable: false),
                    Mph = table.Column<double>(type: "float", nullable: false),
                    Altitude = table.Column<double>(type: "float", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemovalReason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideLocations", x => x.RideLocationId);
                    table.ForeignKey(
                        name: "FK_RideLocations_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "RideId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDistanceAchievements",
                columns: table => new
                {
                    UserDistanceAchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DistanceAchievementId = table.Column<int>(type: "int", nullable: false),
                    RideId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDistanceAchievements", x => x.UserDistanceAchievementId);
                    table.ForeignKey(
                        name: "FK_UserDistanceAchievements_DistanceAchievements_DistanceAchievementId",
                        column: x => x.DistanceAchievementId,
                        principalTable: "DistanceAchievements",
                        principalColumn: "DistanceAchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDistanceAchievements_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "RideId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDistanceAchievements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserJumpAchievements",
                columns: table => new
                {
                    UserJumpAchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JumpAchievementId = table.Column<int>(type: "int", nullable: false),
                    RideId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJumpAchievements", x => x.UserJumpAchievementId);
                    table.ForeignKey(
                        name: "FK_UserJumpAchievements_JumpAchievements_JumpAchievementId",
                        column: x => x.JumpAchievementId,
                        principalTable: "JumpAchievements",
                        principalColumn: "JumpAchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserJumpAchievements_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "RideId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserJumpAchievements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSpeedAchievements",
                columns: table => new
                {
                    UserSpeedAchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SpeedAchievementId = table.Column<int>(type: "int", nullable: false),
                    RideId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSpeedAchievements", x => x.UserSpeedAchievementId);
                    table.ForeignKey(
                        name: "FK_UserSpeedAchievements_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "RideId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSpeedAchievements_SpeedAchievements_SpeedAchievementId",
                        column: x => x.SpeedAchievementId,
                        principalTable: "SpeedAchievements",
                        principalColumn: "SpeedAchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSpeedAchievements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrailAttempts",
                columns: table => new
                {
                    TrailAttemptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TrailId = table.Column<int>(type: "int", nullable: false),
                    RideId = table.Column<int>(type: "int", nullable: false),
                    StartUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Medal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailAttempts", x => x.TrailAttemptId);
                    table.ForeignKey(
                        name: "FK_TrailAttempts_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "RideId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrailAttempts_Trails_TrailId",
                        column: x => x.TrailId,
                        principalTable: "Trails",
                        principalColumn: "TrailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrailAttempts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrailLocations",
                columns: table => new
                {
                    TrailLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrailId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailLocations", x => x.TrailLocationId);
                    table.ForeignKey(
                        name: "FK_TrailLocations_Trails_TrailId",
                        column: x => x.TrailId,
                        principalTable: "Trails",
                        principalColumn: "TrailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccelerometerReadings_RideId",
                table: "AccelerometerReadings",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_Jumps_RideId",
                table: "Jumps",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_RideLocations_RideId",
                table: "RideLocations",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_UserId",
                table: "Rides",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrailAttempts_RideId",
                table: "TrailAttempts",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_TrailAttempts_TrailId",
                table: "TrailAttempts",
                column: "TrailId");

            migrationBuilder.CreateIndex(
                name: "IX_TrailAttempts_UserId",
                table: "TrailAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrailLocations_TrailId",
                table: "TrailLocations",
                column: "TrailId");

            migrationBuilder.CreateIndex(
                name: "IX_Trails_UserId",
                table: "Trails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlocks_BlockUserId",
                table: "UserBlocks",
                column: "BlockUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlocks_UserId",
                table: "UserBlocks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDistanceAchievements_DistanceAchievementId",
                table: "UserDistanceAchievements",
                column: "DistanceAchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDistanceAchievements_RideId",
                table: "UserDistanceAchievements",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDistanceAchievements_UserId",
                table: "UserDistanceAchievements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowUserId",
                table: "UserFollows",
                column: "FollowUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_UserId",
                table: "UserFollows",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJumpAchievements_JumpAchievementId",
                table: "UserJumpAchievements",
                column: "JumpAchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJumpAchievements_RideId",
                table: "UserJumpAchievements",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJumpAchievements_UserId",
                table: "UserJumpAchievements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpeedAchievements_RideId",
                table: "UserSpeedAchievements",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpeedAchievements_SpeedAchievementId",
                table: "UserSpeedAchievements",
                column: "SpeedAchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpeedAchievements_UserId",
                table: "UserSpeedAchievements",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccelerometerReadings");

            migrationBuilder.DropTable(
                name: "Jumps");

            migrationBuilder.DropTable(
                name: "RideLocations");

            migrationBuilder.DropTable(
                name: "TraceMessages");

            migrationBuilder.DropTable(
                name: "TrailAttempts");

            migrationBuilder.DropTable(
                name: "TrailLocations");

            migrationBuilder.DropTable(
                name: "UserBlocks");

            migrationBuilder.DropTable(
                name: "UserDistanceAchievements");

            migrationBuilder.DropTable(
                name: "UserFollows");

            migrationBuilder.DropTable(
                name: "UserJumpAchievements");

            migrationBuilder.DropTable(
                name: "UserSpeedAchievements");

            migrationBuilder.DropTable(
                name: "Trails");

            migrationBuilder.DropTable(
                name: "DistanceAchievements");

            migrationBuilder.DropTable(
                name: "JumpAchievements");

            migrationBuilder.DropTable(
                name: "Rides");

            migrationBuilder.DropTable(
                name: "SpeedAchievements");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
