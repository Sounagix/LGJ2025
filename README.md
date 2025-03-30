Always use Unity’s official .gitignore
Avoid committing unnecessary folders like Library/, Temp/, .vs/, or Builds/.

Don’t commit large files or builds
Git should only track scripts, scenes, prefabs, assets, and configurations.
Use external services (Drive, Itch.io, etc.) to share builds.

BRANCHING
Never work directly on main
Always create branches like feature/inventory, fix/collision-bug, ui/main-menu.

Use clear, descriptive branch names
Example: feature/powerups, hotfix/fuel-bar.

Always pull before you push
To stay synced and avoid merge conflicts.

COMMITS
Commit often and in small, meaningful chunks
Don’t wait until 20 changes are done. Small commits are easier to track.

Write descriptive commit messages
❌ updated stuff
✅ Add projectile system and damage logic to enemies


SCENES & PREFABS
Warn teammates before editing scenes
Unity scenes can easily create merge conflicts. Communication helps.

Use prefab nesting and modularity
Split the scene into reusable prefabs (UI, HUD, enemies) so multiple people can work safely.

PULL REQUESTS (if using GitHub/GitLab)
Don’t push directly to main
Open pull requests for every new feature or fix.

Review your code before merging
Make sure it’s tested and doesn’t include unwanted files.

Run git status before every commit
Know exactly what you’re about to push.

Commit .meta files too
Unity uses them for asset references—don’t skip them.

Never commit local IDE folders or user settings
Ignore folders like .idea, .vs, Builds, UserSettings, and anything system-specific.