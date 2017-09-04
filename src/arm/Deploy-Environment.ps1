param(
    [string] [Parameter(Mandatory=$true)]  $ResourceGroupName,
    [string] [Parameter(Mandatory=$true)]  $ResourceGroupLocation,
    [string] [Parameter(Mandatory=$true)]  $ResourcePrefix,
    [string] [Parameter(Mandatory=$true)]  $Branch,
    [string] [Parameter(Mandatory=$true)]  $Version,
    [string] [Parameter(Mandatory=$true)]  $StorageAccountName
)
./src/arm/Deploy-AzureResourceGroup.ps1 -ResourceGroupName "$ResourceGroupName" -ResourceGroupLocation $ResourceGroupLocation -OtherParams @{ resourcePrefix = $ResourcePrefix; branch = $Branch; includeSourceControlDeployment = "no" } -UploadArtifacts -StorageAccountName $StorageAccountName
cmd /c git checkout . 2>&1
cmd /c git checkout $Branch 2>&1
cmd /c git merge master 2>&1
cmd /c git push origin $Branch 2>&1
./src/arm/Update-VersionSetting.ps1 -ResourceGroupName "$ResourceGroupName" -AppServiceName "$ResourcePrefix-APP" -VersionNumber $Version