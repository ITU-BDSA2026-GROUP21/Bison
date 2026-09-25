using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Testing;

public class ApiFixture : WebApplicationFactory<Program>
{
    public ApiFixture()
    {
        //Add environment variables for the test databases?
        Environment.SetEnvironmentVariable(
        "OBSERVATION_DB_PATH",
        Path.Combine(
            AppContext.BaseDirectory,
            "bison_observe_cli_db_test.csv"));

        Environment.SetEnvironmentVariable(
            "COMMENT_DB_PATH",
            Path.Combine(
                AppContext.BaseDirectory,
                "bison_comment_cli_db_test.csv"));

        Environment.SetEnvironmentVariable(
            "PROPOSAL_DB_PATH",
            Path.Combine(
                AppContext.BaseDirectory,
                "bison_proposal_cli_db.csv"));

        Environment.SetEnvironmentVariable(
            "TAXON_DB_PATH",
            Path.Combine(
                AppContext.BaseDirectory,
                "joined.csv"));
    }
}