using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;

namespace LoadPlugin
{
    public class Main : RocketPlugin
    {
        public static Main Instance;
        public const string Version = "1.0.0";

        protected override void Load()
        {
            Instance = this;

            Logger.Log($"LoadPlugin by Johnanater, version: {Version}");
        }

        protected override void Unload()
        {
            Instance = null;
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            {"failed_to_get_assembly", "Failed to get assembly {0}!"},
            {"successfully_loaded", "Successfully loaded {0}, version {1}!"},
            {"reloaded_rocket", "Completely reloaded RocketMod! This may cause issues!"}
        };
    }
}
