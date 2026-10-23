# Bison

Bison application for BDSA course at IT University of Copenhagen

## Internal instructions for running on a fresh clone

- Make sure you have Z3 installed. Instructions are on learnit. (Week 6 on the BDSA course page.)
- Make sure to run, FROM THE REPO ROOT, "dotnet tool restore" to make sure that Dafny is available as a tool
- Make sure to run, FROM THE REPO ROOT, "dotnet dafny translate cs --include-runtime --output src/Bison.Infrastructure/Dafny/Generated/TaxonFilter src/Bison.Infrastructure/Dafny/TaxonFilter.dfy" to make sure that the dafny code is translated on your machine, as it isn't tracked by git.

## Azure deployment guide

- Config deploy building tools "az webapp config appsettings delete -g <our-resource-group> -n <our-app-name> --setting-names SCM_DO_BUILD_DURING_DEPLOYMENT"
- Deployment command "az webapp deploy -g <our-resource-group> -n <our-app-name> --src-path ../../app.zip --type zip"
