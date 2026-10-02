using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.BannerEditor;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.BannerBuilder
{
	// Token: 0x02000089 RID: 137
	public class BannerBuilderVM : ViewModel
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00027F19 File Offset: 0x00026119
		// (set) Token: 0x06000B3F RID: 2879 RVA: 0x00027F21 File Offset: 0x00026121
		public Banner CurrentBanner { get; private set; }

		// Token: 0x06000B40 RID: 2880 RVA: 0x00027F2C File Offset: 0x0002612C
		public BannerBuilderVM(BasicCharacterObject character, string initialKey, Action<bool> onExit, Action refresh, Action copyBannerCode)
		{
			this._character = character;
			this._onExit = onExit;
			this._refresh = refresh;
			this._copyBannerCode = copyBannerCode;
			this.Categories = new MBBindingList<BannerBuilderCategoryVM>();
			this.Layers = new MBBindingList<BannerBuilderLayerVM>();
			this.ColorSelection = new BannerBuilderColorSelectionVM();
			BannerBuilderLayerVM.SetLayerActions(new Action(this.OnRefreshFromLayer), new Action<BannerBuilderLayerVM>(this.OnLayerSelection), new Action<BannerBuilderLayerVM>(this.OnLayerDeletion), new Action<int, Action<BannerBuilderColorItemVM>>(this.OnColorSelection));
			ItemObject itemObject = BannerBuilderVM.FindShield(this._character, "highland_riders_shield");
			if (itemObject != null)
			{
				this.ShieldRosterElement = new ItemRosterElement(itemObject, 1, null);
			}
			this.CurrentBanner = new Banner(initialKey);
			this.PopulateCategories();
			this.PopulateLayers();
			this.OnLayerSelection(this.Layers[0]);
			this.BannerCodeAsString = initialKey;
			this.BannerImageIdentifier = new BannerImageIdentifierVM(this.CurrentBanner, true);
			this.RefreshValues();
			this.IsEditorPreviewActive = true;
			this.IsLayerPreviewActive = true;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00028034 File Offset: 0x00026234
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Title = new TextObject("{=!}Banner Builder", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.ResetHint = new HintViewModel(GameTexts.FindText("str_reset_icon", null), null);
			this.RandomizeHint = new HintViewModel(GameTexts.FindText("str_randomize", null), null);
			this.UndoHint = new HintViewModel(GameTexts.FindText("str_undo", null), null);
			this.RedoHint = new HintViewModel(GameTexts.FindText("str_redo", null), null);
			this.DrawStrokeHint = new HintViewModel(new TextObject("{=!}Draw Stroke", null), null);
			this.CenterHint = new HintViewModel(new TextObject("{=!}Align Center", null), null);
			this.ResetSizeHint = new HintViewModel(new TextObject("{=!}Reset Size", null), null);
			this.MirrorHint = new HintViewModel(new TextObject("{=!}Mirror", null), null);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00028144 File Offset: 0x00026344
		private void PopulateCategories()
		{
			this.Categories.Clear();
			for (int i = 0; i < BannerManager.Instance.BannerIconGroups.Count; i++)
			{
				BannerIconGroup bannerIconGroup = BannerManager.Instance.BannerIconGroups[i];
				this.Categories.Add(new BannerBuilderCategoryVM(bannerIconGroup, new Action<BannerBuilderItemVM>(this.OnItemSelection)));
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x000281A4 File Offset: 0x000263A4
		private void PopulateLayers()
		{
			this.Layers.Clear();
			for (int i = 0; i < this.CurrentBanner.GetBannerDataListCount(); i++)
			{
				this.Layers.Add(new BannerBuilderLayerVM(this.CurrentBanner.GetBannerDataAtIndex(i), i));
			}
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x000281EF File Offset: 0x000263EF
		private void OnColorSelection(int selectedColorId, Action<BannerBuilderColorItemVM> onSelection)
		{
			this.ColorSelection.EnableWith(selectedColorId, onSelection);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00028200 File Offset: 0x00026400
		private void OnLayerSelection(BannerBuilderLayerVM selectedLayer)
		{
			if (this.CurrentSelectedLayer != null)
			{
				this.CurrentSelectedLayer.IsSelected = false;
			}
			if (this.CurrentSelectedItem != null)
			{
				this.CurrentSelectedItem.IsSelected = false;
			}
			this.CurrentSelectedLayer = selectedLayer;
			if (this.CurrentSelectedLayer != null)
			{
				BannerBuilderItemVM itemFromID = this.GetItemFromID(this.CurrentSelectedLayer.IconID);
				if (itemFromID != null)
				{
					this.CurrentSelectedItem = itemFromID;
					this.CurrentSelectedItem.IsSelected = true;
				}
				this.CurrentSelectedLayer.IsSelected = true;
				bool isPatternLayerSelected = this.CurrentSelectedLayer.LayerIndex == 0;
				this.Categories.ApplyActionOnAllItems(delegate(BannerBuilderCategoryVM c)
				{
					c.IsEnabled = (c.IsPattern ? isPatternLayerSelected : (!isPatternLayerSelected));
				});
				this.UpdateSelectedItem();
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x000282B0 File Offset: 0x000264B0
		private void OnLayerDeletion(BannerBuilderLayerVM layerToDelete)
		{
			if (layerToDelete == null || layerToDelete.LayerIndex != 0)
			{
				this.CurrentBanner.RemoveIconDataAtIndex(layerToDelete.LayerIndex);
				if (this.CurrentSelectedLayer == layerToDelete)
				{
					this.OnLayerSelection(this.Layers[layerToDelete.LayerIndex - 1]);
				}
				this.Layers.RemoveAt(layerToDelete.LayerIndex);
				this.RefreshLayerIndicies();
				this.Refresh();
			}
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00028320 File Offset: 0x00026520
		private void OnItemSelection(BannerBuilderItemVM selectedItem)
		{
			if (this.CurrentSelectedLayer != null)
			{
				this.CurrentBanner.GetBannerDataAtIndex(this.CurrentSelectedLayer.LayerIndex).MeshId = selectedItem.MeshID;
				this.UpdateSelectedItem();
				this.CurrentSelectedLayer.Refresh();
				this.Refresh();
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00028370 File Offset: 0x00026570
		private void UpdateSelectedItem()
		{
			if (this.CurrentSelectedLayer != null)
			{
				int meshId = this.CurrentBanner.GetBannerDataAtIndex(this.CurrentSelectedLayer.LayerIndex).MeshId;
				for (int i = 0; i < this.Categories.Count; i++)
				{
					BannerBuilderCategoryVM bannerBuilderCategoryVM = this.Categories[i];
					for (int j = 0; j < bannerBuilderCategoryVM.ItemsList.Count; j++)
					{
						BannerBuilderItemVM bannerBuilderItemVM = bannerBuilderCategoryVM.ItemsList[j];
						bannerBuilderItemVM.IsSelected = bannerBuilderItemVM.MeshID == meshId;
					}
				}
			}
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x000283F4 File Offset: 0x000265F4
		public void ExecuteCancel()
		{
			Action<bool> onExit = this._onExit;
			if (onExit == null)
			{
				return;
			}
			onExit(true);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00028407 File Offset: 0x00026607
		public void ExecuteDone()
		{
			Action<bool> onExit = this._onExit;
			if (onExit == null)
			{
				return;
			}
			onExit(false);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0002841C File Offset: 0x0002661C
		public void ExecuteAddDefaultLayer()
		{
			BannerData defaultBannerData = BannerBuilderVM.GetDefaultBannerData();
			this.CurrentBanner.AddIconData(defaultBannerData);
			BannerBuilderLayerVM bannerBuilderLayerVM = new BannerBuilderLayerVM(defaultBannerData, this.Layers.Count);
			this.Layers.Add(bannerBuilderLayerVM);
			this.OnLayerSelection(bannerBuilderLayerVM);
			this.Refresh();
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00028468 File Offset: 0x00026668
		public void ExecuteDuplicateCurrentLayer()
		{
			BannerBuilderLayerVM currentSelectedLayer = this.CurrentSelectedLayer;
			if (currentSelectedLayer != null && !currentSelectedLayer.IsLayerPattern)
			{
				BannerData bannerData = new BannerData(this.CurrentSelectedLayer.Data);
				this.CurrentBanner.AddIconData(bannerData);
				BannerBuilderLayerVM bannerBuilderLayerVM = new BannerBuilderLayerVM(bannerData, this.Layers.Count);
				this.Layers.Add(bannerBuilderLayerVM);
				this.OnLayerSelection(bannerBuilderLayerVM);
				this.Refresh();
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x000284D4 File Offset: 0x000266D4
		public void ExecuteCopyBannerCode()
		{
			Action copyBannerCode = this._copyBannerCode;
			if (copyBannerCode == null)
			{
				return;
			}
			copyBannerCode();
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x000284E8 File Offset: 0x000266E8
		public void ExecuteReorderWithParameters(BannerBuilderLayerVM layer, int index, string targetTag)
		{
			if (layer.IsLayerPattern || index == 0)
			{
				return;
			}
			int num = ((layer.LayerIndex >= index) ? index : (index - 1));
			this.Layers.Remove(layer);
			this.Layers.Insert(num, layer);
			this.CurrentBanner.RemoveIconDataAtIndex(layer.LayerIndex);
			this.CurrentBanner.AddIconData(layer.Data, num);
			this.RefreshLayerIndicies();
			this.OnRefreshFromLayer();
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0002855C File Offset: 0x0002675C
		public void ExecuteReorderToEndWithParameters(BannerBuilderLayerVM layer, int index, string targetTag)
		{
			if (layer.IsLayerPattern)
			{
				return;
			}
			this.ExecuteReorderWithParameters(layer, this.Layers.Count, string.Empty);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0002857E File Offset: 0x0002677E
		private void OnRefreshFromLayer()
		{
			this.Refresh();
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00028586 File Offset: 0x00026786
		public string GetBannerCode()
		{
			return this.BannerCodeAsString;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00028590 File Offset: 0x00026790
		public void SetBannerCode(string v)
		{
			string bannerCodeAsString = this.BannerCodeAsString;
			try
			{
				this.CurrentBanner.Deserialize(v);
				this.PopulateLayers();
				this.OnLayerSelection(this.Layers[0]);
				this.Refresh();
			}
			catch (Exception)
			{
				InformationManager.DisplayMessage(new InformationMessage("Couldn't parse the clipboard text."));
				this.CurrentBanner.Deserialize(bannerCodeAsString);
				this.PopulateLayers();
				this.OnLayerSelection(this.Layers[0]);
				this.Refresh();
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0002861C File Offset: 0x0002681C
		private void Refresh()
		{
			Action refresh = this._refresh;
			if (refresh != null)
			{
				refresh();
			}
			this.BannerImageIdentifier = new BannerImageIdentifierVM(this.CurrentBanner, true);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00028644 File Offset: 0x00026844
		private BannerBuilderItemVM GetItemFromID(int id)
		{
			for (int i = 0; i < this.Categories.Count; i++)
			{
				BannerBuilderCategoryVM bannerBuilderCategoryVM = this.Categories[i];
				for (int j = 0; j < bannerBuilderCategoryVM.ItemsList.Count; j++)
				{
					BannerBuilderItemVM bannerBuilderItemVM = bannerBuilderCategoryVM.ItemsList[j];
					if (bannerBuilderItemVM.MeshID == id)
					{
						return bannerBuilderItemVM;
					}
				}
			}
			return null;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x000286A4 File Offset: 0x000268A4
		private void RefreshLayerIndicies()
		{
			for (int i = 0; i < this.Layers.Count; i++)
			{
				this.Layers[i].SetLayerIndex(i);
			}
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x000286DC File Offset: 0x000268DC
		public void TranslateCurrentLayerWith(Vec2 moveDirection)
		{
			this.CurrentSelectedLayer.PositionValueX += moveDirection.x;
			this.CurrentSelectedLayer.PositionValueY += moveDirection.y;
			this.CurrentSelectedLayer.PositionValueX = MathF.Clamp(this.CurrentSelectedLayer.PositionValueX, 0f, 1528f);
			this.CurrentSelectedLayer.PositionValueY = MathF.Clamp(this.CurrentSelectedLayer.PositionValueY, 0f, 1528f);
			this.Refresh();
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00028769 File Offset: 0x00026969
		public void DeleteCurrentLayer()
		{
			BannerBuilderLayerVM currentSelectedLayer = this.CurrentSelectedLayer;
			if (currentSelectedLayer != null && !currentSelectedLayer.IsLayerPattern)
			{
				this.OnLayerDeletion(this.Layers[this.CurrentSelectedLayer.LayerIndex]);
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002879E File Offset: 0x0002699E
		public override void OnFinalize()
		{
			base.OnFinalize();
			BannerBuilderLayerVM.ResetLayerActions();
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x000287AB File Offset: 0x000269AB
		// (set) Token: 0x06000B5A RID: 2906 RVA: 0x000287B3 File Offset: 0x000269B3
		[DataSourceProperty]
		public BannerImageIdentifierVM BannerImageIdentifier
		{
			get
			{
				return this._bannerImageIdentifier;
			}
			set
			{
				if (value != this._bannerImageIdentifier)
				{
					this._bannerImageIdentifier = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "BannerImageIdentifier");
				}
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x000287D1 File Offset: 0x000269D1
		// (set) Token: 0x06000B5C RID: 2908 RVA: 0x000287D9 File Offset: 0x000269D9
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x000287FC File Offset: 0x000269FC
		// (set) Token: 0x06000B5E RID: 2910 RVA: 0x00028804 File Offset: 0x00026A04
		[DataSourceProperty]
		public MBBindingList<BannerBuilderCategoryVM> Categories
		{
			get
			{
				return this._categories;
			}
			set
			{
				if (value != this._categories)
				{
					this._categories = value;
					base.OnPropertyChangedWithValue<MBBindingList<BannerBuilderCategoryVM>>(value, "Categories");
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x00028822 File Offset: 0x00026A22
		// (set) Token: 0x06000B60 RID: 2912 RVA: 0x0002882A File Offset: 0x00026A2A
		[DataSourceProperty]
		public BannerBuilderColorSelectionVM ColorSelection
		{
			get
			{
				return this._colorSelection;
			}
			set
			{
				if (value != this._colorSelection)
				{
					this._colorSelection = value;
					base.OnPropertyChangedWithValue<BannerBuilderColorSelectionVM>(value, "ColorSelection");
				}
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x00028848 File Offset: 0x00026A48
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x00028850 File Offset: 0x00026A50
		[DataSourceProperty]
		public MBBindingList<BannerBuilderLayerVM> Layers
		{
			get
			{
				return this._layers;
			}
			set
			{
				if (value != this._layers)
				{
					this._layers = value;
					base.OnPropertyChangedWithValue<MBBindingList<BannerBuilderLayerVM>>(value, "Layers");
				}
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0002886E File Offset: 0x00026A6E
		// (set) Token: 0x06000B64 RID: 2916 RVA: 0x00028876 File Offset: 0x00026A76
		[DataSourceProperty]
		public BannerBuilderLayerVM CurrentSelectedLayer
		{
			get
			{
				return this._currentSelectedLayer;
			}
			set
			{
				if (value != this._currentSelectedLayer)
				{
					this._currentSelectedLayer = value;
					base.OnPropertyChangedWithValue<BannerBuilderLayerVM>(value, "CurrentSelectedLayer");
				}
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00028894 File Offset: 0x00026A94
		// (set) Token: 0x06000B66 RID: 2918 RVA: 0x0002889C File Offset: 0x00026A9C
		[DataSourceProperty]
		public BannerBuilderItemVM CurrentSelectedItem
		{
			get
			{
				return this._currentSelectedItem;
			}
			set
			{
				if (value != this._currentSelectedItem)
				{
					this._currentSelectedItem = value;
					base.OnPropertyChangedWithValue<BannerBuilderItemVM>(value, "CurrentSelectedItem");
				}
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x000288BA File Offset: 0x00026ABA
		// (set) Token: 0x06000B68 RID: 2920 RVA: 0x000288C2 File Offset: 0x00026AC2
		[DataSourceProperty]
		public HintViewModel RandomizeHint
		{
			get
			{
				return this._randomizeHint;
			}
			set
			{
				if (value != this._randomizeHint)
				{
					this._randomizeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RandomizeHint");
				}
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x000288E0 File Offset: 0x00026AE0
		// (set) Token: 0x06000B6A RID: 2922 RVA: 0x000288E8 File Offset: 0x00026AE8
		[DataSourceProperty]
		public HintViewModel UndoHint
		{
			get
			{
				return this._undoHint;
			}
			set
			{
				if (value != this._undoHint)
				{
					this._undoHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UndoHint");
				}
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00028906 File Offset: 0x00026B06
		// (set) Token: 0x06000B6C RID: 2924 RVA: 0x0002890E File Offset: 0x00026B0E
		[DataSourceProperty]
		public HintViewModel RedoHint
		{
			get
			{
				return this._redoHint;
			}
			set
			{
				if (value != this._redoHint)
				{
					this._redoHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RedoHint");
				}
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0002892C File Offset: 0x00026B2C
		// (set) Token: 0x06000B6E RID: 2926 RVA: 0x00028934 File Offset: 0x00026B34
		[DataSourceProperty]
		public HintViewModel ResetHint
		{
			get
			{
				return this._resetHint;
			}
			set
			{
				if (value != this._resetHint)
				{
					this._resetHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResetHint");
				}
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x00028952 File Offset: 0x00026B52
		// (set) Token: 0x06000B70 RID: 2928 RVA: 0x0002895A File Offset: 0x00026B5A
		[DataSourceProperty]
		public HintViewModel DrawStrokeHint
		{
			get
			{
				return this._drawStrokeHint;
			}
			set
			{
				if (value != this._drawStrokeHint)
				{
					this._drawStrokeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DrawStrokeHint");
				}
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x00028978 File Offset: 0x00026B78
		// (set) Token: 0x06000B72 RID: 2930 RVA: 0x00028980 File Offset: 0x00026B80
		[DataSourceProperty]
		public HintViewModel CenterHint
		{
			get
			{
				return this._centerHint;
			}
			set
			{
				if (value != this._centerHint)
				{
					this._centerHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CenterHint");
				}
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0002899E File Offset: 0x00026B9E
		// (set) Token: 0x06000B74 RID: 2932 RVA: 0x000289A6 File Offset: 0x00026BA6
		[DataSourceProperty]
		public HintViewModel ResetSizeHint
		{
			get
			{
				return this._resetSizeHint;
			}
			set
			{
				if (value != this._resetSizeHint)
				{
					this._resetSizeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResetSizeHint");
				}
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x000289C4 File Offset: 0x00026BC4
		// (set) Token: 0x06000B76 RID: 2934 RVA: 0x000289CC File Offset: 0x00026BCC
		[DataSourceProperty]
		public HintViewModel MirrorHint
		{
			get
			{
				return this._mirrorHint;
			}
			set
			{
				if (value != this._mirrorHint)
				{
					this._mirrorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "MirrorHint");
				}
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x000289EA File Offset: 0x00026BEA
		// (set) Token: 0x06000B78 RID: 2936 RVA: 0x000289F2 File Offset: 0x00026BF2
		[DataSourceProperty]
		public string CurrentShieldName
		{
			get
			{
				return this._currentShieldName;
			}
			set
			{
				if (value != this._currentShieldName)
				{
					this._currentShieldName = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentShieldName");
				}
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x00028A15 File Offset: 0x00026C15
		// (set) Token: 0x06000B7A RID: 2938 RVA: 0x00028A1D File Offset: 0x00026C1D
		[DataSourceProperty]
		public int MinIconSize
		{
			get
			{
				return this._minIconSize;
			}
			set
			{
				if (value != this._minIconSize)
				{
					this._minIconSize = value;
					base.OnPropertyChangedWithValue(value, "MinIconSize");
				}
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00028A3B File Offset: 0x00026C3B
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x00028A43 File Offset: 0x00026C43
		[DataSourceProperty]
		public int MaxIconSize
		{
			get
			{
				return this._maxIconSize;
			}
			set
			{
				if (value != this._maxIconSize)
				{
					this._maxIconSize = value;
					base.OnPropertyChangedWithValue(value, "MaxIconSize");
				}
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00028A61 File Offset: 0x00026C61
		// (set) Token: 0x06000B7E RID: 2942 RVA: 0x00028A69 File Offset: 0x00026C69
		[DataSourceProperty]
		public string BannerCodeAsString
		{
			get
			{
				return this._bannerCodeAsString;
			}
			set
			{
				if (value != this._bannerCodeAsString)
				{
					this._bannerCodeAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "BannerCodeAsString");
				}
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00028A8C File Offset: 0x00026C8C
		// (set) Token: 0x06000B80 RID: 2944 RVA: 0x00028A94 File Offset: 0x00026C94
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00028AB7 File Offset: 0x00026CB7
		// (set) Token: 0x06000B82 RID: 2946 RVA: 0x00028ABF File Offset: 0x00026CBF
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00028AE2 File Offset: 0x00026CE2
		// (set) Token: 0x06000B84 RID: 2948 RVA: 0x00028AEA File Offset: 0x00026CEA
		[DataSourceProperty]
		public BannerViewModel BannerVM
		{
			get
			{
				return this._bannerVM;
			}
			set
			{
				if (value != this._bannerVM)
				{
					this._bannerVM = value;
					base.OnPropertyChangedWithValue<BannerViewModel>(value, "BannerVM");
				}
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x00028B08 File Offset: 0x00026D08
		// (set) Token: 0x06000B86 RID: 2950 RVA: 0x00028B10 File Offset: 0x00026D10
		[DataSourceProperty]
		public string IconCodes
		{
			get
			{
				return this._iconCodes;
			}
			set
			{
				if (value != this._iconCodes)
				{
					this._iconCodes = value;
					base.OnPropertyChangedWithValue<string>(value, "IconCodes");
				}
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x00028B33 File Offset: 0x00026D33
		// (set) Token: 0x06000B88 RID: 2952 RVA: 0x00028B3B File Offset: 0x00026D3B
		[DataSourceProperty]
		public string ColorCodes
		{
			get
			{
				return this._colorCodes;
			}
			set
			{
				if (value != this._colorCodes)
				{
					this._colorCodes = value;
					base.OnPropertyChangedWithValue<string>(value, "ColorCodes");
				}
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00028B5E File Offset: 0x00026D5E
		// (set) Token: 0x06000B8A RID: 2954 RVA: 0x00028B66 File Offset: 0x00026D66
		[DataSourceProperty]
		public bool CanChangeBackgroundColor
		{
			get
			{
				return this._canChangeBackgroundColor;
			}
			set
			{
				if (value != this._canChangeBackgroundColor)
				{
					this._canChangeBackgroundColor = value;
					base.OnPropertyChangedWithValue(value, "CanChangeBackgroundColor");
				}
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x00028B84 File Offset: 0x00026D84
		// (set) Token: 0x06000B8C RID: 2956 RVA: 0x00028B8C File Offset: 0x00026D8C
		[DataSourceProperty]
		public bool IsBannerPreviewsActive
		{
			get
			{
				return this._isBannerPreviewsActive;
			}
			set
			{
				if (value != this._isBannerPreviewsActive)
				{
					this._isBannerPreviewsActive = value;
					base.OnPropertyChangedWithValue(value, "IsBannerPreviewsActive");
				}
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x00028BAA File Offset: 0x00026DAA
		// (set) Token: 0x06000B8E RID: 2958 RVA: 0x00028BB2 File Offset: 0x00026DB2
		[DataSourceProperty]
		public bool IsEditorPreviewActive
		{
			get
			{
				return this._isEditorPreviewActive;
			}
			set
			{
				if (value != this._isEditorPreviewActive)
				{
					this._isEditorPreviewActive = value;
					base.OnPropertyChangedWithValue(value, "IsEditorPreviewActive");
				}
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x00028BD0 File Offset: 0x00026DD0
		// (set) Token: 0x06000B90 RID: 2960 RVA: 0x00028BD8 File Offset: 0x00026DD8
		[DataSourceProperty]
		public bool IsLayerPreviewActive
		{
			get
			{
				return this._isLayerPreviewActive;
			}
			set
			{
				if (value != this._isLayerPreviewActive)
				{
					this._isLayerPreviewActive = value;
					base.OnPropertyChangedWithValue(value, "IsLayerPreviewActive");
				}
			}
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00028BF6 File Offset: 0x00026DF6
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00028C05 File Offset: 0x00026E05
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x00028C14 File Offset: 0x00026E14
		// (set) Token: 0x06000B94 RID: 2964 RVA: 0x00028C1C File Offset: 0x00026E1C
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x00028C3A File Offset: 0x00026E3A
		// (set) Token: 0x06000B96 RID: 2966 RVA: 0x00028C42 File Offset: 0x00026E42
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00028C60 File Offset: 0x00026E60
		private static ItemObject FindShield(BasicCharacterObject character, string desiredShieldID = "")
		{
			for (int i = 0; i < 4; i++)
			{
				EquipmentElement equipmentFromSlot = character.Equipment.GetEquipmentFromSlot((EquipmentIndex)i);
				ItemObject item = equipmentFromSlot.Item;
				if (((item != null) ? item.PrimaryWeapon : null) != null && equipmentFromSlot.Item.PrimaryWeapon.IsShield && equipmentFromSlot.Item.IsUsingTableau)
				{
					return equipmentFromSlot.Item;
				}
			}
			if (!string.IsNullOrEmpty(desiredShieldID))
			{
				ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(desiredShieldID);
				if (@object != null)
				{
					WeaponComponentData primaryWeapon = @object.PrimaryWeapon;
					if (primaryWeapon != null && primaryWeapon.IsShield)
					{
						return @object;
					}
				}
			}
			MBReadOnlyList<ItemObject> objectTypeList = Game.Current.ObjectManager.GetObjectTypeList<ItemObject>();
			for (int j = 0; j < objectTypeList.Count; j++)
			{
				ItemObject itemObject = objectTypeList[j];
				WeaponComponentData primaryWeapon2 = itemObject.PrimaryWeapon;
				if (primaryWeapon2 != null && primaryWeapon2.IsShield && itemObject.IsUsingTableau)
				{
					return itemObject;
				}
			}
			return null;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00028D47 File Offset: 0x00026F47
		private static BannerData GetDefaultBannerData()
		{
			return new BannerData(133, 171, 171, new Vec2(483f, 483f), new Vec2(764f, 764f), false, false, 0f);
		}

		// Token: 0x0400051B RID: 1307
		private const int PatternLayerIndex = 0;

		// Token: 0x0400051C RID: 1308
		public int ShieldSlotIndex = 3;

		// Token: 0x0400051D RID: 1309
		public int CurrentShieldIndex;

		// Token: 0x0400051E RID: 1310
		public ItemRosterElement ShieldRosterElement;

		// Token: 0x0400051F RID: 1311
		private readonly BasicCharacterObject _character;

		// Token: 0x04000520 RID: 1312
		private readonly Action<bool> _onExit;

		// Token: 0x04000521 RID: 1313
		private readonly Action _refresh;

		// Token: 0x04000522 RID: 1314
		private readonly Action _copyBannerCode;

		// Token: 0x04000523 RID: 1315
		private BannerImageIdentifierVM _bannerImageIdentifier;

		// Token: 0x04000524 RID: 1316
		private string _iconCodes;

		// Token: 0x04000525 RID: 1317
		private string _colorCodes;

		// Token: 0x04000526 RID: 1318
		private string _bannerCodeAsString;

		// Token: 0x04000527 RID: 1319
		private BannerViewModel _bannerVM;

		// Token: 0x04000528 RID: 1320
		private MBBindingList<BannerBuilderCategoryVM> _categories;

		// Token: 0x04000529 RID: 1321
		private MBBindingList<BannerBuilderLayerVM> _layers;

		// Token: 0x0400052A RID: 1322
		private BannerBuilderLayerVM _currentSelectedLayer;

		// Token: 0x0400052B RID: 1323
		private BannerBuilderItemVM _currentSelectedItem;

		// Token: 0x0400052C RID: 1324
		private BannerBuilderColorSelectionVM _colorSelection;

		// Token: 0x0400052D RID: 1325
		private string _title;

		// Token: 0x0400052E RID: 1326
		private string _cancelText;

		// Token: 0x0400052F RID: 1327
		private string _doneText;

		// Token: 0x04000530 RID: 1328
		private string _currentShieldName;

		// Token: 0x04000531 RID: 1329
		private bool _canChangeBackgroundColor;

		// Token: 0x04000532 RID: 1330
		private bool _isBannerPreviewsActive;

		// Token: 0x04000533 RID: 1331
		private bool _isEditorPreviewActive;

		// Token: 0x04000534 RID: 1332
		private bool _isLayerPreviewActive;

		// Token: 0x04000535 RID: 1333
		private int _minIconSize;

		// Token: 0x04000536 RID: 1334
		private int _maxIconSize;

		// Token: 0x04000537 RID: 1335
		private HintViewModel _resetHint;

		// Token: 0x04000538 RID: 1336
		private HintViewModel _randomizeHint;

		// Token: 0x04000539 RID: 1337
		private HintViewModel _undoHint;

		// Token: 0x0400053A RID: 1338
		private HintViewModel _redoHint;

		// Token: 0x0400053B RID: 1339
		private HintViewModel _drawStrokeHint;

		// Token: 0x0400053C RID: 1340
		private HintViewModel _centerHint;

		// Token: 0x0400053D RID: 1341
		private HintViewModel _resetSizeHint;

		// Token: 0x0400053E RID: 1342
		private HintViewModel _mirrorHint;

		// Token: 0x0400053F RID: 1343
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000540 RID: 1344
		private InputKeyItemVM _doneInputKey;
	}
}
