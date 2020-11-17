using System;
using System.Windows;
using System.Windows.Controls;

namespace sakura_usagi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SettingLoader setting;
        WebViweController[,] webViweControllers;
        BrowserWindow browserWindow;

        public MainWindow()
        {
            InitializeComponent();
            setting = new SettingLoader();
            Add.IsEnabled = false;
            Close.IsEnabled = false;
            browserWindow = new BrowserWindow();
            browserWindow.Show();
             
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Reflect_Click(object sender, RoutedEventArgs e)
        {
            SwitchEnable(false);

            uint row_count = 0;
            uint column_count = 0;

            if (!uint.TryParse(TextBox_X.Text, out column_count))
            {
                SwitchEnable(true);
                return;
            }

            if(!uint.TryParse(TextBox_Y.Text, out row_count))
            {
                SwitchEnable(true);
                return;
            }

            var temp_controllers = new WebViweController[column_count, row_count];

            int before_x = 0;
            int before_y = 0;

            if(webViweControllers != null)
            {
                before_x = webViweControllers.GetLength(0);
                before_y = webViweControllers.GetLength(1);
                for (int x = 0; x < webViweControllers.GetLength(0) && x < temp_controllers.GetLength(0); x++)
                {
                    for (int y = 0; y < webViweControllers.GetLength(1) && y < temp_controllers.GetLength(1); y++)
                    {
                        temp_controllers[x, y] = webViweControllers[x, y];
                    }
                }
            }

            webViweControllers = temp_controllers;

            browserWindow.BrowserGrid.Children.Clear();
            ControllerGrid.Children.Clear();

            browserWindow.BrowserGrid.ColumnDefinitions.Clear();
            browserWindow.BrowserGrid.RowDefinitions.Clear();

            ControllerGrid.ColumnDefinitions.Clear();
            ControllerGrid.RowDefinitions.Clear();

            for (int y = 0; y < row_count; y++)
            {
                browserWindow.BrowserGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                ControllerGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
            }

            for (int x = 0; x < column_count; x++)
            {
                browserWindow.BrowserGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
                ControllerGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            }


            for (int x = 0; x < column_count; x++)
            {
                for(int y = 0; y < row_count; y++)
                {
                    WebViweController web;
                    if (webViweControllers[x, y] == null)
                    {
                        web = new WebViweController(setting);

                        Grid.SetColumn(web, x);
                        Grid.SetColumn(web.webcontrol, x);
                        Grid.SetRow(web, y);
                        Grid.SetRow(web.webcontrol, y);

                        ControllerGrid.Children.Add(web);
                        browserWindow.BrowserGrid.Children.Add(web.webcontrol);
                        web.InitWebView();

                        webViweControllers[x, y] = web;
                    }
                    else
                    {
                        web = webViweControllers[x, y];

                        ControllerGrid.Children.Add(web);
                        browserWindow.BrowserGrid.Children.Add(web.webcontrol);
                    }
                }
            }
            GC.Collect();

            SwitchEnable(true);
        }

        private void SwitchEnable(bool isEnable)
        {
            TextBox_X.IsEnabled = isEnable;
            TextBox_Y.IsEnabled = isEnable;
            Reflect.IsEnabled = isEnable;
        }
    }
}
