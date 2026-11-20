using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Text.RegularExpressions;

using var playwright = await Playwright.CreateAsync();


[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class E2ETest : PageTest
{
    [Test]
    public async Task PageTitle()
    {
        await Page.GotoAsync("http://localhost:5273/obs");
        await Expect(Page).ToHaveTitleAsync(new Regex("Bison"));

    }
}