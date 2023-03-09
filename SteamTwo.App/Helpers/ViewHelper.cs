using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;

namespace SteamTwo.App.Helpers
{
    public static class ViewHelper
    {
        private static Dictionary<Type, MetroWindow> _viewDictionary = new();

        public static void Register(MetroWindow window, params Type[] args)
        {
            foreach (var type in args)
            {
                if(_viewDictionary.ContainsKey(type))
                    _viewDictionary.Remove(type);

                _viewDictionary.Add(type, window);
            }
        }

        public static MetroWindow GetView(object viewModel)
        {
            var type = viewModel.GetType();

            if (!_viewDictionary.ContainsKey(type))
                throw new Exception("View not registered");

            return _viewDictionary[type];   
        }
    }
}
