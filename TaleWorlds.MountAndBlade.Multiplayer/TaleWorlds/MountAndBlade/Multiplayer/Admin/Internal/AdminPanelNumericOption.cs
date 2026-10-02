using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin.Internal
{
	// Token: 0x02000079 RID: 121
	internal class AdminPanelNumericOption : AdminPanelOption<int>, IAdminPanelNumericOption, IAdminPanelOption<int>, IAdminPanelOption
	{
		// Token: 0x060003BF RID: 959 RVA: 0x00010212 File Offset: 0x0000E412
		public AdminPanelNumericOption(string uniqueId)
			: base(uniqueId)
		{
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0001021B File Offset: 0x0000E41B
		protected override bool AreEqualValues(int first, int second)
		{
			return first == second;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00010221 File Offset: 0x0000E421
		public AdminPanelNumericOption SetMinimumValue(int value)
		{
			this._minimumValue = new int?(value);
			return this;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00010230 File Offset: 0x0000E430
		public AdminPanelNumericOption SetMaximumValue(int value)
		{
			this._maximumValue = new int?(value);
			return this;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00010240 File Offset: 0x0000E440
		public AdminPanelNumericOption SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType optionType)
		{
			MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
			if (optionProperty != null && optionProperty.HasBounds)
			{
				this._minimumValue = new int?(optionType.GetMinimumValue());
				this._maximumValue = new int?(optionType.GetMaximumValue());
				base.SetValue(MBMath.ClampInt(base.CurrentValue, this._minimumValue.Value, this._maximumValue.Value));
			}
			return this;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000102AB File Offset: 0x0000E4AB
		public int? GetMinimumValue()
		{
			return this._minimumValue;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000102B3 File Offset: 0x0000E4B3
		public int? GetMaximumValue()
		{
			return this._maximumValue;
		}

		// Token: 0x0400011D RID: 285
		private int? _minimumValue;

		// Token: 0x0400011E RID: 286
		private int? _maximumValue;
	}
}
