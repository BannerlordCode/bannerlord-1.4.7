using System;
using System.Collections.Generic;
using System.Xml;

namespace TaleWorlds.Engine
{
	// Token: 0x02000075 RID: 117
	public class PerformanceAnalyzer
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x0000AD68 File Offset: 0x00008F68
		public void Start(string name)
		{
			PerformanceAnalyzer.PerformanceObject performanceObject = new PerformanceAnalyzer.PerformanceObject(name);
			this.currentObject = performanceObject;
			this.objects.Add(performanceObject);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0000AD8F File Offset: 0x00008F8F
		public void End()
		{
			this.currentObject = null;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0000AD98 File Offset: 0x00008F98
		public void FinalizeAndWrite(string filePath)
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlNode xmlNode = xmlDocument.CreateElement("objects");
				xmlDocument.AppendChild(xmlNode);
				foreach (PerformanceAnalyzer.PerformanceObject performanceObject in this.objects)
				{
					XmlNode xmlNode2 = xmlDocument.CreateElement("object");
					performanceObject.Write(xmlNode2, xmlDocument);
					xmlNode.AppendChild(xmlNode2);
				}
				xmlDocument.Save(filePath);
			}
			catch (Exception ex)
			{
				MBDebug.ShowWarning("Exception occurred while trying to write " + filePath + ": " + ex.ToString());
			}
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0000AE50 File Offset: 0x00009050
		public void Tick(float dt)
		{
			if (this.currentObject != null)
			{
				this.currentObject.AddFps(Utilities.GetFps(), Utilities.GetMainFps(), Utilities.GetRendererFps());
			}
		}

		// Token: 0x04000161 RID: 353
		private List<PerformanceAnalyzer.PerformanceObject> objects = new List<PerformanceAnalyzer.PerformanceObject>();

		// Token: 0x04000162 RID: 354
		private PerformanceAnalyzer.PerformanceObject currentObject;

		// Token: 0x020000CD RID: 205
		private class PerformanceObject
		{
			// Token: 0x170000D2 RID: 210
			// (get) Token: 0x06000FFD RID: 4093 RVA: 0x00014922 File Offset: 0x00012B22
			private float AverageMainFps
			{
				get
				{
					if (this.frameCount > 0)
					{
						return this.totalMainFps / (float)this.frameCount;
					}
					return 0f;
				}
			}

			// Token: 0x170000D3 RID: 211
			// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00014941 File Offset: 0x00012B41
			private float AverageRendererFps
			{
				get
				{
					if (this.frameCount > 0)
					{
						return this.totalRendererFps / (float)this.frameCount;
					}
					return 0f;
				}
			}

			// Token: 0x170000D4 RID: 212
			// (get) Token: 0x06000FFF RID: 4095 RVA: 0x00014960 File Offset: 0x00012B60
			private float AverageFps
			{
				get
				{
					if (this.frameCount > 0)
					{
						return this.totalFps / (float)this.frameCount;
					}
					return 0f;
				}
			}

			// Token: 0x06001000 RID: 4096 RVA: 0x0001497F File Offset: 0x00012B7F
			public void AddFps(float fps, float main, float renderer)
			{
				this.frameCount++;
				this.totalFps += fps;
				this.totalMainFps += main;
				this.totalRendererFps += renderer;
			}

			// Token: 0x06001001 RID: 4097 RVA: 0x000149BC File Offset: 0x00012BBC
			public void Write(XmlNode node, XmlDocument document)
			{
				XmlAttribute xmlAttribute = document.CreateAttribute("name");
				xmlAttribute.Value = this.name;
				node.Attributes.Append(xmlAttribute);
				XmlAttribute xmlAttribute2 = document.CreateAttribute("frameCount");
				xmlAttribute2.Value = this.frameCount.ToString();
				node.Attributes.Append(xmlAttribute2);
				XmlAttribute xmlAttribute3 = document.CreateAttribute("averageFps");
				xmlAttribute3.Value = this.AverageFps.ToString();
				node.Attributes.Append(xmlAttribute3);
				XmlAttribute xmlAttribute4 = document.CreateAttribute("averageMainFps");
				xmlAttribute4.Value = this.AverageMainFps.ToString();
				node.Attributes.Append(xmlAttribute4);
				XmlAttribute xmlAttribute5 = document.CreateAttribute("averageRendererFps");
				xmlAttribute5.Value = this.AverageRendererFps.ToString();
				node.Attributes.Append(xmlAttribute5);
			}

			// Token: 0x06001002 RID: 4098 RVA: 0x00014AA5 File Offset: 0x00012CA5
			public PerformanceObject(string objectName)
			{
				this.name = objectName;
			}

			// Token: 0x04000435 RID: 1077
			private string name;

			// Token: 0x04000436 RID: 1078
			private int frameCount;

			// Token: 0x04000437 RID: 1079
			private float totalMainFps;

			// Token: 0x04000438 RID: 1080
			private float totalRendererFps;

			// Token: 0x04000439 RID: 1081
			private float totalFps;
		}
	}
}
