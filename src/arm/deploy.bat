if "%APPVEYOR_REPO_TAG%" == "true" (
  if "%APPVEYOR_REPO_TAG_NAME%" == "prod" (  
    echo "Deploying to Production";
    git checkout .
    git checkout %PROD_BRANCH_NAME%
    git merge master
    git push origin %PROD_BRANCH_NAME%
  )
) else (
  if "%APPVEYOR_REPO_TAG%" == "false" ( 
    echo "Deploying to Dev";
    git checkout .
    git checkout %DEV_BRANCH_NAME%
    git merge master
    git push origin %DEV_BRANCH_NAME%
  ) else ( 
    echo "Non-Prod tagged commit, skipping deployment."
  )
)