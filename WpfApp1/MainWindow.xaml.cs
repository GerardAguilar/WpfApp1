using OpenQA.Selenium.Remote;
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

namespace TouchAuto
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void RecordMode(object sender, RoutedEventArgs e) {
            RecordPage recordPage = new RecordPage();
            recordPage.ToggleView(this);
            //this.Hide();
            //recordPage.Activate();
        }

        public void ReplayMode(object sender, RoutedEventArgs e) {
            ReplayPage replayPage = new ReplayPage();
            replayPage.ToggleView(this);
        }
    }
}
