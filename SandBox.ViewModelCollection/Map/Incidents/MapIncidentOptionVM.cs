using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Map.Incidents
{
	// Token: 0x0200004C RID: 76
	public class MapIncidentOptionVM : ViewModel
	{
		// Token: 0x060004B2 RID: 1202 RVA: 0x000125AE File Offset: 0x000107AE
		public MapIncidentOptionVM(TextObject description, List<TextObject> hints, int index, Action<MapIncidentOptionVM> onSelected, Action<MapIncidentOptionVM> onFocused)
		{
			this.Index = index;
			this._descriptionText = description;
			this._hints = hints.ToList<TextObject>();
			this._onSelected = onSelected;
			this._onFocused = onFocused;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000125E0 File Offset: 0x000107E0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Description = this._descriptionText.ToString();
			this.Hint = CampaignUIHelper.MergeTextObjectsWithNewline(this._hints);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001260A File Offset: 0x0001080A
		public override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00012612 File Offset: 0x00010812
		public void ExecuteSelect()
		{
			this._onSelected(this);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00012620 File Offset: 0x00010820
		public void ExecuteFocus()
		{
			this._onFocused(this);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001262E File Offset: 0x0001082E
		public void ExecuteUnfocus()
		{
			this._onFocused(null);
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0001263C File Offset: 0x0001083C
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x00012644 File Offset: 0x00010844
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x00012662 File Offset: 0x00010862
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x0001266A File Offset: 0x0001086A
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x00012688 File Offset: 0x00010888
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x00012690 File Offset: 0x00010890
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x000126B3 File Offset: 0x000108B3
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x000126BB File Offset: 0x000108BB
		[DataSourceProperty]
		public string Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<string>(value, "Hint");
				}
			}
		}

		// Token: 0x04000252 RID: 594
		public readonly int Index;

		// Token: 0x04000253 RID: 595
		private readonly TextObject _descriptionText;

		// Token: 0x04000254 RID: 596
		private readonly List<TextObject> _hints;

		// Token: 0x04000255 RID: 597
		private readonly Action<MapIncidentOptionVM> _onSelected;

		// Token: 0x04000256 RID: 598
		private readonly Action<MapIncidentOptionVM> _onFocused;

		// Token: 0x04000257 RID: 599
		private bool _isSelected;

		// Token: 0x04000258 RID: 600
		private bool _isFocused;

		// Token: 0x04000259 RID: 601
		private string _description;

		// Token: 0x0400025A RID: 602
		private string _hint;
	}
}
