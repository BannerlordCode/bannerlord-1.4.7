using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200028C RID: 652
	public class MissionBoundaryPlacer : MissionLogic
	{
		// Token: 0x0600244B RID: 9291 RVA: 0x00083E08 File Offset: 0x00082008
		public override void EarlyStart()
		{
			this.AddMissionBoundaries();
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x00083E10 File Offset: 0x00082010
		public void AddMissionBoundaries()
		{
			MBList<Vec2> softBoundaryPoints = MBSceneUtilities.GetSoftBoundaryPoints(base.Mission.Scene);
			if (softBoundaryPoints.Count == 0)
			{
				Vec3 vec;
				Vec3 vec2;
				base.Mission.Scene.GetBoundingBox(out vec, out vec2);
				float num = MathF.Min(2f, vec2.x - vec.x);
				float num2 = MathF.Min(2f, vec2.y - vec.y);
				List<Vec2> list = new List<Vec2>
				{
					new Vec2(vec.x + num, vec.y + num2),
					new Vec2(vec2.x - num, vec.y + num2),
					new Vec2(vec2.x - num, vec2.y - num2),
					new Vec2(vec.x + num, vec2.y - num2)
				};
				softBoundaryPoints.AddRange(list);
			}
			base.Mission.Boundaries.Add("walk_area", softBoundaryPoints);
		}
	}
}
