
using System.Collections.Generic;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2;
using scp4aiur;
using System.Linq;
using Smod2.EventSystem.Events;

namespace BlackInfection
{
    partial class PlayersEvents : IEventHandlerPlayerDie, IEventHandlerPocketDimensionDie, IEventHandlerSetSCPConfig, IEventHandlerRoundStart,
           IEventHandler106CreatePortal, IEventHandlerSetRole, IEventHandler106Teleport, IEventHandlerWaitingForPlayers
    {
        /////////////////////////////////////////////////////////////////Variables////////////////////////////////////////////////////////////////

        private BlackInfection plugin;
        public PlayersEvents(BlackInfection plugin)
        {
            this.plugin = plugin;
        }
        static Dictionary<int, Vector> Portales = new Dictionary<int, Vector>();


        Vector portal;
        Vector nulo;
        static int posiciones;
        Vector spawns;
        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            if (ev.Killer.TeamRole.Role == Role.SCP_106)
            {
                if (posiciones == 4) { posiciones = 0; }
                if (posiciones == 0) { spawns = PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_106).First(); }
                if (posiciones == 1) { spawns = PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_096).First(); }
                if (posiciones == 2) { spawns = PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_939_89).First(); }
                if (posiciones == 3) { spawns = PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_049).First(); }
                posiciones = posiciones + 1;
                Timing.Run(Respawn(ev.Player, spawns));

            }
        }

        public void OnPocketDimensionDie(PlayerPocketDimensionDieEvent ev)
        {
            if (posiciones == 4) { posiciones = 0; }
            if (posiciones == 0) { spawns = PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_106).First(); }
            if (posiciones == 1) { spawns = PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_096).First(); }
            if (posiciones == 2) { spawns = PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_939_89).First(); }
            if (posiciones == 3) { spawns = PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_049).First(); }
            posiciones = posiciones + 1;
            Timing.Run(Respawn(ev.Player, spawns));
        }


        public static IEnumerable<float> Respawn(Player player, Vector spawn)
        {
            posiciones = posiciones + 1;
            yield return 0.4f;
            player.ChangeRole(Role.SCP_106);
            player.Teleport(spawn);


        }

        public void OnSetSCPConfig(SetSCPConfigEvent ev)
        {
            ev.Ban049 = true;
            ev.Ban079 = true;
            ev.Ban096 = true;
            ev.Ban173 = true;
            ev.Ban939_53 = true;
            ev.Ban939_89 = true;
            if (PluginManager.Manager.Server.NumPlayers > 15) { ev.SCP106amount = 3; } else { ev.SCP106amount = 2; }
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            Timing.Run(Anuncio());
        }

        public static IEnumerable<float> Anuncio()
        {

            yield return 3f;
            PluginManager.Manager.Server.Map.AnnounceCustomMessage("attention all facility personnel, Containment Breach. Class. Keter. Please Evacuate Immediately ");
        }

        public void On106CreatePortal(Player106CreatePortalEvent ev)
        {
            Portales[ev.Player.PlayerId] = ev.Player.GetPosition();
        }

        public void OnSetRole(PlayerSetRoleEvent ev)
        {

            if (!Portales.ContainsKey(ev.Player.PlayerId))
            {
                Portales.Add(ev.Player.PlayerId, (nulo));
            }
        }

        public void On106Teleport(Player106TeleportEvent ev)
        {
            Timing.Run(Portaltp(ev.Player));
        }

        public static IEnumerable<float> Portaltp(Player player)
        {

            yield return 5f;
            foreach (KeyValuePair<int,Vector> key in Portales)
            {
                    if(player.PlayerId == key.Key) { player.Teleport(key.Value); }
            }                        
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            plugin.RefreshConfig();
        }
    }
}