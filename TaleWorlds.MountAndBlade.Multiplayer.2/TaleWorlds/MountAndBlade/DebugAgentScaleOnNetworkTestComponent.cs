using System;
using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade.Missions;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000023 RID: 35
	internal sealed class DebugAgentScaleOnNetworkTestComponent : UdpNetworkComponent
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x00007C90 File Offset: 0x00005E90
		public override void OnUdpNetworkHandlerTick(float dt)
		{
			if (GameNetwork.IsServer)
			{
				float totalMissionTime = MBCommon.GetTotalMissionTime();
				if (this._lastTestSendTime < totalMissionTime + 10f)
				{
					AgentReadOnlyList agents = Mission.Current.Agents;
					int count = agents.Count;
					this._lastTestSendTime = totalMissionTime;
					int num = (int)(new Random().NextDouble() * (double)count);
					Agent agent = agents[num];
					if (agent.IsActive())
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new DebugAgentScaleOnNetworkTest(agent.Index, agent.AgentScale));
						GameNetwork.EndBroadcastModuleEventUnreliable(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00007D11 File Offset: 0x00005F11
		public DebugAgentScaleOnNetworkTestComponent()
		{
			if (GameNetwork.IsClientOrReplay)
			{
				DebugAgentScaleOnNetworkTestComponent.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00007D26 File Offset: 0x00005F26
		public override void OnUdpNetworkHandlerClose()
		{
			base.OnUdpNetworkHandlerClose();
			if (GameNetwork.IsClientOrReplay)
			{
				DebugAgentScaleOnNetworkTestComponent.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007D3C File Offset: 0x00005F3C
		private static void HandleServerMessageDebugAgentScaleOnNetworkTest(DebugAgentScaleOnNetworkTest message)
		{
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(message.AgentToTestIndex, true);
			if (agentFromIndex != null && agentFromIndex.IsActive())
			{
				CompressionMission.DebugScaleValueCompressionInfo.GetPrecision();
				float agentScale = agentFromIndex.AgentScale;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00007D73 File Offset: 0x00005F73
		private static void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			new GameNetwork.NetworkMessageHandlerRegisterer(mode).Register<DebugAgentScaleOnNetworkTest>(new GameNetworkMessage.ServerMessageHandlerDelegate<DebugAgentScaleOnNetworkTest>(DebugAgentScaleOnNetworkTestComponent.HandleServerMessageDebugAgentScaleOnNetworkTest));
		}

		// Token: 0x04000066 RID: 102
		private float _lastTestSendTime;
	}
}
