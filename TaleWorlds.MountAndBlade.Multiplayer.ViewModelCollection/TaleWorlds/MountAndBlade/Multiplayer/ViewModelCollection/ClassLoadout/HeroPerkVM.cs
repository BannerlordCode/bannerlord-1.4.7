using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A5 RID: 165
	public class HeroPerkVM : ViewModel
	{
		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x00030A4E File Offset: 0x0002EC4E
		// (set) Token: 0x06000FCB RID: 4043 RVA: 0x00030A56 File Offset: 0x0002EC56
		public IReadOnlyPerkObject SelectedPerk { get; private set; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x00030A5F File Offset: 0x0002EC5F
		// (set) Token: 0x06000FCD RID: 4045 RVA: 0x00030A67 File Offset: 0x0002EC67
		public MPPerkVM SelectedPerkItem { get; private set; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00030A70 File Offset: 0x0002EC70
		public int PerkIndex { get; }

		// Token: 0x06000FCF RID: 4047 RVA: 0x00030A78 File Offset: 0x0002EC78
		public HeroPerkVM(Action<HeroPerkVM, MPPerkVM> onSelectPerk, IReadOnlyPerkObject perk, List<IReadOnlyPerkObject> candidatePerks, int perkIndex)
		{
			HeroPerkVM <>4__this = this;
			this.Hint = new BasicTooltipViewModel(() => <>4__this.SelectedPerkItem.Description);
			this.CandidatePerks = new MBBindingList<MPPerkVM>();
			this.PerkIndex = perkIndex;
			this._onSelectPerk = onSelectPerk;
			for (int i = 0; i < candidatePerks.Count; i++)
			{
				IReadOnlyPerkObject readOnlyPerkObject = candidatePerks[i];
				bool flag = readOnlyPerkObject != perk;
				this.CandidatePerks.Add(new MPPerkVM(new Action<MPPerkVM>(this.OnSelectPerk), readOnlyPerkObject, flag, i));
			}
			this.OnSelectPerk(this.CandidatePerks.SingleOrDefault<MPPerkVM>((MPPerkVM x) => x.Perk == perk));
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00030B40 File Offset: 0x0002ED40
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.SelectedPerkItem.Name;
			this.CandidatePerks.ApplyActionOnAllItems(delegate(MPPerkVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00030B90 File Offset: 0x0002ED90
		[UsedImplicitly]
		private void OnSelectPerk(MPPerkVM perkVm)
		{
			this.OnRefreshWithPerk(perkVm);
			foreach (MPPerkVM mpperkVM in this.CandidatePerks)
			{
				mpperkVM.IsSelectable = true;
			}
			perkVm.IsSelectable = false;
			this._onSelectPerk(this, perkVm);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00030BF8 File Offset: 0x0002EDF8
		private void OnRefreshWithPerk(MPPerkVM perk)
		{
			this.SelectedPerkItem = perk;
			MPPerkVM selectedPerkItem = this.SelectedPerkItem;
			this.SelectedPerk = ((selectedPerkItem != null) ? selectedPerkItem.Perk : null);
			if (perk == null)
			{
				this.Name = "";
				this.IconType = "";
				return;
			}
			this.IconType = perk.IconType;
			this.RefreshValues();
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x00030C50 File Offset: 0x0002EE50
		// (set) Token: 0x06000FD4 RID: 4052 RVA: 0x00030C58 File Offset: 0x0002EE58
		[DataSourceProperty]
		public MBBindingList<MPPerkVM> CandidatePerks
		{
			get
			{
				return this._candidatePerks;
			}
			set
			{
				if (value != this._candidatePerks)
				{
					this._candidatePerks = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPerkVM>>(value, "CandidatePerks");
				}
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x00030C76 File Offset: 0x0002EE76
		// (set) Token: 0x06000FD6 RID: 4054 RVA: 0x00030C7E File Offset: 0x0002EE7E
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00030CA1 File Offset: 0x0002EEA1
		// (set) Token: 0x06000FD8 RID: 4056 RVA: 0x00030CA9 File Offset: 0x0002EEA9
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
				}
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x00030CCC File Offset: 0x0002EECC
		// (set) Token: 0x06000FDA RID: 4058 RVA: 0x00030CD4 File Offset: 0x0002EED4
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
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
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x0400075A RID: 1882
		private readonly Action<HeroPerkVM, MPPerkVM> _onSelectPerk;

		// Token: 0x0400075C RID: 1884
		private string _name = "";

		// Token: 0x0400075D RID: 1885
		private string _iconType;

		// Token: 0x0400075E RID: 1886
		private BasicTooltipViewModel _hint;

		// Token: 0x0400075F RID: 1887
		private MBBindingList<MPPerkVM> _candidatePerks;
	}
}
