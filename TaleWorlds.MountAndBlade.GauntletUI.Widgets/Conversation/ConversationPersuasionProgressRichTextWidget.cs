using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x02000170 RID: 368
	public class ConversationPersuasionProgressRichTextWidget : RichTextWidget
	{
		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x00034549 File Offset: 0x00032749
		// (set) Token: 0x06001345 RID: 4933 RVA: 0x00034551 File Offset: 0x00032751
		public float FadeInTime { get; set; } = 1f;

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x0003455A File Offset: 0x0003275A
		// (set) Token: 0x06001347 RID: 4935 RVA: 0x00034562 File Offset: 0x00032762
		public float FadeOutTime { get; set; } = 1f;

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001348 RID: 4936 RVA: 0x0003456B File Offset: 0x0003276B
		// (set) Token: 0x06001349 RID: 4937 RVA: 0x00034573 File Offset: 0x00032773
		public float StayTime { get; set; } = 2.5f;

		// Token: 0x0600134A RID: 4938 RVA: 0x0003457C File Offset: 0x0003277C
		public ConversationPersuasionProgressRichTextWidget(UIContext context)
			: base(context)
		{
			base.PropertyChanged += this.OnSelfPropertyChanged;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x000345D0 File Offset: 0x000327D0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._startTime == -1f)
			{
				this.SetGlobalAlphaRecursively(0f);
				return;
			}
			float num;
			if (base.EventManager.Time - this._startTime < this.FadeInTime)
			{
				num = Mathf.Lerp(0f, 1f, (base.EventManager.Time - this._startTime) / this.FadeInTime);
			}
			else if (base.EventManager.Time - this._startTime < this.StayTime + this.FadeInTime)
			{
				num = 1f;
			}
			else
			{
				num = Mathf.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, 0f, (base.EventManager.Time - (this._startTime + this.StayTime + this.FadeInTime)) / this.FadeOutTime);
				if (base.ReadOnlyBrush.GlobalAlphaFactor <= 0.001f)
				{
					this._startTime = -1f;
				}
			}
			this.SetGlobalAlphaRecursively(num);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x000346D5 File Offset: 0x000328D5
		private void OnSelfPropertyChanged(PropertyOwnerObject arg1, string propertyName, object newState)
		{
			if (propertyName == "Text" && !string.IsNullOrEmpty(newState as string))
			{
				this._startTime = base.EventManager.Time;
			}
		}

		// Token: 0x040008BF RID: 2239
		private float _startTime = -1f;
	}
}
