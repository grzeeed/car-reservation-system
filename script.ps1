# Przejdź do repo
cd C:\sources\car-reservation-system

# Usuń z git cache (ale zostaw pliki lokalnie)
git rm -r --cached bin/
git rm -r --cached obj/
git rm -r --cached .vs/
git rm -r --cached frontend/node_modules/
git rm --cached .git/index
git rm --cached .git/logs/HEAD
git rm --cached .git/logs/refs/heads/feature/frontend-backend-integration
git rm --cached .git/refs/heads/feature/frontend-backend-integration
git rm --cached .git/sourcetreeconfig.json