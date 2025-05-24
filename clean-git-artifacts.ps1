# Clean Git Artifacts - Car Reservation System
# This script removes build artifacts and temporary files from git tracking

Write-Host "🧹 Cleaning build artifacts from git..." -ForegroundColor Yellow

# Remove build artifacts from git cache
Write-Host "Removing .NET build artifacts..." -ForegroundColor Cyan
git rm -r --cached src/*/bin/ 2>$null
git rm -r --cached src/*/obj/ 2>$null
git rm -r --cached tests/*/bin/ 2>$null
git rm -r --cached tests/*/obj/ 2>$null

# Remove Visual Studio files
Write-Host "Removing Visual Studio artifacts..." -ForegroundColor Cyan
git rm -r --cached .vs/ 2>$null

# Remove Node.js artifacts
Write-Host "Removing Node.js artifacts..." -ForegroundColor Cyan
git rm -r --cached frontend/node_modules/ 2>$null

# Remove git internal files that got accidentally tracked
Write-Host "Removing git internal files..." -ForegroundColor Cyan
git rm --cached .git/index 2>$null
git rm --cached .git/logs/HEAD 2>$null
git rm --cached .git/logs/refs/heads/feature/frontend-backend-integration 2>$null
git rm --cached .git/refs/heads/feature/frontend-backend-integration 2>$null
git rm --cached .git/sourcetreeconfig.json 2>$null

# Clean up any other build artifacts
Write-Host "Removing other build artifacts..." -ForegroundColor Cyan
git rm --cached **/*.deps.json 2>$null
git rm --cached **/*.runtimeconfig.json 2>$null
git rm --cached **/*.AssemblyInfo.cs 2>$null
git rm --cached **/*.GlobalUsings.g.cs 2>$null
git rm --cached **/project.assets.json 2>$null
git rm --cached **/*.nuget.dgspec.json 2>$null

Write-Host "✅ Git cache cleaned!" -ForegroundColor Green
Write-Host "Now run: git status" -ForegroundColor Yellow
Write-Host "Then commit the cleanup: git commit -m 'chore: remove build artifacts from git tracking'" -ForegroundColor Yellow
