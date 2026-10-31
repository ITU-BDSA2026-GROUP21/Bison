# Bison

Bison application for BDSA course at the IT University of Copenhagen.

Developed by Group 21, autumn 2026.

## Internal instructions for running on a fresh clone

- If you already have a Bison.db file in Bison.Razor, delete it, to make sure the database follows the latest migration.
- Make sure you have Z3 installed. Instructions are on learnit. (Week 6 on the BDSA course page.)
- Make sure to run, FROM THE REPO ROOT, "dotnet tool restore" to make sure that Dafny is available as a tool
- Make sure to run, FROM THE REPO ROOT, "dotnet dafny translate cs --include-runtime --output src/Bison.Infrastructure/Dafny/Generated/TaxonFilter src/Bison.Infrastructure/Dafny/TaxonFilter.dfy" to make sure that the dafny code is translated on your machine, as it isn't tracked by git.
- Make sure to set the user secrets for Eduard and Debasmita's Emails and Passwords:
  - In Bison.Razor, run this command 4 times: dotnet user-secrets set "KEY" "VALUE"
  - The 4 keys are:
    1. "Seeding:Eduard:Email"
    2. "Seeding:Eduard:Password"
    3. "Seeding:Debasmita:Email"
    4. "Seeding:Debasmita:Password"
  - The values for these 4 keys can be found in section 1.a of the week 10 project guide.

## Azure deployment guide

For the team: If you want to manually deploy our application on your own azure account, run these commands from Bison.Razor

- Config deploy building tools "az webapp config appsettings delete -g our-resource-group -n our-app-name --setting-names SCM_DO_BUILD_DURING_DEPLOYMENT"
- Deployment command "az webapp deploy -g our-resource-group -n our-app-name --src-path ../../app.zip --type zip"
- The Azure
