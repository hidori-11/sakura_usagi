using System.Windows;

namespace sakura_usagi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SettingLoader setting;

        public MainWindow()
        {
            InitializeComponent();
            setting = new SettingLoader();
            var ct = new WebViweController(setting);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Reflect_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
