using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000077 RID: 119
	public class MissionItemContourControllerView : MissionView
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x000216D4 File Offset: 0x0001F8D4
		private static bool IsAllowedByOption
		{
			get
			{
				return !BannerlordConfig.HideBattleUI || GameNetwork.IsMultiplayer;
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000216E4 File Offset: 0x0001F8E4
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (MissionItemContourControllerView.IsAllowedByOption)
			{
				if (Agent.Main != null && base.MissionScreen.InputManager.IsGameKeyDown(5))
				{
					this.RemoveContourFromAllItems();
					this.PopulateContourListWithNearbyItems();
					this.ApplyContourToAllItems();
					this._lastItemQueryTime = base.Mission.CurrentTime;
				}
				else
				{
					this.RemoveContourFromAllItems();
					this._contourItems.Clear();
				}
				if (this._isContourAppliedToAllItems)
				{
					float currentTime = base.Mission.CurrentTime;
					if (currentTime - this._lastItemQueryTime > 1f)
					{
						this.RemoveContourFromAllItems();
						this.PopulateContourListWithNearbyItems();
						this._lastItemQueryTime = currentTime;
					}
				}
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00021788 File Offset: 0x0001F988
		public override void OnFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
		{
			base.OnFocusGained(agent, focusableObject, isInteractable);
			if (MissionItemContourControllerView.IsAllowedByOption && focusableObject != this._currentFocusedObject && isInteractable)
			{
				this._currentFocusedObject = focusableObject;
				UsableMissionObject usableMissionObject;
				if ((usableMissionObject = focusableObject as UsableMissionObject) != null)
				{
					SpawnedItemEntity spawnedItemEntity;
					if ((spawnedItemEntity = usableMissionObject as SpawnedItemEntity) != null)
					{
						this._focusedGameEntity = GameEntity.CreateFromWeakEntity(spawnedItemEntity.GameEntity);
					}
					else if (!string.IsNullOrEmpty(usableMissionObject.ActionMessage.ToString()) && !string.IsNullOrEmpty(usableMissionObject.DescriptionMessage.ToString()))
					{
						this._focusedGameEntity = GameEntity.CreateFromWeakEntity(usableMissionObject.GameEntity);
					}
					else
					{
						UsableMachine usableMachineFromPoint = this.GetUsableMachineFromPoint(usableMissionObject);
						if (usableMachineFromPoint != null)
						{
							this._focusedGameEntity = GameEntity.CreateFromWeakEntity(usableMachineFromPoint.GameEntity);
						}
					}
				}
				this.AddContourToFocusedItem();
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00021842 File Offset: 0x0001FA42
		public override void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
			base.OnFocusLost(agent, focusableObject);
			if (MissionItemContourControllerView.IsAllowedByOption)
			{
				this.RemoveContourFromFocusedItem();
				this._currentFocusedObject = null;
				this._focusedGameEntity = null;
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00021868 File Offset: 0x0001FA68
		private void PopulateContourListWithNearbyItems()
		{
			this._contourItems.Clear();
			float num = (GameNetwork.IsSessionActive ? 1f : 3f);
			Agent main = Agent.Main;
			float num2 = main.GetMaximumForwardUnlimitedSpeed() * num;
			Vec3 vec = main.Position - new Vec3(num2, num2, 1f, -1f);
			Vec3 vec2 = main.Position + new Vec3(num2, num2, 2.5f, -1f);
			Vec3 position = base.MissionScreen.CombatCamera.Position;
			Vec3 position2 = main.Position;
			float num3 = new Vec3(position.x, position.y, 0f, -1f).Distance(new Vec3(position2.x, position2.y, 0f, -1f));
			Vec3 vec3 = position * (1f - num3) + (position + base.MissionScreen.CombatCamera.Direction) * num3;
			int num4 = base.Mission.Scene.SelectEntitiesInBoxWithScriptComponent<SpawnedItemEntity>(ref vec, ref vec2, this._tempPickableEntities, this._pickableItemsId, false);
			for (int i = 0; i < num4; i++)
			{
				WeakGameEntity weakGameEntity = this._tempPickableEntities[i];
				SpawnedItemEntity firstScriptOfType = weakGameEntity.GetFirstScriptOfType<SpawnedItemEntity>();
				if (firstScriptOfType != null)
				{
					Vec3 vec4 = weakGameEntity.ComputeGlobalPhysicsBoundingBoxCenter();
					Vec3 vec5 = (vec4 - vec3).NormalizedCopy();
					Vec3 globalPosition = weakGameEntity.GlobalPosition;
					Vec3 vec6 = (globalPosition - vec3).NormalizedCopy();
					float num5;
					WeakGameEntity weakGameEntity2;
					WeakGameEntity weakGameEntity3;
					if ((base.Mission.Scene.RayCastForClosestEntityOrTerrain(vec3 + vec5 * 0.2f, vec4, out num5, out weakGameEntity2, 0.2f, BodyFlags.CommonFocusRayCastExcludeFlags) && weakGameEntity2.IsValid && weakGameEntity2 == weakGameEntity) || (base.Mission.Scene.RayCastForClosestEntityOrTerrain(vec3 + vec6 * 0.2f, globalPosition, out num5, out weakGameEntity3, 0.2f, BodyFlags.CommonFocusRayCastExcludeFlags) && weakGameEntity3.IsValid && weakGameEntity3 == weakGameEntity))
					{
						if (firstScriptOfType.IsBanner())
						{
							if (MissionGameModels.Current.BattleBannerBearersModel.IsInteractableFormationBanner(firstScriptOfType, main))
							{
								this._contourItems.Add(GameEntity.CreateFromWeakEntity(weakGameEntity));
							}
						}
						else
						{
							this._contourItems.Add(GameEntity.CreateFromWeakEntity(weakGameEntity));
						}
					}
				}
			}
			int num6 = base.Mission.Scene.SelectEntitiesInBoxWithScriptComponent<SpawnedItemEntity>(ref vec, ref vec2, this._tempPickableEntities, this._pickableItemsId, true);
			for (int j = 0; j < num6; j++)
			{
				WeakGameEntity weakGameEntity4 = this._tempPickableEntities[j];
				SpawnedItemEntity firstScriptOfType2 = weakGameEntity4.GetFirstScriptOfType<SpawnedItemEntity>();
				if (firstScriptOfType2 != null)
				{
					Vec3 vec7 = weakGameEntity4.ComputeGlobalPhysicsBoundingBoxCenter();
					Vec3 vec8 = (vec7 - vec3).NormalizedCopy();
					Vec3 globalPosition2 = weakGameEntity4.GlobalPosition;
					Vec3 vec9 = (globalPosition2 - vec3).NormalizedCopy();
					float num5;
					WeakGameEntity weakGameEntity5;
					WeakGameEntity weakGameEntity6;
					if ((base.Mission.Scene.RayCastForClosestEntityOrTerrainFixedPhysics(vec3 + vec8 * 0.2f, vec7, out num5, out weakGameEntity5, 0.2f, BodyFlags.CommonFocusRayCastExcludeFlags) && weakGameEntity5.IsValid && weakGameEntity5 == weakGameEntity4) || (base.Mission.Scene.RayCastForClosestEntityOrTerrainFixedPhysics(vec3 + vec9 * 0.2f, globalPosition2, out num5, out weakGameEntity6, 0.2f, BodyFlags.CommonFocusRayCastExcludeFlags) && weakGameEntity6.IsValid && weakGameEntity6 == weakGameEntity4))
					{
						if (firstScriptOfType2.IsBanner())
						{
							if (MissionGameModels.Current.BattleBannerBearersModel.IsInteractableFormationBanner(firstScriptOfType2, main))
							{
								this._contourItems.Add(GameEntity.CreateFromWeakEntity(weakGameEntity4));
							}
						}
						else
						{
							this._contourItems.Add(GameEntity.CreateFromWeakEntity(weakGameEntity4));
						}
					}
				}
			}
			int num7 = base.Mission.Scene.SelectEntitiesInBoxWithScriptComponent<UsableMachine>(ref vec, ref vec2, this._tempPickableEntities, this._pickableItemsId, false);
			for (int k = 0; k < num7; k++)
			{
				WeakGameEntity weakGameEntity7 = this._tempPickableEntities[k];
				UsableMachine firstScriptOfType3 = weakGameEntity7.GetFirstScriptOfType<UsableMachine>();
				if (firstScriptOfType3 != null && !firstScriptOfType3.IsDisabled)
				{
					WeakGameEntity validStandingPointForAgentWithoutDistanceCheck = firstScriptOfType3.GetValidStandingPointForAgentWithoutDistanceCheck(main);
					if (validStandingPointForAgentWithoutDistanceCheck.IsValid && !(validStandingPointForAgentWithoutDistanceCheck.GetFirstScriptOfType<UsableMissionObject>() is SpawnedItemEntity))
					{
						IFocusable focusable;
						if ((focusable = validStandingPointForAgentWithoutDistanceCheck.GetScriptComponents().FirstOrDefault<ScriptComponentBehavior>((ScriptComponentBehavior sc) => sc is IFocusable) as IFocusable) != null && focusable is UsableMissionObject)
						{
							this._contourItems.Add(GameEntity.CreateFromWeakEntity(weakGameEntity7));
						}
					}
				}
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00021D14 File Offset: 0x0001FF14
		private void ApplyContourToAllItems()
		{
			if (!this._isContourAppliedToAllItems)
			{
				foreach (GameEntity gameEntity in this._contourItems)
				{
					uint nonFocusedColor = this.GetNonFocusedColor(gameEntity);
					uint num = ((gameEntity == this._focusedGameEntity) ? this._focusedContourColor : nonFocusedColor);
					gameEntity.SetContourColor(new uint?(num), true);
				}
				this._isContourAppliedToAllItems = true;
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00021D9C File Offset: 0x0001FF9C
		private uint GetNonFocusedColor(GameEntity entity)
		{
			SpawnedItemEntity firstScriptOfType = entity.GetFirstScriptOfType<SpawnedItemEntity>();
			ItemObject itemObject = ((firstScriptOfType != null) ? firstScriptOfType.WeaponCopy.Item : null);
			WeaponComponentData weaponComponentData = ((itemObject != null) ? itemObject.PrimaryWeapon : null);
			ItemObject.ItemTypeEnum? itemTypeEnum = ((itemObject != null) ? new ItemObject.ItemTypeEnum?(itemObject.ItemType) : null);
			if (itemObject != null && itemObject.HasBannerComponent)
			{
				return this._nonFocusedBannerContourColor;
			}
			if (weaponComponentData == null || !weaponComponentData.IsAmmo)
			{
				ItemObject.ItemTypeEnum? itemTypeEnum2 = itemTypeEnum;
				ItemObject.ItemTypeEnum itemTypeEnum3 = ItemObject.ItemTypeEnum.Arrows;
				if (!((itemTypeEnum2.GetValueOrDefault() == itemTypeEnum3) & (itemTypeEnum2 != null)))
				{
					itemTypeEnum2 = itemTypeEnum;
					itemTypeEnum3 = ItemObject.ItemTypeEnum.Bolts;
					if (!((itemTypeEnum2.GetValueOrDefault() == itemTypeEnum3) & (itemTypeEnum2 != null)))
					{
						itemTypeEnum2 = itemTypeEnum;
						itemTypeEnum3 = ItemObject.ItemTypeEnum.SlingStones;
						if (!((itemTypeEnum2.GetValueOrDefault() == itemTypeEnum3) & (itemTypeEnum2 != null)))
						{
							itemTypeEnum2 = itemTypeEnum;
							itemTypeEnum3 = ItemObject.ItemTypeEnum.Bullets;
							if (!((itemTypeEnum2.GetValueOrDefault() == itemTypeEnum3) & (itemTypeEnum2 != null)))
							{
								itemTypeEnum2 = itemTypeEnum;
								itemTypeEnum3 = ItemObject.ItemTypeEnum.Thrown;
								if ((itemTypeEnum2.GetValueOrDefault() == itemTypeEnum3) & (itemTypeEnum2 != null))
								{
									return this._nonFocusedThrowableContourColor;
								}
								return this._nonFocusedDefaultContourColor;
							}
						}
					}
				}
			}
			return this._nonFocusedAmmoContourColor;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00021EA4 File Offset: 0x000200A4
		private void RemoveContourFromAllItems()
		{
			if (this._isContourAppliedToAllItems)
			{
				foreach (GameEntity gameEntity in this._contourItems)
				{
					if (this._focusedGameEntity == null || gameEntity != this._focusedGameEntity)
					{
						gameEntity.SetContourColor(null, true);
					}
				}
				this._isContourAppliedToAllItems = false;
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00021F2C File Offset: 0x0002012C
		private void AddContourToFocusedItem()
		{
			if (this._focusedGameEntity != null && !this._isContourAppliedToFocusedItem)
			{
				this._focusedGameEntity.SetContourColor(new uint?(this._focusedContourColor), true);
				this._isContourAppliedToFocusedItem = true;
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00021F64 File Offset: 0x00020164
		private void RemoveContourFromFocusedItem()
		{
			if (this._focusedGameEntity != null && this._isContourAppliedToFocusedItem)
			{
				if (this._contourItems.Contains(this._focusedGameEntity))
				{
					this._focusedGameEntity.SetContourColor(new uint?(this._nonFocusedDefaultContourColor), true);
				}
				else
				{
					this._focusedGameEntity.SetContourColor(null, true);
				}
				this._isContourAppliedToFocusedItem = false;
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00021FD0 File Offset: 0x000201D0
		private UsableMachine GetUsableMachineFromPoint(UsableMissionObject standingPoint)
		{
			WeakGameEntity weakGameEntity = standingPoint.GameEntity;
			while (weakGameEntity.IsValid && !weakGameEntity.HasScriptOfType<UsableMachine>())
			{
				weakGameEntity = weakGameEntity.Parent;
			}
			if (weakGameEntity.IsValid)
			{
				UsableMachine firstScriptOfType = weakGameEntity.GetFirstScriptOfType<UsableMachine>();
				if (firstScriptOfType != null)
				{
					return firstScriptOfType;
				}
			}
			return null;
		}

		// Token: 0x0400028E RID: 654
		private const float SceneItemQueryFreq = 1f;

		// Token: 0x0400028F RID: 655
		private readonly WeakGameEntity[] _tempPickableEntities = new WeakGameEntity[128];

		// Token: 0x04000290 RID: 656
		private readonly UIntPtr[] _pickableItemsId = new UIntPtr[128];

		// Token: 0x04000291 RID: 657
		private readonly List<GameEntity> _contourItems = new List<GameEntity>();

		// Token: 0x04000292 RID: 658
		private GameEntity _focusedGameEntity;

		// Token: 0x04000293 RID: 659
		private IFocusable _currentFocusedObject;

		// Token: 0x04000294 RID: 660
		private bool _isContourAppliedToAllItems;

		// Token: 0x04000295 RID: 661
		private bool _isContourAppliedToFocusedItem;

		// Token: 0x04000296 RID: 662
		private readonly uint _nonFocusedDefaultContourColor = new Color(0.85f, 0.85f, 0.85f, 1f).ToUnsignedInteger();

		// Token: 0x04000297 RID: 663
		private readonly uint _nonFocusedAmmoContourColor = new Color(0f, 0.73f, 1f, 1f).ToUnsignedInteger();

		// Token: 0x04000298 RID: 664
		private readonly uint _nonFocusedThrowableContourColor = new Color(0.051f, 0.988f, 0.18f, 1f).ToUnsignedInteger();

		// Token: 0x04000299 RID: 665
		private readonly uint _nonFocusedBannerContourColor = new Color(0.521f, 0.988f, 0.521f, 1f).ToUnsignedInteger();

		// Token: 0x0400029A RID: 666
		private readonly uint _focusedContourColor = new Color(1f, 0.84f, 0.35f, 1f).ToUnsignedInteger();

		// Token: 0x0400029B RID: 667
		private float _lastItemQueryTime;
	}
}
