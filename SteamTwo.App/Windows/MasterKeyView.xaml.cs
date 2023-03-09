﻿using MahApps.Metro.Controls;
using SteamTwo.App.ViewModels;
using System.Windows;

namespace SteamTwo.App.Windows
{
    /// <summary>
    /// Interaction logic for MasterKeyView.xaml
    /// </summary>
    public partial class MasterKeyView : MetroWindow
    {
        public MasterKeyView()
        {
            InitializeComponent();
            this.DataContext = new MasterKeyViewModel();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if(e.NewValue != null && e.NewValue is MasterKeyViewModel vm)
            {
                vm.SetMasterKey = () =>
                {
                    vm.MasterKey = MasterKeyInput.Password;
                };
                vm.OpenMainWindow = () =>
                {
                    MainWindowView mainWindow = new MainWindowView();
                    mainWindow.Show();
                    this.Close();
                };
            }
        }
    }
}
