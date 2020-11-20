
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Windows.Controls;
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
            webcontrol.Unloaded += This_Unloaded;
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

            sliderPan.ValueChanged += sliderPan_ValueChanged;
            sliderVolume.ValueChanged += sliderVolume_ValueChanged;
        }

        private void ButtonGo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsUrl(TextBoxAddress.Text))
            {
                webcontrol.CoreWebView2.Navigate(TextBoxAddress.Text);
            }
        }

        private void This_Unloaded(object sender, EventArgs e)
        {
            webcontrol.Dispose();
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

        private void sliderPan_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            setPanAndVolume((float)(sliderVolume.Value / 100), (float)(sliderPan.Value / 100));
        }

        private void sliderVolume_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            setPanAndVolume((float)(sliderVolume.Value / 100), (float)(sliderPan.Value / 100));
        }

        private async void setPanAndVolume(float volume, float pan)
        {
            string command =

                @"const video = document.querySelector('video');" +
                @"const audioCtx = new (window.AudioContext)();" +
                @"const audioSource = audioCtx.createMediaElementSource(video);" +
                @"const audioGainNode = audioCtx.createGain();" +
                @"const audioPanNode = audioCtx.createStereoPanner();" +
                @"audioSource.connect(audioGainNode);" +
                @"audioGainNode.connect(audioPanNode);" +
                @"audioPanNode.connect(audioCtx.destination);";

            await webcontrol.CoreWebView2.ExecuteScriptAsync(command);

            string command2 =
                // ボリュームとパン操作
                @"audioGainNode.gain.value =" + volume.ToString("0.00") +@";"+
                @"audioPanNode.pan.setValueAtTime("+ pan.ToString("0.00") +@", audioCtx.currentTime);";

            await webcontrol.CoreWebView2.ExecuteScriptAsync(command2);
        }
    }
}
