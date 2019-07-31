using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CommonTrace.Common
{
    public interface ISimpleConfig
    {
        void AddOrUpdate<T>(string key, T value);
        T TryGet<T>(string key, T defaultValue);
    }

    public class SimpleConfig : ConcurrentDictionary<string, object>, ISimpleConfig
    {
        private readonly object _lock = new object();

        public SimpleConfig() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public void AddOrUpdate<T>(string key, T value)
        {
            lock (_lock)
            {
                this[key] = value;
            }
        }

        public T TryGet<T>(string key, T defaultValue)
        {
            lock (_lock)
            {
                if (!this.ContainsKey(key))
                {
                    return defaultValue;
                }
                return (T)this[key];
            }
        }
    }

    public static class SimpleConfigExtensions
    {
        public static T TryGetModel<T>(this ISimpleConfig simpleConfig, T defaultValue)
        {
            return simpleConfig.TryGet(typeof(T).FullName, defaultValue);
        }

        public static void AddOrUpdateModel<T>(this ISimpleConfig simpleConfig, T value)
        {
            simpleConfig.AddOrUpdate(typeof(T).FullName, value);
        }
    }

    public interface ISimpleConfigFile
    {
        Task<ISimpleConfig> ReadFile(string filePath, ISimpleConfig defaultValue);
        Task SaveFile(string filePath, ISimpleConfig config);
    }

    public class SimpleConfigFile : ISimpleConfigFile
    {
        private readonly ISimpleJsonFile _simpleJsonFile;

        public SimpleConfigFile(ISimpleJsonFile simpleJsonFile)
        {
            _simpleJsonFile = simpleJsonFile;
        }

        public Task<ISimpleConfig> ReadFile(string filePath, ISimpleConfig defaultValue)
        {
            return _simpleJsonFile.ReadFileAsSingle<ISimpleConfig>(filePath);
        }

        public Task SaveFile(string filePath, ISimpleConfig config)
        {
            return _simpleJsonFile.SaveFileAsSingle(filePath, config, true);
        }
    }

    public class SimpleConfigFactory
    {
        #region for di extensions

        private static readonly ISimpleConfig Instance = new SimpleConfig();
        private static readonly ISimpleConfigFile InstanceFile = new SimpleConfigFile(SimpleJson.ResolveSimpleJsonFile());
        public static Func<ISimpleConfig> Resolve { get; set; } = () => Instance;
        public static Func<ISimpleConfigFile> ResolveFile { get; set; } = () => InstanceFile;

        #endregion
    }
}
