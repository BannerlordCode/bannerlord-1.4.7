using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Source.Objects
{
	// Token: 0x020003CC RID: 972
	public class SceneLeveler : ScriptComponentBehavior
	{
		// Token: 0x0600362A RID: 13866 RVA: 0x000DF8F8 File Offset: 0x000DDAF8
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(variableName);
			if (num <= 90459893U)
			{
				if (num != 40127036U)
				{
					if (num != 73682274U)
					{
						if (num != 90459893U)
						{
							return;
						}
						if (!(variableName == "CreateLevel2"))
						{
							return;
						}
						this.OnLevelizeButtonPressed(2);
						return;
					}
					else
					{
						if (!(variableName == "CreateLevel3"))
						{
							return;
						}
						this.OnLevelizeButtonPressed(3);
						return;
					}
				}
				else
				{
					if (!(variableName == "CreateLevel1"))
					{
						return;
					}
					this.OnLevelizeButtonPressed(1);
					return;
				}
			}
			else if (num <= 1310461563U)
			{
				if (num != 804927328U)
				{
					if (num != 1310461563U)
					{
						return;
					}
					if (!(variableName == "DeleteLevel1"))
					{
						return;
					}
					this.OnDeleteButtonPressed(1);
					return;
				}
				else
				{
					if (!(variableName == "SelectEntitiesWithoutLevel"))
					{
						return;
					}
					this.OnSelectEntitiesWithoutLevelButtonPressed();
					return;
				}
			}
			else if (num != 1327239182U)
			{
				if (num != 1344016801U)
				{
					return;
				}
				if (!(variableName == "DeleteLevel3"))
				{
					return;
				}
				this.OnDeleteButtonPressed(3);
				return;
			}
			else
			{
				if (!(variableName == "DeleteLevel2"))
				{
					return;
				}
				this.OnDeleteButtonPressed(2);
				return;
			}
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x000DF9F0 File Offset: 0x000DDBF0
		private void OnLevelizeButtonPressed(int level)
		{
			if (this.SourceSelectionSetName.IsEmpty<char>())
			{
				MessageManager.DisplayMessage("ApplyToSelectionSet is empty!");
				return;
			}
			if (this.TargetSelectionSetName.IsEmpty<char>())
			{
				MessageManager.DisplayMessage("NewSelectionSetName is empty!");
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			GameEntity.UpgradeLevelMask levelMask = this.GetLevelMask(level);
			List<GameEntity> list = this.CollectEntitiesWithLevel();
			List<GameEntity> list2 = new List<GameEntity>();
			foreach (GameEntity gameEntity in list)
			{
				string text = this.FindPossiblePrefabName(gameEntity);
				if (text.IsEmpty<char>())
				{
					num++;
				}
				else
				{
					GameEntity.UpgradeLevelMask upgradeLevelMask = gameEntity.GetUpgradeLevelMask();
					if ((upgradeLevelMask & levelMask) != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.None)
					{
						num2++;
						list2.Add(gameEntity);
					}
					else
					{
						string text2 = this.ConvertPrefabName(text, levelMask);
						GameEntity gameEntity2 = TaleWorlds.Engine.GameEntity.Instantiate(base.Scene, text2, gameEntity.GetGlobalFrame(), true);
						if (gameEntity2 == null)
						{
							num3++;
						}
						else
						{
							num4++;
							GameEntity.UpgradeLevelMask upgradeLevelMask2 = upgradeLevelMask & ~TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1 & ~TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2 & ~TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3;
							upgradeLevelMask2 |= levelMask;
							gameEntity2.SetUpgradeLevelMask(upgradeLevelMask2);
							this.CopyScriptParameters(gameEntity2, gameEntity);
							list2.Add(gameEntity2);
						}
					}
				}
			}
			Debug.Print(string.Concat(new object[] { "Created Entities : ", num4, "\nAlready Visible In Desired Level : ", num2, "\nWithout Prefab For Level : ", num3, "\nWithout Prefab Info : ", num }), 0, Debug.DebugColor.Magenta, 17592186044416UL);
			Utilities.CreateSelectionInEditor(list2, this.TargetSelectionSetName);
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x000DFB9C File Offset: 0x000DDD9C
		private void CopyScriptParameters(GameEntity entity, GameEntity copyFromEntity)
		{
			if (copyFromEntity.HasScriptComponent("WallSegment") && !entity.HasScriptComponent("WallSegment"))
			{
				entity.CopyScriptComponentFromAnotherEntity(copyFromEntity, "WallSegment");
			}
			if (copyFromEntity.HasScriptComponent("mesh_bender") && !entity.HasScriptComponent("mesh_bender"))
			{
				entity.CopyScriptComponentFromAnotherEntity(copyFromEntity, "mesh_bender");
			}
			int num = 0;
			while (num < entity.ChildCount && num < copyFromEntity.ChildCount)
			{
				this.CopyScriptParameters(entity.GetChild(num), copyFromEntity.GetChild(num));
				num++;
			}
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000DFC23 File Offset: 0x000DDE23
		private GameEntity.UpgradeLevelMask GetLevelMask(int level)
		{
			if (level == 1)
			{
				return TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1;
			}
			if (level != 2)
			{
				return TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3;
			}
			return TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2;
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000DFC32 File Offset: 0x000DDE32
		private string GetLevelSubString(GameEntity.UpgradeLevelMask levelMask)
		{
			if (levelMask == TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1)
			{
				return "_l1";
			}
			if (levelMask == TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2)
			{
				return "_l2";
			}
			if (levelMask != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3)
			{
				return "";
			}
			return "_l3";
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000DFC5C File Offset: 0x000DDE5C
		private string ConvertPrefabName(string prefabName, GameEntity.UpgradeLevelMask newLevelMask)
		{
			string text = prefabName;
			string levelSubString = this.GetLevelSubString(newLevelMask);
			if (newLevelMask != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1)
			{
				text = text.Replace(this.GetLevelSubString(TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1), levelSubString);
			}
			if (newLevelMask != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2)
			{
				text = text.Replace(this.GetLevelSubString(TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2), levelSubString);
			}
			if (newLevelMask != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3)
			{
				text = text.Replace(this.GetLevelSubString(TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3), levelSubString);
			}
			if (text.Equals(prefabName))
			{
				return "";
			}
			return text;
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000DFCBC File Offset: 0x000DDEBC
		private string FindPossiblePrefabName(GameEntity gameEntity)
		{
			string prefabName = gameEntity.GetPrefabName();
			if (prefabName.IsEmpty<char>())
			{
				return gameEntity.GetOldPrefabName();
			}
			return prefabName;
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000DFCE0 File Offset: 0x000DDEE0
		private void OnDeleteButtonPressed(int level)
		{
			if (this.SourceSelectionSetName.IsEmpty<char>())
			{
				MessageManager.DisplayMessage("ApplyToSelectionSet is empty!");
				return;
			}
			List<GameEntity> list = this.CollectEntitiesWithLevel();
			GameEntity.UpgradeLevelMask levelMask = this.GetLevelMask(level);
			List<GameEntity> list2 = new List<GameEntity>();
			int num = 0;
			int num2 = 0;
			foreach (GameEntity gameEntity in list)
			{
				GameEntity.UpgradeLevelMask upgradeLevelMask = gameEntity.GetUpgradeLevelMask();
				if (upgradeLevelMask == levelMask)
				{
					list2.Add(gameEntity);
					num++;
				}
				else if ((upgradeLevelMask & levelMask) != TaleWorlds.Engine.GameEntity.UpgradeLevelMask.None)
				{
					gameEntity.SetUpgradeLevelMask(upgradeLevelMask & ~levelMask);
					num2++;
				}
			}
			Utilities.DeleteEntitiesInEditorScene(list2);
			TextObject textObject = new TextObject("{=!}Deleted entity count : {DELETED_ENTRY_COUNT}", null);
			TextObject textObject2 = new TextObject("{=!}Removed level mask count : {REMOVED_LEVEL_MASK}", null);
			textObject.SetTextVariable("DELETED_ENTRY_COUNT", num);
			textObject2.SetTextVariable("REMOVED_LEVEL_MASK", num2);
			MessageManager.DisplayMessage(textObject.ToString());
			MessageManager.DisplayMessage(textObject2.ToString());
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000DFDDC File Offset: 0x000DDFDC
		private void OnSelectEntitiesWithoutLevelButtonPressed()
		{
			List<GameEntity> list = new List<GameEntity>();
			base.Scene.GetEntities(ref list);
			List<GameEntity> list2 = list.FindAll((GameEntity x) => x.GetUpgradeLevelMask() == TaleWorlds.Engine.GameEntity.UpgradeLevelMask.None);
			TextObject textObject = new TextObject("{=!}Selected entity count : {SELECTED_ENTITIES}", null);
			textObject.SetTextVariable("SELECTED_ENTITIES", list2.Count);
			MessageManager.DisplayMessage(textObject.ToString());
			if (list2.Count > 0)
			{
				Utilities.SelectEntities(list2);
			}
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000DFE58 File Offset: 0x000DE058
		private List<GameEntity> CollectEntitiesWithLevel()
		{
			List<GameEntity> list = new List<GameEntity>();
			Utilities.GetEntitiesOfSelectionSet(this.SourceSelectionSetName, ref list);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if ((list[i].GetUpgradeLevelMask() & (TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level1 | TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level2 | TaleWorlds.Engine.GameEntity.UpgradeLevelMask.Level3)) == TaleWorlds.Engine.GameEntity.UpgradeLevelMask.None)
				{
					list.RemoveAt(i);
				}
			}
			return list;
		}

		// Token: 0x0400173B RID: 5947
		public string SourceSelectionSetName = "";

		// Token: 0x0400173C RID: 5948
		public string TargetSelectionSetName = "";

		// Token: 0x0400173D RID: 5949
		public SimpleButton CreateLevel1;

		// Token: 0x0400173E RID: 5950
		public SimpleButton CreateLevel2;

		// Token: 0x0400173F RID: 5951
		public SimpleButton CreateLevel3;

		// Token: 0x04001740 RID: 5952
		public SimpleButton DeleteLevel1;

		// Token: 0x04001741 RID: 5953
		public SimpleButton DeleteLevel2;

		// Token: 0x04001742 RID: 5954
		public SimpleButton DeleteLevel3;

		// Token: 0x04001743 RID: 5955
		public SimpleButton SelectEntitiesWithoutLevel;
	}
}
