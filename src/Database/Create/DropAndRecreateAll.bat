set server=localhost\MSSQLSERVER01
set database=TrackedDev
set filePath=..\..\Database\Create

sqlcmd -E -S %server% -i %filePath%\DropAndCreate.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\User.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\Ride.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\RideLocation.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\Jump.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\Trail.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\TrailLocation.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\TrailAttempt.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\AccelerometerReading.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\SpeedAchievement.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\JumpAchievement.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\DistanceAchievement.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\UserSpeedAchievement.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\UserJumpAchievement.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\UserDistanceAchievement.sql
sqlcmd -E -S %server% -d %database% -i %filePath%\Tables\TraceMessage.sql

cd /d "..\..\DataAccess"
dotnet-ef dbcontext scaffold "Server=%server%;Database=%database%;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models --context-dir Models -c ModelDataContext --force --data-annotations --no-onconfiguring

pause