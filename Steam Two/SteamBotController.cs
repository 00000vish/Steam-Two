using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using SteamKit2;
using SteamKit2.Internal;

namespace SteamTwo
{

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

    class FriendsArray
    {
        ArrayList array = new ArrayList();       

        public void Add(Friend item)
        {
            array.Add(item);
        }

        public void updateName(Friend input)
        {
            foreach (var item in array)
            {

                Friend temp = (Friend)item;
                if (temp.steamFrindsID.Equals(input.steamFrindsID))
                {
                    temp.customName = input.customName;
                   
                }
            }            
        }

        public void updateChatLog(Friend input, String mgs, bool outgoing)
        {
            foreach (var item in array)
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

        public Friend get(int x)
        {
            return (Friend)array[x];
        }

        public ArrayList getArray()
        {
            return array;
        }
    }


    static class SteamBotController
    {
        public static bool isRunning;
        public static bool loggedIn = false;
        public static FriendsArray fObject = null;
        private static SteamUser steamUser;
        private static SteamClient steamClient;
        private static CallbackManager manager;
        private static SteamFriends steamFriends;
        private static string user, pass;
        private static string authCode, twoFactorAuth;
        private static Thread workThread = null;

        public static void steamLogin(String username, String password)
        {
            fObject = new FriendsArray();
            workThread = new Thread(steamLogin);
            user = username;
            pass = password;
            workThread.Start();
        }

        public static void logBotIn()
        {
            workThread = new Thread(steamLogin);
            workThread.Start();
        }

        public static void logBotOff()
        {
            steamUser.LogOff();
            workThread.Abort();
        }

        private static void steamLogin()
        {

            // create our steamclient instance
            var configuration = SteamConfiguration.Create(b => b.WithProtocolTypes(ProtocolTypes.Tcp));
            steamClient = new SteamClient();
            // create the callback manager which will route callbacks to function calls
            manager = new CallbackManager(steamClient);

            // get the steamuser handler, which is used for logging on after successfully connecting
            steamUser = steamClient.GetHandler<SteamUser>();
            steamFriends = steamClient.GetHandler<SteamFriends>();
            // register a few callbacks we're interested in
            // these are registered upon creation to a callback manager, which will then route the callbacks
            // to the functions specified
            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);

            // this callback is triggered when the steam servers wish for the client to store the sentry file
            manager.Subscribe<SteamUser.UpdateMachineAuthCallback>(OnMachineAuth);



            // we use the following callbacks for friends related activities
            manager.Subscribe<SteamUser.AccountInfoCallback>(OnAccountInfo);
            manager.Subscribe<SteamFriends.FriendsListCallback>(OnFriendsList);
            manager.Subscribe<SteamFriends.PersonaStateCallback>(OnPersonaState);
            manager.Subscribe<SteamFriends.FriendAddedCallback>(OnFriendAdded);
            manager.Subscribe<SteamFriends.FriendMsgCallback>(OnChatMessage);

            isRunning = true;

            Console.WriteLine("Connecting to Steam...");

            // initiate the connection
            steamClient.Connect();

            // create our callback handling loop
            while (isRunning)
            {
                // in order for the callbacks to get routed, they need to be handled by the manager
                manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }
        }

