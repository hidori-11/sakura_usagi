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
        WebViewControl[,] webViweControllers;

        TextBox[] rowRatioTextBoxs;
        TextBox[] columnRatioTextBoxs;

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

        private void Add_Click(object sender, EventArgs e)
        {

        }

        private void Close_Click(object sender, EventArgs e)
        {

        }

        private void Reflect_Click(object sender, EventArgs e)
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

            var temp_controllers = new WebViewControl[column_count, row_count];

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
            HeightRatio.Children.Clear();
            WidthRatio.Children.Clear();

            browserWindow.BrowserGrid.ColumnDefinitions.Clear();
            browserWindow.BrowserGrid.RowDefinitions.Clear();

            ControllerGrid.ColumnDefinitions.Clear();
            ControllerGrid.RowDefinitions.Clear();

            HeightRatio.ColumnDefinitions.Clear();

            WidthRatio.ColumnDefinitions.Clear();

            HeightRatio.ColumnDefinitions.Add(new ColumnDefinition());
            HeightRatio.ColumnDefinitions.Add(new ColumnDefinition());

            WidthRatio.ColumnDefinitions.Add(new ColumnDefinition());
            WidthRatio.ColumnDefinitions.Add(new ColumnDefinition());

            rowRatioTextBoxs = new TextBox[row_count];
            columnRatioTextBoxs = new TextBox[column_count];

            for (int y = 0; y < row_count; y++)
            {
                browserWindow.BrowserGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                ControllerGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                HeightRatio.ColumnDefinitions.Add(new ColumnDefinition());
                
                var tb = SetRatioTextBox();
                Grid.SetColumn(tb, y + 1);
                HeightRatio.Children.Add(tb);
                rowRatioTextBoxs[y] = tb;
            }

            for (int x = 0; x < column_count; x++)
            {
                browserWindow.BrowserGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
                ControllerGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
                WidthRatio.ColumnDefinitions.Add(new ColumnDefinition());

                var tb = SetRatioTextBox();
                Grid.SetColumn(tb, x + 1);
                WidthRatio.Children.Add(tb);
                columnRatioTextBoxs[x] = tb;
            }


            for (uint x = 0; x < column_count; x++)
            {
                for(uint y = 0; y < row_count; y++)
                {
                    WebViewControl web;
                    if (webViweControllers[x, y] == null)
                    {
                        web = new WebViewControl(setting);

                        web.SetGrid(x, y);

                        ControllerGrid.Children.Add(web.WebViewController);
                        browserWindow.BrowserGrid.Children.Add(web.WebControl);
                        web.InitWebView();

                        webViweControllers[x, y] = web;
                    }
                    else
                    {
                        web = webViweControllers[x, y];

                        ControllerGrid.Children.Add(web.WebViewController);
                        browserWindow.BrowserGrid.Children.Add(web.WebControl);
                    }
                }
            }
            GC.Collect();

            SwitchEnable(true);
        }

        private TextBox SetRatioTextBox()
        {
            TextBox tb = new TextBox();
            tb.Text = "1";
            tb.TextChanged += RatioTextBoxTextChangedEvent;
            return tb;

        }

        private void RatioTextBoxTextChangedEvent(object sender, EventArgs e)
        {
            browserWindow.BrowserGrid.ColumnDefinitions.Clear();
            browserWindow.BrowserGrid.RowDefinitions.Clear();

            ControllerGrid.ColumnDefinitions.Clear();
            ControllerGrid.RowDefinitions.Clear();

            for (int y = 0; y < rowRatioTextBoxs.Length; y++)
            {
                uint heightRatio = 1;
                if (uint.TryParse(rowRatioTextBoxs[y].Text, out heightRatio))
                {
                    browserWindow.BrowserGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
                    {
                        Height = new GridLength(heightRatio, GridUnitType.Star)
                    });

                    ControllerGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
                    {
                        Height = new GridLength(heightRatio, GridUnitType.Star)
                    });
                }
            }

            for (int x = 0; x < columnRatioTextBoxs.Length; x++)
            {
                uint widthRatio = 1;

                if (uint.TryParse(columnRatioTextBoxs[x].Text, out widthRatio))
                {
                    browserWindow.BrowserGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
                    {
                        Width = new GridLength(widthRatio, GridUnitType.Star)
                    });

                    ControllerGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
                    {
                        Width = new GridLength(widthRatio, GridUnitType.Star)
                    });
                }

            }

        }

        private void SwitchEnable(bool isEnable)
        {
            TextBox_X.IsEnabled = isEnable;
            TextBox_Y.IsEnabled = isEnable;
            Reflect.IsEnabled = isEnable;
        }

        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutInfo aboutWindow = new AboutInfo();
            aboutWindow.ShowDialog();
        }
    }
}
