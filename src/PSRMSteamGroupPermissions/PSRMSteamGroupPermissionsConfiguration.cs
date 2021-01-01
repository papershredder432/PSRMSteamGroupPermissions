using System.Collections.Generic;
using Rocket.API;

namespace PSRMSteamGroupPermissions
{
    public class PSRMSteamGroupPermissionsConfiguration : IRocketPluginConfiguration
    {
        public bool LogJoins;
        
        public List<Group> Groups = new List<Group>();
        
        public void LoadDefaults()
        {
            LogJoins = true;

            Groups = new List<Group>
            {
                new Group
                {
                    SteamGroup = 103582791457933883,
                    RocketGroup = "RocketGroupIDHere"
                }
            };
        }
    }
}