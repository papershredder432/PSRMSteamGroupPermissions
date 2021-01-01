using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Permissions;
using Rocket.Unturned.Player;
using Steamworks;
using Logger = Rocket.Core.Logging.Logger;

namespace PSRMSteamGroupPermissions
{
    public class PSRMSteamGroupPermissions : RocketPlugin<PSRMSteamGroupPermissionsConfiguration>
    {
        public PSRMSteamGroupPermissions Instance { get; set; }
        
        public List<Group> ValidGroups { get; set; }
        
        protected override void Load()
        {
            Instance = this;
            ValidGroups = Configuration.Instance.Groups;
            Logger.Log($"{Name} {Assembly.GetName().Version} loaded! Made by papershredder432, join the support Discord here: https://discord.gg/ydjYVJ2", ConsoleColor.Blue);

            U.Events.OnPlayerConnected += OnPlayerConnected;
            
            foreach (var validGroup in ValidGroups.ToList())
            {
                RocketPermissionsGroup g = R.Permissions.GetGroup(validGroup.RocketGroup);

                if (g != null) continue;
                Logger.LogWarning($"\"{validGroup.RocketGroup}\" does not exist, and any player trying to purchase this rank will not be able to buy it.");
                ValidGroups.Remove(validGroup);
            }
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            var selectedGroup = ValidGroups.FirstOrDefault(x => x.SteamGroup == player.SteamGroupID.m_SteamID);
            if (selectedGroup == null) return;

            R.Permissions.AddPlayerToGroup(selectedGroup.RocketGroup, new RocketPlayer(player.Id));
            
            if (Instance.Configuration.Instance.LogJoins) Logger.Log($"{player} was added to Rocket group: {selectedGroup.RocketGroup}", ConsoleColor.Cyan);
        }

        protected override void Unload()
        {
            Instance = null;

            U.Events.OnPlayerConnected -= OnPlayerConnected;
            
            Logger.Log($"{Name} {Assembly.GetName().Version} unloaded.", ConsoleColor.Blue);
        }
    }
}