# Publishing EasyConsoleAsync Package with GitHub Actions

Follow these steps to publish a new package version to GitHub Packages and create a release using the automated workflow:

1. **Update the Changelog**
   - Edit [`CHANGELOG.md`](CHANGELOG.md) and add release notes for the new version at the top, under a new `##` heading (e.g., `## 1.2.3`).

2. **Commit Your Changes**
   - Commit all changes, including code updates and the changelog, to the `master` branch (or your main branch).

3. **Tag the Release**
   - Create a new Git tag following the format `vX.Y.Z` (e.g., `v1.2.3`).
   - Example command:
     ```sh
     git tag v1.2.3
     git push origin v1.2.3
     ```

4. **GitHub Actions Workflow**
   - Pushing the tag triggers the `Publish Lib + Create Release` workflow automatically.
   - The workflow will:
     - Check out the code
     - Set up .NET 10
     - Determine the version from the tag
     - Extract the latest changelog notes into `release-notes.md`
     - Add the GitHub NuGet package source
     - Restore dependencies
     - Build the project in Release mode
     - Pack the NuGet package
     - Publish the package to GitHub Packages
     - Create a GitHub Release and attach the package as an asset, using the extracted release notes

5. **Check the Results**
   - After the workflow completes, verify:
     - The package is published to GitHub Packages
     - A new GitHub Release is created with the correct version and release notes

---

**Notes:**
- The workflow only runs for tags matching the pattern `v*.*.*`.
- Make sure your changelog is up to date before tagging.
- The file `release-notes.md` is generated automatically and should not be edited manually.
