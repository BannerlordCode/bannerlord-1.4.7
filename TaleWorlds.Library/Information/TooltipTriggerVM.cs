using System;

namespace TaleWorlds.Library.Information
{
	// Token: 0x020000AD RID: 173
	public class TooltipTriggerVM : ViewModel
	{
		// Token: 0x06000690 RID: 1680 RVA: 0x00016C1C File Offset: 0x00014E1C
		public TooltipTriggerVM(Type linkedTooltipType, params object[] args)
		{
			this._linkedTooltipType = linkedTooltipType;
			this._args = args;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00016C32 File Offset: 0x00014E32
		public void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(this._linkedTooltipType, this._args);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00016C45 File Offset: 0x00014E45
		public void ExecuteEndHint()
		{
			InformationManager.HideTooltip();
		}

		// Token: 0x040001F5 RID: 501
		private Type _linkedTooltipType;

		// Token: 0x040001F6 RID: 502
		private object[] _args;
	}
}
