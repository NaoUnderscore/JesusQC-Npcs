using GameCore;
using HarmonyLib;
using JesusQC_Npcs.Features;
using UnityEngine;
using Log = Exiled.API.Features.Log;

namespace JesusQC_Npcs.Patches
{
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.AddPlayer))]
    internal static class ListCount
    {
        [HarmonyPrefix]
        private static bool AddPlayer(GameObject player, int maxPlayers)
        {
            Console.AddDebugLog("PLIST", "[PlayerManager] AddPlayer: " + player.GetComponent<NicknameSync>().MyNick + string.Format(" Max Slots: {0}", maxPlayers), MessageImportance.LessImportant);
            if (!PlayerManager.players.Contains(player))
            {
                PlayerManager.players.Add(player);
                ServerConsole.PlayersAmount = PlayerManager.players.Count - Dummy.List.Count;
                ServerConsole.PlayersListChanged = true;
            }

            IdleMode.SetIdleMode(false);

            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.RemovePlayer))]
    internal static class ListRemove
    {
        [HarmonyPrefix]
        private static bool RemovePlayer(GameObject player, int maxPlayers)
        {
            PlayerList.DestroyPlayer(player);
            if (PlayerManager.players.Contains(player))
            {
                PlayerManager.players.Remove(player);
                ServerConsole.PlayersAmount = PlayerManager.players.Count - Dummy.List.Count;
                ServerConsole.PlayersListChanged = true;
            }

            if (PlayerManager.players.Count == 0)
            {
                IdleMode.SetIdleMode(true);
            }

            return false;
        }
    }
}