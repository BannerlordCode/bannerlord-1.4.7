using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000031 RID: 49
	public class NavigationScopeTargeter : Widget
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00008FD5 File Offset: 0x000071D5
		// (set) Token: 0x060002AC RID: 684 RVA: 0x00008FDD File Offset: 0x000071DD
		public GamepadNavigationScope NavigationScope { get; private set; }

		// Token: 0x060002AD RID: 685 RVA: 0x00008FE6 File Offset: 0x000071E6
		public NavigationScopeTargeter(UIContext context)
			: base(context)
		{
			this.NavigationScope = new GamepadNavigationScope();
			base.WidthSizePolicy = SizePolicy.Fixed;
			base.HeightSizePolicy = SizePolicy.Fixed;
			base.SuggestedHeight = 0f;
			base.SuggestedWidth = 0f;
			base.IsVisible = false;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00009025 File Offset: 0x00007225
		// (set) Token: 0x060002AF RID: 687 RVA: 0x00009032 File Offset: 0x00007232
		public string ScopeID
		{
			get
			{
				return this.NavigationScope.ScopeID;
			}
			set
			{
				if (value != this.NavigationScope.ScopeID)
				{
					this.NavigationScope.ScopeID = value;
				}
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00009053 File Offset: 0x00007253
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x00009060 File Offset: 0x00007260
		public GamepadNavigationTypes ScopeMovements
		{
			get
			{
				return this.NavigationScope.ScopeMovements;
			}
			set
			{
				if (value != this.NavigationScope.ScopeMovements)
				{
					this.NavigationScope.ScopeMovements = value;
				}
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000907C File Offset: 0x0000727C
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x00009089 File Offset: 0x00007289
		public GamepadNavigationTypes AlternateScopeMovements
		{
			get
			{
				return this.NavigationScope.AlternateScopeMovements;
			}
			set
			{
				if (value != this.NavigationScope.AlternateScopeMovements)
				{
					this.NavigationScope.AlternateScopeMovements = value;
				}
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x000090A5 File Offset: 0x000072A5
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x000090B2 File Offset: 0x000072B2
		public int AlternateMovementStepSize
		{
			get
			{
				return this.NavigationScope.AlternateMovementStepSize;
			}
			set
			{
				if (value != this.NavigationScope.AlternateMovementStepSize)
				{
					this.NavigationScope.AlternateMovementStepSize = value;
				}
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x000090CE File Offset: 0x000072CE
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x000090DB File Offset: 0x000072DB
		public bool HasCircularMovement
		{
			get
			{
				return this.NavigationScope.HasCircularMovement;
			}
			set
			{
				if (value != this.NavigationScope.HasCircularMovement)
				{
					this.NavigationScope.HasCircularMovement = value;
				}
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x000090F7 File Offset: 0x000072F7
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x00009104 File Offset: 0x00007304
		public bool DoNotAutomaticallyFindChildren
		{
			get
			{
				return this.NavigationScope.DoNotAutomaticallyFindChildren;
			}
			set
			{
				if (value != this.NavigationScope.DoNotAutomaticallyFindChildren)
				{
					this.NavigationScope.DoNotAutomaticallyFindChildren = value;
				}
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002BA RID: 698 RVA: 0x00009120 File Offset: 0x00007320
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000912D File Offset: 0x0000732D
		public bool DoNotAutoGainNavigationOnInit
		{
			get
			{
				return this.NavigationScope.DoNotAutoGainNavigationOnInit;
			}
			set
			{
				if (value != this.NavigationScope.DoNotAutoGainNavigationOnInit)
				{
					this.NavigationScope.DoNotAutoGainNavigationOnInit = value;
				}
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002BC RID: 700 RVA: 0x00009149 File Offset: 0x00007349
		// (set) Token: 0x060002BD RID: 701 RVA: 0x00009156 File Offset: 0x00007356
		public bool ForceGainNavigationBasedOnDirection
		{
			get
			{
				return this.NavigationScope.ForceGainNavigationBasedOnDirection;
			}
			set
			{
				if (value != this.NavigationScope.ForceGainNavigationBasedOnDirection)
				{
					this.NavigationScope.ForceGainNavigationBasedOnDirection = value;
				}
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002BE RID: 702 RVA: 0x00009172 File Offset: 0x00007372
		// (set) Token: 0x060002BF RID: 703 RVA: 0x0000917F File Offset: 0x0000737F
		public bool ForceGainNavigationOnClosestChild
		{
			get
			{
				return this.NavigationScope.ForceGainNavigationOnClosestChild;
			}
			set
			{
				if (value != this.NavigationScope.ForceGainNavigationOnClosestChild)
				{
					this.NavigationScope.ForceGainNavigationOnClosestChild = value;
				}
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000919B File Offset: 0x0000739B
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x000091A8 File Offset: 0x000073A8
		public bool ForceGainNavigationOnFirstChild
		{
			get
			{
				return this.NavigationScope.ForceGainNavigationOnFirstChild;
			}
			set
			{
				if (value != this.NavigationScope.ForceGainNavigationOnFirstChild)
				{
					this.NavigationScope.ForceGainNavigationOnFirstChild = value;
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x000091C4 File Offset: 0x000073C4
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x000091D1 File Offset: 0x000073D1
		public bool NavigateFromScopeEdges
		{
			get
			{
				return this.NavigationScope.NavigateFromScopeEdges;
			}
			set
			{
				if (value != this.NavigationScope.NavigateFromScopeEdges)
				{
					this.NavigationScope.NavigateFromScopeEdges = value;
				}
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x000091ED File Offset: 0x000073ED
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x000091FA File Offset: 0x000073FA
		public bool UseDiscoveryAreaAsScopeEdges
		{
			get
			{
				return this.NavigationScope.UseDiscoveryAreaAsScopeEdges;
			}
			set
			{
				if (value != this.NavigationScope.UseDiscoveryAreaAsScopeEdges)
				{
					this.NavigationScope.UseDiscoveryAreaAsScopeEdges = value;
				}
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x00009216 File Offset: 0x00007416
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x00009223 File Offset: 0x00007423
		public bool DoNotAutoNavigateAfterSort
		{
			get
			{
				return this.NavigationScope.DoNotAutoNavigateAfterSort;
			}
			set
			{
				if (value != this.NavigationScope.DoNotAutoNavigateAfterSort)
				{
					this.NavigationScope.DoNotAutoNavigateAfterSort = value;
				}
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000923F File Offset: 0x0000743F
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0000924C File Offset: 0x0000744C
		public bool FollowMobileTargets
		{
			get
			{
				return this.NavigationScope.FollowMobileTargets;
			}
			set
			{
				if (value != this.NavigationScope.FollowMobileTargets)
				{
					this.NavigationScope.FollowMobileTargets = value;
				}
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00009268 File Offset: 0x00007468
		// (set) Token: 0x060002CB RID: 715 RVA: 0x00009275 File Offset: 0x00007475
		public bool DoNotAutoCollectChildScopes
		{
			get
			{
				return this.NavigationScope.DoNotAutoCollectChildScopes;
			}
			set
			{
				if (value != this.NavigationScope.DoNotAutoCollectChildScopes)
				{
					this.NavigationScope.DoNotAutoCollectChildScopes = value;
				}
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00009291 File Offset: 0x00007491
		// (set) Token: 0x060002CD RID: 717 RVA: 0x0000929E File Offset: 0x0000749E
		public bool IsDefaultNavigationScope
		{
			get
			{
				return this.NavigationScope.IsDefaultNavigationScope;
			}
			set
			{
				if (value != this.NavigationScope.IsDefaultNavigationScope)
				{
					this.NavigationScope.IsDefaultNavigationScope = value;
				}
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002CE RID: 718 RVA: 0x000092BA File Offset: 0x000074BA
		// (set) Token: 0x060002CF RID: 719 RVA: 0x000092C7 File Offset: 0x000074C7
		public float ExtendDiscoveryAreaTop
		{
			get
			{
				return this.NavigationScope.ExtendDiscoveryAreaTop;
			}
			set
			{
				if (value != this.NavigationScope.ExtendDiscoveryAreaTop)
				{
					this.NavigationScope.ExtendDiscoveryAreaTop = value;
				}
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x000092E3 File Offset: 0x000074E3
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x000092F0 File Offset: 0x000074F0
		public float ExtendDiscoveryAreaRight
		{
			get
			{
				return this.NavigationScope.ExtendDiscoveryAreaRight;
			}
			set
			{
				if (value != this.NavigationScope.ExtendDiscoveryAreaRight)
				{
					this.NavigationScope.ExtendDiscoveryAreaRight = value;
				}
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000930C File Offset: 0x0000750C
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x00009319 File Offset: 0x00007519
		public float ExtendDiscoveryAreaBottom
		{
			get
			{
				return this.NavigationScope.ExtendDiscoveryAreaBottom;
			}
			set
			{
				if (value != this.NavigationScope.ExtendDiscoveryAreaBottom)
				{
					this.NavigationScope.ExtendDiscoveryAreaBottom = value;
				}
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00009335 File Offset: 0x00007535
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x00009342 File Offset: 0x00007542
		public float ExtendDiscoveryAreaLeft
		{
			get
			{
				return this.NavigationScope.ExtendDiscoveryAreaLeft;
			}
			set
			{
				if (value != this.NavigationScope.ExtendDiscoveryAreaLeft)
				{
					this.NavigationScope.ExtendDiscoveryAreaLeft = value;
				}
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000935E File Offset: 0x0000755E
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000936B File Offset: 0x0000756B
		public float ExtendChildrenCursorAreaLeft
		{
			get
			{
				return this.NavigationScope.ExtendChildrenCursorAreaLeft;
			}
			set
			{
				if (value != this.NavigationScope.ExtendChildrenCursorAreaLeft)
				{
					this.NavigationScope.ExtendChildrenCursorAreaLeft = value;
				}
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00009387 File Offset: 0x00007587
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00009394 File Offset: 0x00007594
		public float ExtendChildrenCursorAreaRight
		{
			get
			{
				return this.NavigationScope.ExtendChildrenCursorAreaRight;
			}
			set
			{
				if (value != this.NavigationScope.ExtendChildrenCursorAreaRight)
				{
					this.NavigationScope.ExtendChildrenCursorAreaRight = value;
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060002DA RID: 730 RVA: 0x000093B0 File Offset: 0x000075B0
		// (set) Token: 0x060002DB RID: 731 RVA: 0x000093BD File Offset: 0x000075BD
		public float ExtendChildrenCursorAreaTop
		{
			get
			{
				return this.NavigationScope.ExtendChildrenCursorAreaTop;
			}
			set
			{
				if (value != this.NavigationScope.ExtendChildrenCursorAreaTop)
				{
					this.NavigationScope.ExtendChildrenCursorAreaTop = value;
				}
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060002DC RID: 732 RVA: 0x000093D9 File Offset: 0x000075D9
		// (set) Token: 0x060002DD RID: 733 RVA: 0x000093E6 File Offset: 0x000075E6
		public float ExtendChildrenCursorAreaBottom
		{
			get
			{
				return this.NavigationScope.ExtendChildrenCursorAreaBottom;
			}
			set
			{
				if (value != this.NavigationScope.ExtendChildrenCursorAreaBottom)
				{
					this.NavigationScope.ExtendChildrenCursorAreaBottom = value;
				}
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00009402 File Offset: 0x00007602
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000940F File Offset: 0x0000760F
		public float DiscoveryAreaOffsetX
		{
			get
			{
				return this.NavigationScope.DiscoveryAreaOffsetX;
			}
			set
			{
				if (value != this.NavigationScope.DiscoveryAreaOffsetX)
				{
					this.NavigationScope.DiscoveryAreaOffsetX = value;
				}
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000942B File Offset: 0x0000762B
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x00009438 File Offset: 0x00007638
		public float DiscoveryAreaOffsetY
		{
			get
			{
				return this.NavigationScope.DiscoveryAreaOffsetY;
			}
			set
			{
				if (value != this.NavigationScope.DiscoveryAreaOffsetY)
				{
					this.NavigationScope.DiscoveryAreaOffsetY = value;
				}
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00009454 File Offset: 0x00007654
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00009461 File Offset: 0x00007661
		public bool IsScopeEnabled
		{
			get
			{
				return this.NavigationScope.IsEnabled;
			}
			set
			{
				if (value != this.NavigationScope.IsEnabled)
				{
					this.NavigationScope.IsEnabled = value;
				}
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000947D File Offset: 0x0000767D
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000948A File Offset: 0x0000768A
		public bool IsScopeDisabled
		{
			get
			{
				return this.NavigationScope.IsDisabled;
			}
			set
			{
				if (value != this.NavigationScope.IsDisabled)
				{
					this.NavigationScope.IsDisabled = value;
				}
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x000094A6 File Offset: 0x000076A6
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x000094B3 File Offset: 0x000076B3
		public string UpNavigationScope
		{
			get
			{
				return this.NavigationScope.UpNavigationScopeID;
			}
			set
			{
				if (value != this.NavigationScope.UpNavigationScopeID)
				{
					this.NavigationScope.UpNavigationScopeID = value;
				}
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000094D4 File Offset: 0x000076D4
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x000094E1 File Offset: 0x000076E1
		public string RightNavigationScope
		{
			get
			{
				return this.NavigationScope.RightNavigationScopeID;
			}
			set
			{
				if (value != this.NavigationScope.RightNavigationScopeID)
				{
					this.NavigationScope.RightNavigationScopeID = value;
				}
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00009502 File Offset: 0x00007702
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000950F File Offset: 0x0000770F
		public string DownNavigationScope
		{
			get
			{
				return this.NavigationScope.DownNavigationScopeID;
			}
			set
			{
				if (value != this.NavigationScope.DownNavigationScopeID)
				{
					this.NavigationScope.DownNavigationScopeID = value;
				}
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00009530 File Offset: 0x00007730
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000953D File Offset: 0x0000773D
		public string LeftNavigationScope
		{
			get
			{
				return this.NavigationScope.LeftNavigationScopeID;
			}
			set
			{
				if (value != this.NavigationScope.LeftNavigationScopeID)
				{
					this.NavigationScope.LeftNavigationScopeID = value;
				}
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000955E File Offset: 0x0000775E
		// (set) Token: 0x060002EF RID: 751 RVA: 0x00009566 File Offset: 0x00007766
		public NavigationScopeTargeter UpNavigationScopeTargeter
		{
			get
			{
				return this._upNavigationScopeTargeter;
			}
			set
			{
				if (value != this._upNavigationScopeTargeter)
				{
					this._upNavigationScopeTargeter = value;
					this.NavigationScope.UpNavigationScope = value.NavigationScope;
				}
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00009589 File Offset: 0x00007789
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x00009591 File Offset: 0x00007791
		public NavigationScopeTargeter RightNavigationScopeTargeter
		{
			get
			{
				return this._rightNavigationScopeTargeter;
			}
			set
			{
				if (value != this._rightNavigationScopeTargeter)
				{
					this._rightNavigationScopeTargeter = value;
					this.NavigationScope.RightNavigationScope = value.NavigationScope;
				}
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x000095B4 File Offset: 0x000077B4
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x000095BC File Offset: 0x000077BC
		public NavigationScopeTargeter DownNavigationScopeTargeter
		{
			get
			{
				return this._downNavigationScopeTargeter;
			}
			set
			{
				if (value != this._downNavigationScopeTargeter)
				{
					this._downNavigationScopeTargeter = value;
					this.NavigationScope.DownNavigationScope = value.NavigationScope;
				}
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x000095DF File Offset: 0x000077DF
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x000095E7 File Offset: 0x000077E7
		public NavigationScopeTargeter LeftNavigationScopeTargeter
		{
			get
			{
				return this._leftNavigationScopeTargeter;
			}
			set
			{
				if (value != this._leftNavigationScopeTargeter)
				{
					this._leftNavigationScopeTargeter = value;
					this.NavigationScope.LeftNavigationScope = value.NavigationScope;
				}
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000960A File Offset: 0x0000780A
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x00009618 File Offset: 0x00007818
		public Widget ScopeParent
		{
			get
			{
				return this.NavigationScope.ParentWidget;
			}
			set
			{
				if (this.NavigationScope.ParentWidget != value)
				{
					if (this.NavigationScope.ParentWidget != null)
					{
						base.GamepadNavigationContext.RemoveNavigationScope(this.NavigationScope);
					}
					this.NavigationScope.ParentWidget = value;
					this.NavigationScope.ParentWidget.EventFire += this.OnParentConnectedToTheRoot;
					base.GamepadNavigationContext.AddNavigationScope(this.NavigationScope, false);
				}
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000968B File Offset: 0x0000788B
		private void OnParentConnectedToTheRoot(Widget widget, string eventName, object[] arguments)
		{
			if (eventName == "ConnectedToRoot" && !base.GamepadNavigationContext.HasNavigationScope(this.NavigationScope))
			{
				base.GamepadNavigationContext.AddNavigationScope(this.NavigationScope, false);
			}
		}

		// Token: 0x04000131 RID: 305
		private NavigationScopeTargeter _upNavigationScopeTargeter;

		// Token: 0x04000132 RID: 306
		private NavigationScopeTargeter _rightNavigationScopeTargeter;

		// Token: 0x04000133 RID: 307
		private NavigationScopeTargeter _downNavigationScopeTargeter;

		// Token: 0x04000134 RID: 308
		private NavigationScopeTargeter _leftNavigationScopeTargeter;
	}
}
