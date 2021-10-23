using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Mirror;
using UnityEngine;

namespace JesusQC_Npcs.Features
{
    public class Dummy
    {
        public static readonly Dictionary<GameObject, Dummy> Dictionary = new Dictionary<GameObject, Dummy>();
        public static List<Dummy> List => Dictionary.Values.ToList();
        
        public Player PlayerWrapper { get; private set; }

        public Dummy(Vector3 spawnPosition, Vector3 scale, RoleType role = RoleType.Tutorial, string nick = "Dummy", bool hasGoodMode = true)
        {
            var gameObject = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            var referenceHub = gameObject.GetComponent<ReferenceHub>();

            gameObject.transform.localScale = scale;
            gameObject.transform.position = spawnPosition;

            referenceHub.queryProcessor.PlayerId = -1;
            referenceHub.queryProcessor.NetworkPlayerId = -1;
            referenceHub.queryProcessor._ipAddress = "127.0.0.WAN";

            referenceHub.characterClassManager.CurClass = role;
            referenceHub.characterClassManager.GodMode = hasGoodMode;
            referenceHub.playerStats.SetHPAmount(100);

            referenceHub.nicknameSync.Network_myNickSync = nick;

            NetworkServer.Spawn(gameObject);
            PlayerWrapper = new Player(gameObject);
            
            Dictionary.Add(gameObject, this);
            PlayerManager.AddPlayer(gameObject, CustomNetworkManager.slots);
        }
        
        public void Destroy(bool spawnRagdoll = false)
        {
            if (spawnRagdoll)
            {
                var go = PlayerWrapper.GameObject;
                go.GetComponent<RagdollManager>().SpawnRagdoll(go.transform.position, go.transform.rotation, Vector3.zero, (int)PlayerWrapper.Role, new PlayerStats.HitInfo(), false, "", PlayerWrapper.Nickname, -1);
            }
            
            Dictionary.Remove(PlayerWrapper.GameObject);
            PlayerManager.RemovePlayer(PlayerWrapper.GameObject, CustomNetworkManager.slots);
            Object.Destroy(PlayerWrapper.GameObject);
            
            PlayerWrapper = null;
        }
    }
}