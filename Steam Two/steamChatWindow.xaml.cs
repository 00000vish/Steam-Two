using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for steamChatWindow.xaml
    /// </summary>
    public partial class steamChatWindow
    {
        private DispatcherTimer dispatcherTimer;

        public steamChatWindow()
        {
            InitializeComponent();
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            updateFriendList();
            updateChatLog();
        }

        public void updateFriendList()
        {
            int temp = friendsList1.SelectedIndex;
            friendsList1.Items.Clear();
            if (SteamBotController.fObject != null)
                foreach (var item in SteamBotController.fObject.getArray())
                {
                    Friend obj = (Friend)item;
                    friendsList1.Items.Add(obj.getName());
                }
            friendsList1.SelectedIndex = temp;
        }

        public void updateChatLog()
        {
            chatLog1.Items.Clear();
            chatLog1.Items.Add("                                                                                                        ");
            chatLog1.Items.RemoveAt(0);
            if (friendsList1.SelectedIndex != -1)
            {
                foreach (var item in SteamBotController.fObject.get(friendsList1.SelectedIndex).chatLog)
                {
                    if(item.ToString().Length > 104)
                    {
                        chatLog1.Items.Add(item.ToString().Substring(0,104));
                        chatLog1.Items.Add(item.ToString().Substring(104, item.ToString().Length-104));
                    }
                    else
                    {
                        chatLog1.Items.Add(item);
                    }                   

                }
            }
            chatLog1.UpdateLayout();
        }

        public void setMessage()
        {
            if (friendsList1.SelectedIndex != -1)
            {
                SteamBotController.sendChatMessage(SteamBotController.fObject.get(friendsList1.SelectedIndex).SteamIDObject, textbox1.Text);
            }
        }

        private void textbox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().Equals("Return"))
            {               
                setMessage();
                textbox1.Text = "";
            }            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            setMessage();
            textbox1.Text = "";
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}

//