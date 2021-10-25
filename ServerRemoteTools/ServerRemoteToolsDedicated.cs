using NetworkedPlugins.API;
using NetworkedPlugins.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerRemoteTools
{
    public class ServerRemoteToolsDedicated : NPAddonDedicated<AddonConfig>
    {
        public override string AddonAuthor { get; } = "Killers0992";
        public override string AddonId { get; } = "8rwn6FE9cjCngeXB";
        public override string AddonName { get; } = "ServerRemoteTools";
        public override Version AddonVersion { get; } = new Version(1, 0, 0);

        public override void OnConsoleCommand(string cmd, List<string> arguments)
        {
            switch(cmd.ToUpper())
            {
                case "SRT":
                    if (arguments.Count == 0)
                    {
                        Logger.Info(string.Concat($"Commands: ",
                            Environment.NewLine,
                            " - SRT globalcommand <all/server> <command> - Command is sended to all servers.",
                            Environment.NewLine,
                            " - SRT redirectplayers <server> <targetserver> - Redirect all players to other server.",
                            Environment.NewLine,
                            " - SRT servers - Connected servers to networkedplugins."));
                        return;
                    }
                    switch (arguments[0].ToUpper())
                    {
                        case "GLOBALCOMMAND":
                            if (arguments.Count == 1)
                            {
                                Logger.Info($"Syntax: SRT globalcommand <all/server> <command>");
                                return;
                            }
                            foreach(var server in GetServers().Where(p => p.FullAddress == arguments[1] || arguments[1].ToUpper() == "ALL"))
                                server.ExecuteCommand(arguments[2], arguments.Skip(3).ToList());
                            break;
                        case "REDIRECTPLAYERS":
                            if (arguments.Count != 3)
                            {
                                Logger.Info($"Syntax: SRT redirectplayers <server> <targetserver>");
                                return;
                            }
                            var servers = GetServers();
                            var currentserver = servers.FirstOrDefault(p => p.FullAddress == arguments[1]);
                            if (currentserver == null)
                            {
                                Logger.Info($"Server \"{arguments[1]}\" is not connected!");
                                return;
                            }
                            var targetserver = servers.FirstOrDefault(p => p.FullAddress == arguments[2]);
                            if (targetserver == null)
                            {
                                Logger.Info($"Server \"{arguments[2]}\" is not connected!");
                                return;
                            }

                            foreach(var plr in currentserver.Players)
                            {
                                plr.Value.Redirect(targetserver.ServerPort);
                            }
                            Logger.Info($"Redirecting \"{currentserver.Players.Count}\" players from server \"{currentserver.FullAddress}\" to \"{targetserver.FullAddress}\"!");
                            break;
                        case "SERVERS":
                            string str = "Connected servers:";
                            var connectedServers = GetServers();
                            foreach (var server in connectedServers)
                            {
                                str += $"{Environment.NewLine} - {server.FullAddress} | Players {server.Players.Count}/{server.MaxPlayers}";
                            }
                            if (connectedServers.Count == 0)
                                str += " - 0 servers connected!";
                            Logger.Info(str);
                            break;
                    }
                    break;
            }
        }

        public override void OnConsoleResponse(NPServer server, string command, string response, bool isRa)
        {
            Logger.Info($"Server \"{server.FullAddress}\", response from command \"{command}\", {response}");
        }
    }
}
