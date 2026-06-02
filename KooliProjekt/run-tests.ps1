# run-tests.ps1
param(
    [string]$TestProject = "KooliProjekt",
    [string]$CoverageOutput = "TestResults/Coverage",
    [string]$ReportOutput = "CoverageReport"
)

Write-Host "Running tests with coverage analysis..." -ForegroundColor Green

# Eemalda vanad tulemused
if (Test-Path $CoverageOutput) {
    Remove-Item -Recurse -Force $CoverageOutput
}
if (Test-Path $ReportOutput) {
    Remove-Item -Recurse -Force $ReportOutput
}

# Käivita testid coverage'iga
dotnet test $TestProject `
    --collect:"XPlat Code Coverage" `
    --results-directory $CoverageOutput `
    --settings coverlet.settings `
    --verbosity normal

# Leia coverage fail
$coverageFile = Get-ChildItem -Path $CoverageOutput -Recurse -Filter "coverage.cobertura.xml" | Select-Object -First 1

if ($coverageFile) {
    Write-Host "Coverage file found: $($coverageFile.FullName)" -ForegroundColor Green
    
    # Genereeri HTML report
    reportgenerator `
        -reports:$($coverageFile.FullName) `
        -targetdir:$ReportOutput `
        -reporttypes:Html
    
    Write-Host "Coverage report generated in: $ReportOutput" -ForegroundColor Green
    Write-Host "Open: $ReportOutput\index.html to view the report" -ForegroundColor Yellow
    
    # Kuva kokkuvõte
    [xml]$coverage = Get-Content $coverageFile.FullName
    $lineCoverage = $coverage.coverage.'line-rate'
    $branchCoverage = $coverage.coverage.'branch-rate'
    
    Write-Host "`nCoverage Summary:" -ForegroundColor Cyan
    Write-Host "Line Coverage: $([Math]::Round($lineCoverage * 100, 2))%" -ForegroundColor $(if($lineCoverage -ge 0.8){"Green"}else{"Red"})
    Write-Host "Branch Coverage: $([Math]::Round($branchCoverage * 100, 2))%" -ForegroundColor $(if($branchCoverage -ge 0.8){"Green"}else{"Red"})
} else {
    Write-Host "Coverage file not found!" -ForegroundColor Red
}