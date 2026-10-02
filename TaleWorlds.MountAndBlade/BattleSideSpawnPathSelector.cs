using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200020C RID: 524
	public class BattleSideSpawnPathSelector
	{
		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x00068891 File Offset: 0x00066A91
		public SpawnPathData InitialSpawnPath
		{
			get
			{
				return this._initialSpawnPath;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x00068899 File Offset: 0x00066A99
		public MBReadOnlyList<SpawnPathData> ReinforcementPaths
		{
			get
			{
				return this._reinforcementSpawnPaths;
			}
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000688A4 File Offset: 0x00066AA4
		public BattleSideSpawnPathSelector(Mission mission, Path initialPath, float initialPivotOffset, bool initialPathIsInverted)
		{
			this._mission = mission;
			SpawnPathData.SnapMethod snapMethod = (mission.IsNavalBattle ? SpawnPathData.SnapMethod.SnapToWaterLevel : (mission.IsFieldBattle ? SpawnPathData.SnapMethod.SnapToTerrain : SpawnPathData.SnapMethod.DontSnap));
			this._initialSpawnPath = SpawnPathData.Create(this._mission.Scene, initialPath, initialPivotOffset, initialPathIsInverted, snapMethod);
			this._reinforcementSpawnPaths = new MBList<SpawnPathData>();
			this.FindReinforcementPaths();
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x00068904 File Offset: 0x00066B04
		public bool HasReinforcementPath(Path path)
		{
			return path != null && this._reinforcementSpawnPaths.Exists((SpawnPathData pathData) => pathData.Path.Pointer == path.Pointer);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x00068948 File Offset: 0x00066B48
		private void FindReinforcementPaths()
		{
			this._reinforcementSpawnPaths.Clear();
			float num = this._initialSpawnPath.PathLength / 2f;
			SpawnPathData spawnPathData = SpawnPathData.Create(this._initialSpawnPath.Scene, this._initialSpawnPath.Path, num, this._initialSpawnPath.IsInverted, SpawnPathData.SnapMethod.DontSnap);
			this._reinforcementSpawnPaths.Add(spawnPathData);
			MBList<Path> allSpawnPaths = MBSceneUtilities.GetAllSpawnPaths(this._mission.Scene);
			if (allSpawnPaths.Count == 0)
			{
				return;
			}
			bool flag = false;
			if (allSpawnPaths.Count > 1)
			{
				MatrixFrame[] array = new MatrixFrame[100];
				spawnPathData.Path.GetPoints(array);
				MatrixFrame matrixFrame = (spawnPathData.IsInverted ? array[spawnPathData.Path.NumberOfPoints - 1] : array[0]);
				SortedList<float, SpawnPathData> sortedList = new SortedList<float, SpawnPathData>();
				foreach (Path path in allSpawnPaths)
				{
					if (path.NumberOfPoints > 1 && path.Pointer != spawnPathData.Path.Pointer)
					{
						path.GetPoints(array);
						MatrixFrame matrixFrame2 = array[0];
						MatrixFrame matrixFrame3 = array[path.NumberOfPoints - 1];
						float num2 = matrixFrame2.origin.DistanceSquared(matrixFrame.origin);
						float num3 = matrixFrame3.origin.DistanceSquared(matrixFrame.origin);
						float num4 = path.GetTotalLength() / 2f;
						sortedList.Add(num2, SpawnPathData.Create(this._initialSpawnPath.Scene, path, num4, false, SpawnPathData.SnapMethod.DontSnap));
						sortedList.Add(num3, SpawnPathData.Create(this._initialSpawnPath.Scene, path, num4, true, SpawnPathData.SnapMethod.DontSnap));
					}
					else
					{
						flag = flag || spawnPathData.Path.Pointer == path.Pointer;
					}
				}
				int num5 = 0;
				using (IEnumerator<KeyValuePair<float, SpawnPathData>> enumerator2 = sortedList.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<float, SpawnPathData> keyValuePair = enumerator2.Current;
						this._reinforcementSpawnPaths.Add(keyValuePair.Value);
						num5++;
						if ((float)num5 >= 2f)
						{
							break;
						}
					}
					return;
				}
			}
			flag = spawnPathData.Path.Pointer == allSpawnPaths[0].Pointer;
		}

		// Token: 0x04000A61 RID: 2657
		public const float MaxNeighborCount = 2f;

		// Token: 0x04000A62 RID: 2658
		private readonly Mission _mission;

		// Token: 0x04000A63 RID: 2659
		private readonly SpawnPathData _initialSpawnPath;

		// Token: 0x04000A64 RID: 2660
		private readonly MBList<SpawnPathData> _reinforcementSpawnPaths;
	}
}
