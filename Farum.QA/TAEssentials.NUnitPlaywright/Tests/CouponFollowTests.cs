using FluentAssertions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Threading.Tasks;

namespace TAEssentials.NUnitPlaywright.Tests
{
    public class CouponFollowTests : PageTest
    {
        IBrowser _browser;
        // Used only for screenshots for the last TC
        IPage _page;

        [SetUp]
        public async Task SetupAsync()
        {
            _browser = await (new BrowserBuilder()).GetBrowserDriver(NUnitPlaywright.Browser.Chrome, false, 0);
        }

        // Playwright is capable of launching variable viewports with predefined device names!
        [TestCase("Galaxy S5", TestName = "Today's Top Coupons section displays 3 or 6 or 9 Coupons - Galaxy S5")]
        [TestCase("Galaxy S8 landscape", TestName = "Today's Top Coupons section displays 3 or 6 or 9 Coupons - Galaxy S8 landscape")]
        [TestCase("Desktop Chrome HiDPI", TestName = "Today's Top Coupons section displays 3 or 6 or 9 Coupons - Desktop Chrome HiDPI")]
        public async Task TC_0001_Top_Deal(string deviceName)
        {
            await using var context = await _browser.NewContextAsync(Playwright.Devices[deviceName]);
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://couponfollow.com/");

            var test = await page.QuerySelectorAllAsync("xpath=//div[contains(@class,'top-deal')]");

            // Fluent Assertions library - allows for many, useful assertions
            test.Count.Should().BeOneOf(new int[3] {3,6,9 });

        }
        
        // Playwright is also capable of multibrowser testing!
        [TestCase(NUnitPlaywright.Browser.Chrome, TestName = "User is able to view 30 Today's Trending Coupons on the main page - Chrome")]
        [TestCase(NUnitPlaywright.Browser.Firefox, TestName = "User is able to view 30 Today's Trending Coupons on the main page - Firefox")]
        [TestCase(NUnitPlaywright.Browser.WebKit, TestName = "User is able to view 30 Today's Trending Coupons on the main page - WebKit")]
        [TestCase(NUnitPlaywright.Browser.Edge, TestName = "User is able to view 30 Today's Trending Coupons on the main page - Edge")]
        public async Task TC_0002_Todays_Trending_Coupons(Browser browser)
        {
            var localVariableTestbrowser = await (new BrowserBuilder()).GetBrowserDriver(browser, false, 100);

            var page = await localVariableTestbrowser.NewPageAsync();
            await page.GotoAsync("https://couponfollow.com/");

            var test = await page.QuerySelectorAllAsync("xpath=//article[@class='trending-offer']");

            test.Count.Should().BeGreaterThan(30);

        }

        // Playwright is also capable of testing various browsers with many pre-defined viewport configurations!
        // This simulates various mobile devices with various browsers!
        [TestCase("Desktop Edge HiDPI", NUnitPlaywright.Browser.Edge , TestName = "User is able to view Staff Picks, which contain unique stores with proper discounts for monetary, percentage or text values - Desktop with Edge")]
        [TestCase("iPhone 12", NUnitPlaywright.Browser.WebKit,TestName = "User is able to view Staff Picks, which contain unique stores with proper discounts for monetary, percentage or text values - iPhone with WebKit")]
        [TestCase("Galaxy S8", NUnitPlaywright.Browser.Chromium, TestName = "User is able to view Staff Picks, which contain unique stores with proper discounts for monetary, percentage or text values - Galaxy S8 with Chromium")]
        public async Task TC_0003_Staff_Picks(string deviceName, Browser browser)
        {
            var page = await (new DeviceBuilder()).GetDevice(deviceName, browser, false, 100);
            await page.GotoAsync("https://couponfollow.com/");

            var test = await page.QuerySelectorAllAsync("xpath=//div[@class='staff-pick']");

            foreach (var element in test)
            {
                (await element.TextContentAsync()).Should().MatchRegex(@"((Save )(\d|\d\d)(% Off))|((Take \$)(\d|\d\d)( Off))");
            }

        }

        // Playwright is also capable of PageObject patterns!
        // List of stores is taken from 'Stores' sub-page
        [TestCase("American Eagle Outfitters", "Coupon Codes", TestName = "User is able to open existing CouponFollow store - American Eagle Outfitters")]
        [TestCase("Chewy", "Promo Codes & Coupons", TestName = "User is able to open existing CouponFollow store - Chewy")]
        [TestCase("eBay", "Coupon & Promo Codes", TestName = "User is able to open existing CouponFollow store - eBay")]
        [TestCase("Michael Kors", "Coupon Codes", TestName = "User is able to open existing CouponFollow store - Michael Kors")]
        [TestCase("Target", "Coupons & Promo Codes", TestName = "User is able to open existing CouponFollow store - Target")]
        public async Task TC_0004_Store_Page(string storeName, string storeHeader)
        {
            var mainPage = new PageObjects.MainPage(await _browser.NewPageAsync());
            await mainPage.GotoAsync();
            // I've returned IPage here for more flexible way to get back to PlayWright PageDriver syntax/methods
            var page = await mainPage.SearchAsync(storeName);

            // What defines if 'page is properly displayed'?
            // I'm assuming that page is properly displayed when:
            // - it's header contains 'Coupons & Promo Codes'
            // - icon is displayed
            // - contains at least 1 coupon
            // - Target Coupon Stats container is displayed
            // - Rate Target container is displayed
            // - etc. (not covered)
            // Coded without the documentation; please, treat this as an example :)

            await Expect(page.Locator("xpath=//h1")).ToContainTextAsync($"{storeName} {storeHeader}");
            await Expect(page.Locator("xpath=//div[@class='logo']")).ToBeVisibleAsync();
            (await page.Locator("xpath=//article[contains(@class,'type-deal')]").CountAsync()).Should().BeGreaterThanOrEqualTo(1);
            await Expect(page.Locator($"xpath=//section/h2[text()='{storeName} Coupon Stats']")).ToBeVisibleAsync();
            await Expect(page.Locator($"xpath=//section/h2[text()='Rate {storeName}']")).ToBeVisibleAsync();
        }

        // Playwright is capable of capturing a screenshot!
        [TestCase(TestName = "Additional test - failing on purpose")]
        public async Task TC_0005_Failing_Test()
        {
            _page = await _browser.NewPageAsync();
            await _page.GotoAsync("https://couponfollow.com/");

            var test = await _page.QuerySelectorAllAsync("xpath=//div[contains(@class,'top-deal')]");

            // Fluent Assertions library - allows for many, useful assertions
            test.Count.Should().BeOneOf(new int[3] { 1,1, 1 });

        }


        [TearDown]
        public async Task TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                await _page.ScreenshotAsync(new PageScreenshotOptions()
                {
                    Path = "screenshot.png"
                });
            }
        }
    }
}