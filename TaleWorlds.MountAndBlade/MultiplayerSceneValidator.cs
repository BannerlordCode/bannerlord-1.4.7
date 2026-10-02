using System;
using System.Collections.Generic;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000337 RID: 823
	public class MultiplayerSceneValidator : ScriptComponentBehavior
	{
		// Token: 0x06002E2A RID: 11818 RVA: 0x000B26C6 File Offset: 0x000B08C6
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "SelectFaultyEntities")
			{
				this.SelectInvalidEntities();
			}
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000B26E4 File Offset: 0x000B08E4
		protected internal override void OnSceneSave(string saveFolder)
		{
			base.OnSceneSave(saveFolder);
			foreach (GameEntity gameEntity in this.GetInvalidEntities())
			{
			}
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000B2738 File Offset: 0x000B0938
		private List<GameEntity> GetInvalidEntities()
		{
			List<GameEntity> list = new List<GameEntity>();
			List<GameEntity> list2 = new List<GameEntity>();
			base.Scene.GetEntities(ref list2);
			foreach (GameEntity gameEntity in list2)
			{
				foreach (ScriptComponentBehavior scriptComponentBehavior in gameEntity.GetScriptComponents())
				{
					if (scriptComponentBehavior != null && (scriptComponentBehavior.GetType().IsSubclassOf(typeof(MissionObject)) || (scriptComponentBehavior.GetType() == typeof(MissionObject) && scriptComponentBehavior.IsOnlyVisual())))
					{
						list.Add(gameEntity);
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x000B2820 File Offset: 0x000B0A20
		private void SelectInvalidEntities()
		{
			base.GameEntity.DeselectEntityOnEditor();
			foreach (GameEntity gameEntity in this.GetInvalidEntities())
			{
				gameEntity.SelectEntityOnEditor();
			}
		}

		// Token: 0x04001256 RID: 4694
		public SimpleButton SelectFaultyEntities;
	}
}
