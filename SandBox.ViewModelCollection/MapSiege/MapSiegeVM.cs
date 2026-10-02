using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.MapSiege
{
	// Token: 0x02000056 RID: 86
	public class MapSiegeVM : ViewModel
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000142C5 File Offset: 0x000124C5
		private bool IsPlayerLeaderOfSiegeEvent
		{
			get
			{
				SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
				return playerSiegeEvent != null && playerSiegeEvent.IsPlayerSiegeEvent && Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(PlayerSiege.PlayerSiegeEvent, PlayerSiege.PlayerSide) == Hero.MainHero;
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00014304 File Offset: 0x00012504
		public MapSiegeVM(Camera mapCamera, MatrixFrame[] batteringRamFrames, MatrixFrame[] rangedSiegeEngineFrames, MatrixFrame[] towerSiegeEngineFrames, MatrixFrame[] defenderSiegeEngineFrames, MatrixFrame[] breachableWallFrames)
		{
			this._mapCamera = mapCamera;
			this.PointsOfInterest = new MBBindingList<MapSiegePOIVM>();
			this._poiDistanceComparer = new MapSiegeVM.SiegePOIDistanceComparer();
			for (int i = 0; i < batteringRamFrames.Length; i++)
			{
				this.PointsOfInterest.Add(new MapSiegePOIVM(MapSiegePOIVM.POIType.AttackerRamSiegeMachine, batteringRamFrames[i], this._mapCamera, i, new Action<MapSiegePOIVM>(this.OnPOISelection)));
			}
			for (int j = 0; j < rangedSiegeEngineFrames.Length; j++)
			{
				this.PointsOfInterest.Add(new MapSiegePOIVM(MapSiegePOIVM.POIType.AttackerRangedSiegeMachine, rangedSiegeEngineFrames[j], this._mapCamera, j, new Action<MapSiegePOIVM>(this.OnPOISelection)));
			}
			for (int k = 0; k < towerSiegeEngineFrames.Length; k++)
			{
				this.PointsOfInterest.Add(new MapSiegePOIVM(MapSiegePOIVM.POIType.AttackerTowerSiegeMachine, towerSiegeEngineFrames[k], this._mapCamera, batteringRamFrames.Length + k, new Action<MapSiegePOIVM>(this.OnPOISelection)));
			}
			for (int l = 0; l < defenderSiegeEngineFrames.Length; l++)
			{
				this.PointsOfInterest.Add(new MapSiegePOIVM(MapSiegePOIVM.POIType.DefenderSiegeMachine, defenderSiegeEngineFrames[l], this._mapCamera, l, new Action<MapSiegePOIVM>(this.OnPOISelection)));
			}
			for (int m = 0; m < breachableWallFrames.Length; m++)
			{
				this.PointsOfInterest.Add(new MapSiegePOIVM(MapSiegePOIVM.POIType.WallSection, breachableWallFrames[m], this._mapCamera, m, new Action<MapSiegePOIVM>(this.OnPOISelection)));
			}
			this.ProductionController = new MapSiegeProductionVM();
			this.RefreshValues();
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00014474 File Offset: 0x00012674
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PreparationTitleText = GameTexts.FindText("str_building_siege_camp", null).ToString();
			SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
			this.IsPreparationsCompleted = (playerSiegeEvent != null && playerSiegeEvent.BesiegerCamp.IsPreparationComplete) || PlayerSiege.PlayerSide == BattleSideEnum.Defender;
			this.ProductionController.RefreshValues();
			this.PointsOfInterest.ApplyActionOnAllItems(delegate(MapSiegePOIVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000144FB File Offset: 0x000126FB
		private void OnPOISelection(MapSiegePOIVM poi)
		{
			if (this.ProductionController.LatestSelectedPOI != null)
			{
				this.ProductionController.LatestSelectedPOI.IsSelected = false;
			}
			if (this.IsPlayerLeaderOfSiegeEvent)
			{
				this.ProductionController.OnMachineSelection(poi);
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00014530 File Offset: 0x00012730
		public void OnSelectionFromScene(MatrixFrame frameOfEngine)
		{
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				Settlement besiegedSettlement = PlayerSiege.BesiegedSettlement;
				if ((besiegedSettlement == null || besiegedSettlement.CurrentSiegeState != Settlement.SiegeState.InTheLordsHall) && this.IsPlayerLeaderOfSiegeEvent)
				{
					IEnumerable<MapSiegePOIVM> enumerable = this.PointsOfInterest.Where<MapSiegePOIVM>((MapSiegePOIVM poi) => frameOfEngine.NearlyEquals(poi.MapSceneLocationFrame, 1E-05f));
					if (enumerable == null)
					{
						return;
					}
					MapSiegePOIVM mapSiegePOIVM = enumerable.FirstOrDefault<MapSiegePOIVM>();
					if (mapSiegePOIVM == null)
					{
						return;
					}
					mapSiegePOIVM.ExecuteSelection();
				}
			}
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x000145A0 File Offset: 0x000127A0
		public void Update(float mapCameraDistanceValue)
		{
			SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
			this.IsPreparationsCompleted = (playerSiegeEvent != null && playerSiegeEvent.BesiegerCamp.IsPreparationComplete) || PlayerSiege.PlayerSide == BattleSideEnum.Defender;
			SiegeEvent playerSiegeEvent2 = PlayerSiege.PlayerSiegeEvent;
			float? num;
			if (playerSiegeEvent2 == null)
			{
				num = null;
			}
			else
			{
				SiegeEvent.SiegeEnginesContainer siegeEngines = playerSiegeEvent2.BesiegerCamp.SiegeEngines;
				if (siegeEngines == null)
				{
					num = null;
				}
				else
				{
					SiegeEvent.SiegeEngineConstructionProgress siegePreparations = siegeEngines.SiegePreparations;
					num = ((siegePreparations != null) ? new float?(siegePreparations.Progress) : null);
				}
			}
			this.PreparationProgress = num ?? 0f;
			TWParallel.For(0, this.PointsOfInterest.Count, delegate(int startInclusive, int endExclusive)
			{
				for (int i = startInclusive; i < endExclusive; i++)
				{
					this.PointsOfInterest[i].RefreshDistanceValue(mapCameraDistanceValue);
					this.PointsOfInterest[i].RefreshPosition();
					this.PointsOfInterest[i].UpdateProperties();
				}
			}, 16);
			foreach (MapSiegePOIVM mapSiegePOIVM in this.PointsOfInterest)
			{
				mapSiegePOIVM.RefreshBinding();
			}
			this.ProductionController.Update();
			this.PointsOfInterest.Sort(this._poiDistanceComparer);
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x000146CC File Offset: 0x000128CC
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x000146D4 File Offset: 0x000128D4
		[DataSourceProperty]
		public float PreparationProgress
		{
			get
			{
				return this._preparationProgress;
			}
			set
			{
				if (value != this._preparationProgress)
				{
					this._preparationProgress = value;
					base.OnPropertyChangedWithValue(value, "PreparationProgress");
				}
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x000146F2 File Offset: 0x000128F2
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x000146FA File Offset: 0x000128FA
		[DataSourceProperty]
		public bool IsPreparationsCompleted
		{
			get
			{
				return this._isPreparationsCompleted;
			}
			set
			{
				if (value != this._isPreparationsCompleted)
				{
					this._isPreparationsCompleted = value;
					base.OnPropertyChangedWithValue(value, "IsPreparationsCompleted");
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00014718 File Offset: 0x00012918
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00014720 File Offset: 0x00012920
		[DataSourceProperty]
		public string PreparationTitleText
		{
			get
			{
				return this._preparationTitleText;
			}
			set
			{
				if (value != this._preparationTitleText)
				{
					this._preparationTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreparationTitleText");
				}
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00014743 File Offset: 0x00012943
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x0001474B File Offset: 0x0001294B
		[DataSourceProperty]
		public MapSiegeProductionVM ProductionController
		{
			get
			{
				return this._productionController;
			}
			set
			{
				if (value != this._productionController)
				{
					this._productionController = value;
					base.OnPropertyChangedWithValue<MapSiegeProductionVM>(value, "ProductionController");
				}
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00014769 File Offset: 0x00012969
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x00014771 File Offset: 0x00012971
		[DataSourceProperty]
		public MBBindingList<MapSiegePOIVM> PointsOfInterest
		{
			get
			{
				return this._pointsOfInterest;
			}
			set
			{
				if (value != this._pointsOfInterest)
				{
					this._pointsOfInterest = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapSiegePOIVM>>(value, "PointsOfInterest");
				}
			}
		}

		// Token: 0x040002A7 RID: 679
		private readonly Camera _mapCamera;

		// Token: 0x040002A8 RID: 680
		private readonly MapSiegeVM.SiegePOIDistanceComparer _poiDistanceComparer;

		// Token: 0x040002A9 RID: 681
		private MBBindingList<MapSiegePOIVM> _pointsOfInterest;

		// Token: 0x040002AA RID: 682
		private MapSiegeProductionVM _productionController;

		// Token: 0x040002AB RID: 683
		private float _preparationProgress;

		// Token: 0x040002AC RID: 684
		private string _preparationTitleText;

		// Token: 0x040002AD RID: 685
		private bool _isPreparationsCompleted;

		// Token: 0x020000AE RID: 174
		public class SiegePOIDistanceComparer : IComparer<MapSiegePOIVM>
		{
			// Token: 0x060006D2 RID: 1746 RVA: 0x000172A8 File Offset: 0x000154A8
			public int Compare(MapSiegePOIVM x, MapSiegePOIVM y)
			{
				return y.LatestW.CompareTo(x.LatestW);
			}
		}
	}
}
