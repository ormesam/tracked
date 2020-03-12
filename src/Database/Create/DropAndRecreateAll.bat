sqlcmd -E -S localhost -i ..\..\Database\Create\DropAndCreate.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\User.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\Ride.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\RideLocation.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\Jump.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\Segment.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\SegmentLocation.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\SegmentAttempt.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\SegmentAttemptLocation.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\SegmentAttemptJump.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\AccelerometerReading.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\SpeedAchievement.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\JumpAchievement.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\UserSpeedAchievement.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\UserJumpAchievement.sql
sqlcmd -E -S localhost -d TrackedDev -i ..\..\Database\Create\Tables\TraceMessage.sql

cd /d "..\..\DataAccess"
dotnet-ef dbcontext scaffold "Server=localhost;Database=TrackedDev;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models --context-dir Models -c ModelDataContext --force

pause