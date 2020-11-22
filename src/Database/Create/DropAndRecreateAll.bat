sqlcmd -E -S localhost\MSSQLSERVER01 -i ..\..\Database\Create\DropAndCreate.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\User.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\Ride.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\RideLocation.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\Jump.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\Trail.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\TrailLocation.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\TrailAttempt.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\AccelerometerReading.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\SpeedAchievement.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\JumpAchievement.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\DistanceAchievement.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\UserSpeedAchievement.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\UserJumpAchievement.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\UserDistanceAchievement.sql
sqlcmd -E -S localhost\MSSQLSERVER01 -d TrackedDev -i ..\..\Database\Create\Tables\TraceMessage.sql

cd /d "..\..\DataAccess"
dotnet-ef dbcontext scaffold "Server=localhost\MSSQLSERVER01;Database=TrackedDev;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models --context-dir Models -c ModelDataContext --force

pause