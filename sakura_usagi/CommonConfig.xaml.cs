using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace sakura_usagi
{
    /// <summary>
    /// CommonConfig.xaml の相互作用ロジック
    /// </summary>
    public partial class CommonConfig : Window
    {
        private SettingLoader Setting;

        public CommonConfig(SettingLoader setting)
        {
            InitializeComponent();
            Setting = setting;
            ButtonAddFav.Click += ButtonAddFav_Click;
            ButtonDeleteFav.Click += ButtonDeleteFav_Click;
            ButtonEditFav.Click += ButtonEditFav_Click;
            DataGridFav.SelectedCellsChanged += DataGridFav_SelectedCellsChanged;
            VersionLabel.Content = "Version: " + ProgramInfo.VERSION_STRING;
            DataGridFav.ItemsSource = Setting.Settings.Favorites.ToList();
        }

        private void ButtonEditFav_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridFav.SelectedItems.Count > 1)
            {
                return;
            }

            if (Setting.Settings.Favorites.TryGetValue(TextBoxTitle.Text, out _) || String.IsNullOrEmpty(TextBoxTitle.Text))
            {
                return;
            }

            foreach (var d in DataGridFav.SelectedItems)
            {
                Setting.Settings.Favorites.Remove(((KeyValuePair<string, string>)d).Key);
            }

            Setting.Settings.Favorites.Add(TextBoxTitle.Text, TextBoxURL.Text);
            Setting.SaveSetting();
            DataGridFav.ItemsSource = null;
            DataGridFav.Items.Clear();
            DataGridFav.ItemsSource = Setting.Settings.Favorites.ToList();
        }

        private void DataGridFav_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DataGridFav.SelectedItems.Count > 1)
            {
                TextBoxURL.Text = String.Empty;
                TextBoxTitle.Text = String.Empty;
            }
            else
            {
                foreach (var d in DataGridFav.SelectedItems)
                {
                    TextBoxTitle.Text = ((KeyValuePair<string, string>)d).Key;
                    TextBoxURL.Text = ((KeyValuePair<string, string>)d).Value;
                }
            }
        }

        private void ButtonDeleteFav_Click(object sender, RoutedEventArgs e)
        {
            foreach(var d in DataGridFav.SelectedItems)
            {
                Setting.Settings.Favorites.Remove(((KeyValuePair<string, string>)d).Key);
            }
            Setting.SaveSetting();
            DataGridFav.ItemsSource = null;
            DataGridFav.Items.Clear();
            DataGridFav.ItemsSource = Setting.Settings.Favorites.ToList();
        }

        private void ButtonAddFav_Click(object sender, RoutedEventArgs e)
        {
            if (Setting.Settings.Favorites.TryGetValue(TextBoxTitle.Text, out _) || String.IsNullOrEmpty(TextBoxTitle.Text))
            {
                return;
            }
            Setting.Settings.Favorites.Add(TextBoxTitle.Text, TextBoxURL.Text);
            Setting.SaveSetting();
            DataGridFav.ItemsSource = null;
            DataGridFav.Items.Clear();
            DataGridFav.ItemsSource = Setting.Settings.Favorites.ToList();
        }
    }
}
