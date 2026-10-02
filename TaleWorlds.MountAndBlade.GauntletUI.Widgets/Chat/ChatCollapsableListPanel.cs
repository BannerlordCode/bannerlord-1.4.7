using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Chat
{
	// Token: 0x0200017A RID: 378
	public class ChatCollapsableListPanel : ListPanel
	{
		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001394 RID: 5012 RVA: 0x00035318 File Offset: 0x00033518
		// (set) Token: 0x06001395 RID: 5013 RVA: 0x00035320 File Offset: 0x00033520
		public bool IsLinesVisible { get; private set; }

		// Token: 0x06001396 RID: 5014 RVA: 0x00035329 File Offset: 0x00033529
		public ChatCollapsableListPanel(UIContext context)
			: base(context)
		{
			this.RefreshAlphaValues(this.Alpha);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00035340 File Offset: 0x00033540
		private void ToggleLines(bool isVisible)
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				base.GetChild(i).IsVisible = i == 0 || isVisible;
			}
			this.IsLinesVisible = isVisible;
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00035377 File Offset: 0x00033577
		protected override void OnMousePressed()
		{
			base.OnMousePressed();
			this.ToggleLines(!this.IsLinesVisible);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0003538E File Offset: 0x0003358E
		protected override bool OnPreviewMousePressed()
		{
			return base.OnPreviewMousePressed();
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00035396 File Offset: 0x00033596
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			this.RefreshAlphaValues(this.Alpha);
			this.ToggleLines(true);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x000353B2 File Offset: 0x000335B2
		private void RefreshAlphaValues(float newAlpha)
		{
			this.SetGlobalAlphaRecursively(newAlpha);
			if (newAlpha > 0f)
			{
				ChatLogWidget parentChatLogWidget = this.ParentChatLogWidget;
				if (parentChatLogWidget == null)
				{
					return;
				}
				parentChatLogWidget.RegisterMultiLineElement(this);
				return;
			}
			else
			{
				ChatLogWidget parentChatLogWidget2 = this.ParentChatLogWidget;
				if (parentChatLogWidget2 == null)
				{
					return;
				}
				parentChatLogWidget2.RemoveMultiLineElement(this);
				return;
			}
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x000353E8 File Offset: 0x000335E8
		private void UpdateColorValuesOfChildren(Widget widget, Color newColor)
		{
			foreach (Widget widget2 in widget.Children)
			{
				BrushWidget brushWidget;
				if ((brushWidget = widget2 as BrushWidget) != null)
				{
					brushWidget.Brush.FontColor = newColor;
				}
				else
				{
					widget2.Color = newColor;
				}
				this.UpdateColorValuesOfChildren(widget2, newColor);
			}
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0003545C File Offset: 0x0003365C
		private void RefreshColorValues(Color newColor)
		{
			this.UpdateColorValuesOfChildren(this, newColor);
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x00035466 File Offset: 0x00033666
		// (set) Token: 0x0600139F RID: 5023 RVA: 0x0003546E File Offset: 0x0003366E
		[DataSourceProperty]
		public float Alpha
		{
			get
			{
				return this._alpha;
			}
			set
			{
				if (value != this._alpha)
				{
					this._alpha = value;
					base.OnPropertyChanged(value, "Alpha");
					this.RefreshAlphaValues(value);
				}
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x00035493 File Offset: 0x00033693
		// (set) Token: 0x060013A1 RID: 5025 RVA: 0x0003549B File Offset: 0x0003369B
		[DataSourceProperty]
		public Color LineColor
		{
			get
			{
				return this._lineColor;
			}
			set
			{
				if (value != this._lineColor)
				{
					this._lineColor = value;
					base.OnPropertyChanged(value, "LineColor");
					this.RefreshColorValues(value);
				}
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x000354C5 File Offset: 0x000336C5
		// (set) Token: 0x060013A3 RID: 5027 RVA: 0x000354CD File Offset: 0x000336CD
		[DataSourceProperty]
		public ChatLogWidget ParentChatLogWidget
		{
			get
			{
				return this._parentChatLogWidget;
			}
			set
			{
				if (value != this._parentChatLogWidget)
				{
					this._parentChatLogWidget = value;
					base.OnPropertyChanged<ChatLogWidget>(value, "ParentChatLogWidget");
				}
			}
		}

		// Token: 0x040008DC RID: 2268
		private float _alpha;

		// Token: 0x040008DD RID: 2269
		private Color _lineColor;

		// Token: 0x040008DE RID: 2270
		private ChatLogWidget _parentChatLogWidget;
	}
}
