using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036C RID: 876
	public static class DebugSiegeBehavior
	{
		// Token: 0x06003241 RID: 12865 RVA: 0x000CD1EC File Offset: 0x000CB3EC
		public static void SiegeDebug()
		{
			if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtRam"))
			{
				DebugSiegeBehavior.DebugDefendState = DebugSiegeBehavior.DebugStateDefender.DebugDefendersToRam;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtSt"))
			{
				DebugSiegeBehavior.DebugDefendState = DebugSiegeBehavior.DebugStateDefender.DebugDefendersToTower;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtBallistas2"))
			{
				DebugSiegeBehavior.DebugDefendState = DebugSiegeBehavior.DebugStateDefender.DebugDefendersToBallistae;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtMangonels2"))
			{
				DebugSiegeBehavior.DebugDefendState = DebugSiegeBehavior.DebugStateDefender.DebugDefendersToMangonels;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtNone2"))
			{
				DebugSiegeBehavior.DebugDefendState = DebugSiegeBehavior.DebugStateDefender.None;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtBallistas"))
			{
				DebugSiegeBehavior.DebugAttackState = DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToBallistae;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtMangonels"))
			{
				DebugSiegeBehavior.DebugAttackState = DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToMangonels;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtBattlements"))
			{
				DebugSiegeBehavior.DebugAttackState = DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToBattlements;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtNone"))
			{
				DebugSiegeBehavior.DebugAttackState = DebugSiegeBehavior.DebugStateAttacker.None;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyTargetDebugActive"))
			{
				DebugSiegeBehavior.ToggleTargetDebug = true;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyTargetDebugDisactive"))
			{
				DebugSiegeBehavior.ToggleTargetDebug = false;
			}
			bool toggleTargetDebug = DebugSiegeBehavior.ToggleTargetDebug;
		}

		// Token: 0x04001541 RID: 5441
		public static bool ToggleTargetDebug;

		// Token: 0x04001542 RID: 5442
		public static DebugSiegeBehavior.DebugStateAttacker DebugAttackState;

		// Token: 0x04001543 RID: 5443
		public static DebugSiegeBehavior.DebugStateDefender DebugDefendState;

		// Token: 0x02000648 RID: 1608
		public enum DebugStateAttacker
		{
			// Token: 0x04002136 RID: 8502
			None,
			// Token: 0x04002137 RID: 8503
			DebugAttackersToBallistae,
			// Token: 0x04002138 RID: 8504
			DebugAttackersToMangonels,
			// Token: 0x04002139 RID: 8505
			DebugAttackersToBattlements
		}

		// Token: 0x02000649 RID: 1609
		public enum DebugStateDefender
		{
			// Token: 0x0400213B RID: 8507
			None,
			// Token: 0x0400213C RID: 8508
			DebugDefendersToBallistae,
			// Token: 0x0400213D RID: 8509
			DebugDefendersToMangonels,
			// Token: 0x0400213E RID: 8510
			DebugDefendersToRam,
			// Token: 0x0400213F RID: 8511
			DebugDefendersToTower
		}
	}
}
