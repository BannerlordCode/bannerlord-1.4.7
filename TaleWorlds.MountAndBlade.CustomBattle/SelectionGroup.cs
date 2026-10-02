using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x0200000E RID: 14
	public class SelectionGroup : ViewModel
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00007B58 File Offset: 0x00005D58
		public SelectionGroup(string name, List<string> textList = null)
		{
			this._name = name;
			if (textList != null)
			{
				this._textList = textList;
			}
			this.Text = ((this._textList.Count > 0) ? this._textList[0] : "");
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007BB0 File Offset: 0x00005DB0
		protected virtual void ClickSelectionLeft()
		{
			this._index--;
			if (this._index < 0)
			{
				this._index = this._textList.Count - 1;
			}
			this.Text = ((this._textList.Count > 0) ? this._textList[this._index] : "");
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00007C14 File Offset: 0x00005E14
		protected virtual void ClickSelectionRight()
		{
			this._index++;
			this._index %= this._textList.Count;
			this.Text = ((this._textList.Count > 0) ? this._textList[this._index] : "");
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00007C73 File Offset: 0x00005E73
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00007C7B File Offset: 0x00005E7B
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00007C9E File Offset: 0x00005E9E
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00007CA6 File Offset: 0x00005EA6
		public List<string> TextList
		{
			get
			{
				return this._textList;
			}
			set
			{
				if (value != this._textList)
				{
					this._textList = value;
					this.Text = ((this._textList.Count > 0) ? this._textList[this._index] : "");
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00007CE4 File Offset: 0x00005EE4
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00007CEC File Offset: 0x00005EEC
		public int Index
		{
			get
			{
				return this._index;
			}
			private set
			{
				value = this._index;
			}
		}

		// Token: 0x0400007C RID: 124
		protected List<string> _textList = new List<string>();

		// Token: 0x0400007D RID: 125
		private int _index;

		// Token: 0x0400007E RID: 126
		private string _name;

		// Token: 0x0400007F RID: 127
		private string _text;
	}
}
