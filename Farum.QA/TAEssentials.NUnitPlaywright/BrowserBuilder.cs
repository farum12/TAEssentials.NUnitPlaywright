using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace TAEssentials.NUnitPlaywright
{
    /// <summary>
    /// Class intended to initalize any supported web browser.
    /// </summary>
    public class BrowserBuilder
    {
        /// <summary>
        /// Method wich returns desired IBrowser with given BrowserTypeLaunchOptions.
        /// </summary>
        /// <param name="browser">Browser type from supported Browsers.</param>
        /// <param name="headless">Sets if the returned IBrowser will start in headless mode.</param>
        /// <param name="slowmo">Slow downs interactions execution.</param>
        public async Task<IBrowser> GetBrowserDriver(Browser browser, bool? headless = true, float? slowmo = null)
        {
            return browser switch
            {
                Browser.Chrome => await GetChromeDriverAsync(headless,slowmo),
                Browser.Firefox => await GetFirefoxDriverAsync(headless, slowmo),
                Browser.Edge => await GetEdgeDriverAsync(headless, slowmo),
                Browser.WebKit => await GetWebKitDriverAsync(headless, slowmo),
                Browser.Chromium => await GetChromiumDriverAsync(headless, slowmo),
                _ => throw new NotImplementedException($"Support for browser {browser} is not implemented yet!"),
            };
        }
        private async Task<IBrowser> GetChromeDriverAsync(bool? headless = true, float? slowmo = null)
        {
            var playwright = await Playwright.CreateAsync();
            return await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = headless,
                SlowMo = slowmo,
                Channel = "chrome",
            });
        }

        private async Task<IBrowser> GetChromiumDriverAsync(bool? headless = true, float? slowmo = null)
        {
            var playwright = await Playwright.CreateAsync();
            return await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = headless,
                SlowMo = slowmo
            });
        }

        private async Task<IBrowser> GetEdgeDriverAsync(bool? headless = true, float? slowmo = null)
        {
            var playwright = await Playwright.CreateAsync();
            return await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = headless,
                SlowMo = slowmo,
                Channel = "msedge",
            });
        }

        private async Task<IBrowser> GetWebKitDriverAsync(bool? headless = true, float? slowmo = null)
        {
            var playwright = await Playwright.CreateAsync();
            return await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = headless,
                SlowMo = slowmo
            });
        }

        private async Task<IBrowser> GetFirefoxDriverAsync(bool? headless = true, float? slowmo = null)
        {
            var playwright = await Playwright.CreateAsync();
            return await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = headless,
                SlowMo = slowmo
            });
        }
    }
}
