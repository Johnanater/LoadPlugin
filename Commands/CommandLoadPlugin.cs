using System.Collections.Generic;
using System.Reflection;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Utils;
using Rocket.Unturned.Chat;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LoadPlugin.Commands
{
    public class CommandLoadPlugin : IRocketCommand
    {
        private List<GameObject> _plugins = new List<GameObject>();
        private List<Assembly> _pluginAssemblies = new List<Assembly>();

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "loadplugin";

        public string Help => "Load a plugin right from it's DLL!";

        public string Syntax => "/loadplugin <plugin>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "loadplugin.loadplugin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                UnturnedChat.Say(caller, Syntax);
                return;
            }
            
            var pluginName = command[0];
            if (!pluginName.EndsWith(".dll"))
                pluginName += ".dll";

            if (!Utils.TryGetAssembly(pluginName, out var newAssembly))
            {
                UnturnedChat.Say(caller, Main.Instance.Translate("failed_to_get_assembly", pluginName));
                return;
            }

            _plugins = (List<GameObject>) Utils.GetInstanceField(R.Plugins, "plugins");
            _pluginAssemblies = (List<Assembly>) Utils.GetInstanceField(R.Plugins, "pluginAssemblies");
            
            var newAssemblies = new List<Assembly> { newAssembly };
            var pluginImplementations = RocketHelper.GetTypesFromInterface(newAssemblies, "IRocketPlugin");
            
            foreach (var pluginType in pluginImplementations)
            {
                var plugin = new GameObject(pluginType.Name, pluginType);
                Object.DontDestroyOnLoad(plugin);
                _plugins.Add(plugin);
                _pluginAssemblies.Add(newAssembly);
            }
            
            Utils.Invoke(R.Plugins, "PluginsLoaded");
            Utils.SetInstanceField(R.Plugins, "plugins", _plugins);
            Utils.SetInstanceField(R.Plugins, "pluginAssemblies", _pluginAssemblies);
            
            UnturnedChat.Say(caller, Main.Instance.Translate("successfully_loaded", newAssembly.GetName().Name, newAssembly.GetName().Version));
        }
    }
}
