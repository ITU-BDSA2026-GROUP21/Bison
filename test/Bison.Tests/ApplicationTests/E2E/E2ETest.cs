using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Bison.Tests.ApplicationTests.E2E;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class E2ETest : PageTest
{


    [Test]
    public async Task PageTitle()
    {
        await Page.GotoAsync($"{BisonFixture.BaseUrl}/obs");
        await Expect(Page).ToHaveTitleAsync(new Regex("Bison"));

    }
}