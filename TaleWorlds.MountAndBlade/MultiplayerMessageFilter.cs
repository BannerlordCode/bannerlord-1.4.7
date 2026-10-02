using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002EB RID: 747
	[Flags]
	public enum MultiplayerMessageFilter : ulong
	{
		// Token: 0x0400109E RID: 4254
		None = 0UL,
		// Token: 0x0400109F RID: 4255
		Peers = 1UL,
		// Token: 0x040010A0 RID: 4256
		Messaging = 2UL,
		// Token: 0x040010A1 RID: 4257
		Items = 4UL,
		// Token: 0x040010A2 RID: 4258
		General = 8UL,
		// Token: 0x040010A3 RID: 4259
		Equipment = 16UL,
		// Token: 0x040010A4 RID: 4260
		EquipmentDetailed = 32UL,
		// Token: 0x040010A5 RID: 4261
		Formations = 64UL,
		// Token: 0x040010A6 RID: 4262
		Agents = 128UL,
		// Token: 0x040010A7 RID: 4263
		AgentsDetailed = 256UL,
		// Token: 0x040010A8 RID: 4264
		Mission = 512UL,
		// Token: 0x040010A9 RID: 4265
		MissionDetailed = 1024UL,
		// Token: 0x040010AA RID: 4266
		AgentAnimations = 2048UL,
		// Token: 0x040010AB RID: 4267
		SiegeWeapons = 4096UL,
		// Token: 0x040010AC RID: 4268
		MissionObjects = 8192UL,
		// Token: 0x040010AD RID: 4269
		MissionObjectsDetailed = 16384UL,
		// Token: 0x040010AE RID: 4270
		SiegeWeaponsDetailed = 32768UL,
		// Token: 0x040010AF RID: 4271
		Orders = 65536UL,
		// Token: 0x040010B0 RID: 4272
		GameMode = 131072UL,
		// Token: 0x040010B1 RID: 4273
		Administration = 262144UL,
		// Token: 0x040010B2 RID: 4274
		Particles = 524288UL,
		// Token: 0x040010B3 RID: 4275
		RPC = 1048576UL,
		// Token: 0x040010B4 RID: 4276
		All = 4294967295UL,
		// Token: 0x040010B5 RID: 4277
		LightLogging = 139913UL,
		// Token: 0x040010B6 RID: 4278
		NormalLogging = 1979037UL,
		// Token: 0x040010B7 RID: 4279
		AllWithoutDetails = 2044639UL
	}
}
