using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.Parley
{
	// Token: 0x02000037 RID: 55
	public class MapParleyAnimationVM : ViewModel
	{
		// Token: 0x06000579 RID: 1401 RVA: 0x0001DB92 File Offset: 0x0001BD92
		public MapParleyAnimationVM(PartyBase parleyedParty, float animationDuration)
		{
			this._parleyedParty = parleyedParty;
			this.AnimationDuration = animationDuration;
			this.RefreshValues();
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001DBBF File Offset: 0x0001BDBF
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ParleyTextObj.SetTextVariable("PARTY_NAME", this._parleyedParty.Name);
			this.ParleyText = this.ParleyTextObj.ToString();
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001DBF4 File Offset: 0x0001BDF4
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._parleyedParty = null;
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0001DC03 File Offset: 0x0001BE03
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x0001DC0B File Offset: 0x0001BE0B
		[DataSourceProperty]
		public string ParleyText
		{
			get
			{
				return this._parleyText;
			}
			set
			{
				if (this._parleyText != value)
				{
					this._parleyText = value;
					base.OnPropertyChangedWithValue<string>(value, "ParleyText");
				}
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0001DC2E File Offset: 0x0001BE2E
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x0001DC36 File Offset: 0x0001BE36
		[DataSourceProperty]
		public float AnimationDuration
		{
			get
			{
				return this._animationDuration;
			}
			set
			{
				if (this._animationDuration != value)
				{
					this._animationDuration = value;
					base.OnPropertyChangedWithValue(value, "AnimationDuration");
				}
			}
		}

		// Token: 0x04000259 RID: 601
		private readonly TextObject ParleyTextObj = new TextObject("{=LZbHWkCB}Parleying with {PARTY_NAME}", null);

		// Token: 0x0400025A RID: 602
		private PartyBase _parleyedParty;

		// Token: 0x0400025B RID: 603
		private string _parleyText;

		// Token: 0x0400025C RID: 604
		private float _animationDuration;
	}
}
