using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x0200009F RID: 159
	public class MultiplayerLobbyBadgeProgressInformationWidget : Widget
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0001854A File Offset: 0x0001674A
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x00018552 File Offset: 0x00016752
		public float CenterBadgeSize { get; set; } = 200f;

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0001855B File Offset: 0x0001675B
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x00018563 File Offset: 0x00016763
		public float OuterBadgeBaseSize { get; set; } = 175f;

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0001856C File Offset: 0x0001676C
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x00018574 File Offset: 0x00016774
		public float SizeDecayFromCenterPerElement { get; set; } = 25f;

		// Token: 0x06000880 RID: 2176 RVA: 0x0001857D File Offset: 0x0001677D
		public MultiplayerLobbyBadgeProgressInformationWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000185A7 File Offset: 0x000167A7
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.ActiveBadgesList != null)
			{
				this.ArrangeChildrenSizes();
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x000185C0 File Offset: 0x000167C0
		private void ArrangeChildrenSizes()
		{
			this.ActiveBadgesList.IsVisible = this.ShownBadgeCount > 0;
			int centerIndex = this.ShownBadgeCount / 2;
			int currentIndex = 0;
			this.ActiveBadgesList.ApplyActionToAllChildrenRecursive(delegate(Widget widget)
			{
				MultiplayerPlayerBadgeVisualWidget multiplayerPlayerBadgeVisualWidget;
				if ((multiplayerPlayerBadgeVisualWidget = widget as MultiplayerPlayerBadgeVisualWidget) != null)
				{
					float num = this.CenterBadgeSize;
					if (currentIndex != centerIndex)
					{
						float num2 = (float)MathF.Abs(currentIndex - centerIndex);
						num = this.OuterBadgeBaseSize - this.SizeDecayFromCenterPerElement * num2;
					}
					multiplayerPlayerBadgeVisualWidget.SetForcedSize(num, num);
					int currentIndex2 = currentIndex;
					currentIndex = currentIndex2 + 1;
				}
			});
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x0001861A File Offset: 0x0001681A
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x00018622 File Offset: 0x00016822
		[Editor(false)]
		public int ShownBadgeCount
		{
			get
			{
				return this._shownBadgeCount;
			}
			set
			{
				if (value != this._shownBadgeCount)
				{
					this._shownBadgeCount = value;
					base.OnPropertyChanged(value, "ShownBadgeCount");
				}
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x00018640 File Offset: 0x00016840
		// (set) Token: 0x06000886 RID: 2182 RVA: 0x00018648 File Offset: 0x00016848
		[Editor(false)]
		public ListPanel ActiveBadgesList
		{
			get
			{
				return this._activeBadgesList;
			}
			set
			{
				if (value != this._activeBadgesList)
				{
					this._activeBadgesList = value;
					base.OnPropertyChanged<ListPanel>(value, "ActiveBadgesList");
				}
			}
		}

		// Token: 0x040003CD RID: 973
		private int _shownBadgeCount;

		// Token: 0x040003CE RID: 974
		private ListPanel _activeBadgesList;
	}
}
