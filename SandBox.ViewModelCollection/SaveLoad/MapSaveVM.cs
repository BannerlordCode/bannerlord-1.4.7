using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.SaveLoad
{
	// Token: 0x02000012 RID: 18
	public class MapSaveVM : ViewModel
	{
		// Token: 0x06000196 RID: 406 RVA: 0x0000807C File Offset: 0x0000627C
		public MapSaveVM(Action<bool> onActiveStateChange)
		{
			this._onActiveStateChange = onActiveStateChange;
			CampaignEvents.OnSaveStartedEvent.AddNonSerializedListener(this, new Action(this.OnSaveStarted));
			CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(this, new Action<bool, string>(this.OnSaveOver));
			this.RefreshValues();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000080CC File Offset: 0x000062CC
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject textObject = new TextObject("{=cp2XDjeq}Saving...", null);
			this.SavingText = textObject.ToString();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000080F7 File Offset: 0x000062F7
		private void OnSaveOver(bool isSuccessful, string saveName)
		{
			this.IsActive = false;
			Action<bool> onActiveStateChange = this._onActiveStateChange;
			if (onActiveStateChange == null)
			{
				return;
			}
			onActiveStateChange(false);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008111 File Offset: 0x00006311
		private void OnSaveStarted()
		{
			this.IsActive = true;
			Action<bool> onActiveStateChange = this._onActiveStateChange;
			if (onActiveStateChange == null)
			{
				return;
			}
			onActiveStateChange(true);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000812B File Offset: 0x0000632B
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnSaveStartedEvent.ClearListeners(this);
			CampaignEvents.OnSaveOverEvent.ClearListeners(this);
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00008149 File Offset: 0x00006349
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00008151 File Offset: 0x00006351
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000816F File Offset: 0x0000636F
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00008177 File Offset: 0x00006377
		[DataSourceProperty]
		public string SavingText
		{
			get
			{
				return this._savingText;
			}
			set
			{
				if (value != this._savingText)
				{
					this._savingText = value;
					base.OnPropertyChangedWithValue<string>(value, "SavingText");
				}
			}
		}

		// Token: 0x040000B0 RID: 176
		private readonly Action<bool> _onActiveStateChange;

		// Token: 0x040000B1 RID: 177
		private string _savingText;

		// Token: 0x040000B2 RID: 178
		private bool _isActive;
	}
}
