using System.Reflection;
using Rocket.Core.Logging;
using Rocket.Core.Utils;

namespace LoadPlugin
{
    public static class Utils
    {
        public static bool TryGetAssembly(string name, out Assembly assembly)
        {
            assembly = null;
            
            try
            {
                assembly = Assembly.LoadFile($"Plugins/{name}");
                var types = RocketHelper.GetTypesFromInterface(assembly, "IRocketPlugin").FindAll(x => !x.IsAbstract);

                if (types.Count == 1)
                {
                    Logger.Log($"Loading {assembly.GetName().Name} from {assembly.Location}");
                }
                else
                {
                    Logger.LogError($"Invalid or outdated plugin assembly: {assembly.GetName().Name}");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public static void Invoke(object instance, string methodName)
        {
            var m = instance.GetType().GetMethod(methodName);
            
            m?.Invoke(instance, null);
        }
        
        public static object GetInstanceField<T>(T instance, string fieldName)
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var field = typeof(T).GetField(fieldName, bindFlags);
            return field?.GetValue(instance);
        }

        public static void SetInstanceField<T>(T instance, string fieldName, object value)
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            typeof(T).GetField(fieldName, bindFlags)?.SetValue(instance, value);
        }
    }
}
