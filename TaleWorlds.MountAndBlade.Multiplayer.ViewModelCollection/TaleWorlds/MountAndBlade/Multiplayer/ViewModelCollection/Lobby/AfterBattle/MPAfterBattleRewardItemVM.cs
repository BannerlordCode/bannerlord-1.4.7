using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.AfterBattle
{
	// Token: 0x02000087 RID: 135
	public abstract class MPAfterBattleRewardItemVM : ViewModel
	{
		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x00029088 File Offset: 0x00027288
		// (set) Token: 0x06000D4A RID: 3402 RVA: 0x00029090 File Offset: 0x00027290
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x000290AE File Offset: 0x000272AE
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x000290B6 File Offset: 0x000272B6
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

		// Token: 0x04000614 RID: 1556
		private int _type;

		// Token: 0x04000615 RID: 1557
		private string _name;

		// Token: 0x02000175 RID: 373
		public enum RewardType
		{
			// Token: 0x04000A0F RID: 2575
			Loot,
			// Token: 0x04000A10 RID: 2576
			Badge
		}
	}
}
