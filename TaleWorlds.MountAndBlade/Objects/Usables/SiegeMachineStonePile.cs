using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003AB RID: 939
	public class SiegeMachineStonePile : UsableMachine, ISpawnable
	{
		// Token: 0x0600352C RID: 13612 RVA: 0x000DAB11 File Offset: 0x000D8D11
		protected internal override void OnInit()
		{
			base.OnInit();
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x000DAB1C File Offset: 0x000D8D1C
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			if (usableGameObject.GameEntity.HasTag(this.AmmoPickUpTag))
			{
				TextObject textObject = new TextObject("{=jfcceEoE}{PILE_TYPE} Pile", null);
				textObject.SetTextVariable("PILE_TYPE", new TextObject("{=1CPdu9K0}Stone", null));
				return textObject;
			}
			return null;
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x000DAB63 File Offset: 0x000D8D63
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (gameEntity.HasTag(this.AmmoPickUpTag))
			{
				TextObject textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
				textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
				return textObject;
			}
			return null;
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x000DABA3 File Offset: 0x000D8DA3
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x000DABAC File Offset: 0x000D8DAC
		public override OrderType GetOrder(BattleSideEnum side)
		{
			return OrderType.None;
		}

		// Token: 0x04001698 RID: 5784
		private bool _spawnedFromSpawner;
	}
}
