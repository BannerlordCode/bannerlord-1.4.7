using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.CampaignSystem.Map.DistanceCache
{
	// Token: 0x02000225 RID: 549
	public abstract class NavigationCache<T> where T : ISettlementDataHolder
	{
		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060020F0 RID: 8432 RVA: 0x00091FCD File Offset: 0x000901CD
		// (set) Token: 0x060020F1 RID: 8433 RVA: 0x00091FD5 File Offset: 0x000901D5
		public float MaximumDistanceBetweenTwoConnectedSettlements { get; protected set; }

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x00091FDE File Offset: 0x000901DE
		// (set) Token: 0x060020F3 RID: 8435 RVA: 0x00091FE6 File Offset: 0x000901E6
		private protected MobileParty.NavigationType _navigationType { protected get; private set; }

		// Token: 0x060020F4 RID: 8436 RVA: 0x00091FEF File Offset: 0x000901EF
		protected NavigationCache(MobileParty.NavigationType navigationType)
		{
			this._navigationType = navigationType;
			this._settlementToSettlementDistanceWithLandRatio = new Dictionary<NavigationCacheElement<T>, Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>>();
			this._fortificationNeighbors = new Dictionary<T, MBReadOnlyList<T>>();
			this._closestSettlementsToFaceIndices = new Dictionary<int, NavigationCacheElement<T>>();
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x00092020 File Offset: 0x00090220
		protected void FinalizeCacheInitialization()
		{
			if (this._fortificationNeighbors != null)
			{
				if (!this._fortificationNeighbors.AnyQ<KeyValuePair<T, MBReadOnlyList<T>>>((KeyValuePair<T, MBReadOnlyList<T>> x) => x.Value.Count == 0))
				{
					return;
				}
			}
			Debug.FailedAssert("There is settlement with zero neighbor in neighbor cache, this should not be happening, check here", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Map\\DistanceCache\\NavigationCache.cs", "FinalizeCacheInitialization", 44);
			this.GenerateNeighborSettlementsCache();
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x00092080 File Offset: 0x00090280
		public static void CopyTo<T1>(NavigationCache<T1> source, NavigationCache<T> target) where T1 : ISettlementDataHolder
		{
			target._navigationType = source._navigationType;
			target.MaximumDistanceBetweenTwoConnectedSettlements = source.MaximumDistanceBetweenTwoConnectedSettlements;
			target._settlementToSettlementDistanceWithLandRatio = new Dictionary<NavigationCacheElement<T>, Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>>(source._settlementToSettlementDistanceWithLandRatio.Count);
			foreach (KeyValuePair<NavigationCacheElement<T1>, Dictionary<NavigationCacheElement<T1>, ValueTuple<float, float>>> keyValuePair in source._settlementToSettlementDistanceWithLandRatio)
			{
				NavigationCacheElement<T> cacheElement = target.GetCacheElement(target.GetCacheElement(keyValuePair.Key.StringId), keyValuePair.Key.IsPortUsed);
				Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>> dictionary = new Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>(keyValuePair.Value.Count);
				target._settlementToSettlementDistanceWithLandRatio.Add(cacheElement, dictionary);
				foreach (KeyValuePair<NavigationCacheElement<T1>, ValueTuple<float, float>> keyValuePair2 in keyValuePair.Value)
				{
					NavigationCacheElement<T> cacheElement2 = target.GetCacheElement(target.GetCacheElement(keyValuePair2.Key.StringId), keyValuePair2.Key.IsPortUsed);
					dictionary.Add(cacheElement2, keyValuePair2.Value);
				}
			}
			target._fortificationNeighbors = new Dictionary<T, MBReadOnlyList<T>>(source._fortificationNeighbors.Count);
			foreach (KeyValuePair<T1, MBReadOnlyList<T1>> keyValuePair3 in source._fortificationNeighbors)
			{
				T1 key = keyValuePair3.Key;
				T cacheElement3 = target.GetCacheElement(key.StringId);
				List<T> list = new List<T>(keyValuePair3.Value.Count);
				target._fortificationNeighbors.Add(cacheElement3, list.ToMBList<T>());
				foreach (T1 t in keyValuePair3.Value)
				{
					T cacheElement4 = target.GetCacheElement(t.StringId);
					list.Add(cacheElement4);
				}
			}
			target._closestSettlementsToFaceIndices = new Dictionary<int, NavigationCacheElement<T>>();
			foreach (KeyValuePair<int, NavigationCacheElement<T1>> keyValuePair4 in source._closestSettlementsToFaceIndices)
			{
				NavigationCacheElement<T> cacheElement5 = target.GetCacheElement(target.GetCacheElement(keyValuePair4.Value.StringId), keyValuePair4.Value.IsPortUsed);
				target._closestSettlementsToFaceIndices.Add(keyValuePair4.Key, cacheElement5);
			}
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x00092340 File Offset: 0x00090540
		public MBReadOnlyList<T> GetNeighbors(T settlement)
		{
			MBReadOnlyList<T> mbreadOnlyList;
			if (!this._fortificationNeighbors.TryGetValue(settlement, out mbreadOnlyList))
			{
				mbreadOnlyList = new MBReadOnlyList<T>();
			}
			return mbreadOnlyList;
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x00092364 File Offset: 0x00090564
		public T GetClosestSettlementToFaceIndex(int faceId, out bool isAtSea)
		{
			NavigationCacheElement<T> navigationCacheElement;
			if (this._closestSettlementsToFaceIndices.TryGetValue(faceId, out navigationCacheElement))
			{
				isAtSea = navigationCacheElement.IsPortUsed;
				return navigationCacheElement.Settlement;
			}
			isAtSea = false;
			return default(T);
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x0009239C File Offset: 0x0009059C
		public void GenerateCacheData()
		{
			this.GenerateClosestSettlementToFaceCache();
			this.GenerateSettlementToSettlementDistanceCache();
			this.GenerateNeighborSettlementsCache();
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000923B0 File Offset: 0x000905B0
		protected float GetSettlementToSettlementDistanceWithLandRatio(NavigationCacheElement<T> settlement1, NavigationCacheElement<T> settlement2, out float landRatio)
		{
			bool flag;
			NavigationCacheElement<T>.Sort(ref settlement1, ref settlement2, out flag);
			Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>> dictionary;
			if (!this._settlementToSettlementDistanceWithLandRatio.TryGetValue(settlement1, out dictionary))
			{
				dictionary = new Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>();
				this._settlementToSettlementDistanceWithLandRatio.Add(settlement1, dictionary);
			}
			ValueTuple<float, float> valueTuple;
			if (!dictionary.TryGetValue(settlement2, out valueTuple))
			{
				float realDistanceAndLandRatioBetweenSettlements = this.GetRealDistanceAndLandRatioBetweenSettlements(settlement1, settlement2, out landRatio);
				this.SetSettlementToSettlementDistanceWithLandRatio(settlement1, settlement2, realDistanceAndLandRatioBetweenSettlements, landRatio);
				valueTuple = new ValueTuple<float, float>(realDistanceAndLandRatioBetweenSettlements, landRatio);
			}
			landRatio = valueTuple.Item2;
			return valueTuple.Item1;
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x00092424 File Offset: 0x00090624
		protected void SetSettlementToSettlementDistanceWithLandRatio(NavigationCacheElement<T> settlement1, NavigationCacheElement<T> settlement2, float distance, float landRatio)
		{
			bool flag;
			NavigationCacheElement<T>.Sort(ref settlement1, ref settlement2, out flag);
			Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>> dictionary;
			if (!this._settlementToSettlementDistanceWithLandRatio.TryGetValue(settlement1, out dictionary))
			{
				dictionary = new Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>();
				this._settlementToSettlementDistanceWithLandRatio.Add(settlement1, dictionary);
			}
			ValueTuple<float, float> valueTuple;
			if (dictionary.TryGetValue(settlement2, out valueTuple))
			{
				Debug.FailedAssert("Element already exists", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Map\\DistanceCache\\NavigationCache.cs", "SetSettlementToSettlementDistanceWithLandRatio", 215);
			}
			else
			{
				dictionary.Add(settlement2, new ValueTuple<float, float>(distance, landRatio));
			}
			if (distance < 100000000f && distance > this.MaximumDistanceBetweenTwoConnectedSettlements)
			{
				this.MaximumDistanceBetweenTwoConnectedSettlements = distance;
			}
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000924AC File Offset: 0x000906AC
		protected void AddNeighbor(T settlement1, T settlement2)
		{
			bool flag = false;
			foreach (KeyValuePair<T, MBReadOnlyList<T>> keyValuePair in this._fortificationNeighbors)
			{
				T t = keyValuePair.Key;
				if (!t.StringId.Equals(settlement1.StringId) || !keyValuePair.Value.Contains(settlement2))
				{
					t = keyValuePair.Key;
					if (!t.StringId.Equals(settlement2.StringId) || !keyValuePair.Value.Contains(settlement1))
					{
						continue;
					}
				}
				flag = true;
				break;
			}
			if (!flag)
			{
				MBReadOnlyList<T> mbreadOnlyList;
				if (!this._fortificationNeighbors.TryGetValue(settlement1, out mbreadOnlyList))
				{
					this._fortificationNeighbors.Add(settlement1, new MBReadOnlyList<T>());
				}
				MBList<T> mblist;
				if (mbreadOnlyList != null)
				{
					mblist = new MBList<T>(mbreadOnlyList.Count + 1);
					mblist.AddRange(mbreadOnlyList);
				}
				else
				{
					mblist = new MBList<T>(1);
				}
				mblist.Add(settlement2);
				this._fortificationNeighbors[settlement1] = mblist;
				MBReadOnlyList<T> mbreadOnlyList2;
				if (!this._fortificationNeighbors.TryGetValue(settlement2, out mbreadOnlyList2))
				{
					this._fortificationNeighbors.Add(settlement2, new MBReadOnlyList<T>());
				}
				if (mbreadOnlyList2 != null)
				{
					mblist = new MBList<T>(mbreadOnlyList2.Count + 1);
					mblist.AddRange(mbreadOnlyList2);
				}
				else
				{
					mblist = new MBList<T>(1);
				}
				mblist.Add(settlement1);
				this._fortificationNeighbors[settlement2] = mblist;
			}
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x00092634 File Offset: 0x00090834
		protected void SetClosestSettlementToFaceIndex(int faceId, NavigationCacheElement<T> settlement)
		{
			this._closestSettlementsToFaceIndices.Add(faceId, settlement);
		}

		// Token: 0x060020FE RID: 8446
		protected abstract float GetRealDistanceAndLandRatioBetweenSettlements(NavigationCacheElement<T> settlement1, NavigationCacheElement<T> settlement2, out float landRatio);

		// Token: 0x060020FF RID: 8447
		protected abstract T GetCacheElement(string settlementId);

		// Token: 0x06002100 RID: 8448
		protected abstract NavigationCacheElement<T> GetCacheElement(T settlement, bool isPortUsed);

		// Token: 0x06002101 RID: 8449 RVA: 0x00092644 File Offset: 0x00090844
		protected float GetLandRatioOfPath(NavigationPath path, Vec2 startPosition)
		{
			float num = 0f;
			float num2 = 0f;
			if (path.Size > 1)
			{
				List<Vec2> list = new List<Vec2>(path.PathPoints);
				list.Insert(0, startPosition);
				for (int i = 0; i < list.Count - 1; i++)
				{
					Vec2 vec = list[i];
					Vec2 vec2 = list[i + 1];
					if (vec2 == Vec2.Zero)
					{
						IL_015F:
						return MBMath.ClampFloat(num / num2, 0f, 1f);
					}
					Vec2 vec3 = vec2 - vec;
					float num3 = vec3.Length / 0.5f;
					vec3.Normalize();
					int num4 = 0;
					while ((float)num4 < num3 - 1f)
					{
						Vec2 vec4 = vec + vec3 * (float)num4 * 0.5f;
						Vec2 vec5 = vec + vec3 * (float)(num4 + 1) * 0.5f;
						bool flag;
						this.GetFaceRecordForPoint(vec4, out flag);
						bool flag2;
						this.GetFaceRecordForPoint(vec5, out flag2);
						float num5 = vec4.Distance(vec5);
						if (flag2 && flag)
						{
							num += num5;
						}
						else if (flag2 != flag)
						{
							num += num5 / 2f;
						}
						num2 += num5;
						num4++;
					}
				}
				goto IL_015F;
			}
			bool flag3;
			this.GetFaceRecordForPoint(startPosition, out flag3);
			bool flag4;
			this.GetFaceRecordForPoint(path[0], out flag4);
			if (flag4 != flag3)
			{
				return 0.5f;
			}
			if (flag4)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06002102 RID: 8450
		protected abstract void GetFaceRecordForPoint(Vec2 position, out bool isOnRegion1);

		// Token: 0x06002103 RID: 8451 RVA: 0x000927C4 File Offset: 0x000909C4
		protected void GenerateClosestSettlementToFaceCache()
		{
			int navMeshFaceCount = this.GetNavMeshFaceCount();
			for (int i = 0; i < navMeshFaceCount; i++)
			{
				Debug.Print(string.Format("Face-Settlement cache creation progress % {0}     {1}", i * 100 / navMeshFaceCount, this._navigationType), 0, Debug.DebugColor.White, 17592186044416UL);
				Vec2 navMeshFaceCenterPosition = this.GetNavMeshFaceCenterPosition(i);
				PathFaceRecord faceRecordAtIndex = this.GetFaceRecordAtIndex(i);
				bool flag = false;
				T closestSettlementToPosition = this.GetClosestSettlementToPosition(navMeshFaceCenterPosition, faceRecordAtIndex, this.GetExcludedFaceIds(), this.GetAllRegisteredSettlements(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1(), float.MaxValue, out flag);
				if (!object.Equals(closestSettlementToPosition, default(T)))
				{
					this.SetClosestSettlementToFaceIndex(i, new NavigationCacheElement<T>(closestSettlementToPosition, flag));
				}
			}
		}

		// Token: 0x06002104 RID: 8452
		protected abstract int GetNavMeshFaceCount();

		// Token: 0x06002105 RID: 8453
		protected abstract Vec2 GetNavMeshFaceCenterPosition(int faceIndex);

		// Token: 0x06002106 RID: 8454
		protected abstract PathFaceRecord GetFaceRecordAtIndex(int faceIndex);

		// Token: 0x06002107 RID: 8455
		protected abstract int[] GetExcludedFaceIds();

		// Token: 0x06002108 RID: 8456
		protected abstract int GetRegionSwitchCostTo0();

		// Token: 0x06002109 RID: 8457
		protected abstract int GetRegionSwitchCostTo1();

		// Token: 0x0600210A RID: 8458 RVA: 0x00092884 File Offset: 0x00090A84
		protected void GenerateSettlementToSettlementDistanceCache()
		{
			List<T> allRegisteredSettlements = this.GetAllRegisteredSettlements();
			for (int i = 0; i < allRegisteredSettlements.Count; i++)
			{
				Debug.Print(string.Format("Settlement to settlement cache creation index {0},    total count: {1}     {2}", i, allRegisteredSettlements.Count, this._navigationType), 0, Debug.DebugColor.White, 17592186044416UL);
				T t = allRegisteredSettlements[i];
				for (int j = ((this._navigationType == MobileParty.NavigationType.All) ? i : (i + 1)); j < allRegisteredSettlements.Count; j++)
				{
					T t2 = allRegisteredSettlements[j];
					if (this._navigationType == MobileParty.NavigationType.Default)
					{
						this.AddClosestEntrancePairBase(t, false, t2, false);
					}
					else if (this._navigationType == MobileParty.NavigationType.Naval)
					{
						if (t.HasPort && t2.HasPort)
						{
							this.AddClosestEntrancePairBase(t, true, t2, true);
						}
					}
					else if (this._navigationType == MobileParty.NavigationType.All)
					{
						this.AddClosestEntrancePairBase(t, false, t2, false);
						if (t.HasPort && t2.HasPort)
						{
							this.AddClosestEntrancePairBase(t, true, t2, true);
						}
						if (t2.HasPort)
						{
							this.AddClosestEntrancePairBase(t, false, t2, true);
						}
						if (t.HasPort)
						{
							this.AddClosestEntrancePairBase(t, true, t2, false);
						}
					}
				}
			}
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x000929E0 File Offset: 0x00090BE0
		private void AddClosestEntrancePairBase(T settlement1, bool isPort1, T settlement2, bool isPort2)
		{
			NavigationCacheElement<T> cacheElement = this.GetCacheElement(settlement1, isPort1);
			NavigationCacheElement<T> cacheElement2 = this.GetCacheElement(settlement2, isPort2);
			float num;
			float realDistanceAndLandRatioBetweenSettlements = this.GetRealDistanceAndLandRatioBetweenSettlements(cacheElement, cacheElement2, out num);
			float num2;
			float realDistanceAndLandRatioBetweenSettlements2 = this.GetRealDistanceAndLandRatioBetweenSettlements(cacheElement2, cacheElement, out num2);
			float num3 = (realDistanceAndLandRatioBetweenSettlements + realDistanceAndLandRatioBetweenSettlements2) * 0.5f;
			if (num3 > 0f)
			{
				float num4 = 1f;
				if (this._navigationType == MobileParty.NavigationType.Naval)
				{
					num4 = 0f;
				}
				else if (this._navigationType == MobileParty.NavigationType.All)
				{
					num4 = num;
				}
				bool flag;
				NavigationCacheElement<T>.Sort(ref cacheElement, ref cacheElement2, out flag);
				if (flag)
				{
					num4 = num2;
				}
				this.SetSettlementToSettlementDistanceWithLandRatio(cacheElement, cacheElement2, num3, num4);
			}
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x00092A6C File Offset: 0x00090C6C
		protected void GenerateNeighborSettlementsCache()
		{
			this._fortificationNeighbors.Clear();
			List<T> updatedSettlementsForNeighborDetection = this.GetUpdatedSettlementsForNeighborDetection(this.GetAllRegisteredSettlements());
			for (int i = 0; i < updatedSettlementsForNeighborDetection.Count - 1; i++)
			{
				Debug.Print(string.Format("Neighbor cache progress for navigation {0}, current index: {1}  - total count: {2}", this._navigationType, i, updatedSettlementsForNeighborDetection.Count), 0, Debug.DebugColor.White, 17592186044416UL);
				T t = updatedSettlementsForNeighborDetection[i];
				if (t.IsFortification)
				{
					for (int j = i + 1; j < updatedSettlementsForNeighborDetection.Count; j++)
					{
						T t2 = updatedSettlementsForNeighborDetection[j];
						if (t2.IsFortification && this.CheckBeingNeighbor(updatedSettlementsForNeighborDetection, t, t2))
						{
							this.AddNeighbor(t, t2);
						}
					}
				}
			}
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x00092B38 File Offset: 0x00090D38
		private void CheckNeighbourAux(List<T> settlementsToConsider, T settlement1, T settlement2, bool useGate1, bool useGate2, ref float distance, ref bool isNeighbour)
		{
			float num;
			bool flag = this.CheckBeingNeighbor(settlementsToConsider, settlement1, settlement2, useGate1, useGate2, out num);
			if (num < distance)
			{
				distance = num;
				isNeighbour = flag;
			}
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x00092B64 File Offset: 0x00090D64
		protected bool CheckBeingNeighbor(List<T> settlementsToConsider, T settlement1, T settlement2)
		{
			float maxValue = float.MaxValue;
			bool flag = false;
			if (this._navigationType == MobileParty.NavigationType.Default || this._navigationType == MobileParty.NavigationType.All)
			{
				this.CheckNeighbourAux(settlementsToConsider, settlement1, settlement2, true, true, ref maxValue, ref flag);
				this.CheckNeighbourAux(settlementsToConsider, settlement2, settlement1, true, true, ref maxValue, ref flag);
			}
			if (this._navigationType == MobileParty.NavigationType.Naval || this._navigationType == MobileParty.NavigationType.All)
			{
				bool hasPort = settlement1.HasPort;
				bool hasPort2 = settlement2.HasPort;
				if (hasPort)
				{
					this.CheckNeighbourAux(settlementsToConsider, settlement1, settlement2, false, true, ref maxValue, ref flag);
					this.CheckNeighbourAux(settlementsToConsider, settlement2, settlement1, true, false, ref maxValue, ref flag);
				}
				if (hasPort2)
				{
					this.CheckNeighbourAux(settlementsToConsider, settlement1, settlement2, true, false, ref maxValue, ref flag);
					this.CheckNeighbourAux(settlementsToConsider, settlement2, settlement1, false, true, ref maxValue, ref flag);
				}
				if (hasPort2 && hasPort)
				{
					this.CheckNeighbourAux(settlementsToConsider, settlement1, settlement2, false, false, ref maxValue, ref flag);
					this.CheckNeighbourAux(settlementsToConsider, settlement2, settlement1, false, false, ref maxValue, ref flag);
				}
			}
			return flag;
		}

		// Token: 0x0600210F RID: 8463
		protected abstract List<T> GetAllRegisteredSettlements();

		// Token: 0x06002110 RID: 8464 RVA: 0x00092C3C File Offset: 0x00090E3C
		protected List<T> GetUpdatedSettlementsForNeighborDetection(List<T> settlements)
		{
			if (this._navigationType == MobileParty.NavigationType.Naval)
			{
				return settlements.Where<T>((T x) => x.IsFortification && x.HasPort).ToList<T>();
			}
			return settlements.Where<T>((T x) => x.IsFortification).ToList<T>();
		}

		// Token: 0x06002111 RID: 8465
		protected abstract bool CheckBeingNeighbor(List<T> settlementsToConsider, T settlement1, T settlement2, bool useGate1, bool useGate2, out float foundDistance);

		// Token: 0x06002112 RID: 8466
		protected abstract float GetRealPathDistanceFromPositionToSettlement(Vec2 checkPosition, PathFaceRecord currentFaceRecord, float maxDistanceToLookForPathDetection, T currentSettlementToLook, out bool isPort);

		// Token: 0x06002113 RID: 8467 RVA: 0x00092CA8 File Offset: 0x00090EA8
		protected T GetClosestSettlementToPosition(Vec2 checkPosition, PathFaceRecord currentFaceRecord, int[] excludedFaceIds, List<T> settlementRecords, int regionSwitchCostTo0, int regionSwitchCostTo1, float minPathScoreEverFound, out bool isPort)
		{
			isPort = false;
			T t = default(T);
			foreach (T t2 in this.GetClosestSettlementsToPositionInCache(checkPosition, settlementRecords))
			{
				bool flag;
				float realPathDistanceFromPositionToSettlement = this.GetRealPathDistanceFromPositionToSettlement(checkPosition, currentFaceRecord, minPathScoreEverFound * 2f, t2, out flag);
				if (realPathDistanceFromPositionToSettlement < minPathScoreEverFound)
				{
					minPathScoreEverFound = realPathDistanceFromPositionToSettlement;
					t = t2;
					isPort = flag;
				}
			}
			return t;
		}

		// Token: 0x06002114 RID: 8468
		protected abstract IEnumerable<T> GetClosestSettlementsToPositionInCache(Vec2 checkPosition, List<T> settlements);

		// Token: 0x06002115 RID: 8469
		public abstract void GetSceneXmlCrcValues(out uint sceneXmlCrc, out uint sceneNavigationMeshCrc);

		// Token: 0x06002116 RID: 8470 RVA: 0x00092D24 File Offset: 0x00090F24
		public bool GetSettlementsDistanceCacheFileForCapability(string moduleId, out string filePath)
		{
			string text = ModuleHelper.GetModuleFullPath(moduleId) + "ModuleData/DistanceCaches";
			string text2 = this._navigationType.ToString();
			filePath = text + "/settlements_distance_cache_" + text2 + ".bin";
			bool flag = File.Exists(filePath);
			if (flag)
			{
				Debug.Print(string.Format("Found distance cache at: {0}, {1}, {2}", moduleId, text, this._navigationType), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			return flag;
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x00092D9C File Offset: 0x00090F9C
		public void Serialize(string path)
		{
			BinaryWriter binaryWriter = new BinaryWriter(File.Open(path, FileMode.Create));
			uint num;
			uint num2;
			this.GetSceneXmlCrcValues(out num, out num2);
			binaryWriter.Write(num);
			binaryWriter.Write(num2);
			binaryWriter.Write(this._settlementToSettlementDistanceWithLandRatio.Count);
			foreach (KeyValuePair<NavigationCacheElement<T>, Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>> keyValuePair in this._settlementToSettlementDistanceWithLandRatio)
			{
				binaryWriter.Write(keyValuePair.Key.StringId);
				binaryWriter.Write(keyValuePair.Key.IsPortUsed);
				binaryWriter.Write(keyValuePair.Value.Count);
				foreach (KeyValuePair<NavigationCacheElement<T>, ValueTuple<float, float>> keyValuePair2 in keyValuePair.Value)
				{
					binaryWriter.Write(keyValuePair2.Key.StringId);
					binaryWriter.Write(keyValuePair2.Key.IsPortUsed);
					binaryWriter.Write(keyValuePair2.Value.Item1);
					if (this._navigationType == MobileParty.NavigationType.All)
					{
						binaryWriter.Write(keyValuePair2.Value.Item2);
					}
				}
			}
			binaryWriter.Write(this._fortificationNeighbors.SumQ<KeyValuePair<T, MBReadOnlyList<T>>>((KeyValuePair<T, MBReadOnlyList<T>> x) => x.Value.Count));
			foreach (KeyValuePair<T, MBReadOnlyList<T>> keyValuePair3 in this._fortificationNeighbors)
			{
				T key = keyValuePair3.Key;
				string stringId = key.StringId;
				foreach (T t in keyValuePair3.Value)
				{
					binaryWriter.Write(stringId);
					binaryWriter.Write(t.StringId);
				}
			}
			binaryWriter.Write(this._closestSettlementsToFaceIndices.Count);
			foreach (KeyValuePair<int, NavigationCacheElement<T>> keyValuePair4 in this._closestSettlementsToFaceIndices)
			{
				binaryWriter.Write(keyValuePair4.Key);
				binaryWriter.Write(keyValuePair4.Value.StringId);
				binaryWriter.Write(keyValuePair4.Value.IsPortUsed);
			}
			binaryWriter.Close();
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x0009305C File Offset: 0x0009125C
		public void Deserialize(string path)
		{
			Debug.Print("Reading SettlementsDistanceCacheFilePath: " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
			binaryReader.ReadUInt32();
			binaryReader.ReadUInt32();
			Campaign.Current.MapSceneWrapper.GetSceneXmlCrc();
			Campaign.Current.MapSceneWrapper.GetSceneNavigationMeshCrc();
			int num = binaryReader.ReadInt32();
			this._settlementToSettlementDistanceWithLandRatio = new Dictionary<NavigationCacheElement<T>, Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>>(num);
			for (int i = 0; i < num; i++)
			{
				T cacheElement = this.GetCacheElement(binaryReader.ReadString());
				bool flag = binaryReader.ReadBoolean();
				NavigationCacheElement<T> cacheElement2 = this.GetCacheElement(cacheElement, flag);
				int num2 = binaryReader.ReadInt32();
				this._settlementToSettlementDistanceWithLandRatio.Add(cacheElement2, new Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>(num2));
				for (int j = 0; j < num2; j++)
				{
					T cacheElement3 = this.GetCacheElement(binaryReader.ReadString());
					bool flag2 = binaryReader.ReadBoolean();
					NavigationCacheElement<T> cacheElement4 = this.GetCacheElement(cacheElement3, flag2);
					bool flag3;
					NavigationCacheElement<T>.Sort(ref cacheElement2, ref cacheElement4, out flag3);
					float num3 = binaryReader.ReadSingle();
					float num4 = ((this._navigationType == MobileParty.NavigationType.Naval) ? 0f : 1f);
					if (this._navigationType == MobileParty.NavigationType.All)
					{
						num4 = binaryReader.ReadSingle();
					}
					this.SetSettlementToSettlementDistanceWithLandRatio(cacheElement2, cacheElement4, num3, num4);
				}
			}
			int num5 = binaryReader.ReadInt32();
			this._fortificationNeighbors = new Dictionary<T, MBReadOnlyList<T>>(num5);
			for (int k = 0; k < num5; k++)
			{
				T cacheElement5 = this.GetCacheElement(binaryReader.ReadString());
				T cacheElement6 = this.GetCacheElement(binaryReader.ReadString());
				this.AddNeighbor(cacheElement5, cacheElement6);
			}
			int num6 = binaryReader.ReadInt32();
			this._closestSettlementsToFaceIndices = new Dictionary<int, NavigationCacheElement<T>>(num6);
			for (int l = 0; l < num6; l++)
			{
				int num7 = binaryReader.ReadInt32();
				T cacheElement7 = this.GetCacheElement(binaryReader.ReadString());
				bool flag4 = binaryReader.ReadBoolean();
				NavigationCacheElement<T> cacheElement8 = this.GetCacheElement(cacheElement7, flag4);
				this.SetClosestSettlementToFaceIndex(num7, cacheElement8);
			}
			binaryReader.Close();
		}

		// Token: 0x040009B9 RID: 2489
		private Dictionary<NavigationCacheElement<T>, Dictionary<NavigationCacheElement<T>, ValueTuple<float, float>>> _settlementToSettlementDistanceWithLandRatio;

		// Token: 0x040009BA RID: 2490
		private Dictionary<T, MBReadOnlyList<T>> _fortificationNeighbors;

		// Token: 0x040009BB RID: 2491
		private Dictionary<int, NavigationCacheElement<T>> _closestSettlementsToFaceIndices;

		// Token: 0x040009BC RID: 2492
		protected const float AgentRadius = 0.3f;

		// Token: 0x040009BD RID: 2493
		protected const float ExtraCostMultiplierForNeighborDetection = 2f;
	}
}
