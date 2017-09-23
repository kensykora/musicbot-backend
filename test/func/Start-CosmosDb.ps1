Set-Content -Value '"C:\Program Files\Azure Cosmos DB Emulator\CosmosDB.Emulator.exe" /NoUI /NoExplorer /NoFirewall' -Path .\startCosmosDb.cmd
Start-Process -FilePath .\startCosmosDb.cmd

$attempt = 0
$max = 3
while(!$client.Connected -and $attempt -lt $max) {
  try {    
    $client = New-Object System.Net.Sockets.TcpClient([System.Net.Sockets.AddressFamily]::InterNetwork)
    $attempt++; $client.Connect("127.0.0.1", 8081); write-host "CosmosDB started"
  }
  catch {    
    if($attempt -eq $max) {
      write-host "CosmosDB was not started"; $client.Close(); return
      }  
    [int]$sleepTime = 5*$attempt
    write-host "CosmosDB is not started. Retry after $sleepTime seconds..."
    sleep $sleepTime;
    $client.Close()        
  }  
}