        static void OnConnected(SteamClient.ConnectedCallback callback)
        {
            Console.WriteLine("Connected to Steam! Logging in '{0}'...", user);

            byte[] sentryHash = null;
            if (File.Exists("sentry.bin"))
            {
                // if we have a saved sentry file, read and sha-1 hash it
                byte[] sentryFile = File.ReadAllBytes("sentry.bin");
                sentryHash = CryptoHelper.SHAHash(sentryFile);
            }

            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = user,
                Password = pass,

                // in this sample, we pass in an additional authcode
                // this value will be null (which is the default) for our first logon attempt
                AuthCode = authCode,

                // if the account is using 2-factor auth, we'll provide the two factor code instead
                // this will also be null on our first logon attempt
                TwoFactorCode = twoFactorAuth,

                // our subsequent logons use the hash of the sentry file as proof of ownership of the file
                // this will also be null for our first (no authcode) and second (authcode only) logon attempts
                SentryFileHash = sentryHash,
            });
        }



        static void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            // after recieving an AccountLogonDenied, we'll be disconnected from steam
            // so after we read an authcode from the user, we need to reconnect to begin the logon flow again

            Console.WriteLine("Disconnected from Steam, reconnecting in 5...");

            Thread.Sleep(TimeSpan.FromSeconds(5));

            steamClient.Connect();
        }

        static void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            bool isSteamGuard = callback.Result == EResult.AccountLogonDenied;
            bool is2FA = callback.Result == EResult.AccountLoginDeniedNeedTwoFactor;

            if (isSteamGuard || is2FA)
            {
                Console.WriteLine("This account is SteamGuard protected!");

                if (is2FA)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        //Console.Write("Please enter your 2 factor auth code from your authenticator app: ");
                        // MainWindow.currentHandle.Show();
                        GetInput GI = new GetInput();
                        twoFactorAuth = GI.Show("Authentication", "Please enter your 2 factor auth code from your authenticator app below", false);
                        GI.Close();
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        //Console.Write("Please enter the auth code sent to the email at {0}: ", callback.EmailDomain);
                        //MainWindow.currentHandle.Show();
                        GetInput GI = new GetInput();
                        authCode = GI.Show("Authentication", "Please enter the auth code sent to the email at " + callback.EmailDomain, false);
                        GI.Close();
                    });
                }

                return;
            }

            if (callback.Result != EResult.OK)
            {
                Console.WriteLine("Unable to logon to Steam: {0} / {1}", callback.Result, callback.ExtendedResult);

                isRunning = false;
                return;
            }

            Console.WriteLine("Successfully logged on!");
            loggedIn = true;
            // at this point, we'd be able to perform actions on Steam
        }

        static void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            Console.WriteLine("Logged off of Steam: {0}", callback.Result);
        }

        static void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callback)
        {
            Console.WriteLine("Updating sentryfile...");

            // write out our sentry file
            // ideally we'd want to write to the filename specified in the callback
            // but then this sample would require more code to find the correct sentry file to read during logon
            // for the sake of simplicity, we'll just use "sentry.bin"

            int fileSize;
            byte[] sentryHash;
            using (var fs = File.Open("sentry.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fs.Seek(callback.Offset, SeekOrigin.Begin);
                fs.Write(callback.Data, 0, callback.BytesToWrite);
                fileSize = (int)fs.Length;

                fs.Seek(0, SeekOrigin.Begin);
                using (var sha = SHA1.Create())
                {
                    sentryHash = sha.ComputeHash(fs);
                }
            }

            // inform the steam servers that we're accepting this sentry file
            steamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails
            {
                JobID = callback.JobID,

                FileName = callback.FileName,

                BytesWritten = callback.BytesToWrite,
                FileSize = fileSize,
                Offset = callback.Offset,

                Result = EResult.OK,
                LastError = 0,

                OneTimePassword = callback.OneTimePassword,

                SentryFileHash = sentryHash,
            });

            Console.WriteLine("Done!");
        }

        static void OnAccountInfo(SteamUser.AccountInfoCallback callback)
        {
            // before being able to interact with friends, you must wait for the account info callback
            // this callback is posted shortly after a successful logon

            // at this point, we can go online on friends, so lets do that
            steamFriends.SetPersonaState(EPersonaState.Online);
        }

        static void OnFriendsList(SteamFriends.FriendsListCallback callback)
        {
            // at this point, the client has received it's friends list

            int friendCount = steamFriends.GetFriendCount();

            Console.WriteLine("We have {0} friends", friendCount);

            for (int x = 0; x < friendCount; x++)
            {
                // steamids identify objects that exist on the steam network, such as friends, as an example
                SteamID steamIdFriend = steamFriends.GetFriendByIndex(x);


                fObject.Add(new Friend() { steamFrindsID = "" + steamIdFriend.ConvertToUInt64().ToString(), chatLog = new ArrayList(), SteamIDObject = steamIdFriend });

                // we'll just display the STEAM_ rendered version
                Console.WriteLine("Friend: {0}", steamIdFriend.Render());
            }

            // we can also iterate over our friendslist to accept or decline any pending invites
            if (SteamTwoProperties.jsonSetting.autoAddFriendSetting)
            {
                foreach (var friend in callback.FriendList)
                {
                    if (friend.Relationship == EFriendRelationship.RequestRecipient)
                    {
                        // this user has added us, let's add him back
                        steamFriends.AddFriend(friend.SteamID);
                    }
                }
            }
        }

        static void OnFriendAdded(SteamFriends.FriendAddedCallback callback)
        {
            // someone accepted our friend request, or we accepted one
            Console.WriteLine("{0} is now a friend", callback.PersonaName);
        }

        static void OnPersonaState(SteamFriends.PersonaStateCallback callback)
        {
            // this callback is received when the persona state (friend information) of a friend changes

            // for this sample we'll simply display the names of the friends            
            fObject.updateName(new Friend() { customName = callback.Name, steamFrindsID = callback.FriendID.ConvertToUInt64().ToString() });
            Console.WriteLine("State change: {0}", callback.Name);
        }

        static void OnChatMessage(SteamFriends.FriendMsgCallback callback)
        {
            if (callback.EntryType == EChatEntryType.ChatMsg)
            {             
                if (SteamTwoProperties.jsonSetting.notifyOnMessage && steamChatWindow.current == null)
                {
                    System.Windows.Forms.MessageBox.Show("New Message!" , "Steam Two" , System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Information);
                }
                fObject.updateChatLog(new Friend() { steamFrindsID =callback.Sender.ConvertToUInt64().ToString() }, callback.Message.ToString(), false);
            }
        }

        public static String getSteamUserID()
        {
            if (steamUser != null)
            {
                return steamUser.SteamID.ConvertToUInt64().ToString();
            }
            return "";
        }

        public static void sendChatMessage(SteamID id, String msg)
        {
            fObject.updateChatLog(new Friend() { steamFrindsID = id.ConvertToUInt64().ToString()},msg,true);
            steamFriends.SendChatMessage(id , EChatEntryType.ChatMsg, msg);
        }

        public static void playGame(int gameID)
        {
            var request = new ClientMsgProtobuf<CMsgClientGamesPlayed>(EMsg.ClientGamesPlayed);

            request.Body.games_played.Add(new CMsgClientGamesPlayed.GamePlayed
            {
                game_id = new GameID(gameID),
            });
            steamClient.Send(request);
        }
    }
}

