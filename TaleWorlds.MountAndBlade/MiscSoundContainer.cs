using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F0 RID: 496
	public static class MiscSoundContainer
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x0006306C File Offset: 0x0006126C
		// (set) Token: 0x06001D24 RID: 7460 RVA: 0x00063073 File Offset: 0x00061273
		public static int SoundCodeMovementFoleyDoorOpen { get; private set; }

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x0006307B File Offset: 0x0006127B
		// (set) Token: 0x06001D26 RID: 7462 RVA: 0x00063082 File Offset: 0x00061282
		public static int SoundCodeMovementFoleyDoorClose { get; private set; }

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001D27 RID: 7463 RVA: 0x0006308A File Offset: 0x0006128A
		// (set) Token: 0x06001D28 RID: 7464 RVA: 0x00063091 File Offset: 0x00061291
		public static int SoundCodeAmbientNodeSiegeBallistaFire { get; private set; }

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001D29 RID: 7465 RVA: 0x00063099 File Offset: 0x00061299
		// (set) Token: 0x06001D2A RID: 7466 RVA: 0x000630A0 File Offset: 0x000612A0
		public static int SoundCodeAmbientNodeSiegeMangonelFire { get; private set; }

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001D2B RID: 7467 RVA: 0x000630A8 File Offset: 0x000612A8
		// (set) Token: 0x06001D2C RID: 7468 RVA: 0x000630AF File Offset: 0x000612AF
		public static int SoundCodeAmbientNodeSiegeTrebuchetFire { get; private set; }

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001D2D RID: 7469 RVA: 0x000630B7 File Offset: 0x000612B7
		// (set) Token: 0x06001D2E RID: 7470 RVA: 0x000630BE File Offset: 0x000612BE
		public static int SoundCodeAmbientNodeSiegeBallistaHit { get; private set; }

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001D2F RID: 7471 RVA: 0x000630C6 File Offset: 0x000612C6
		// (set) Token: 0x06001D30 RID: 7472 RVA: 0x000630CD File Offset: 0x000612CD
		public static int SoundCodeAmbientNodeSiegeBoulderHit { get; private set; }

		// Token: 0x06001D31 RID: 7473 RVA: 0x000630D5 File Offset: 0x000612D5
		static MiscSoundContainer()
		{
			MiscSoundContainer.UpdateMiscSoundCodes();
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x000630DC File Offset: 0x000612DC
		private static void UpdateMiscSoundCodes()
		{
			MiscSoundContainer.SoundCodeMovementFoleyDoorOpen = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/door_open");
			MiscSoundContainer.SoundCodeMovementFoleyDoorClose = SoundEvent.GetEventIdFromString("event:/mission/movement/foley/door_close");
			MiscSoundContainer.SoundCodeAmbientNodeSiegeBallistaFire = SoundEvent.GetEventIdFromString("event:/map/ambient/node/siege/ballista_fire");
			MiscSoundContainer.SoundCodeAmbientNodeSiegeMangonelFire = SoundEvent.GetEventIdFromString("event:/map/ambient/node/siege/mangonel_fire");
			MiscSoundContainer.SoundCodeAmbientNodeSiegeTrebuchetFire = SoundEvent.GetEventIdFromString("event:/map/ambient/node/siege/trebuchet_fire");
			MiscSoundContainer.SoundCodeAmbientNodeSiegeBallistaHit = SoundEvent.GetEventIdFromString("event:/map/ambient/node/siege/ballista_hit");
			MiscSoundContainer.SoundCodeAmbientNodeSiegeBoulderHit = SoundEvent.GetEventIdFromString("event:/map/ambient/node/siege/boulder_hit");
		}
	}
}
