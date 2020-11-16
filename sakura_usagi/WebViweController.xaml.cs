
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Windows.Controls;
using sakura_usagi.Properties;
using System.Text.RegularExpressions;

namespace sakura_usagi
{
    /// <summary>
    /// WebViweController.xaml の相互作用ロジック
    /// </summary>
    public partial class WebViweController : UserControl
    {
        public WebView2 webcontrol;
        public SettingLoader setting;

        public WebViweController(SettingLoader setting)
        {
            InitializeComponent();
            webcontrol = new WebView2();
            this.setting = setting;
        }

        public void InitWebView()
        {
            InitializeWebViewAsync(webcontrol, setting);
        }

        async void InitializeWebViewAsync(WebView2 webView, SettingLoader setting)
        {
            if (setting != null)
            {
                string userDataPath = setting.UserDataPath;
                if (!System.IO.Directory.Exists(userDataPath))
                    System.IO.Directory.CreateDirectory(userDataPath);

                string argment = null;
                if (!string.IsNullOrEmpty(setting.CachePath))
                    argment = $"--disk-cache-dir=\"{setting.CachePath}\"";

                CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions(argment);
                var environment = await CoreWebView2Environment.CreateAsync(userDataFolder: userDataPath, options: options);
                await webView.EnsureCoreWebView2Async(environment);
            }
            webView.Source = new Uri(setting?.HomePage ?? "https://www.google.com");
        }

        private void ButtonGo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsUrl(TextBoxAddress.Text))
            {
                webcontrol.CoreWebView2.Navigate(TextBoxAddress.Text);
            }
        }

        public static bool IsUrl(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(
               input,
               @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"
            );
        }
    }
}
