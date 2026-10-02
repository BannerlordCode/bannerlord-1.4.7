using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x0200002E RID: 46
	[DefaultView]
	public class MissionGauntletCategoryLoadManager : MissionView, IMissionListener
	{
		// Token: 0x060001DC RID: 476 RVA: 0x0000ADB0 File Offset: 0x00008FB0
		public override void AfterStart()
		{
			base.AfterStart();
			if (this._fullBackgroundCategory == null)
			{
				this._fullBackgroundCategory = UIResourceManager.GetSpriteCategory("ui_fullbackgrounds");
			}
			if (this._encyclopediaCategory == null)
			{
				this._encyclopediaCategory = UIResourceManager.GetSpriteCategory("ui_encyclopedia");
			}
			if (this._mapBarCategory == null)
			{
				SpriteCategory spriteCategory = UIResourceManager.GetSpriteCategory("ui_mapbar");
				if (spriteCategory != null && spriteCategory.IsLoaded)
				{
					this._mapBarCategory = spriteCategory;
				}
			}
			if (this._optionsView == null)
			{
				this._optionsView = base.Mission.GetMissionBehavior<MissionGauntletOptionsUIHandler>();
				base.Mission.AddListener(this);
			}
			this.HandleCategoryLoadingUnloading();
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000AE43 File Offset: 0x00009043
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this._optionsView = null;
			base.Mission.RemoveListener(this);
			this.LoadUnloadAllCategories(true);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000AE65 File Offset: 0x00009065
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			this.HandleCategoryLoadingUnloading();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000AE74 File Offset: 0x00009074
		private void HandleCategoryLoadingUnloading()
		{
			bool flag = true;
			if (base.Mission != null)
			{
				flag = this.IsBackgroundsUsedInMission(base.Mission);
			}
			this.LoadUnloadAllCategories(flag);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000AEA0 File Offset: 0x000090A0
		private void LoadUnloadAllCategories(bool load)
		{
			if (load)
			{
				if (!this._fullBackgroundCategory.IsLoaded)
				{
					this._fullBackgroundCategory.Load(UIResourceManager.ResourceContext, UIResourceManager.ResourceDepot);
				}
				if (!this._encyclopediaCategory.IsLoaded)
				{
					this._encyclopediaCategory.Load(UIResourceManager.ResourceContext, UIResourceManager.ResourceDepot);
				}
				SpriteCategory mapBarCategory = this._mapBarCategory;
				if (mapBarCategory != null && !mapBarCategory.IsLoaded)
				{
					this._mapBarCategory.Load(UIResourceManager.ResourceContext, UIResourceManager.ResourceDepot);
					return;
				}
			}
			else
			{
				if (this._fullBackgroundCategory.IsLoaded)
				{
					this._fullBackgroundCategory.Unload();
				}
				if (this._encyclopediaCategory.IsLoaded)
				{
					Mission mission = base.Mission;
					if (mission == null || mission.Mode != MissionMode.Conversation)
					{
						this._encyclopediaCategory.Unload();
					}
				}
				SpriteCategory mapBarCategory2 = this._mapBarCategory;
				if (mapBarCategory2 != null && mapBarCategory2.IsLoaded)
				{
					this._mapBarCategory.Unload();
				}
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000AF8A File Offset: 0x0000918A
		private bool IsBackgroundsUsedInMission(Mission mission)
		{
			return mission.IsInventoryAccessAllowed || mission.IsCharacterWindowAccessAllowed || mission.IsClanWindowAccessAllowed || mission.IsKingdomWindowAccessAllowed || mission.IsQuestScreenAccessAllowed || mission.IsPartyWindowAccessAllowed || mission.IsEncyclopediaWindowAccessAllowed;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000AFC4 File Offset: 0x000091C4
		void IMissionListener.OnEquipItemsFromSpawnEquipmentBegin(Agent agent, Agent.CreationType creationType)
		{
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000AFC6 File Offset: 0x000091C6
		void IMissionListener.OnEquipItemsFromSpawnEquipment(Agent agent, Agent.CreationType creationType)
		{
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000AFC8 File Offset: 0x000091C8
		void IMissionListener.OnEndMission()
		{
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000AFCA File Offset: 0x000091CA
		void IMissionListener.OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			this.HandleCategoryLoadingUnloading();
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000AFD2 File Offset: 0x000091D2
		void IMissionListener.OnConversationCharacterChanged()
		{
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000AFD4 File Offset: 0x000091D4
		void IMissionListener.OnResetMission()
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000AFD6 File Offset: 0x000091D6
		void IMissionListener.OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
		}

		// Token: 0x040000F1 RID: 241
		private SpriteCategory _fullBackgroundCategory;

		// Token: 0x040000F2 RID: 242
		private SpriteCategory _mapBarCategory;

		// Token: 0x040000F3 RID: 243
		private SpriteCategory _encyclopediaCategory;

		// Token: 0x040000F4 RID: 244
		private MissionGauntletOptionsUIHandler _optionsView;
	}
}
