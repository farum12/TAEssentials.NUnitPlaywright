using System.Threading.Tasks;
using Microsoft.Playwright;

namespace TAEssentials.NUnitPlaywright.PageObjects
{
    public class MainPage
    {
        private readonly IPage _page;
        private readonly ILocator _searchBoxInput;

        public MainPage(IPage page)
        {
            _page = page;

            // Bake Locators
            _searchBoxInput = page.Locator("xpath=//input[@class='search-field']");
        }

        public async Task GotoAsync()
        {
            await _page.GotoAsync("https://couponfollow.com/");
        }

        public async Task<IPage> SearchAsync(string text)
        {
            await _searchBoxInput.TypeAsync(text);
            await _page.Locator($"xpath=//a[@class='suggestion-item' and @data-sitename='{text}']").ClickAsync();
            return _page;
        }
    }
}
