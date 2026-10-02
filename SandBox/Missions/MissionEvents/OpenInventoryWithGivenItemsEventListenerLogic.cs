using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.MountAndBlade.Objects.Usables;
using TaleWorlds.ObjectSystem;

namespace SandBox.Missions.MissionEvents
{
	// Token: 0x0200009D RID: 157
	public class OpenInventoryWithGivenItemsEventListenerLogic : MissionLogic
	{
		// Token: 0x0600068C RID: 1676 RVA: 0x0002C76F File Offset: 0x0002A96F
		public OpenInventoryWithGivenItemsEventListenerLogic()
		{
			Game.Current.EventManager.RegisterEvent<GenericMissionEvent>(new Action<GenericMissionEvent>(this.OnGenericMissionEventTriggered));
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0002C79D File Offset: 0x0002A99D
		protected override void OnEndMission()
		{
			Game.Current.EventManager.UnregisterEvent<GenericMissionEvent>(new Action<GenericMissionEvent>(this.OnGenericMissionEventTriggered));
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0002C7BA File Offset: 0x0002A9BA
		private void OnGenericMissionEventTriggered(GenericMissionEvent missionEvent)
		{
			if (missionEvent.EventId == "open_inventory_with_given_items")
			{
				this.OpenInventoryWithGivenEquipment(missionEvent.Parameter);
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0002C7DC File Offset: 0x0002A9DC
		private void OpenInventoryWithGivenEquipment(string parameters)
		{
			string[] array = parameters.Split(new char[] { ' ' });
			string text = array[0];
			if (!this._openedInventoryItemRosters.ContainsKey(text))
			{
				this._openedInventoryItemRosters.Add(text, new ItemRoster());
				string[] array2 = new string[array.Length - 2];
				Array.Copy(array, 2, array2, 0, array2.Length);
				this.InitializeEventItemRoster(array2, this._openedInventoryItemRosters[text]);
			}
			EventTriggeringUsableMachine firstScriptOfType = Mission.Current.Scene.FindEntityWithTag(text).GetFirstScriptOfType<EventTriggeringUsableMachine>();
			for (int i = 0; i < firstScriptOfType.StandingPoints.Count; i++)
			{
				if (firstScriptOfType.StandingPoints[i].HasUser)
				{
					firstScriptOfType.StandingPoints[i].UserAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
			}
			TextObject descriptionText = firstScriptOfType.DescriptionText;
			string text2 = array[1];
			if (text2.Equals("battle"))
			{
				InventoryScreenHelper.OpenScreenAsReceiveItems(this._openedInventoryItemRosters[text], descriptionText, new Action(this.DoneLogicForBattleEquipmentUpdate));
				return;
			}
			if (text2.Equals("civilian"))
			{
				InventoryScreenHelper.OpenScreenAsReceiveItems(this._openedInventoryItemRosters[text], descriptionText, new Action(this.DoneLogicForCivilianEquipmentUpdate));
				return;
			}
			if (text2.Equals("stealth"))
			{
				InventoryScreenHelper.OpenScreenAsReceiveItems(this._openedInventoryItemRosters[text], descriptionText, new Action(this.DoneLogicForStealthEquipmentUpdate));
				return;
			}
			if (text2.Equals("none"))
			{
				InventoryScreenHelper.OpenScreenAsReceiveItems(this._openedInventoryItemRosters[text], descriptionText, null);
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002C95D File Offset: 0x0002AB5D
		private void DoneLogicForBattleEquipmentUpdate()
		{
			Agent.Main.UpdateSpawnEquipmentAndRefreshVisuals(Hero.MainHero.BattleEquipment);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0002C973 File Offset: 0x0002AB73
		private void DoneLogicForCivilianEquipmentUpdate()
		{
			Agent.Main.UpdateSpawnEquipmentAndRefreshVisuals(Hero.MainHero.CivilianEquipment);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0002C989 File Offset: 0x0002AB89
		private void DoneLogicForStealthEquipmentUpdate()
		{
			Agent.Main.UpdateSpawnEquipmentAndRefreshVisuals(Hero.MainHero.StealthEquipment);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0002C9A0 File Offset: 0x0002ABA0
		private void InitializeEventItemRoster(string[] itemsWithModifiers, ItemRoster eventItemRoster)
		{
			for (int i = 0; i < itemsWithModifiers.Length; i++)
			{
				string[] array = itemsWithModifiers[i].Split(new char[] { ',' });
				string text = array[0];
				string text2 = array[1];
				string text3 = ((array.Length > 2) ? array[2] : "");
				ItemRosterElement itemRosterElement = new ItemRosterElement(MBObjectManager.Instance.GetObject<ItemObject>(text), int.Parse(text2), string.IsNullOrEmpty(text3) ? null : MBObjectManager.Instance.GetObject<ItemModifier>(text3));
				eventItemRoster.Add(itemRosterElement);
			}
		}

		// Token: 0x0400038E RID: 910
		private const string OpenInventoryWithGivenItemsEventId = "open_inventory_with_given_items";

		// Token: 0x0400038F RID: 911
		private readonly Dictionary<string, ItemRoster> _openedInventoryItemRosters = new Dictionary<string, ItemRoster>();
	}
}
