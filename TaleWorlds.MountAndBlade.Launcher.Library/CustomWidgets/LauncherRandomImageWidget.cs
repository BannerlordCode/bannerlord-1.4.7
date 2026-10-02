using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Launcher.Library.CustomWidgets
{
	// Token: 0x02000026 RID: 38
	public class LauncherRandomImageWidget : Widget
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00006DFA File Offset: 0x00004FFA
		public LauncherRandomImageWidget(UIContext context)
			: base(context)
		{
			this._random = new Random();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006E10 File Offset: 0x00005010
		private void ShuffleList<T>(List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i];
				int num = this._random.Next(i, list.Count);
				list[i] = list[num];
				list[num] = t;
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006E60 File Offset: 0x00005060
		private void CreateIndicesList()
		{
			this._imageIndices = new List<int>();
			for (int i = 0; i < this.ImageCount; i++)
			{
				this._imageIndices.Add(i);
			}
			this.ShuffleList<int>(this._imageIndices);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006EA4 File Offset: 0x000050A4
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			this.CreateIndicesList();
			int num = this._imageIndices[this._currentIndex];
			base.Sprite = base.Context.SpriteData.GetSprite("ConceptArts\\ConceptArt_" + num);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006EF8 File Offset: 0x000050F8
		private void TriggerChanged()
		{
			this._currentIndex = (this._currentIndex + 1) % this._imageIndices.Count;
			int num = this._imageIndices[this._currentIndex];
			base.Sprite = base.Context.SpriteData.GetSprite("ConceptArts\\ConceptArt_" + num);
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00006F57 File Offset: 0x00005157
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00006F5F File Offset: 0x0000515F
		[DataSourceProperty]
		public int ImageCount
		{
			get
			{
				return this._imageCount;
			}
			set
			{
				if (value != this._imageCount)
				{
					this._imageCount = value;
					base.OnPropertyChanged(value, "ImageCount");
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006F7D File Offset: 0x0000517D
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00006F85 File Offset: 0x00005185
		[DataSourceProperty]
		public bool ChangeTrigger
		{
			get
			{
				return this._changeTrigger;
			}
			set
			{
				if (value != this._changeTrigger)
				{
					this._changeTrigger = value;
					base.OnPropertyChanged(value, "ChangeTrigger");
					this.TriggerChanged();
				}
			}
		}

		// Token: 0x040000B7 RID: 183
		private readonly Random _random;

		// Token: 0x040000B8 RID: 184
		private List<int> _imageIndices;

		// Token: 0x040000B9 RID: 185
		private int _currentIndex;

		// Token: 0x040000BA RID: 186
		private int _imageCount;

		// Token: 0x040000BB RID: 187
		private bool _changeTrigger;
	}
}
