param(
    [string] [Parameter(Mandatory=$true)]  $ResourceGroupName,
    [string] [Parameter(Mandatory=$true)]  $ResourceGroupLocation,
    [string] [Parameter(Mandatory=$true)]  $ResourcePrefix,
    [string] [Parameter(Mandatory=$true)]  $Branch,
    [string] [Parameter(Mandatory=$true)]  $Version,
    [string] [Parameter(Mandatory=$true)]  $StorageAccountName
)
$ErrorActionPreference = 'Stop'
./src/arm/Deploy-AzureResourceGroup.ps1 -ResourceGroupName "$ResourceGroupName" -ResourceGroupLocation $ResourceGroupLocation -OtherParams @{ resourcePrefix = $ResourcePrefix; branch = $Branch; includeSourceControlDeployment = "no" } -UploadArtifacts -StorageAccountName $StorageAccountName
git checkout .
git checkout $Branch 2>&1
git merge master
git push origin $Branch 2>&1
./src/arm/Update-VersionSetting.ps1 -ResourceGroupName "$ResourceGroupName" -AppServiceName "$ResourcePrefix-APP" -VersionNumber $Version