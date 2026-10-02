using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems
{
	// Token: 0x02000041 RID: 65
	public class MissionGenericInteractionItemVM : MissionInteractionItemBaseVM
	{
		// Token: 0x060005CB RID: 1483 RVA: 0x00016203 File Offset: 0x00014403
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject messageTextObj = this._messageTextObj;
			base.Message = ((messageTextObj != null) ? messageTextObj.ToString() : null);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00016223 File Offset: 0x00014423
		public void SetData(TextObject message, bool isDisabled = false)
		{
			this._messageTextObj = message;
			base.IsDisabled = isDisabled;
			this.RefreshValues();
			this.OnSetData(message, isDisabled);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00016241 File Offset: 0x00014441
		public void ResetData()
		{
			this._messageTextObj = null;
			base.IsDisabled = false;
			base.Message = string.Empty;
			base.IsDisplayed = false;
			this.OnResetData();
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00016269 File Offset: 0x00014469
		protected virtual void OnSetData(TextObject message, bool isDisabled)
		{
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001626B File Offset: 0x0001446B
		protected virtual void OnResetData()
		{
		}

		// Token: 0x0400029A RID: 666
		private TextObject _messageTextObj;
	}
}
