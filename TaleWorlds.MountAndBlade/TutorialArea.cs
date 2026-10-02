using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000369 RID: 873
	public class TutorialArea : MissionObject
	{
		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06003211 RID: 12817 RVA: 0x000CC280 File Offset: 0x000CA480
		public MBReadOnlyList<TrainingIcon> TrainingIconsReadOnly
		{
			get
			{
				return this._trainingIcons;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003212 RID: 12818 RVA: 0x000CC288 File Offset: 0x000CA488
		// (set) Token: 0x06003213 RID: 12819 RVA: 0x000CC290 File Offset: 0x000CA490
		public TutorialArea.TrainingType TypeOfTraining
		{
			get
			{
				return this._typeOfTraining;
			}
			private set
			{
				this._typeOfTraining = value;
			}
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x000CC299 File Offset: 0x000CA499
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.GatherWeapons();
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x000CC2A8 File Offset: 0x000CA4A8
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (MBEditor.IsEntitySelected(base.GameEntity))
			{
				uint num = 4294901760U;
				using (List<TutorialArea.TutorialEntity>.Enumerator enumerator = this._tagWeapon.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TutorialArea.TutorialEntity tutorialEntity = enumerator.Current;
						foreach (Tuple<GameEntity, MatrixFrame> tuple in tutorialEntity.EntityList)
						{
							tuple.Item1.SetContourColor(new uint?(num), true);
							this._highlightedEntities.Add(tuple.Item1);
						}
					}
					return;
				}
			}
			foreach (GameEntity gameEntity in this._highlightedEntities)
			{
				gameEntity.SetContourColor(null, true);
			}
			this._highlightedEntities.Clear();
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x000CC3C4 File Offset: 0x000CA5C4
		protected internal override void OnInit()
		{
			base.OnInit();
			List<GameEntity> list = new List<GameEntity>();
			base.GameEntity.Scene.GetEntities(ref list);
			foreach (GameEntity gameEntity in list)
			{
				string[] tags = gameEntity.Tags;
				for (int i = 0; i < tags.Length; i++)
				{
					if (tags[i].StartsWith(this._tagPrefix) && gameEntity.HasScriptOfType<WeaponSpawner>())
					{
						gameEntity.GetFirstScriptOfType<WeaponSpawner>().SpawnWeapon();
						break;
					}
				}
			}
			this.GatherWeapons();
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x000CC474 File Offset: 0x000CA674
		public override void AfterMissionStart()
		{
			this.DeactivateAllWeapons(true);
			this.MarkTrainingIcons(false);
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x000CC484 File Offset: 0x000CA684
		private void GatherWeapons()
		{
			List<GameEntity> list = new List<GameEntity>();
			base.GameEntity.Scene.GetEntities(ref list);
			foreach (GameEntity gameEntity in list)
			{
				foreach (string text in gameEntity.Tags)
				{
					TrainingIcon firstScriptOfType = gameEntity.GetFirstScriptOfType<TrainingIcon>();
					if (firstScriptOfType != null)
					{
						if (firstScriptOfType.GetTrainingSubTypeTag().StartsWith(this._tagPrefix))
						{
							this._trainingIcons.Add(firstScriptOfType);
						}
					}
					else if (text == this._tagPrefix + "boundary")
					{
						this.AddBoundary(gameEntity);
					}
					else if (text.StartsWith(this._tagPrefix))
					{
						this.AddTaggedWeapon(gameEntity, text);
					}
				}
			}
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x000CC578 File Offset: 0x000CA778
		public void MarkTrainingIcons(bool mark)
		{
			foreach (TrainingIcon trainingIcon in this._trainingIcons)
			{
				trainingIcon.SetMarked(mark);
			}
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x000CC5CC File Offset: 0x000CA7CC
		public TrainingIcon GetActiveTrainingIcon()
		{
			foreach (TrainingIcon trainingIcon in this._trainingIcons)
			{
				if (trainingIcon.GetIsActivated())
				{
					return trainingIcon;
				}
			}
			return null;
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x000CC628 File Offset: 0x000CA828
		private void AddBoundary(GameEntity boundary)
		{
			this._boundaries.Add(boundary);
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x000CC638 File Offset: 0x000CA838
		private void AddTaggedWeapon(GameEntity weapon, string tag)
		{
			if (weapon.HasScriptOfType<VolumeBox>())
			{
				this._volumeBoxes.Add(weapon.GetFirstScriptOfType<VolumeBox>());
				return;
			}
			bool flag = false;
			foreach (TutorialArea.TutorialEntity tutorialEntity in this._tagWeapon)
			{
				if (tutorialEntity.Tag == tag)
				{
					tutorialEntity.EntityList.Add(Tuple.Create<GameEntity, MatrixFrame>(weapon, weapon.GetGlobalFrame()));
					if (weapon.HasScriptOfType<DestructableComponent>())
					{
						tutorialEntity.DestructableComponents.Add(weapon.GetFirstScriptOfType<DestructableComponent>());
					}
					else if (weapon.HasScriptOfType<SpawnedItemEntity>())
					{
						tutorialEntity.WeaponList.Add(weapon);
						tutorialEntity.WeaponNames.Add(MBObjectManager.Instance.GetObject<ItemObject>(weapon.GetFirstScriptOfType<SpawnedItemEntity>().WeaponCopy.Item.StringId));
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this._tagWeapon.Add(new TutorialArea.TutorialEntity(tag, new List<Tuple<GameEntity, MatrixFrame>> { Tuple.Create<GameEntity, MatrixFrame>(weapon, weapon.GetGlobalFrame()) }, new List<DestructableComponent>(), new List<GameEntity>(), new List<ItemObject>()));
				if (weapon.HasScriptOfType<DestructableComponent>())
				{
					this._tagWeapon[this._tagWeapon.Count - 1].DestructableComponents.Add(weapon.GetFirstScriptOfType<DestructableComponent>());
					return;
				}
				if (weapon.HasScriptOfType<SpawnedItemEntity>())
				{
					this._tagWeapon[this._tagWeapon.Count - 1].WeaponList.Add(weapon);
					this._tagWeapon[this._tagWeapon.Count - 1].WeaponNames.Add(MBObjectManager.Instance.GetObject<ItemObject>(weapon.GetFirstScriptOfType<SpawnedItemEntity>().WeaponCopy.Item.StringId));
				}
			}
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x000CC808 File Offset: 0x000CAA08
		public int GetIndexFromTag(string tag)
		{
			for (int i = 0; i < this._tagWeapon.Count; i++)
			{
				if (this._tagWeapon[i].Tag == tag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x000CC848 File Offset: 0x000CAA48
		public List<string> GetSubTrainingTags()
		{
			List<string> list = new List<string>();
			foreach (TutorialArea.TutorialEntity tutorialEntity in this._tagWeapon)
			{
				list.Add(tutorialEntity.Tag);
			}
			return list;
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x000CC8A8 File Offset: 0x000CAAA8
		public void ActivateTaggedWeapons(int index)
		{
			if (index >= this._tagWeapon.Count)
			{
				return;
			}
			this.DeactivateAllWeapons(false);
			foreach (Tuple<GameEntity, MatrixFrame> tuple in this._tagWeapon[index].EntityList)
			{
				tuple.Item1.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x000CC920 File Offset: 0x000CAB20
		public void EquipWeaponsToPlayer(int index)
		{
			foreach (GameEntity gameEntity in this._tagWeapon[index].WeaponList)
			{
				bool flag;
				Agent.Main.OnItemPickup(gameEntity.GetFirstScriptOfType<SpawnedItemEntity>(), EquipmentIndex.None, out flag);
			}
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x000CC98C File Offset: 0x000CAB8C
		public void DeactivateAllWeapons(bool resetDestructibles)
		{
			foreach (TutorialArea.TutorialEntity tutorialEntity in this._tagWeapon)
			{
				if (resetDestructibles)
				{
					foreach (DestructableComponent destructableComponent in tutorialEntity.DestructableComponents)
					{
						destructableComponent.Reset();
						destructableComponent.HitPoint = 1000000f;
						Markable firstScriptOfType = destructableComponent.GameEntity.GetFirstScriptOfType<Markable>();
						if (firstScriptOfType != null)
						{
							firstScriptOfType.DisableMarkerActivation();
						}
					}
				}
				foreach (Tuple<GameEntity, MatrixFrame> tuple in tutorialEntity.EntityList)
				{
					if (!tuple.Item1.HasScriptOfType<DestructableComponent>())
					{
						if (tuple.Item1.HasScriptOfType<SpawnedItemEntity>())
						{
							tuple.Item1.GetFirstScriptOfType<SpawnedItemEntity>().StopPhysicsAndSetFrameForClient(tuple.Item2, null);
							tuple.Item1.GetFirstScriptOfType<SpawnedItemEntity>().HasLifeTime = false;
						}
						GameEntity item = tuple.Item1;
						MatrixFrame item2 = tuple.Item2;
						item.SetGlobalFrame(in item2, true);
					}
					tuple.Item1.SetVisibilityExcludeParents(false);
				}
			}
			this.HideBoundaries();
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x000CCB18 File Offset: 0x000CAD18
		public void ActivateBoundaries()
		{
			if (this._boundariesHidden)
			{
				foreach (GameEntity gameEntity in this._boundaries)
				{
					gameEntity.SetVisibilityExcludeParents(true);
				}
				this._boundariesHidden = false;
			}
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000CCB78 File Offset: 0x000CAD78
		public void HideBoundaries()
		{
			if (!this._boundariesHidden)
			{
				foreach (GameEntity gameEntity in this._boundaries)
				{
					gameEntity.SetVisibilityExcludeParents(false);
				}
				this._boundariesHidden = true;
			}
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x000CCBD8 File Offset: 0x000CADD8
		public int GetBreakablesCount(int index)
		{
			return this._tagWeapon[index].DestructableComponents.Count;
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x000CCBF0 File Offset: 0x000CADF0
		public void MakeDestructible(int index)
		{
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				this._tagWeapon[index].DestructableComponents[i].HitPoint = this._tagWeapon[index].DestructableComponents[i].MaxHitPoint;
			}
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x000CCC58 File Offset: 0x000CAE58
		public void MarkAllTargets(int index, bool mark)
		{
			foreach (DestructableComponent destructableComponent in this._tagWeapon[index].DestructableComponents)
			{
				if (mark)
				{
					Markable firstScriptOfType = destructableComponent.GameEntity.GetFirstScriptOfType<Markable>();
					if (firstScriptOfType != null)
					{
						firstScriptOfType.ActivateMarkerFor(3f, 10f);
					}
				}
				else
				{
					Markable firstScriptOfType2 = destructableComponent.GameEntity.GetFirstScriptOfType<Markable>();
					if (firstScriptOfType2 != null)
					{
						firstScriptOfType2.DisableMarkerActivation();
					}
				}
			}
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x000CCCF0 File Offset: 0x000CAEF0
		public void ResetMarkingTargetTimers(int index)
		{
			foreach (DestructableComponent destructableComponent in this._tagWeapon[index].DestructableComponents)
			{
				Markable firstScriptOfType = destructableComponent.GameEntity.GetFirstScriptOfType<Markable>();
				if (firstScriptOfType != null)
				{
					firstScriptOfType.ResetPassiveDurationTimer();
				}
			}
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x000CCD60 File Offset: 0x000CAF60
		public void MakeInDestructible(int index)
		{
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				this._tagWeapon[index].DestructableComponents[i].HitPoint = 1000000f;
			}
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x000CCDB0 File Offset: 0x000CAFB0
		public bool AllBreakablesAreBroken(int index)
		{
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				if (!this._tagWeapon[index].DestructableComponents[i].IsDestroyed)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x000CCE00 File Offset: 0x000CB000
		public int GetBrokenBreakableCount(int index)
		{
			int num = 0;
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				if (this._tagWeapon[index].DestructableComponents[i].IsDestroyed)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x000CCE54 File Offset: 0x000CB054
		public int GetUnbrokenBreakableCount(int index)
		{
			int num = 0;
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				if (!this._tagWeapon[index].DestructableComponents[i].IsDestroyed)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x000CCEA8 File Offset: 0x000CB0A8
		public void ResetBreakables(int index, bool makeIndestructible = true)
		{
			for (int i = 0; i < this._tagWeapon[index].DestructableComponents.Count; i++)
			{
				if (makeIndestructible)
				{
					this._tagWeapon[index].DestructableComponents[i].HitPoint = 1000000f;
				}
				this._tagWeapon[index].DestructableComponents[i].Reset();
			}
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x000CCF18 File Offset: 0x000CB118
		public bool HasMainAgentPickedAll(int index)
		{
			using (List<GameEntity>.Enumerator enumerator = this._tagWeapon[index].WeaponList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasScriptOfType<SpawnedItemEntity>())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x000CCF7C File Offset: 0x000CB17C
		public void CheckMainAgentEquipment(int index)
		{
			this._allowedWeaponsHelper.Clear();
			this._allowedWeaponsHelper.AddRange(this._tagWeapon[index].WeaponNames);
			EquipmentIndex i;
			EquipmentIndex j;
			for (i = EquipmentIndex.WeaponItemBeginSlot; i <= EquipmentIndex.Weapon3; i = j + 1)
			{
				if (!Mission.Current.MainAgent.Equipment[i].IsEmpty)
				{
					if (this._allowedWeaponsHelper.Exists((ItemObject x) => x == Mission.Current.MainAgent.Equipment[i].Item))
					{
						this._allowedWeaponsHelper.Remove(Mission.Current.MainAgent.Equipment[i].Item);
					}
					else
					{
						Mission.Current.MainAgent.DropItem(i, WeaponClass.Undefined);
						MBInformationManager.AddQuickInformation(new TextObject("{=3PP01vFv}Keep away from that weapon.", null), 0, null, null, "");
					}
				}
				j = i;
			}
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x000CD07C File Offset: 0x000CB27C
		public void CheckWeapons(int index)
		{
			foreach (GameEntity gameEntity in this._tagWeapon[index].WeaponList)
			{
				if (gameEntity.HasScriptOfType<SpawnedItemEntity>())
				{
					gameEntity.GetFirstScriptOfType<SpawnedItemEntity>().HasLifeTime = false;
				}
			}
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x000CD0E8 File Offset: 0x000CB2E8
		public bool IsPositionInsideTutorialArea(Vec3 position, out string[] volumeBoxTags)
		{
			foreach (VolumeBox volumeBox in this._volumeBoxes)
			{
				if (volumeBox.IsPointIn(position))
				{
					volumeBoxTags = volumeBox.GameEntity.Tags;
					return true;
				}
			}
			volumeBoxTags = null;
			return false;
		}

		// Token: 0x04001538 RID: 5432
		[EditableScriptComponentVariable(true, "")]
		private TutorialArea.TrainingType _typeOfTraining;

		// Token: 0x04001539 RID: 5433
		[EditableScriptComponentVariable(true, "")]
		private string _tagPrefix = "A_";

		// Token: 0x0400153A RID: 5434
		private readonly List<TutorialArea.TutorialEntity> _tagWeapon = new List<TutorialArea.TutorialEntity>();

		// Token: 0x0400153B RID: 5435
		private readonly List<VolumeBox> _volumeBoxes = new List<VolumeBox>();

		// Token: 0x0400153C RID: 5436
		private readonly List<GameEntity> _boundaries = new List<GameEntity>();

		// Token: 0x0400153D RID: 5437
		private bool _boundariesHidden;

		// Token: 0x0400153E RID: 5438
		private readonly List<GameEntity> _highlightedEntities = new List<GameEntity>();

		// Token: 0x0400153F RID: 5439
		private readonly List<ItemObject> _allowedWeaponsHelper = new List<ItemObject>();

		// Token: 0x04001540 RID: 5440
		private readonly MBList<TrainingIcon> _trainingIcons = new MBList<TrainingIcon>();

		// Token: 0x02000645 RID: 1605
		public enum TrainingType
		{
			// Token: 0x0400212B RID: 8491
			Bow,
			// Token: 0x0400212C RID: 8492
			Melee,
			// Token: 0x0400212D RID: 8493
			Mounted,
			// Token: 0x0400212E RID: 8494
			AdvancedMelee
		}

		// Token: 0x02000646 RID: 1606
		private struct TutorialEntity
		{
			// Token: 0x0600402E RID: 16430 RVA: 0x000F8461 File Offset: 0x000F6661
			public TutorialEntity(string tag, List<Tuple<GameEntity, MatrixFrame>> entityList, List<DestructableComponent> destructableComponents, List<GameEntity> weapon, List<ItemObject> weaponNames)
			{
				this.Tag = tag;
				this.EntityList = entityList;
				this.DestructableComponents = destructableComponents;
				this.WeaponList = weapon;
				this.WeaponNames = weaponNames;
			}

			// Token: 0x0400212F RID: 8495
			public string Tag;

			// Token: 0x04002130 RID: 8496
			public List<Tuple<GameEntity, MatrixFrame>> EntityList;

			// Token: 0x04002131 RID: 8497
			public List<DestructableComponent> DestructableComponents;

			// Token: 0x04002132 RID: 8498
			public List<GameEntity> WeaponList;

			// Token: 0x04002133 RID: 8499
			public List<ItemObject> WeaponNames;
		}
	}
}
