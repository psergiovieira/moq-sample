cmd /c SonarScanner.MSBuild.exe begin /k:moq-sample-cit
cmd /c MSBuild.exe /t:Rebuild
cmd /c SonarScanner.MSBuild.exe end
