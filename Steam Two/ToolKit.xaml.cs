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
            backHandle.Show();
        }

        public void Show(MainWindow backHandle)
        {
            this.backHandle = backHandle;
            Show();
        }
    }
}
