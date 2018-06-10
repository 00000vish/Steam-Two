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

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for ToolKit.xaml
    /// </summary>
    public partial class ToolKit
    {
        private MainWindow backHandle = null;

        public ToolKit()
        {
            InitializeComponent();
        }

        private void backButton(object sender, RoutedEventArgs e)
        {
            if (settingButton.Content.ToString().Equals("Back to Main Page"))
            {
                backHandle.Show();                
                settingButton.Content = "Hide Main Page";
            }
            else
            {
                backHandle.Hide();
                settingButton.Content = "Back to Main Page";
            }
        }

        public void Show(MainWindow backHandle)
        {
            this.backHandle = backHandle;
            Show();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            backHandle.Show();
        }
    }
}
