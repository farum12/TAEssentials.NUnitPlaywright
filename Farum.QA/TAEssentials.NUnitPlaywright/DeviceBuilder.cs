using Microsoft.Playwright;
using System.Threading.Tasks;

namespace TAEssentials.NUnitPlaywright
{
    /// <summary>
    /// Class intended to initalize any supported device with any supported browser.
    /// </summary>
    public class DeviceBuilder
    {
        /// <summary>
        /// Method wich returns IPage object, with given device options.
        /// </summary>
        /// <param name="deviceName">Device name from Playwright's supported devices.</param>
        /// <param name="browserType">Browser type from supported Browsers.</param>
        /// <param name="headless">Sets if the returned IBrowser will start in headless mode.</param>
        /// <param name="slowmo">Slow downs interactions execution.</param>
        public async Task<IPage> GetDevice(string deviceName, Browser browserType, bool? headless = true, float? slowmo = null)
        {
            var playwright = await Playwright.CreateAsync();
            BrowserBuilder builder = new BrowserBuilder();
            var browser = await builder.GetBrowserDriver(browserType, false, 100);
            var context = await browser.NewContextAsync(playwright.Devices[deviceName]);
            return await context.NewPageAsync();
        }
    }
}
