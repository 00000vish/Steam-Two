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
        public static steamChatWindow current = null;
        public string user = "";

        public steamChatWindow()
        {
            InitializeComponent();
            current = this;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        public void Show(String username)
        {
            user = username;
            Show();
        }

        //checks for chat message updates
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            updateFriendList();
            updateChatLog();
        }

        //update friends list
        public void updateFriendList()
        {
            int temp = friendsList1.SelectedIndex;
            friendsList1.Items.Clear();
            if (AccountController.getAccount(user).getFriendArray() != null)
                foreach (var item in AccountController.getAccount(user).getFriendArray())
                {
                    Friend obj = (Friend)item;
                    friendsList1.Items.Add(obj.getName());
                }
            friendsList1.SelectedIndex = temp;
        }

        //update chat
        public void updateChatLog()
        {
            chatLog1.Items.Clear();
            chatLog1.Items.Add("                                                                                                        ");
            chatLog1.Items.RemoveAt(0);
            if (friendsList1.SelectedIndex != -1)
            {
                foreach (var item in AccountController.getAccount(user).getFriend(friendsList1.SelectedIndex).chatLog)
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

        //sends message
        public void sendMessage()
        {
            if (friendsList1.SelectedIndex != -1)
            {
                SteamBotController.sendChatMessage(AccountController.getAccount(user).getFriend(friendsList1.SelectedIndex).SteamIDObject, textbox1.Text);
            }
        }

        //send message enter is pressed
        private void textbox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().Equals("Return"))
            {               
                sendMessage();
                textbox1.Text = "";
            }            
        }

        //send button is clicked
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sendMessage();
            textbox1.Text = "";
        }

        //since wpf u cant check windows state, just to keep track is chat window is focused and its checked from steam bot thread
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            current = null;
        }

        private void MetroWindow_Deactivated(object sender, EventArgs e)
        {
            current = null;
        }

        private void MetroWindow_Activated(object sender, EventArgs e)
        {
            current = this;
        }
    }
}