using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x0200016E RID: 366
	public class ConversationNameButtonWidget : ButtonWidget
	{
		// Token: 0x0600133A RID: 4922 RVA: 0x000344AD File Offset: 0x000326AD
		public ConversationNameButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x000344B6 File Offset: 0x000326B6
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			this.RelationBarContainer.IsVisible = this.IsRelationEnabled;
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x000344CF File Offset: 0x000326CF
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			this.RelationBarContainer.IsVisible = false;
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x0600133D RID: 4925 RVA: 0x000344E3 File Offset: 0x000326E3
		// (set) Token: 0x0600133E RID: 4926 RVA: 0x000344EB File Offset: 0x000326EB
		[Editor(false)]
		public bool IsRelationEnabled
		{
			get
			{
				return this._isRelationEnabled;
			}
			set
			{
				if (value != this._isRelationEnabled)
				{
					this._isRelationEnabled = value;
					base.OnPropertyChanged(value, "IsRelationEnabled");
				}
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x00034509 File Offset: 0x00032709
		// (set) Token: 0x06001340 RID: 4928 RVA: 0x00034511 File Offset: 0x00032711
		[Editor(false)]
		public Widget RelationBarContainer
		{
			get
			{
				return this._relationBarContainer;
			}
			set
			{
				if (value != this._relationBarContainer)
				{
					this._relationBarContainer = value;
					base.OnPropertyChanged<Widget>(value, "RelationBarContainer");
				}
			}
		}

		// Token: 0x040008B9 RID: 2233
		private bool _isRelationEnabled;

		// Token: 0x040008BA RID: 2234
		private Widget _relationBarContainer;
	}
}
