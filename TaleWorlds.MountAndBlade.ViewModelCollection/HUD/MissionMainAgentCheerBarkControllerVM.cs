using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000052 RID: 82
	public class MissionMainAgentCheerBarkControllerVM : ViewModel
	{
		// Token: 0x060006B7 RID: 1719 RVA: 0x0001860C File Offset: 0x0001680C
		public MissionMainAgentCheerBarkControllerVM(Action<int> onSelectCheer, Action<int> onSelectBark)
		{
			this._onSelectCheer = onSelectCheer;
			this._onSelectBark = onSelectBark;
			this.Nodes = new MBBindingList<CheerBarkNodeItemVM>();
			if (GameNetwork.IsMultiplayer)
			{
				this._ownedTauntCosmetics = NetworkMain.GameClient.OwnedCosmetics.ToList<string>();
				this.UpdatePlayerTauntIndices();
			}
			CheerBarkNodeItemVM.OnSelection += this.OnNodeFocused;
			CheerBarkNodeItemVM.OnNodeFocused += this.OnNodeTooltipToggled;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001867C File Offset: 0x0001687C
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Nodes.ApplyActionOnAllItems(delegate(CheerBarkNodeItemVM n)
			{
				n.OnFinalize();
			});
			CheerBarkNodeItemVM.OnSelection -= this.OnNodeFocused;
			CheerBarkNodeItemVM.OnNodeFocused -= this.OnNodeTooltipToggled;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000186DC File Offset: 0x000168DC
		private void PopulateList()
		{
			bool isClient = GameNetwork.IsClient;
			this.IsNodesCategories = isClient;
			this.Nodes.Clear();
			GameKeyContext category = HotKeyManager.GetCategory("CombatHotKeyCategory");
			HotKey hotKey = category.GetHotKey("CheerBarkCloseMenu");
			SkinVoiceManager.SkinVoiceType[] mpBarks = SkinVoiceManager.VoiceType.MpBarks;
			if (isClient)
			{
				HotKey hotKey2 = category.GetHotKey("CheerBarkSelectFirstCategory");
				CheerBarkNodeItemVM cheerBarkNodeItemVM = new CheerBarkNodeItemVM(new TextObject("{=KxH4VVU3}Taunt", null), "cheer", hotKey2, false, TauntUsageManager.TauntUsage.TauntUsageFlag.None);
				this.Nodes.Add(cheerBarkNodeItemVM);
				TauntCosmeticElement[] array = new TauntCosmeticElement[TauntCosmeticElement.MaxNumberOfTaunts];
				foreach (TauntIndexData tauntIndexData in this._playerTauntsWithIndices)
				{
					string tauntId = tauntIndexData.TauntId;
					int tauntIndex = tauntIndexData.TauntIndex;
					TauntCosmeticElement tauntCosmeticElement = CosmeticsManager.GetCosmeticElement(tauntId) as TauntCosmeticElement;
					if (!tauntCosmeticElement.IsFree && !this._ownedTauntCosmetics.Contains(tauntId))
					{
						Debug.FailedAssert("Taunt list have invalid taunt: " + tauntId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\HUD\\MissionMainAgentCheerBarkControllerVM.cs", "PopulateList", 86);
					}
					else if (tauntIndex >= 0 && tauntIndex < TauntCosmeticElement.MaxNumberOfTaunts)
					{
						array[tauntIndex] = tauntCosmeticElement;
					}
				}
				for (int i = 0; i < array.Length; i++)
				{
					TauntCosmeticElement tauntCosmeticElement2 = array[i];
					if (tauntCosmeticElement2 != null)
					{
						int indexOfAction = TauntUsageManager.Instance.GetIndexOfAction(tauntCosmeticElement2.Id);
						TauntUsageManager.TauntUsage.TauntUsageFlag actionNotUsableReason = CosmeticsManagerHelper.GetActionNotUsableReason(Agent.Main, indexOfAction);
						cheerBarkNodeItemVM.AddSubNode(new CheerBarkNodeItemVM(tauntCosmeticElement2.Id, new TextObject("{=!}" + tauntCosmeticElement2.Name, null), tauntCosmeticElement2.Id, this.GetCheerShortcut(i), true, actionNotUsableReason));
					}
					else
					{
						cheerBarkNodeItemVM.AddSubNode(new CheerBarkNodeItemVM(string.Empty, TextObject.GetEmpty(), string.Empty, null, true, TauntUsageManager.TauntUsage.TauntUsageFlag.None));
					}
				}
				HotKey hotKey3 = category.GetHotKey("CheerBarkSelectSecondCategory");
				CheerBarkNodeItemVM cheerBarkNodeItemVM2 = new CheerBarkNodeItemVM(new TextObject("{=5Xoilj6r}Shout", null), "bark", hotKey3, false, TauntUsageManager.TauntUsage.TauntUsageFlag.None);
				this.Nodes.Add(cheerBarkNodeItemVM2);
				cheerBarkNodeItemVM2.AddSubNode(new CheerBarkNodeItemVM(new TextObject("{=koX9okuG}None", null), "none", hotKey, true, TauntUsageManager.TauntUsage.TauntUsageFlag.None));
				for (int j = 0; j < mpBarks.Length; j++)
				{
					cheerBarkNodeItemVM2.AddSubNode(new CheerBarkNodeItemVM(mpBarks[j].GetName(), "bark" + j, this.GetCheerShortcut(j), true, TauntUsageManager.TauntUsage.TauntUsageFlag.None));
				}
			}
			else
			{
				ActionIndexCache[] array2 = Agent.DefaultTauntActions.ToArray<ActionIndexCache>();
				this.Nodes.Add(new CheerBarkNodeItemVM(new TextObject("{=koX9okuG}None", null), "none", hotKey, true, TauntUsageManager.TauntUsage.TauntUsageFlag.None));
				for (int k = 0; k < array2.Length; k++)
				{
					this.Nodes.Add(new CheerBarkNodeItemVM(new TextObject("{=!}" + (k + 1), null), array2[k].GetName(), this.GetCheerShortcut(k), true, TauntUsageManager.TauntUsage.TauntUsageFlag.None));
				}
			}
			this.DisabledReasonText = string.Empty;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000189E0 File Offset: 0x00016BE0
		private void UpdatePlayerTauntIndices()
		{
			LobbyClient gameClient = NetworkMain.GameClient;
			if (((gameClient != null) ? gameClient.PlayerData : null) != null)
			{
				string text = NetworkMain.GameClient.PlayerData.UserId.ToString();
				this._playerTauntsWithIndices = MultiplayerLocalDataManager.Instance.TauntSlotData.GetTauntIndicesForPlayer(text);
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00018A30 File Offset: 0x00016C30
		private HotKey GetCheerShortcut(int cheerIndex)
		{
			GameKeyContext category = HotKeyManager.GetCategory("CombatHotKeyCategory");
			switch (cheerIndex)
			{
			case 0:
				return category.GetHotKey("CheerBarkItem1");
			case 1:
				return category.GetHotKey("CheerBarkItem2");
			case 2:
				return category.GetHotKey("CheerBarkItem3");
			case 3:
				return category.GetHotKey("CheerBarkItem4");
			default:
				return null;
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00018A94 File Offset: 0x00016C94
		public void SelectItem(int itemIndex, int subNodeIndex = -1)
		{
			if (subNodeIndex == -1)
			{
				for (int i = 0; i < this.Nodes.Count; i++)
				{
					this.Nodes[i].IsSelected = itemIndex == i;
				}
				return;
			}
			if (itemIndex >= 0 && itemIndex < this.Nodes.Count)
			{
				for (int j = 0; j < this.Nodes[itemIndex].SubNodes.Count; j++)
				{
					this.Nodes[itemIndex].SubNodes[j].IsSelected = subNodeIndex == j;
				}
			}
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00018B24 File Offset: 0x00016D24
		public void ExecuteActivate()
		{
			this.IsActive = true;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00018B30 File Offset: 0x00016D30
		public void ExecuteDeactivate(bool applySelection)
		{
			if (applySelection)
			{
				CheerBarkNodeItemVM cheerBarkNodeItemVM = this.Nodes.FirstOrDefault<CheerBarkNodeItemVM>((CheerBarkNodeItemVM c) => c.IsSelected);
				if (cheerBarkNodeItemVM != null)
				{
					if (this.IsNodesCategories)
					{
						bool flag = cheerBarkNodeItemVM.TypeAsString == "bark";
						CheerBarkNodeItemVM cheerBarkNodeItemVM2;
						if (cheerBarkNodeItemVM == null)
						{
							cheerBarkNodeItemVM2 = null;
						}
						else
						{
							cheerBarkNodeItemVM2 = cheerBarkNodeItemVM.SubNodes.FirstOrDefault<CheerBarkNodeItemVM>((CheerBarkNodeItemVM c) => c.IsSelected);
						}
						CheerBarkNodeItemVM cheerBarkNodeItemVM3 = cheerBarkNodeItemVM2;
						if (cheerBarkNodeItemVM3 != null && cheerBarkNodeItemVM3.TypeAsString != "none")
						{
							if (flag)
							{
								Action<int> onSelectBark = this._onSelectBark;
								if (onSelectBark != null)
								{
									onSelectBark(cheerBarkNodeItemVM.SubNodes.IndexOf(cheerBarkNodeItemVM3) - 1);
								}
							}
							else
							{
								int indexOfAction = TauntUsageManager.Instance.GetIndexOfAction(cheerBarkNodeItemVM3.TypeAsString);
								Action<int> onSelectCheer = this._onSelectCheer;
								if (onSelectCheer != null)
								{
									onSelectCheer(indexOfAction);
								}
							}
						}
					}
					else if (cheerBarkNodeItemVM.TypeAsString != "none")
					{
						int num = TauntUsageManager.Instance.GetIndexOfAction(cheerBarkNodeItemVM.TypeAsString);
						if (num == -1)
						{
							ActionIndexCache[] defaultTauntActions = Agent.DefaultTauntActions;
							for (int i = 0; i < defaultTauntActions.Length; i++)
							{
								string name = defaultTauntActions[i].GetName();
								if (cheerBarkNodeItemVM.TypeAsString == name)
								{
									num = i;
									break;
								}
							}
						}
						Action<int> onSelectCheer2 = this._onSelectCheer;
						if (onSelectCheer2 != null)
						{
							onSelectCheer2(num);
						}
					}
				}
			}
			this.Nodes.ApplyActionOnAllItems(delegate(CheerBarkNodeItemVM n)
			{
				n.IsSelected = false;
			});
			this.IsActive = false;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00018CDC File Offset: 0x00016EDC
		public void OnNodeFocused(CheerBarkNodeItemVM focusedNode)
		{
			string text = ((focusedNode != null) ? focusedNode.CheerNameText : null) ?? string.Empty;
			if (this.IsNodesCategories)
			{
				bool flag = focusedNode != null && focusedNode.TypeAsString.Contains("bark");
				string typeId = (flag ? "bark" : "cheer");
				this.Nodes.First<CheerBarkNodeItemVM>((CheerBarkNodeItemVM c) => c.TypeAsString == typeId).SelectedNodeText = text;
				return;
			}
			this.SelectedNodeText = text;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00018D5E File Offset: 0x00016F5E
		public void OnNodeTooltipToggled(CheerBarkNodeItemVM node)
		{
			if (node != null && node.TauntUsageDisabledReason != TauntUsageManager.TauntUsage.TauntUsageFlag.None)
			{
				this.DisabledReasonText = TauntUsageManager.GetActionDisabledReasonText(node.TauntUsageDisabledReason);
				return;
			}
			this.DisabledReasonText = string.Empty;
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00018D88 File Offset: 0x00016F88
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x00018D90 File Offset: 0x00016F90
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
					if (this._isActive)
					{
						this.PopulateList();
					}
				}
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00018DBC File Offset: 0x00016FBC
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x00018DC4 File Offset: 0x00016FC4
		[DataSourceProperty]
		public string DisabledReasonText
		{
			get
			{
				return this._disabledReasonText;
			}
			set
			{
				if (value != this._disabledReasonText)
				{
					this._disabledReasonText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisabledReasonText");
				}
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00018DE7 File Offset: 0x00016FE7
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x00018DEF File Offset: 0x00016FEF
		[DataSourceProperty]
		public string SelectedNodeText
		{
			get
			{
				return this._selectedNodeText;
			}
			set
			{
				if (value != this._selectedNodeText)
				{
					this._selectedNodeText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedNodeText");
				}
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00018E12 File Offset: 0x00017012
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x00018E1A File Offset: 0x0001701A
		[DataSourceProperty]
		public bool IsNodesCategories
		{
			get
			{
				return this._isNodesCategories;
			}
			set
			{
				if (value != this._isNodesCategories)
				{
					this._isNodesCategories = value;
					base.OnPropertyChangedWithValue(value, "IsNodesCategories");
				}
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00018E38 File Offset: 0x00017038
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x00018E40 File Offset: 0x00017040
		[DataSourceProperty]
		public MBBindingList<CheerBarkNodeItemVM> Nodes
		{
			get
			{
				return this._nodes;
			}
			set
			{
				if (value != this._nodes)
				{
					this._nodes = value;
					base.OnPropertyChangedWithValue<MBBindingList<CheerBarkNodeItemVM>>(value, "Nodes");
				}
			}
		}

		// Token: 0x040002FC RID: 764
		private const string CheerId = "cheer";

		// Token: 0x040002FD RID: 765
		private const string BarkId = "bark";

		// Token: 0x040002FE RID: 766
		private const string NoneId = "none";

		// Token: 0x040002FF RID: 767
		private readonly Action<int> _onSelectCheer;

		// Token: 0x04000300 RID: 768
		private readonly Action<int> _onSelectBark;

		// Token: 0x04000301 RID: 769
		private List<string> _ownedTauntCosmetics;

		// Token: 0x04000302 RID: 770
		private IEnumerable<TauntIndexData> _playerTauntsWithIndices;

		// Token: 0x04000303 RID: 771
		private bool _isActive;

		// Token: 0x04000304 RID: 772
		private bool _isNodesCategories;

		// Token: 0x04000305 RID: 773
		private string _disabledReasonText;

		// Token: 0x04000306 RID: 774
		private string _selectedNodeText;

		// Token: 0x04000307 RID: 775
		private MBBindingList<CheerBarkNodeItemVM> _nodes;
	}
}
