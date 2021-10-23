using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using HarmonyLib;
using JesusQC_Npcs.Features;
using MEC;
using UnityEngine;

namespace JesusQC_Npcs
{
    public class MainClass : Plugin<Config>
    {
        public override string Name { get; } = "Npcs";
        public override string Prefix { get; } = "Npcs";
        public override string Author { get; } = "Jesus-QC";
        public override Version Version { get; } = new Version(0, 0, 1);

        public static Config Cfg { get; private set; }
        private Harmony _harmony;
        
        public override void OnEnabled()
        {
            foreach (var patch in Exiled.Events.Events.Instance.Harmony.GetPatchedMethods())
                if(patch.DeclaringType.Name.Equals("RoundSummary") && patch.Name.Equals("Start"))
                    Exiled.Events.Events.DisabledPatchesHashSet.Add(patch);
            
            try
            {
                _harmony = new Harmony($"jesusqc-a-{DateTime.Now.ToString()}");
                _harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            
            Cfg = Config;
            
            Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;

            Cfg = null;
            
            _harmony.UnpatchAll();
            _harmony = null;
            
            base.OnDisabled();
        }

        void OnRestartingRound()
        {
            foreach (var npc in Dummy.List)
            {   
                npc.Destroy();
            }
            
            Dummy.Dictionary.Clear();
        }
    }
}