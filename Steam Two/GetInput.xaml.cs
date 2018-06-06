using System;
using System.Windows;
using System.Windows.Threading;


namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for GetInput.xaml
    /// </summary>
    public partial class GetInput
    {
        bool givenInput = false;

        public GetInput()
        {
            InitializeComponent();
        }

        public String Show(String title, String discription, bool password)
        {
            String key = "";
            if (password)
            {
                PasswordBox1.IsEnabled = true;
                PasswordBox1.Visibility = Visibility.Visible;
                textBox1.IsEnabled = false;
                textBox1.Visibility = Visibility.Hidden;
            }
            Title = title;
            label1.Text = discription;
            Show();

            while (!givenInput)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            }

            key = textBox1.Text;
            if (password)
            {
                key = new System.Net.NetworkCredential(string.Empty, PasswordBox1.SecurePassword).Password;
            }

            if (key == "")
            {
                return "-1";
            }
            else
            {
                return key;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            givenInput = true;
        }

        private void GetInput1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            givenInput = true;
        }

        private void textBox1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.ToString().Equals("Return"))
            {
                givenInput = true;
            }
        }

        private void PasswordBox1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.ToString().Equals("Return"))
            {
                givenInput = true;
            }
        }
    }
}
