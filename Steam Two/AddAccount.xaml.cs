using System;
using System.Windows;
using System.Windows.Threading;

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    public partial class AddAccount
    {
        bool givenInput = false; // wait for add account button to be pressed

        public AddAccount()
        {
            InitializeComponent();
        }
        
        //ignore the parameter lol, overiding doesnt work so
        //once add account button is pressed it returns username and password
        public String[] Show(String lol)
        {
            Show();
            while (!givenInput)
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));

                } catch (Exception) { }                
            }
            return new string[] { textbox1.Text, textbox2.Text};
        }

        //add account button
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            givenInput = true;
        }
    }

}
