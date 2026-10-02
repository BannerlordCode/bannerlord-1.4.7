using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200002A RID: 42
	public class AtmosphereGrid
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x000138D8 File Offset: 0x00011AD8
		public void Initialize()
		{
			this.states = Campaign.Current.MapSceneWrapper.GetAtmosphereStates().ToList<AtmosphereState>();
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000138F4 File Offset: 0x00011AF4
		public AtmosphereState GetInterpolatedStateInfo(Vec3 pos)
		{
			AtmosphereGrid.<>c__DisplayClass3_0 CS$<>8__locals1 = new AtmosphereGrid.<>c__DisplayClass3_0();
			CS$<>8__locals1.pos = pos;
			List<AtmosphereGrid.AtmosphereStateSortData> list = new List<AtmosphereGrid.AtmosphereStateSortData>();
			int num = 0;
			foreach (AtmosphereState atmosphereState in this.states)
			{
				list.Add(new AtmosphereGrid.AtmosphereStateSortData
				{
					Position = atmosphereState.Position,
					InitialIndex = num++
				});
			}
			AtmosphereGrid.<>c__DisplayClass3_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.pos.z = CS$<>8__locals2.pos.z * 0.3f;
			list.Sort((AtmosphereGrid.AtmosphereStateSortData x, AtmosphereGrid.AtmosphereStateSortData y) => x.Position.Distance(CS$<>8__locals1.pos).CompareTo(y.Position.Distance(CS$<>8__locals1.pos)));
			AtmosphereState atmosphereState2 = new AtmosphereState();
			float num2 = 0f;
			bool flag = true;
			string text = "color_grade_empire_harsh";
			atmosphereState2.ColorGradeTexture = text;
			foreach (AtmosphereGrid.AtmosphereStateSortData atmosphereStateSortData in list)
			{
				AtmosphereState atmosphereState3 = this.states[atmosphereStateSortData.InitialIndex];
				float num3 = atmosphereState3.Position.Distance(CS$<>8__locals1.pos);
				float num4 = 1f - MBMath.SmoothStep(atmosphereState3.distanceForMaxWeight, atmosphereState3.distanceForMinWeight, num3);
				if ((double)num4 >= 0.001)
				{
					if (flag)
					{
						text = atmosphereState3.ColorGradeTexture;
					}
					atmosphereState2.HumidityAverage += atmosphereState3.HumidityAverage * num4;
					atmosphereState2.HumidityVariance += atmosphereState3.HumidityVariance * num4;
					atmosphereState2.TemperatureAverage += atmosphereState3.TemperatureAverage * num4;
					atmosphereState2.TemperatureVariance += atmosphereState3.TemperatureVariance * num4;
					num2 += num4;
					flag = false;
				}
			}
			if (num2 > 0f)
			{
				atmosphereState2.ColorGradeTexture = text;
				atmosphereState2.HumidityAverage /= num2;
				atmosphereState2.HumidityVariance /= num2;
				atmosphereState2.TemperatureAverage /= num2;
				atmosphereState2.TemperatureVariance /= num2;
			}
			return atmosphereState2;
		}

		// Token: 0x0400001E RID: 30
		private List<AtmosphereState> states = new List<AtmosphereState>();

		// Token: 0x020004F6 RID: 1270
		private struct AtmosphereStateSortData
		{
			// Token: 0x04001567 RID: 5479
			public Vec3 Position;

			// Token: 0x04001568 RID: 5480
			public int InitialIndex;
		}
	}
}
