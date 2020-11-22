using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace sakura_usagi
{
    public class WebViewControl
    {
        public WebView2 WebControl { get; set; }
        public WebViewController WebViewController { get; set; }
        public SettingLoader Setting { get; set; }

        public WebViewControl(SettingLoader setting)
        {
            WebViewController = new WebViewController();
            WebViewController.ButtonGo.Click += ButtonGo_Click;
            WebControl = new WebView2();
            WebControl.Unloaded += This_Unloaded;
            WebControl.NavigationCompleted += Webcontrol_NavigationCompleted;
            this.Setting = setting;
        }

        public void InitWebView()
        {
            InitializeWebViewAsync(WebControl, Setting);
        }

        async void InitializeWebViewAsync(WebView2 webView, SettingLoader setting)
        {
            if (setting != null)
            {
                string userDataPath = setting.Settings.UserDataPath;
                if (!System.IO.Directory.Exists(userDataPath))
                    System.IO.Directory.CreateDirectory(userDataPath);

                string argment = null;
                if (!string.IsNullOrEmpty(setting.Settings.CachePath))
                    argment = $"--disk-cache-dir=\"{setting.Settings.CachePath}\"";

                CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions(argment);
                var environment = await CoreWebView2Environment.CreateAsync(userDataFolder: userDataPath, options: options);
                await webView.EnsureCoreWebView2Async(environment);
            }
            webView.Source = new Uri(setting?.Settings.HomePage ?? "https://www.google.com");

            WebViewController.sliderPan.ValueChanged += sliderPan_ValueChanged;
            WebViewController.sliderVolume.ValueChanged += sliderVolume_ValueChanged;
        }

        public void SetGrid(uint x, uint y)
        {
            Grid.SetColumn(WebControl, (int)x);
            Grid.SetColumn(WebViewController, (int)x);
            Grid.SetRow(WebControl, (int)y);
            Grid.SetRow(WebViewController, (int)y);
        }

        private void sliderPan_ValueChanged(object sender, EventArgs e)
        {
            setPanAndVolume((float)(WebViewController.sliderVolume.Value / 100), (float)(WebViewController.sliderPan.Value / 100));
        }

        private void sliderVolume_ValueChanged(object sender, EventArgs e)
        {
            setPanAndVolume((float)(WebViewController.sliderVolume.Value / 100), (float)(WebViewController.sliderPan.Value / 100));
        }


        private void Webcontrol_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            setPanAndVolume((float)(WebViewController.sliderVolume.Value / 100), (float)(WebViewController.sliderPan.Value / 100));
        }

        private void ButtonGo_Click(object sender, EventArgs e)
        {
            if (IsUrl(WebViewController.TextBoxAddress.Text))
            {
                WebControl.CoreWebView2.Navigate(WebViewController.TextBoxAddress.Text);
            }
        }

        private void This_Unloaded(object sender, EventArgs e)
        {
            WebControl.Dispose();
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


            await WebControl.CoreWebView2.ExecuteScriptAsync(command);

            string command2 =
                // ボリュームとパン操作
                @"audioGainNode.gain.value =" + volume.ToString("0.00") + @";" +
                @"audioPanNode.pan.setValueAtTime(" + pan.ToString("0.00") + @", audioCtx.currentTime);";

            await WebControl.CoreWebView2.ExecuteScriptAsync(command2);
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
