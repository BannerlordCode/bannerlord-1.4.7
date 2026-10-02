using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200000D RID: 13
	public class RichTextTag
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00004ABE File Offset: 0x00002CBE
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00004AC6 File Offset: 0x00002CC6
		public string Name { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00004ACF File Offset: 0x00002CCF
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00004AD7 File Offset: 0x00002CD7
		public RichTextTagType Type { get; set; }

		// Token: 0x06000089 RID: 137 RVA: 0x00004AE0 File Offset: 0x00002CE0
		public RichTextTag(string name)
		{
			this.Name = name;
			this._attributes = new Dictionary<string, string>();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004AFA File Offset: 0x00002CFA
		public void AddAtrribute(string key, string value)
		{
			this._attributes.Add(key, value);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004B09 File Offset: 0x00002D09
		public string GetAttribute(string key)
		{
			if (this._attributes.ContainsKey(key))
			{
				return this._attributes[key];
			}
			return "";
		}

		// Token: 0x04000054 RID: 84
		private Dictionary<string, string> _attributes;
	}
}
