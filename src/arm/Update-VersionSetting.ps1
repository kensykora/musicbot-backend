#Requires -Version 3.0
#Requires -Module AzureRM.Resources

param(
    [string] [Parameter(Mandatory=$true)]  $ResourceGroupName,
    [string] [Parameter(Mandatory=$true)]  $AppServiceName,
    [string] [Parameter(Mandatory=$true)]  $VersionNumber
)

$webApp = Get-AzureRMWebApp -ResourceGroupName $ResourceGroupName -Name $AppServiceName
$appSettingList = $webApp.SiteConfig.AppSettings

$hash = @{}
ForEach ($kvp in $appSettingList) {
    $hash[$kvp.Name] = $kvp.Value
}

$hash['MusicBotVersion'] = $VersionNumber

Set-AzureRMWebApp -ResourceGroupName $ResourceGroupName -Name $AppServiceName -AppSettings $hash