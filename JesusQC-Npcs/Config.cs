using System.ComponentModel;
using Exiled.API.Interfaces;

namespace JesusQC_Npcs
{
    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;
        [Description("Whether or not this plugin should show debug logs.")]
        public bool ShouldDebug { get; set; } = true;
    }
}