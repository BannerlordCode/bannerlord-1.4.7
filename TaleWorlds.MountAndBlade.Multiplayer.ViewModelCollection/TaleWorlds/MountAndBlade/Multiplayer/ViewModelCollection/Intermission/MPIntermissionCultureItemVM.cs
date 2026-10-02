using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Intermission
{
	// Token: 0x0200008E RID: 142
	public class MPIntermissionCultureItemVM : MPCultureItemVM
	{
		// Token: 0x06000DAC RID: 3500 RVA: 0x0002A00E File Offset: 0x0002820E
		public MPIntermissionCultureItemVM(string cultureCode, Action<MPIntermissionCultureItemVM> onPlayerVoted)
			: base(cultureCode, null)
		{
			this._onPlayerVoted = onPlayerVoted;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0002A01F File Offset: 0x0002821F
		public void ExecuteVote()
		{
			this._onPlayerVoted(this);
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x0002A02D File Offset: 0x0002822D
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x0002A035 File Offset: 0x00028235
		[DataSourceProperty]
		public int Votes
		{
			get
			{
				return this._votes;
			}
			set
			{
				if (value != this._votes)
				{
					this._votes = value;
					base.OnPropertyChangedWithValue(value, "Votes");
				}
			}
		}

		// Token: 0x0400063B RID: 1595
		private readonly Action<MPIntermissionCultureItemVM> _onPlayerVoted;

		// Token: 0x0400063C RID: 1596
		private int _votes;
	}
}
