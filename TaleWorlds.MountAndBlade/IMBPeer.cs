using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001AE RID: 430
	[ScriptingInterfaceBase]
	internal interface IMBPeer
	{
		// Token: 0x0600188B RID: 6283
		[EngineMethod("set_user_data", false, null, false)]
		void SetUserData(int index, MBNetworkPeer data);

		// Token: 0x0600188C RID: 6284
		[EngineMethod("set_controlled_agent", false, null, false)]
		void SetControlledAgent(int index, UIntPtr missionPointer, int agentIndex);

		// Token: 0x0600188D RID: 6285
		[EngineMethod("set_team", false, null, false)]
		void SetTeam(int index, int teamIndex);

		// Token: 0x0600188E RID: 6286
		[EngineMethod("is_active", false, null, false)]
		bool IsActive(int index);

		// Token: 0x0600188F RID: 6287
		[EngineMethod("set_is_synchronized", false, null, false)]
		void SetIsSynchronized(int index, bool value);

		// Token: 0x06001890 RID: 6288
		[EngineMethod("get_is_synchronized", false, null, false)]
		bool GetIsSynchronized(int index);

		// Token: 0x06001891 RID: 6289
		[EngineMethod("send_existing_objects", false, null, false)]
		void SendExistingObjects(int index, UIntPtr missionPointer);

		// Token: 0x06001892 RID: 6290
		[EngineMethod("begin_module_event", false, null, false)]
		void BeginModuleEvent(int index, bool isReliable);

		// Token: 0x06001893 RID: 6291
		[EngineMethod("end_module_event", false, null, false)]
		void EndModuleEvent(bool isReliable);

		// Token: 0x06001894 RID: 6292
		[EngineMethod("get_average_ping_in_milliseconds", false, null, false)]
		double GetAveragePingInMilliseconds(int index);

		// Token: 0x06001895 RID: 6293
		[EngineMethod("get_average_loss_percent", false, null, false)]
		double GetAverageLossPercent(int index);

		// Token: 0x06001896 RID: 6294
		[EngineMethod("set_relevant_game_options", false, null, false)]
		void SetRelevantGameOptions(int index, bool sendMeBloodEvents, bool sendMeSoundEvents);

		// Token: 0x06001897 RID: 6295
		[EngineMethod("get_reversed_host", false, null, false)]
		uint GetReversedHost(int index);

		// Token: 0x06001898 RID: 6296
		[EngineMethod("get_host", false, null, false)]
		uint GetHost(int index);

		// Token: 0x06001899 RID: 6297
		[EngineMethod("get_port", false, null, false)]
		ushort GetPort(int index);
	}
}
