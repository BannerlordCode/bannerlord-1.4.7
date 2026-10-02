using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001EF RID: 495
	public static class ItemPhysicsSoundContainer
	{
		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x00062D69 File Offset: 0x00060F69
		// (set) Token: 0x06001CF0 RID: 7408 RVA: 0x00062D70 File Offset: 0x00060F70
		public static int SoundCodePhysicsBoulderDefault { get; private set; }

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x00062D78 File Offset: 0x00060F78
		// (set) Token: 0x06001CF2 RID: 7410 RVA: 0x00062D7F File Offset: 0x00060F7F
		public static int SoundCodePhysicsArrowlikeDefault { get; private set; }

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x00062D87 File Offset: 0x00060F87
		// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x00062D8E File Offset: 0x00060F8E
		public static int SoundCodePhysicsBowlikeDefault { get; private set; }

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x00062D96 File Offset: 0x00060F96
		// (set) Token: 0x06001CF6 RID: 7414 RVA: 0x00062D9D File Offset: 0x00060F9D
		public static int SoundCodePhysicsDaggerlikeDefault { get; private set; }

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x00062DA5 File Offset: 0x00060FA5
		// (set) Token: 0x06001CF8 RID: 7416 RVA: 0x00062DAC File Offset: 0x00060FAC
		public static int SoundCodePhysicsGreatswordlikeDefault { get; private set; }

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x00062DB4 File Offset: 0x00060FB4
		// (set) Token: 0x06001CFA RID: 7418 RVA: 0x00062DBB File Offset: 0x00060FBB
		public static int SoundCodePhysicsShieldlikeDefault { get; private set; }

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001CFB RID: 7419 RVA: 0x00062DC3 File Offset: 0x00060FC3
		// (set) Token: 0x06001CFC RID: 7420 RVA: 0x00062DCA File Offset: 0x00060FCA
		public static int SoundCodePhysicsSpearlikeDefault { get; private set; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001CFD RID: 7421 RVA: 0x00062DD2 File Offset: 0x00060FD2
		// (set) Token: 0x06001CFE RID: 7422 RVA: 0x00062DD9 File Offset: 0x00060FD9
		public static int SoundCodePhysicsSwordlikeDefault { get; private set; }

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x00062DE1 File Offset: 0x00060FE1
		// (set) Token: 0x06001D00 RID: 7424 RVA: 0x00062DE8 File Offset: 0x00060FE8
		public static int SoundCodePhysicsBoulderWood { get; private set; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x00062DF0 File Offset: 0x00060FF0
		// (set) Token: 0x06001D02 RID: 7426 RVA: 0x00062DF7 File Offset: 0x00060FF7
		public static int SoundCodePhysicsArrowlikeWood { get; private set; }

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x00062DFF File Offset: 0x00060FFF
		// (set) Token: 0x06001D04 RID: 7428 RVA: 0x00062E06 File Offset: 0x00061006
		public static int SoundCodePhysicsBowlikeWood { get; private set; }

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x00062E0E File Offset: 0x0006100E
		// (set) Token: 0x06001D06 RID: 7430 RVA: 0x00062E15 File Offset: 0x00061015
		public static int SoundCodePhysicsDaggerlikeWood { get; private set; }

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x00062E1D File Offset: 0x0006101D
		// (set) Token: 0x06001D08 RID: 7432 RVA: 0x00062E24 File Offset: 0x00061024
		public static int SoundCodePhysicsGreatswordlikeWood { get; private set; }

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x00062E2C File Offset: 0x0006102C
		// (set) Token: 0x06001D0A RID: 7434 RVA: 0x00062E33 File Offset: 0x00061033
		public static int SoundCodePhysicsShieldlikeWood { get; private set; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x00062E3B File Offset: 0x0006103B
		// (set) Token: 0x06001D0C RID: 7436 RVA: 0x00062E42 File Offset: 0x00061042
		public static int SoundCodePhysicsSpearlikeWood { get; private set; }

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x00062E4A File Offset: 0x0006104A
		// (set) Token: 0x06001D0E RID: 7438 RVA: 0x00062E51 File Offset: 0x00061051
		public static int SoundCodePhysicsSwordlikeWood { get; private set; }

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001D0F RID: 7439 RVA: 0x00062E59 File Offset: 0x00061059
		// (set) Token: 0x06001D10 RID: 7440 RVA: 0x00062E60 File Offset: 0x00061060
		public static int SoundCodePhysicsBoulderStone { get; private set; }

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x00062E68 File Offset: 0x00061068
		// (set) Token: 0x06001D12 RID: 7442 RVA: 0x00062E6F File Offset: 0x0006106F
		public static int SoundCodePhysicsArrowlikeStone { get; private set; }

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001D13 RID: 7443 RVA: 0x00062E77 File Offset: 0x00061077
		// (set) Token: 0x06001D14 RID: 7444 RVA: 0x00062E7E File Offset: 0x0006107E
		public static int SoundCodePhysicsBowlikeStone { get; private set; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x00062E86 File Offset: 0x00061086
		// (set) Token: 0x06001D16 RID: 7446 RVA: 0x00062E8D File Offset: 0x0006108D
		public static int SoundCodePhysicsDaggerlikeStone { get; private set; }

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x00062E95 File Offset: 0x00061095
		// (set) Token: 0x06001D18 RID: 7448 RVA: 0x00062E9C File Offset: 0x0006109C
		public static int SoundCodePhysicsGreatswordlikeStone { get; private set; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x00062EA4 File Offset: 0x000610A4
		// (set) Token: 0x06001D1A RID: 7450 RVA: 0x00062EAB File Offset: 0x000610AB
		public static int SoundCodePhysicsShieldlikeStone { get; private set; }

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x00062EB3 File Offset: 0x000610B3
		// (set) Token: 0x06001D1C RID: 7452 RVA: 0x00062EBA File Offset: 0x000610BA
		public static int SoundCodePhysicsSpearlikeStone { get; private set; }

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001D1D RID: 7453 RVA: 0x00062EC2 File Offset: 0x000610C2
		// (set) Token: 0x06001D1E RID: 7454 RVA: 0x00062EC9 File Offset: 0x000610C9
		public static int SoundCodePhysicsSwordlikeStone { get; private set; }

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x00062ED1 File Offset: 0x000610D1
		// (set) Token: 0x06001D20 RID: 7456 RVA: 0x00062ED8 File Offset: 0x000610D8
		public static int SoundCodePhysicsWater { get; private set; }

		// Token: 0x06001D21 RID: 7457 RVA: 0x00062EE0 File Offset: 0x000610E0
		static ItemPhysicsSoundContainer()
		{
			ItemPhysicsSoundContainer.UpdateItemPhysicsSoundCodes();
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x00062EE8 File Offset: 0x000610E8
		private static void UpdateItemPhysicsSoundCodes()
		{
			ItemPhysicsSoundContainer.SoundCodePhysicsBoulderDefault = SoundEvent.GetEventIdFromString("event:/physics/boulder/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/arrowlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/bowlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/daggerlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/greatswordlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/shieldlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/spearlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeDefault = SoundEvent.GetEventIdFromString("event:/physics/swordlike/default");
			ItemPhysicsSoundContainer.SoundCodePhysicsBoulderWood = SoundEvent.GetEventIdFromString("event:/physics/boulder/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeWood = SoundEvent.GetEventIdFromString("event:/physics/arrowlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeWood = SoundEvent.GetEventIdFromString("event:/physics/bowlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeWood = SoundEvent.GetEventIdFromString("event:/physics/daggerlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeWood = SoundEvent.GetEventIdFromString("event:/physics/greatswordlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeWood = SoundEvent.GetEventIdFromString("event:/physics/shieldlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeWood = SoundEvent.GetEventIdFromString("event:/physics/spearlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeWood = SoundEvent.GetEventIdFromString("event:/physics/swordlike/wood");
			ItemPhysicsSoundContainer.SoundCodePhysicsBoulderStone = SoundEvent.GetEventIdFromString("event:/physics/boulder/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeStone = SoundEvent.GetEventIdFromString("event:/physics/arrowlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeStone = SoundEvent.GetEventIdFromString("event:/physics/bowlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeStone = SoundEvent.GetEventIdFromString("event:/physics/daggerlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeStone = SoundEvent.GetEventIdFromString("event:/physics/greatswordlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeStone = SoundEvent.GetEventIdFromString("event:/physics/shieldlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeStone = SoundEvent.GetEventIdFromString("event:/physics/spearlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeStone = SoundEvent.GetEventIdFromString("event:/physics/swordlike/stone");
			ItemPhysicsSoundContainer.SoundCodePhysicsWater = SoundEvent.GetEventIdFromString("event:/physics/water");
		}
	}
}
