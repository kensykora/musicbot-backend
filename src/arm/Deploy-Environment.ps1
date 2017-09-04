param(
    [string] [Parameter(Mandatory=$true)]  $ResourceGroupName,
    [string] [Parameter(Mandatory=$true)]  $ResourceGroupLocation,
    [string] [Parameter(Mandatory=$true)]  $ResourcePrefix,
    [string] [Parameter(Mandatory=$true)]  $Branch,
    [string] [Parameter(Mandatory=$true)]  $Version
)

./src/arm/Deploy-AzureResourceGroup.ps1 -ResourceGroupName "$ResourceGroupName" -ResourceGroupLocation $ResourceGroupLocation -OtherParams @{ resourcePrefix = $ResourcePrefix; branch = $Branch; includeSourceControlDeployment = "no" } -UploadArtifacts
git checkout .
git checkout $Branch
git merge master
git push origin $Branch
./src/arm/Update-VersionSetting.ps1 -ResourceGroupName "$ResourceGroupName" -AppServiceName "$ResourcePrefix-APP" -VersionNumber $Version