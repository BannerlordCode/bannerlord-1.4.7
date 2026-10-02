using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000067 RID: 103
	public class MapColorGradeManager : ScriptComponentBehavior
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x0001DC0C File Offset: 0x0001BE0C
		private void Init()
		{
			if (base.Scene.ContainsTerrain)
			{
				Vec2i vec2i;
				float num;
				int num2;
				int num3;
				base.Scene.GetTerrainData(out vec2i, out num, out num2, out num3);
				this.terrainSize.x = (float)vec2i.X * num;
				this.terrainSize.y = (float)vec2i.Y * num;
			}
			this.colorGradeGridMapping.Add(1, this.defaultColorGradeTextureName);
			this.colorGradeGridMapping.Add(2, "worldmap_colorgrade_night");
			this.ReadColorGradesXml();
			MBMapScene.GetColorGradeGridData(base.Scene, this.colorGradeGrid, this.colorGradeGridName);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001DCA1 File Offset: 0x0001BEA1
		protected override void OnInit()
		{
			base.OnInit();
			this.Init();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001DCAF File Offset: 0x0001BEAF
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
			this.Init();
			this.TimeOfDay = base.Scene.TimeOfDay;
			this.lastSceneTimeOfDay = this.TimeOfDay;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001DCDA File Offset: 0x0001BEDA
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001DCDD File Offset: 0x0001BEDD
		protected override void OnTick(float dt)
		{
			this.TimeOfDay = base.Scene.TimeOfDay;
			this.SeasonTimeFactor = MBMapScene.GetSeasonTimeFactor(base.Scene);
			this.ApplyAtmosphere(false);
			this.ApplyColorGrade(dt);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001DD10 File Offset: 0x0001BF10
		protected override void OnEditorTick(float dt)
		{
			if (base.Scene.TimeOfDay != this.lastSceneTimeOfDay)
			{
				this.TimeOfDay = base.Scene.TimeOfDay;
				this.lastSceneTimeOfDay = this.TimeOfDay;
			}
			if (base.Scene.ContainsTerrain)
			{
				Vec2i vec2i;
				float num;
				int num2;
				int num3;
				base.Scene.GetTerrainData(out vec2i, out num, out num2, out num3);
				this.terrainSize.x = (float)vec2i.X * num;
				this.terrainSize.y = (float)vec2i.Y * num;
			}
			else
			{
				this.terrainSize.x = 1f;
				this.terrainSize.y = 1f;
			}
			if (this.AtmosphereSimulationEnabled)
			{
				this.TimeOfDay += dt;
				if (this.TimeOfDay >= 24f)
				{
					this.TimeOfDay -= 24f;
				}
				this.ApplyAtmosphere(false);
			}
			if (this.ColorGradeEnabled)
			{
				this.ApplyColorGrade(dt);
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001DE04 File Offset: 0x0001C004
		protected override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "ColorGradeEnabled")
			{
				if (!this.ColorGradeEnabled)
				{
					base.Scene.SetColorGradeBlend("", "", -1f);
					this.lastColorGrade = 0;
					return;
				}
			}
			else
			{
				if (variableName == "TimeOfDay")
				{
					this.ApplyAtmosphere(false);
					return;
				}
				if (variableName == "SeasonTimeFactor")
				{
					this.ApplyAtmosphere(false);
				}
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001DE78 File Offset: 0x0001C078
		private void ReadColorGradesXml()
		{
			List<string> list;
			XmlDocument mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_worldmap_color_grades", out list);
			if (mergedXmlForNative == null)
			{
				return;
			}
			XmlNode xmlNode = mergedXmlForNative.SelectSingleNode("worldmap_color_grades");
			if (xmlNode == null)
			{
				return;
			}
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("color_grade_grid");
			if (xmlNode2 != null && xmlNode2.Attributes["name"] != null)
			{
				this.colorGradeGridName = xmlNode2.Attributes["name"].Value;
			}
			XmlNode xmlNode3 = xmlNode.SelectSingleNode("color_grade_default");
			if (xmlNode3 != null && xmlNode3.Attributes["name"] != null)
			{
				this.defaultColorGradeTextureName = xmlNode3.Attributes["name"].Value;
				this.colorGradeGridMapping[1] = this.defaultColorGradeTextureName;
			}
			XmlNode xmlNode4 = xmlNode.SelectSingleNode("color_grade_night");
			if (xmlNode4 != null && xmlNode4.Attributes["name"] != null)
			{
				this.colorGradeGridMapping[2] = xmlNode4.Attributes["name"].Value;
			}
			XmlNodeList xmlNodeList = xmlNode.SelectNodes("color_grade");
			if (xmlNodeList != null)
			{
				foreach (object obj in xmlNodeList)
				{
					XmlNode xmlNode5 = (XmlNode)obj;
					byte b;
					if (xmlNode5.Attributes["name"] != null && xmlNode5.Attributes["value"] != null && byte.TryParse(xmlNode5.Attributes["value"].Value, out b))
					{
						this.colorGradeGridMapping[b] = xmlNode5.Attributes["name"].Value;
					}
				}
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001E040 File Offset: 0x0001C240
		public void ApplyAtmosphere(bool forceLoadTextures)
		{
			this.TimeOfDay = MBMath.ClampFloat(this.TimeOfDay, 0f, 23.99f);
			this.SeasonTimeFactor = MBMath.ClampFloat(this.SeasonTimeFactor, 0f, 1f);
			MBMapScene.SetFrameForAtmosphere(base.Scene, this.TimeOfDay * 10f, base.Scene.LastFinalRenderCameraFrame.origin.z, forceLoadTextures);
			float num = 0.55f;
			float num2 = -0.1f;
			float seasonTimeFactor = this.SeasonTimeFactor;
			Vec3 vec = new Vec3(0f, 0.65f, 0f, -1f);
			vec.x = MBMath.Lerp(num, num2, seasonTimeFactor, 1E-05f);
			MBMapScene.SetTerrainDynamicParams(base.Scene, vec);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001E100 File Offset: 0x0001C300
		public void ApplyColorGrade(float dt)
		{
			Vec3 origin = base.Scene.LastFinalRenderCameraFrame.origin;
			float num = 1f;
			int num2 = MathF.Floor(origin.x / this.terrainSize.X * 512f);
			int num3 = MathF.Floor(origin.y / this.terrainSize.Y * 512f);
			num2 = MBMath.ClampIndex(num2, 0, 512);
			num3 = MBMath.ClampIndex(num3, 0, 512);
			byte b = this.colorGradeGrid[num3 * 512 + num2];
			if (origin.z > 400f)
			{
				b = 1;
			}
			if (this.TimeOfDay > 22f || this.TimeOfDay < 2f)
			{
				b = 2;
			}
			if (MBMapScene.GetApplyRainColorGrade() && origin.z < 50f)
			{
				b = 160;
				num = 0.2f;
			}
			if (this.lastColorGrade != b)
			{
				string text = "";
				string text2 = "";
				if (!this.colorGradeGridMapping.TryGetValue(this.lastColorGrade, out text))
				{
					text = this.defaultColorGradeTextureName;
				}
				if (!this.colorGradeGridMapping.TryGetValue(b, out text2))
				{
					text2 = this.defaultColorGradeTextureName;
				}
				if (this.primaryTransitionRecord == null)
				{
					this.primaryTransitionRecord = new MapColorGradeManager.ColorGradeBlendRecord
					{
						color1 = text,
						color2 = text2,
						alpha = 0f
					};
				}
				else
				{
					this.secondaryTransitionRecord = new MapColorGradeManager.ColorGradeBlendRecord
					{
						color1 = this.primaryTransitionRecord.color2,
						color2 = text2,
						alpha = 0f
					};
				}
				this.lastColorGrade = b;
			}
			if (this.primaryTransitionRecord != null)
			{
				if (this.primaryTransitionRecord.alpha < 1f)
				{
					this.primaryTransitionRecord.alpha = MathF.Min(this.primaryTransitionRecord.alpha + dt * (1f / num), 1f);
					base.Scene.SetColorGradeBlend(this.primaryTransitionRecord.color1, this.primaryTransitionRecord.color2, this.primaryTransitionRecord.alpha);
					return;
				}
				this.primaryTransitionRecord = null;
				if (this.secondaryTransitionRecord != null)
				{
					this.primaryTransitionRecord = new MapColorGradeManager.ColorGradeBlendRecord(this.secondaryTransitionRecord);
					this.secondaryTransitionRecord = null;
				}
			}
		}

		// Token: 0x0400024D RID: 589
		public bool ColorGradeEnabled;

		// Token: 0x0400024E RID: 590
		public bool AtmosphereSimulationEnabled;

		// Token: 0x0400024F RID: 591
		public float TimeOfDay;

		// Token: 0x04000250 RID: 592
		public float SeasonTimeFactor;

		// Token: 0x04000251 RID: 593
		private string colorGradeGridName = "worldmap_colorgrade_grid";

		// Token: 0x04000252 RID: 594
		private const int colorGradeGridSize = 262144;

		// Token: 0x04000253 RID: 595
		private byte[] colorGradeGrid = new byte[262144];

		// Token: 0x04000254 RID: 596
		private Dictionary<byte, string> colorGradeGridMapping = new Dictionary<byte, string>();

		// Token: 0x04000255 RID: 597
		private MapColorGradeManager.ColorGradeBlendRecord primaryTransitionRecord;

		// Token: 0x04000256 RID: 598
		private MapColorGradeManager.ColorGradeBlendRecord secondaryTransitionRecord;

		// Token: 0x04000257 RID: 599
		private byte lastColorGrade;

		// Token: 0x04000258 RID: 600
		private Vec2 terrainSize = new Vec2(1f, 1f);

		// Token: 0x04000259 RID: 601
		private string defaultColorGradeTextureName = "worldmap_colorgrade_stratosphere";

		// Token: 0x0400025A RID: 602
		private const float transitionSpeedFactor = 1f;

		// Token: 0x0400025B RID: 603
		private float lastSceneTimeOfDay;

		// Token: 0x020000D9 RID: 217
		private class ColorGradeBlendRecord
		{
			// Token: 0x06000640 RID: 1600 RVA: 0x0002ADCC File Offset: 0x00028FCC
			public ColorGradeBlendRecord()
			{
				this.color1 = "";
				this.color2 = "";
				this.alpha = 0f;
			}

			// Token: 0x06000641 RID: 1601 RVA: 0x0002ADF5 File Offset: 0x00028FF5
			public ColorGradeBlendRecord(MapColorGradeManager.ColorGradeBlendRecord other)
			{
				this.color1 = other.color1;
				this.color2 = other.color2;
				this.alpha = other.alpha;
			}

			// Token: 0x040003CE RID: 974
			public string color1;

			// Token: 0x040003CF RID: 975
			public string color2;

			// Token: 0x040003D0 RID: 976
			public float alpha;
		}
	}
}
