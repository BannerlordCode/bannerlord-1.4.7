using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.EndOfRound
{
	// Token: 0x0200009F RID: 159
	public class MultiplayerEndOfRoundSideVM : ViewModel
	{
		// Token: 0x06000F3F RID: 3903 RVA: 0x0002F05C File Offset: 0x0002D25C
		public void SetData(BasicCultureObject culture, int score, bool isWinner, MultiplayerBattleColors.MultiplayerCultureColorInfo cultureColors)
		{
			this._culture = culture;
			this.CultureID = culture.StringId;
			this.Score = score;
			this.IsWinner = isWinner;
			this.CultureColor1 = cultureColors.Color1;
			this.CultureColor2 = cultureColors.Color2;
			this.RefreshValues();
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0002F0AA File Offset: 0x0002D2AA
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CultureName = this._culture.Name.ToString();
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0002F0C8 File Offset: 0x0002D2C8
		// (set) Token: 0x06000F42 RID: 3906 RVA: 0x0002F0D0 File Offset: 0x0002D2D0
		[DataSourceProperty]
		public bool IsWinner
		{
			get
			{
				return this._isWinner;
			}
			set
			{
				if (value != this._isWinner)
				{
					this._isWinner = value;
					base.OnPropertyChangedWithValue(value, "IsWinner");
				}
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0002F0EE File Offset: 0x0002D2EE
		// (set) Token: 0x06000F44 RID: 3908 RVA: 0x0002F0F6 File Offset: 0x0002D2F6
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x0002F119 File Offset: 0x0002D319
		// (set) Token: 0x06000F46 RID: 3910 RVA: 0x0002F121 File Offset: 0x0002D321
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor2");
				}
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x0002F144 File Offset: 0x0002D344
		// (set) Token: 0x06000F48 RID: 3912 RVA: 0x0002F14C File Offset: 0x0002D34C
		[DataSourceProperty]
		public string CultureID
		{
			get
			{
				return this._cultureID;
			}
			set
			{
				if (value != this._cultureID)
				{
					this._cultureID = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureID");
				}
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x0002F16F File Offset: 0x0002D36F
		// (set) Token: 0x06000F4A RID: 3914 RVA: 0x0002F177 File Offset: 0x0002D377
		[DataSourceProperty]
		public string CultureName
		{
			get
			{
				return this._cultureName;
			}
			set
			{
				if (value != this._cultureName)
				{
					this._cultureName = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureName");
				}
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x0002F19A File Offset: 0x0002D39A
		// (set) Token: 0x06000F4C RID: 3916 RVA: 0x0002F1A2 File Offset: 0x0002D3A2
		[DataSourceProperty]
		public int Score
		{
			get
			{
				return this._score;
			}
			set
			{
				if (value != this._score)
				{
					this._score = value;
					base.OnPropertyChangedWithValue(value, "Score");
				}
			}
		}

		// Token: 0x04000708 RID: 1800
		private BasicCultureObject _culture;

		// Token: 0x04000709 RID: 1801
		private bool _isWinner;

		// Token: 0x0400070A RID: 1802
		private string _cultureID;

		// Token: 0x0400070B RID: 1803
		private Color _cultureColor1;

		// Token: 0x0400070C RID: 1804
		private Color _cultureColor2;

		// Token: 0x0400070D RID: 1805
		private string _cultureName;

		// Token: 0x0400070E RID: 1806
		private int _score;
	}
}
