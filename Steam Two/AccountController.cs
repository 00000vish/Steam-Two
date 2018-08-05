using SteamKit2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTwo
{

    static class AccountController
    {
        public static ArrayList userAccounts = new ArrayList();

        public static void addAccount(String paraUsername, String paraPassword, bool paraDesktopAuth)
        {
            bool found = false;
            foreach (var item in userAccounts)
            {
                UserAccount temp = (UserAccount)item;
                if (temp.username.ToLower().Equals(paraUsername.ToLower()))
                {
                    found = true;
                }
            }
            if (!found)
            {
                userAccounts.Add(new UserAccount { username = paraUsername, password = paraPassword, desktopAuth = paraDesktopAuth });
            }
        }

        public static UserAccount getAccount(String username)
        {
            foreach (var item in userAccounts)
            {
                UserAccount temp = (UserAccount)item;
                if (temp.username.ToLower().Equals(username.ToLower()))
                {
                    return temp;
                }
            }
            return null;
        }

        public static void removeAccount(UserAccount account)
        {
            userAccounts.Remove(account);
        }
    }

    class UserAccount
    {
        public String username { get; set; }
        public String password { get; set; }
        public bool desktopAuth { get; set; }

        ArrayList friendArray = new ArrayList();

        public void AddFriend(Friend item)
        {
            friendArray.Add(item);
        }

        public void setFriendsName(Friend input)
        {
            foreach (var item in friendArray)
            {

                Friend temp = (Friend)item;
                if (temp.steamFrindsID.Equals(input.steamFrindsID))
                {
                    temp.customName = input.customName;

                }
            }
        }

        public String getFriendsName(Friend input)
        {
            foreach (var item in friendArray)
            {

                Friend temp = (Friend)item;
                if (temp.steamFrindsID.Equals(input.steamFrindsID))
                {
                    return temp.customName;

                }
            }
            return "";
        }

        public void updateChatLogs(Friend input, String mgs, bool outgoing)
        {
            foreach (var item in friendArray)
            {

                Friend temp = (Friend)item;
                if (temp.steamFrindsID.Equals(input.steamFrindsID))
                {
                    if (outgoing)
                    {
                        temp.newMessage("Me", mgs);
                    }
                    else
                    {
                        temp.newMessage(temp.customName, mgs);
                    }

                }
            }
        }

        public Friend getFriend(int x)
        {
            return (Friend)friendArray[x];
        }

        public ArrayList getFriendArray()
        {
            return friendArray;
        }
    }

    class Friend
    {
        public String customName { get; set; }
        public String steamFrindsID { get; set; }
        public ArrayList chatLog { get; set; }
        public SteamID SteamIDObject { get; set; }

        public string getName()
        {
            return customName;
        }

        public void newMessage(String n, String msg)
        {
            chatLog.Add(n + ": " + msg);
        }

        public string getId()
        {
            return steamFrindsID;
        }
    }
}
