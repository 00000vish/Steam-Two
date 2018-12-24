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
        //array of the accounts
        public static ArrayList userAccounts = new ArrayList();

        //add account to unserAccounts, only adds if the account doesnt exist
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

        //returns account if it exist or return null
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

        //remove account
        public static void removeAccount(UserAccount account)
        {
            userAccounts.Remove(account);
        }
    }

    class UserAccount
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool desktopAuth { get; set; } //if SDA ( steam desktop auth is enabled)

        ArrayList friendArray = new ArrayList();

        //add friend to array if it doesnt exist
        public void AddFriend(Friend item)
        {
            bool found = false;
            foreach (Friend var in friendArray)
            {
                Friend temp = (Friend)var;
                if (temp.steamFrindsID.ToLower().Equals(item.steamFrindsID.ToLower()))
                {
                    found = true;
                }
            }
            if (!found)
            {                
                friendArray.Add(item);
            }                
        }

        //set name
        public void setFriendsName(Friend input)
        {
            foreach (var item in friendArray)
            {

                Friend temp = (Friend)item;
                if (temp.steamFrindsID.Equals(input.steamFrindsID))
                {
                    temp.name = input.name;

                }
            }
        }

        //returns friends name
        public string getFriendsName(Friend input)
        {
            foreach (var item in friendArray)
            {
                Friend temp = (Friend)item;
                if (temp.steamFrindsID.Equals(input.steamFrindsID))
                {
                    return temp.name;

                }
            }
            return "";
        }

        //update friends chat logs
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
                        temp.newMessage(temp.name, mgs);
                    }

                }
            }
        }

        //get friend from index
        public Friend getFriend(int index)
        {
            return (Friend)friendArray[index];
        }

        //get whole friend array
        public ArrayList getFriendArray()
        {
            return friendArray;
        }
    }

    class Friend
    {
        public string name { get; set; }
        public string steamFrindsID { get; set; }
        public ArrayList chatLog { get; set; }
        public SteamID SteamIDObject { get; set; }

        //returns custom name
        public string getName()
        {
            return name;
        }

        //add msg to chat log
        public void newMessage(String n, String msg)
        {
            chatLog.Add(n + ": " + msg);
        }

        //returns steam ID
        public string getId()
        {
            return steamFrindsID;
        }
    }
}
