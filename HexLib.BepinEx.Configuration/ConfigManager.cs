using BepInEx;
using BepInEx.Configuration;
using System;
using System.IO;

namespace HexLib.BepinEx.Configuration
{
    /// <summary>
    /// Manages BepinEx configuration with live-reload support via FileSystemWatcher.
    /// Provides a reusable configuration management utility for mods.
    /// </summary>
    public class ConfigManager
    {
        private static FileSystemWatcher _watcher;
        private static Action<string> _logError = msg => { };

        /// <summary>
        /// Gets the standard debug logging configuration entry.
        /// Available after Initialize() is called.
        /// </summary>
        public static ConfigEntry<bool> EnableAdvancedDebugLogging { get; private set; }

        /// <summary>
        /// Initializes the configuration manager with standard debug settings and file watcher.
        /// </summary>
        /// <param name="config">The BepinEx ConfigFile instance.</param>
        /// <param name="modGuid">The mod's GUID (used for config file naming).</param>
        /// <param name="logError">Optional action for logging errors. Defaults to no-op if not provided.</param>
        public static void Initialize(ConfigFile config, string modGuid, Action<string> logError = null)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (string.IsNullOrWhiteSpace(modGuid))
                throw new ArgumentException("ModGuid cannot be null or empty.", nameof(modGuid));

            _logError = logError ?? (msg => { });

            config.SaveOnConfigSet = false;

            EnableAdvancedDebugLogging = config.Bind(
                "Debug",
                nameof(EnableAdvancedDebugLogging),
                false,
                "Enable advanced debug logging for detailed troubleshooting and development. WARNING: very verbose.");

            config.Save();
            config.SaveOnConfigSet = true;

            SetupConfigWatcher(config, modGuid);
        }

        /// <summary>
        /// Sets up file system watcher for live configuration reload support.
        /// </summary>
        private static void SetupConfigWatcher(ConfigFile config, string modGuid)
        {
            string configPath = BepInEx.Paths.ConfigPath;
            string configFileName = $"{modGuid}.cfg";

            _watcher = new FileSystemWatcher(configPath, configFileName)
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            _watcher.Changed += (sender, e) => ReloadConfig(config, configFileName);
            _watcher.Created += (sender, e) => ReloadConfig(config, configFileName);
            _watcher.Renamed += (sender, e) => ReloadConfig(config, configFileName);

            if (BepInEx.ThreadingHelper.SynchronizingObject != null)
            {
                _watcher.SynchronizingObject = BepInEx.ThreadingHelper.SynchronizingObject;
            }
        }

        /// <summary>
        /// Reloads the configuration from disk.
        /// </summary>
        private static void ReloadConfig(ConfigFile config, string configFileName)
        {
            string filePath = Path.Combine(BepInEx.Paths.ConfigPath, configFileName);

            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                config.Reload();
            }
            catch (Exception ex)
            {
                _logError($"[ConfigManager] Failed to reload configuration: {ex.Message}");
            }
        }
    }
}
