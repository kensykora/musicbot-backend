if "%APPVEYOR_REPO_TAG%" == "true" (
  if "%APPVEYOR_REPO_TAG_NAME%" == "prod" (  
    echo "Deploying to Production";
    powershell "./src/arm/Deploy-AzureResourceGroup.ps1 -ResourceGroupName '%AZ_RG_PROD%' -ResourceGroupLocation '%AZ_LOCATION%' -OtherParams @{ resourcePrefix = '%AZ_RG_PROD%'; branch = '%PROD_BRANCH_NAME%'; includeSourceControlDeployment = 'no' } -UploadArtifacts -StorageAccountName '%AZ_ARM_STORAGE_ACCOUNT_NAME%'"
    git checkout .;
    git checkout %PROD_BRANCH_NAME%;
    git merge master;
    git push origin %PROD_BRANCH_NAME%;
    powershell "./src/arm/Update-VersionSetting.ps1 -ResourceGroupName '%AZ_RG_PROD%' -AppServiceName '%AZ_RG_PROD%-APP' -VersionNumber '%VERSION%%APPVEYOR_BUILD_NUMBER%'
  )
) else (
  if "%APPVEYOR_REPO_TAG%" == "false" ( 
    echo "Deploying to Dev";
    powershell "./src/arm/Deploy-AzureResourceGroup.ps1 -ResourceGroupName '%AZ_RG_DEV%' -ResourceGroupLocation '%AZ_LOCATION%' -OtherParams @{ resourcePrefix = '%AZ_RG_DEV%'; branch = '%DEV_BRANCH_NAME%'; includeSourceControlDeployment = 'no' } -UploadArtifacts -StorageAccountName '%AZ_ARM_STORAGE_ACCOUNT_NAME%'
    git checkout .;
    git checkout %DEV_BRANCH_NAME%;
    git merge master;
    git push origin %DEV_BRANCH_NAME%;
    powershell "./src/arm/Update-VersionSetting.ps1 -ResourceGroupName '%AZ_RG_DEV%' -AppServiceName '%AZ_RG_DEV%-APP' -VersionNumber '%VERSION%%APPVEYOR_BUILD_NUMBER%'
  ) else ( 
    echo "Non-Prod tagged commit, skipping deployment.";
  )
)