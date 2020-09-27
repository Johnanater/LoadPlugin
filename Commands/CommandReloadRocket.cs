using System.Collections.Generic;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;

namespace LoadPlugin.Commands
{
    public class CommandReloadRocket : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "reloadrocket";

        public string Help => "Reload RocketMod, can cause issues";

        public string Syntax => "/reloadrocket";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "loadplugin.reloadrocket" };
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            R.Reload();
            UnturnedChat.Say(caller, Main.Instance.Translate("reloaded_rocket"));
        }
    }
}