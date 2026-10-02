using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000332 RID: 818
	public class HideoutSpawnPointGroup : SynchedMissionObject
	{
		// Token: 0x06002E0B RID: 11787 RVA: 0x000B1A6C File Offset: 0x000AFC6C
		protected internal override void OnInit()
		{
			base.OnInit();
			this._spawnPoints = new GameEntity[4];
			string spawnPointTagAffix = this.Side.ToString().ToLower() + "_";
			string[] array = new string[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = spawnPointTagAffix + ((FormationClass)i).GetName().ToLower();
			}
			IEnumerable<WeakGameEntity> children = base.GameEntity.GetChildren();
			Func<WeakGameEntity, bool> <>9__0;
			Func<WeakGameEntity, bool> func;
			if ((func = <>9__0) == null)
			{
				Func<string, bool> <>9__1;
				func = (<>9__0 = delegate(WeakGameEntity ce)
				{
					IEnumerable<string> tags = ce.Tags;
					Func<string, bool> func2;
					if ((func2 = <>9__1) == null)
					{
						func2 = (<>9__1 = (string t) => t.StartsWith(spawnPointTagAffix));
					}
					return tags.Any<string>(func2);
				});
			}
			foreach (WeakGameEntity weakGameEntity in children.Where<WeakGameEntity>(func))
			{
				for (int j = 0; j < array.Length; j++)
				{
					if (weakGameEntity.HasTag(array[j]))
					{
						this._spawnPoints[j] = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
						break;
					}
				}
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000B1B80 File Offset: 0x000AFD80
		public MatrixFrame[] GetSpawnPointFrames()
		{
			MatrixFrame[] array = new MatrixFrame[this._spawnPoints.Length];
			for (int i = 0; i < this._spawnPoints.Length; i++)
			{
				array[i] = ((this._spawnPoints[i] != null) ? this._spawnPoints[i].GetGlobalFrame() : MatrixFrame.Identity);
			}
			return array;
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000B1BDC File Offset: 0x000AFDDC
		public void RemoveWithAllChildren()
		{
			base.GameEntity.RemoveAllChildren();
			base.GameEntity.Remove(83);
		}

		// Token: 0x04001235 RID: 4661
		private const int NumberOfDefaultFormations = 4;

		// Token: 0x04001236 RID: 4662
		public BattleSideEnum Side;

		// Token: 0x04001237 RID: 4663
		public int PhaseNumber;

		// Token: 0x04001238 RID: 4664
		private GameEntity[] _spawnPoints;
	}
}
