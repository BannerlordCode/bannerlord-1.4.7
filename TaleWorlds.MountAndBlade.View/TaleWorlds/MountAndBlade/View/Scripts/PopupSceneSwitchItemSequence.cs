using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000061 RID: 97
	public class PopupSceneSwitchItemSequence : PopupSceneSequence
	{
		// Token: 0x060003AB RID: 939 RVA: 0x0001B741 File Offset: 0x00019941
		public override void OnInitialState()
		{
			this.AttachItem(this.InitialItem, this.InitialBodyPart);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001B755 File Offset: 0x00019955
		public override void OnPositiveState()
		{
			this.AttachItem(this.PositiveItem, this.PositiveBodyPart);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001B769 File Offset: 0x00019969
		public override void OnNegativeState()
		{
			this.AttachItem(this.NegativeItem, this.NegativeBodyPart);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001B780 File Offset: 0x00019980
		private EquipmentIndex StringToEquipmentIndex(PopupSceneSwitchItemSequence.BodyPartIndex part)
		{
			switch (part)
			{
			case PopupSceneSwitchItemSequence.BodyPartIndex.None:
				return EquipmentIndex.None;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Weapon0:
				return EquipmentIndex.WeaponItemBeginSlot;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Weapon1:
				return EquipmentIndex.Weapon1;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Weapon2:
				return EquipmentIndex.Weapon2;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Weapon3:
				return EquipmentIndex.Weapon3;
			case PopupSceneSwitchItemSequence.BodyPartIndex.ExtraWeaponSlot:
				return EquipmentIndex.ExtraWeaponSlot;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Head:
				return EquipmentIndex.NumAllWeaponSlots;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Body:
				return EquipmentIndex.Body;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Leg:
				return EquipmentIndex.Leg;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Gloves:
				return EquipmentIndex.Gloves;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Cape:
				return EquipmentIndex.Cape;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Horse:
				return EquipmentIndex.ArmorItemEndSlot;
			case PopupSceneSwitchItemSequence.BodyPartIndex.HorseHarness:
				return EquipmentIndex.HorseHarness;
			default:
				return EquipmentIndex.None;
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001B7E8 File Offset: 0x000199E8
		private void AttachItem(string itemName, PopupSceneSwitchItemSequence.BodyPartIndex bodyPart)
		{
			if (this._agentVisuals == null)
			{
				return;
			}
			EquipmentIndex equipmentIndex = this.StringToEquipmentIndex(bodyPart);
			if (equipmentIndex != EquipmentIndex.None)
			{
				AgentVisualsData copyAgentVisualsData = this._agentVisuals.GetCopyAgentVisualsData();
				Equipment equipment = this._agentVisuals.GetEquipment().Clone(false);
				if (itemName == "")
				{
					equipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, default(EquipmentElement));
				}
				else
				{
					equipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>(itemName), null, null, false));
				}
				copyAgentVisualsData.RightWieldedItemIndex(0).LeftWieldedItemIndex(-1).Equipment(equipment);
				this._agentVisuals.Refresh(false, copyAgentVisualsData, false);
			}
		}

		// Token: 0x04000211 RID: 529
		public string InitialItem;

		// Token: 0x04000212 RID: 530
		public string PositiveItem;

		// Token: 0x04000213 RID: 531
		public string NegativeItem;

		// Token: 0x04000214 RID: 532
		public PopupSceneSwitchItemSequence.BodyPartIndex InitialBodyPart;

		// Token: 0x04000215 RID: 533
		public PopupSceneSwitchItemSequence.BodyPartIndex PositiveBodyPart;

		// Token: 0x04000216 RID: 534
		public PopupSceneSwitchItemSequence.BodyPartIndex NegativeBodyPart;

		// Token: 0x020000D8 RID: 216
		public enum BodyPartIndex
		{
			// Token: 0x040003C1 RID: 961
			None,
			// Token: 0x040003C2 RID: 962
			Weapon0,
			// Token: 0x040003C3 RID: 963
			Weapon1,
			// Token: 0x040003C4 RID: 964
			Weapon2,
			// Token: 0x040003C5 RID: 965
			Weapon3,
			// Token: 0x040003C6 RID: 966
			ExtraWeaponSlot,
			// Token: 0x040003C7 RID: 967
			Head,
			// Token: 0x040003C8 RID: 968
			Body,
			// Token: 0x040003C9 RID: 969
			Leg,
			// Token: 0x040003CA RID: 970
			Gloves,
			// Token: 0x040003CB RID: 971
			Cape,
			// Token: 0x040003CC RID: 972
			Horse,
			// Token: 0x040003CD RID: 973
			HorseHarness
		}
	}
}
