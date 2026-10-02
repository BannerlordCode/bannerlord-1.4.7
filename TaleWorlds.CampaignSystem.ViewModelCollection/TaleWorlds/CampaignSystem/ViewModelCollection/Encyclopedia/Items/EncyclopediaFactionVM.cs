using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000E5 RID: 229
	public class EncyclopediaFactionVM : ViewModel
	{
		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x0600158F RID: 5519 RVA: 0x000551FA File Offset: 0x000533FA
		// (set) Token: 0x06001590 RID: 5520 RVA: 0x00055202 File Offset: 0x00053402
		public IFaction Faction { get; private set; }

		// Token: 0x06001591 RID: 5521 RVA: 0x0005520C File Offset: 0x0005340C
		public EncyclopediaFactionVM(IFaction faction)
		{
			this.Faction = faction;
			if (faction != null)
			{
				this.ImageIdentifier = new BannerImageIdentifierVM(faction.Banner, true);
				this.IsDestroyed = faction.IsEliminated;
			}
			else
			{
				this.ImageIdentifier = new BannerImageIdentifierVM(null, false);
				this.IsDestroyed = false;
			}
			this.RefreshValues();
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00055263 File Offset: 0x00053463
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Faction != null)
			{
				this.NameText = this.Faction.Name.ToString();
				return;
			}
			this.NameText = new TextObject("{=2abtb4xu}Independent", null).ToString();
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x000552A0 File Offset: 0x000534A0
		public void ExecuteLink()
		{
			if (this.Faction != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Faction.EncyclopediaLink);
			}
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x000552C4 File Offset: 0x000534C4
		public void ExecuteBeginHint()
		{
			if (this.Faction is Clan)
			{
				InformationManager.ShowTooltip(typeof(Clan), new object[] { this.Faction });
				return;
			}
			if (this.Faction is Kingdom)
			{
				InformationManager.ShowTooltip(typeof(Kingdom), new object[] { this.Faction });
			}
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x00055328 File Offset: 0x00053528
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x0005532F File Offset: 0x0005352F
		// (set) Token: 0x06001597 RID: 5527 RVA: 0x00055337 File Offset: 0x00053537
		[DataSourceProperty]
		public BannerImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChanged("Banner");
				}
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001598 RID: 5528 RVA: 0x00055354 File Offset: 0x00053554
		// (set) Token: 0x06001599 RID: 5529 RVA: 0x0005535C File Offset: 0x0005355C
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x0600159A RID: 5530 RVA: 0x0005537F File Offset: 0x0005357F
		// (set) Token: 0x0600159B RID: 5531 RVA: 0x00055387 File Offset: 0x00053587
		[DataSourceProperty]
		public bool IsDestroyed
		{
			get
			{
				return this._isDestroyed;
			}
			set
			{
				if (value != this._isDestroyed)
				{
					this._isDestroyed = value;
					base.OnPropertyChangedWithValue(value, "IsDestroyed");
				}
			}
		}

		// Token: 0x040009D1 RID: 2513
		private BannerImageIdentifierVM _imageIdentifier;

		// Token: 0x040009D2 RID: 2514
		private string _nameText;

		// Token: 0x040009D3 RID: 2515
		private bool _isDestroyed;
	}
}
