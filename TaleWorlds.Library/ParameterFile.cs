using System;
using System.IO;
using System.Xml;

namespace TaleWorlds.Library
{
	// Token: 0x02000077 RID: 119
	public class ParameterFile
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0000F194 File Offset: 0x0000D394
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x0000F19C File Offset: 0x0000D39C
		public string Path { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x0000F1A5 File Offset: 0x0000D3A5
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x0000F1AD File Offset: 0x0000D3AD
		public DateTime LastCheckedTime { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x0000F1B6 File Offset: 0x0000D3B6
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x0000F1BE File Offset: 0x0000D3BE
		public ParameterContainer ParameterContainer { get; private set; }

		// Token: 0x0600044F RID: 1103 RVA: 0x0000F1C7 File Offset: 0x0000D3C7
		public ParameterFile(string path)
		{
			this.ParameterContainer = new ParameterContainer();
			this.Path = path;
			this.LastCheckedTime = DateTime.MinValue;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000F1EC File Offset: 0x0000D3EC
		public bool CheckIfNeedsToBeRefreshed()
		{
			return File.GetLastWriteTime(this.Path) > this.LastCheckedTime;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000F204 File Offset: 0x0000D404
		public void Refresh()
		{
			this.ParameterContainer.ClearParameters();
			DateTime lastWriteTime = File.GetLastWriteTime(this.Path);
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(this.Path);
			}
			catch
			{
				this._failedAttemptsCount++;
				if (this._failedAttemptsCount >= 100)
				{
					Debug.FailedAssert("Could not load parameters file", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\ParameterFile.cs", "Refresh", 47);
				}
				return;
			}
			this._failedAttemptsCount = 0;
			foreach (object obj in xmlDocument.FirstChild.ChildNodes)
			{
				XmlElement xmlElement = (XmlElement)obj;
				string attribute = xmlElement.GetAttribute("name");
				string attribute2 = xmlElement.GetAttribute("value");
				this.ParameterContainer.AddParameter(attribute, attribute2, true);
			}
			this.LastCheckedTime = lastWriteTime;
		}

		// Token: 0x04000152 RID: 338
		private int _failedAttemptsCount;

		// Token: 0x04000153 RID: 339
		private const int MaxFailedAttemptsCount = 100;
	}
}
