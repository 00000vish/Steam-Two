﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SteamTwo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const String DEFUALT_KEY = "vishwawenga";
        private const String DEFUALT_KEY_TEST = "FteUuLPNgH2K7YjGhHbPGw==";
        private const String SAVE_FILE_NAME = "accounts.file";

        private static Account[] accountsArray = new Account[] { };

        private static bool encrypted = false;
        private static String encryptionKey = DEFUALT_KEY;

        public MainWindow()
        {
            InitializeComponent();
            initVariables();
            initLogics();
        }

        internal static void setEncryptionKey(string temp)
        {
            encryptionKey = temp;
            Properties.Settings.Default.encryptedKeyCheck = SteamTwo.Cryptography.Encrypt(DEFUALT_KEY, encryptionKey);
            Properties.Settings.Default.Save();
        }

        private void initVariables()
        {
            encrypted = Properties.Settings.Default.encrypted;
        }

        private void initLogics()
        {
            setupEncryptionKey(0, "Please enter the encryption key below");
            if (File.Exists(SAVE_FILE_NAME))
            {
                try
                {
                    getAccountData();
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Error getting account information from the save file, if the file was encrypted with different password goto settings and change the password and restart the application", "Error Decrypting", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                updateAccountList();
            }
        }

        private void updateAccountList()
        {
            listView1.Items.Clear();
            for (int i = 0; i < accountsArray.Length; i++)
            {
                listView1.Items.Add(new ListViewItem { Content = accountsArray[i].username, Tag = i });
            }
        }

        private void getAccountData()
        {

            var JsonAccounts = JsonConvert.DeserializeObject<jsonObject>(File.ReadAllText(SAVE_FILE_NAME));
            Account[] accArray = new Account[JsonAccounts.count];
            for (int i = 0; i < JsonAccounts.count; i++)
            {
                accArray[i] = new Account { username = JsonAccounts.accounts[i].username, password = Cryptography.Decrypt(JsonAccounts.accounts[i].password, encryptionKey) };
            }
            accountsArray = accArray;
        }

        internal static void writeAccountData()
        {
            Account[] accArray = new Account[accountsArray.Length];
            for (int i = 0; i < accountsArray.Length; i++)
            {
                accArray[i] = new Account { username = accountsArray[i].username, password = Cryptography.Encrypt(accountsArray[i].password, encryptionKey) };
            }
            string json = JsonConvert.SerializeObject(new jsonObject { count = accountsArray.Length, accounts = accArray });
            System.IO.File.WriteAllText(SAVE_FILE_NAME, json);
        }

        private void setupEncryptionKey(int attempts, String discriptionText)
        {
            if (encrypted)
            {
                if (attempts <= 2)
                {
                    GetInput GI = new GetInput();
                    String temp = GI.Show("Encryption", discriptionText, true);
                    GI.Close();
                    if (temp != "-1" && SteamTwo.Cryptography.Encrypt(DEFUALT_KEY, temp).Equals(Properties.Settings.Default.encryptedKeyCheck))
                    {
                        encryptionKey = temp;
                    }
                    else
                    {
                        attempts++;
                        discriptionText = "Encryption key is invalid, enter a valid encryption key";
                        setupEncryptionKey(attempts, discriptionText);
                    }
                }
                else
                {
                    Properties.Settings.Default.encrypted = false;
                    Properties.Settings.Default.encryptedKeyCheck = DEFUALT_KEY_TEST;
                    Properties.Settings.Default.Save();
                    encryptionKey = DEFUALT_KEY;
                    File.Delete(SAVE_FILE_NAME);
                    System.Windows.Forms.MessageBox.Show("Encryption key will reset back to defualt key and previous account details will be deleted.", "Encryption", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            else
            {
                encryptionKey = DEFUALT_KEY;
            }
        }

        private void settingTitleBarButton(object sender, RoutedEventArgs e)
        {
            Settings SW = new Settings();
            SW.Show("zzz");
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddAccount ASA = new AddAccount();
            String[] newItem = ASA.Show("lol");
            ASA.Close();
            Account[] accArray = new Account[accountsArray.Length + 1];
            for (int i = 0; i < accountsArray.Length; i++)
            {
                accArray[i] = new Account { username = accountsArray[i].username, password = accountsArray[i].password };
            }
            accArray[accountsArray.Length] = new Account { username = newItem[0], password = newItem[1] };
            accountsArray = accArray;
            updateAccountList();
            writeAccountData();
            listView1.SelectedIndex = 0;
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                int index = 0;
                Account[] accArray = new Account[accountsArray.Length - 1];

                foreach (var item in accountsArray)
                {
                    if (item != accountsArray[listView1.SelectedIndex])
                    {
                        accArray[index] = new Account { username = item.username, password = item.password };
                        index++;
                    }
                }
                accountsArray = accArray;
                updateAccountList();
                writeAccountData();
                listView1.SelectedIndex = 0;
            }
        }

        private void loginBot1_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                //login
            }
        }

        private void loginSteam1_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                LocalSteamController.startSteam(accountsArray[listView1.SelectedIndex].username, accountsArray[listView1.SelectedIndex].password);
            }
            if (Properties.Settings.Default.closeStemLaunchSetting)
            {
                beforeClosing();
            }
        }

        private void beforeClosing()
        {
            if (Properties.Settings.Default.alwayRunSetting)
            {
                Hide();
            }
            else
            {
                Properties.Settings.Default.Save();
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void listView1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.copyPasswordSetting)
            {
                if (listView1.SelectedItem != null)
                {
                    Clipboard.SetText(accountsArray[listView1.SelectedIndex].password);
                }
            }
        }
    }
}

public class jsonObject
{
    //  {"count":1,"accounts":[{"username":"1234","password":"qwert"},{"username":"1234","password":"qwert"}]}
    public int count { get; set; }
    public Account[] accounts { get; set; }
}

public class Account
{
    public String username { get; set; }
    public String password { get; set; }
}