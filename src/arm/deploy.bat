if "%APPVEYOR_REPO_BRANCH%" == "%PROD_BRANCH_NAME%" (
  if "%APPVEYOR_PULL_REQUEST_NUMBER%" == "" (  
    echo "Deploying to Production";
    git checkout .
    git checkout %PROD_BRANCH_NAME%
    git merge master
    git push origin %PROD_BRANCH_NAME%
  )
) else (
  if "%APPVEYOR_REPO_BRANCH%" == "%DEV_BRANCH_NAME%" ( 
    if "%APPVEYOR_PULL_REQUEST_NUMBER%" == "" ( 
      echo "Deploying to Dev";
      git checkout .
      git checkout %DEV_BRANCH_NAME%
      git merge master
      git push origin %DEV_BRANCH_NAME%
    ) else ( 
      echo "Non-Prod tagged commit, skipping deployment."
    )
  )
)