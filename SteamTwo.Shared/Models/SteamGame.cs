using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTwo.Shared.Models
{
    public class SteamGame
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string StoreUrl { get; }
        public SteamGame(int id, string name, string description, string storeUrl)
        {
            Id = id;
            Name = name;
            Description = description;
            StoreUrl = storeUrl;
        }
    }
}
