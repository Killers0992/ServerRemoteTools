using NetworkedPlugins.API.Interfaces;

namespace ServerRemoteTools
{
    public class AddonConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}
