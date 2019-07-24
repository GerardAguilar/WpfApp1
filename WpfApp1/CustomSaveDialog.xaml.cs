using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for CustomSaveDialog.xaml
    /// </summary>
    public partial class CustomSaveDialog : Window
    {
        private String installDirectory;
        private Window parent;
        private String eventsString;
        public CustomSaveDialog(Window p) {
            parent = p;
            installDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString();
            InitializeComponent();
        }

        public CustomSaveDialog(Window p, String eventsStringParam)
        {
            parent = p;
            eventsString = eventsStringParam;
            installDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString();
            InitializeComponent();
        }

        //public void CustomSave(object sender, RoutedEventArgs e) {
        //    String eventFolder = installDirectory + eventName.Text;
        //    if (!Directory.Exists(eventFolder)) {
        //        Directory.CreateDirectory(eventFolder);
        //    }
        //    File.WriteAllText(eventFolder + "\\" + eventName.Text + ".json", eventsString);
        //    parent.Show();
        //    this.Hide();
        //}

        public string GetEventName(object sender, RoutedEventArgs e) {
            parent.Show();
            this.Hide();
            return eventName.Text;
        }

        public void CustomSave(object sender, RoutedEventArgs e) {
            String eventFolder = installDirectory + eventName.Text;
            if (!Directory.Exists(eventFolder))
            {
                Directory.CreateDirectory(eventFolder);
            }
            DialogResult = true;
        }
    }
}
