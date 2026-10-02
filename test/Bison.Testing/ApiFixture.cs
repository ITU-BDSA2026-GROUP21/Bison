namespace Bison.Testing;

using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Testing;

public class ApiFixture : WebApplicationFactory<Program>
{
    public ApiFixture()
    {
        //Add environment variables for the test databases?
        Environment.SetEnvironmentVariable(
            "BISONDBPATH",
            Path.Combine(
                AppContext.BaseDirectory,
                "bison.db"
            )
        );
    }
}