param ($MigrationName)
if ($null -eq $MigrationName){
    Write-Output "Migration name is empty" 
} else{
    $Env:ASPNETCORE_ENVIRONMENT = "Migration"
    dotnet ef migrations add $MigrationName --project=../BookingSystem.Persistence --context BSDbContext
}

