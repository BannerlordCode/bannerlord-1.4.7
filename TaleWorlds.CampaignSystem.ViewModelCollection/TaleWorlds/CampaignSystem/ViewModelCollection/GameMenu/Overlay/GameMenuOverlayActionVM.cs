using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000B8 RID: 184
	public class GameMenuOverlayActionVM : StringItemWithEnabledAndHintVM
	{
		// Token: 0x06001249 RID: 4681 RVA: 0x0004A25C File Offset: 0x0004845C
		public GameMenuOverlayActionVM(Action<object> onExecute, string item, bool isEnabled, object identifier, TextObject hint = null)
			: base(onExecute, item, isEnabled, identifier, hint)
		{
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x0004A26B File Offset: 0x0004846B
		// (set) Token: 0x0600124B RID: 4683 RVA: 0x0004A273 File Offset: 0x00048473
		[DataSourceProperty]
		public bool IsHiglightEnabled
		{
			get
			{
				return this._isHiglightEnabled;
			}
			set
			{
				if (value != this._isHiglightEnabled)
				{
					this._isHiglightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHiglightEnabled");
				}
			}
		}

		// Token: 0x04000859 RID: 2137
		private bool _isHiglightEnabled;
	}
}
