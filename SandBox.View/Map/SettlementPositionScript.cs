using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Map.DistanceCache;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;

namespace SandBox.View.Map
{
	// Token: 0x0200005E RID: 94
	public class SettlementPositionScript : ScriptComponentBehavior
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0001CC34 File Offset: 0x0001AE34
		private string SettlementsXmlPath
		{
			get
			{
				string text = base.Scene.GetModulePath();
				if (text.Contains("$BASE"))
				{
					text = text.Remove(0, 6);
					text = BasePath.Name + text;
				}
				return text + "ModuleData/settlements.xml";
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0001CC7C File Offset: 0x0001AE7C
		protected override void OnInit()
		{
			try
			{
				this.InitializeCachedVariables();
				bool flag = false;
				if (this.GetMapIsNavalDLC() || (!this.GetMapIsSandBox() && ModuleHelper.IsModuleActive("NavalDLC")))
				{
					flag = true;
				}
				this.RegisterNavigationCachesOnGameLoad(flag);
			}
			catch (Exception ex)
			{
				Debug.Print("Error when reading distance cache " + ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print("SettlementsDistanceCacheFilePath could not be read!. Campaign starting performance will be affected very badly, cache will be initialized now.", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert("SettlementsDistanceCacheFilePath could not be read!. Campaign starting performance will be affected very badly, cache will be initialized now.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\SettlementPositionScript.cs", "OnInit", 536);
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001CD1C File Offset: 0x0001AF1C
		private void RegisterNavigationCachesOnGameLoad(bool useNavalNavigation)
		{
			SandBoxNavigationCache sandBoxNavigationCache = this.ReadNavigationCacheForNavigationTypeOnGameLoad(MobileParty.NavigationType.Default);
			this._mapDistanceModel.RegisterDistanceCache(MobileParty.NavigationType.Default, sandBoxNavigationCache);
			if (useNavalNavigation)
			{
				SandBoxNavigationCache sandBoxNavigationCache2 = this.ReadNavigationCacheForNavigationTypeOnGameLoad(MobileParty.NavigationType.Naval);
				SandBoxNavigationCache sandBoxNavigationCache3 = this.ReadNavigationCacheForNavigationTypeOnGameLoad(MobileParty.NavigationType.All);
				this._mapDistanceModel.RegisterDistanceCache(MobileParty.NavigationType.Naval, sandBoxNavigationCache2);
				this._mapDistanceModel.RegisterDistanceCache(MobileParty.NavigationType.All, sandBoxNavigationCache3);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001CD6C File Offset: 0x0001AF6C
		private SandBoxNavigationCache ReadNavigationCacheForNavigationTypeOnGameLoad(MobileParty.NavigationType navigationCapability)
		{
			string text = string.Empty;
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetActiveModules())
			{
				string text2;
				if (moduleInfo.IsActive && this.GetSettlementsDistanceCacheFileForCapability(moduleInfo.Id, navigationCapability, out text2))
				{
					text = text2;
				}
			}
			SandBoxNavigationCache sandBoxNavigationCache;
			if (!string.IsNullOrEmpty(text))
			{
				sandBoxNavigationCache = this.ReadNavigationCacheOnGameLoad(text, navigationCapability);
			}
			else
			{
				Debug.FailedAssert(string.Format("Navigation type with id {0} file is not found, this should not be happening, will generate cache (this will take some time)", navigationCapability), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\SettlementPositionScript.cs", "ReadNavigationCacheForNavigationTypeOnGameLoad", 576);
				sandBoxNavigationCache = new SandBoxNavigationCache(navigationCapability);
				sandBoxNavigationCache.GenerateCacheData();
			}
			return sandBoxNavigationCache;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001CE20 File Offset: 0x0001B020
		private SandBoxNavigationCache ReadNavigationCacheOnGameLoad(string path, MobileParty.NavigationType navigationCapability)
		{
			SandBoxNavigationCache sandBoxNavigationCache = new SandBoxNavigationCache(navigationCapability);
			sandBoxNavigationCache.Deserialize(path);
			return sandBoxNavigationCache;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001CE2F File Offset: 0x0001B02F
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
			this._partyNavigationModelOverriddenClassName = "";
			this._distanceModelOverridenClassName = "";
			this.InitializeCachedVariables();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001CE54 File Offset: 0x0001B054
		protected override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "SavePositions")
			{
				this.SaveSettlementPositions();
			}
			if (variableName == "ComputeAndSaveSettlementDistanceCache")
			{
				this.SaveSettlementDistanceCacheEditor();
			}
			if (variableName == "CheckPositions")
			{
				this.CheckSettlementPositions();
			}
			if (variableName == "_partyNavigationModelOverriddenClassName" || variableName == "_distanceModelOverridenClassName")
			{
				this.InitializeCachedVariables();
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001CEC1 File Offset: 0x0001B0C1
		protected override void OnSceneSave(string saveFolder)
		{
			base.OnSceneSave(saveFolder);
			this.SaveSettlementPositions();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001CED0 File Offset: 0x0001B0D0
		private void CheckSettlementPositions()
		{
			XmlDocument xmlDocument = this.LoadXmlFile(this.SettlementsXmlPath);
			base.GameEntity.RemoveAllChildren();
			PartyNavigationModel partyNavigationModel = this.GetPartyNavigationModel();
			bool[] regionMapping = SandBoxHelpers.MapSceneHelper.GetRegionMapping(partyNavigationModel);
			base.GameEntity.Scene.SetNavMeshRegionMap(regionMapping);
			List<int> list = partyNavigationModel.GetInvalidTerrainTypesForNavigationType(MobileParty.NavigationType.Default).ToList<int>();
			list.Add(0);
			List<int> list2 = null;
			foreach (object obj in xmlDocument.DocumentElement.SelectNodes("Settlement"))
			{
				string value = ((XmlNode)obj).Attributes["id"].Value;
				GameEntity campaignEntityWithName = base.Scene.GetCampaignEntityWithName(value);
				if (campaignEntityWithName != null)
				{
					Vec3 origin = campaignEntityWithName.GetGlobalFrame().origin;
					Vec3 vec = default(Vec3);
					Vec3 vec2 = default(Vec3);
					List<GameEntity> list3 = new List<GameEntity>();
					campaignEntityWithName.GetChildrenRecursive(ref list3);
					bool flag = false;
					bool flag2 = false;
					foreach (GameEntity gameEntity in list3)
					{
						if (gameEntity.HasTag("main_map_city_gate"))
						{
							vec = gameEntity.GetGlobalFrame().origin;
							flag = true;
						}
						if (gameEntity.HasTag("main_map_city_port"))
						{
							vec2 = gameEntity.GetGlobalFrame().origin;
							flag2 = true;
						}
					}
					Vec3 vec3 = origin;
					if (flag)
					{
						vec3 = vec;
					}
					PathFaceRecord pathFaceRecord = PathFaceRecord.NullFaceRecord;
					base.GameEntity.Scene.GetNavMeshFaceIndex(ref pathFaceRecord, vec3.AsVec2, true, true, false);
					int num = 0;
					if (pathFaceRecord.IsValid())
					{
						num = pathFaceRecord.FaceGroupIndex;
					}
					if (list.Contains(num))
					{
						Debug.Print(string.Format("There is gate position problem with settlement {0} at position:  {1}", campaignEntityWithName.Name, vec3.AsVec2), 0, Debug.DebugColor.White, 17592186044416UL);
						MBEditor.ZoomToPosition(vec3);
						break;
					}
					if (flag2)
					{
						if (list2 == null)
						{
							list2 = partyNavigationModel.GetInvalidTerrainTypesForNavigationType(MobileParty.NavigationType.Naval).ToList<int>();
							list2.Add(0);
						}
						pathFaceRecord = PathFaceRecord.NullFaceRecord;
						base.GameEntity.Scene.GetNavMeshFaceIndex(ref pathFaceRecord, vec2.AsVec2, false, true, false);
						num = 0;
						if (pathFaceRecord.IsValid())
						{
							num = pathFaceRecord.FaceGroupIndex;
						}
						if (list2.Contains(num))
						{
							Debug.Print(string.Format("There is port position problem with settlement {0} at position:  {1}", campaignEntityWithName.Name, vec2.AsVec2), 0, Debug.DebugColor.White, 17592186044416UL);
							MBEditor.ZoomToPosition(vec2);
							break;
						}
					}
				}
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001D1A8 File Offset: 0x0001B3A8
		private void InitializeCachedVariables()
		{
			this._mapIsNavalDLC = string.Equals("NavalDLC", this.GetMapModuleId(), StringComparison.CurrentCultureIgnoreCase);
			this._mapIsSandBox = string.Equals("Sandbox", this.GetMapModuleId(), StringComparison.CurrentCultureIgnoreCase);
			this._partyNavigationModel = this.GetPartyNavigationModel();
			this._mapDistanceModel = this.GetMapDistanceModel();
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001D1FB File Offset: 0x0001B3FB
		protected override bool IsOnlyVisual()
		{
			return true;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001D1FE File Offset: 0x0001B3FE
		private bool GetMapIsNavalDLC()
		{
			return this._mapIsNavalDLC;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001D206 File Offset: 0x0001B406
		private bool GetMapIsSandBox()
		{
			return this._mapIsSandBox;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001D20E File Offset: 0x0001B40E
		private string GetMapModuleId()
		{
			return base.Scene.GetModulePath().Trim().TrimEnd(new char[] { '/' })
				.Split(new char[] { '/' })
				.Last<string>();
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0001D248 File Offset: 0x0001B448
		private PartyNavigationModel GetPartyNavigationModel()
		{
			if (Campaign.Current != null)
			{
				return Campaign.Current.Models.PartyNavigationModel;
			}
			if (string.IsNullOrEmpty(this._partyNavigationModelOverriddenClassName))
			{
				if (this.GetMapIsSandBox())
				{
					this._partyNavigationModelOverriddenClassName = "DefaultPartyNavigationModel";
					return SettlementPositionScript.CreateBaseNavigationModel(false);
				}
				if (this.GetMapIsNavalDLC())
				{
					if (!ModuleHelper.IsModuleActive("NavalDLC"))
					{
						throw new ApplicationException("NavalDlc map changes can not be made without NavalDlc module!");
					}
					this._partyNavigationModelOverriddenClassName = "NavalPartyNavigationModel";
					return SettlementPositionScript.CreateBaseNavigationModel(true);
				}
				else
				{
					if (ModuleHelper.IsModuleActive("NavalDLC"))
					{
						this._partyNavigationModelOverriddenClassName = "NavalPartyNavigationModel";
						return SettlementPositionScript.CreateBaseNavigationModel(true);
					}
					this._partyNavigationModelOverriddenClassName = "DefaultPartyNavigationModel";
					return SettlementPositionScript.CreateBaseNavigationModel(false);
				}
			}
			else
			{
				if (SettlementPositionScript.FindClass(this._partyNavigationModelOverriddenClassName) == null)
				{
					Debug.FailedAssert("Cant find custom navigation model", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\SettlementPositionScript.cs", "GetPartyNavigationModel", 826);
					return SettlementPositionScript.CreateBaseNavigationModel(this.GetMapIsNavalDLC());
				}
				return SettlementPositionScript.CreateCustomNavigationModel(this._partyNavigationModelOverriddenClassName, !this.GetMapIsSandBox() && ModuleHelper.IsModuleActive("NavalDLC"));
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001D34C File Offset: 0x0001B54C
		private MapDistanceModel GetMapDistanceModel()
		{
			if (Campaign.Current != null)
			{
				return Campaign.Current.Models.MapDistanceModel;
			}
			if (string.IsNullOrEmpty(this._distanceModelOverridenClassName))
			{
				if (this.GetMapIsSandBox())
				{
					this._distanceModelOverridenClassName = "DefaultMapDistanceModel";
					return SettlementPositionScript.CreateBaseDistanceModel(false);
				}
				if (this.GetMapIsNavalDLC())
				{
					if (!ModuleHelper.IsModuleActive("NavalDLC"))
					{
						throw new ApplicationException("NavalDlc map changes can not be made without NavalDlc module!");
					}
					this._distanceModelOverridenClassName = "NavalDLCMapDistanceModel";
					return SettlementPositionScript.CreateBaseDistanceModel(true);
				}
				else
				{
					if (ModuleHelper.IsModuleActive("NavalDLC"))
					{
						this._distanceModelOverridenClassName = "NavalDLCMapDistanceModel";
						return SettlementPositionScript.CreateBaseDistanceModel(true);
					}
					this._distanceModelOverridenClassName = "DefaultMapDistanceModel";
					return SettlementPositionScript.CreateBaseDistanceModel(false);
				}
			}
			else
			{
				if (SettlementPositionScript.FindClass(this._distanceModelOverridenClassName) == null)
				{
					Debug.FailedAssert("Cant find custom navigation model", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\SettlementPositionScript.cs", "GetMapDistanceModel", 882);
					return SettlementPositionScript.CreateBaseDistanceModel(this.GetMapIsNavalDLC());
				}
				return SettlementPositionScript.CreateCustomMapDistanceModel(this._distanceModelOverridenClassName, !this.GetMapIsSandBox() && ModuleHelper.IsModuleActive("NavalDLC"));
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001D450 File Offset: 0x0001B650
		private static PartyNavigationModel CreateCustomNavigationModel(string name, bool naval)
		{
			if (name == "DefaultPartyNavigationModel")
			{
				return SettlementPositionScript.CreateBaseNavigationModel(false);
			}
			Type type = SettlementPositionScript.FindClass(name);
			if (type == null)
			{
				Debug.FailedAssert("Cant find custom navigation model", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\SettlementPositionScript.cs", "CreateCustomNavigationModel", 903);
				return SettlementPositionScript.CreateBaseNavigationModel(naval);
			}
			if (type.GetConstructor(new Type[] { typeof(PartyNavigationModel) }) != null)
			{
				return (PartyNavigationModel)Activator.CreateInstance(type, new object[] { SettlementPositionScript.CreateBaseNavigationModel(naval) });
			}
			return (PartyNavigationModel)Activator.CreateInstance(type);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0001D4E8 File Offset: 0x0001B6E8
		private static MapDistanceModel CreateCustomMapDistanceModel(string name, bool naval)
		{
			if (name == "DefaultMapDistanceModel")
			{
				return SettlementPositionScript.CreateBaseDistanceModel(false);
			}
			Type type = SettlementPositionScript.FindClass(name);
			if (type == null)
			{
				Debug.FailedAssert("Cant find custom navigation model", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\SettlementPositionScript.cs", "CreateCustomMapDistanceModel", 930);
				return SettlementPositionScript.CreateBaseDistanceModel(naval);
			}
			return (MapDistanceModel)Activator.CreateInstance(type);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001D544 File Offset: 0x0001B744
		private static Type FindClass(string name)
		{
			Type type = null;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				foreach (Type type2 in assemblies[i].GetTypesSafe(null))
				{
					if (type2.Name == name)
					{
						type = type2;
						break;
					}
				}
			}
			return type;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0001D5C4 File Offset: 0x0001B7C4
		private static PartyNavigationModel CreateBaseNavigationModel(bool naval)
		{
			if (!naval)
			{
				return new DefaultPartyNavigationModel();
			}
			Type type = SettlementPositionScript.FindClass("NavalPartyNavigationModel");
			if (type == null)
			{
				throw new ArgumentException("Cant find naval navigation model");
			}
			return (PartyNavigationModel)Activator.CreateInstance(type, new object[] { SettlementPositionScript.CreateBaseNavigationModel(false) });
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0001D614 File Offset: 0x0001B814
		private static MapDistanceModel CreateBaseDistanceModel(bool naval)
		{
			if (!naval)
			{
				return new DefaultMapDistanceModel();
			}
			Type type = SettlementPositionScript.FindClass("NavalDLCMapDistanceModel");
			if (type == null)
			{
				throw new ArgumentException("Cant find naval navigation model");
			}
			return (MapDistanceModel)Activator.CreateInstance(type);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001D654 File Offset: 0x0001B854
		private static MapDistanceModel CreateBaseDistanceModel()
		{
			return new DefaultMapDistanceModel();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0001D65C File Offset: 0x0001B85C
		private bool GetSettlementsDistanceCacheFileForCapability(string moduleId, MobileParty.NavigationType navigationType, out string filePath)
		{
			string text = ModuleHelper.GetModuleFullPath(moduleId) + "ModuleData/DistanceCaches";
			string text2 = navigationType.ToString();
			filePath = text + "/settlements_distance_cache_" + text2 + ".bin";
			bool flag = File.Exists(filePath);
			if (flag)
			{
				Debug.Print(string.Format("Found distance cache at: {0}, {1}, {2}", moduleId, text, navigationType), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			return flag;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0001D6C8 File Offset: 0x0001B8C8
		private List<SettlementPositionScript.SettlementRecord> LoadSettlementData(XmlDocument settlementDocument)
		{
			List<SettlementPositionScript.SettlementRecord> list = new List<SettlementPositionScript.SettlementRecord>();
			base.GameEntity.RemoveAllChildren();
			foreach (object obj in settlementDocument.DocumentElement.SelectNodes("Settlement"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				string value = xmlNode.Attributes["name"].Value;
				string value2 = xmlNode.Attributes["id"].Value;
				GameEntity campaignEntityWithName = base.Scene.GetCampaignEntityWithName(value2);
				if (!(campaignEntityWithName == null))
				{
					Vec2 asVec = campaignEntityWithName.GetGlobalFrame().origin.AsVec2;
					Vec2 vec = default(Vec2);
					List<GameEntity> list2 = new List<GameEntity>();
					campaignEntityWithName.GetChildrenRecursive(ref list2);
					bool flag = false;
					bool flag2 = false;
					Vec2 vec2 = default(Vec2);
					foreach (GameEntity gameEntity in list2)
					{
						if (gameEntity.HasTag("main_map_city_gate"))
						{
							vec = gameEntity.GetGlobalFrame().origin.AsVec2;
							flag = true;
						}
						if (gameEntity.HasTag("main_map_city_port"))
						{
							vec2 = gameEntity.GetGlobalFrame().origin.AsVec2;
							flag2 = true;
						}
						if (gameEntity.HasTag("main_map_village_dropoff"))
						{
							vec2 = gameEntity.GetGlobalFrame().origin.AsVec2;
							flag2 = true;
						}
					}
					bool flag3 = false;
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name.Equals("Components"))
						{
							using (IEnumerator enumerator4 = xmlNode2.ChildNodes.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									object obj3 = enumerator4.Current;
									XmlNode xmlNode3 = (XmlNode)obj3;
									if (xmlNode3.Name.Equals("Town"))
									{
										if (xmlNode3.Attributes["is_castle"] != null)
										{
											bool.Parse(xmlNode3.Attributes["is_castle"].Value);
										}
										flag3 = true;
										break;
									}
								}
								break;
							}
						}
					}
					list.Add(new SettlementPositionScript.SettlementRecord(value2, asVec, flag ? vec : asVec, xmlNode, flag, vec2, flag2, flag3));
				}
			}
			return list;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001D9D0 File Offset: 0x0001BBD0
		private XmlDocument LoadXmlFile(string path)
		{
			Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(path);
			string text = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text);
			streamReader.Close();
			return xmlDocument;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001DA1C File Offset: 0x0001BC1C
		private void SaveSettlementPositions()
		{
			XmlDocument xmlDocument = this.LoadXmlFile(this.SettlementsXmlPath);
			foreach (SettlementPositionScript.SettlementRecord settlementRecord in this.LoadSettlementData(xmlDocument))
			{
				string value = settlementRecord.Node.Attributes["name"].Value;
				if (settlementRecord.Node.Attributes["posX"] == null)
				{
					XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("posX");
					settlementRecord.Node.Attributes.Append(xmlAttribute);
				}
				settlementRecord.Node.Attributes["posX"].Value = settlementRecord.Position.X.ToString();
				if (settlementRecord.Node.Attributes["posY"] == null)
				{
					XmlAttribute xmlAttribute2 = xmlDocument.CreateAttribute("posY");
					settlementRecord.Node.Attributes.Append(xmlAttribute2);
				}
				settlementRecord.Node.Attributes["posY"].Value = settlementRecord.Position.Y.ToString();
				if (settlementRecord.HasGate)
				{
					if (settlementRecord.Node.Attributes["gate_posX"] == null)
					{
						XmlAttribute xmlAttribute3 = xmlDocument.CreateAttribute("gate_posX");
						settlementRecord.Node.Attributes.Append(xmlAttribute3);
					}
					settlementRecord.Node.Attributes["gate_posX"].Value = settlementRecord.GatePosition.X.ToString();
					if (settlementRecord.Node.Attributes["gate_posY"] == null)
					{
						XmlAttribute xmlAttribute4 = xmlDocument.CreateAttribute("gate_posY");
						settlementRecord.Node.Attributes.Append(xmlAttribute4);
					}
					settlementRecord.Node.Attributes["gate_posY"].Value = settlementRecord.GatePosition.Y.ToString();
				}
				if (settlementRecord.HasPort)
				{
					if (settlementRecord.Node.Attributes["port_posX"] == null)
					{
						XmlAttribute xmlAttribute5 = xmlDocument.CreateAttribute("port_posX");
						settlementRecord.Node.Attributes.Append(xmlAttribute5);
					}
					settlementRecord.Node.Attributes["port_posX"].Value = settlementRecord.PortPosition.X.ToString();
					if (settlementRecord.Node.Attributes["port_posY"] == null)
					{
						XmlAttribute xmlAttribute6 = xmlDocument.CreateAttribute("port_posY");
						settlementRecord.Node.Attributes.Append(xmlAttribute6);
					}
					settlementRecord.Node.Attributes["port_posY"].Value = settlementRecord.PortPosition.Y.ToString();
				}
			}
			xmlDocument.Save(this.SettlementsXmlPath);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001DD3C File Offset: 0x0001BF3C
		private void SaveSettlementDistanceCacheEditor()
		{
			bool[] regionMapping = SandBoxHelpers.MapSceneHelper.GetRegionMapping(this._partyNavigationModel);
			base.Scene.SetNavMeshRegionMap(regionMapping);
			List<MobileParty.NavigationType> list = new List<MobileParty.NavigationType> { MobileParty.NavigationType.Default };
			if (this.GetMapIsNavalDLC() || (!this.GetMapIsSandBox() && ModuleHelper.IsModuleActive("NavalDLC")))
			{
				list.Add(MobileParty.NavigationType.Naval);
				list.Add(MobileParty.NavigationType.All);
			}
			foreach (MobileParty.NavigationType navigationType in list)
			{
				int[] invalidTerrainTypesForNavigationType = this._partyNavigationModel.GetInvalidTerrainTypesForNavigationType(navigationType);
				try
				{
					XmlDocument xmlDocument = this.LoadXmlFile(this.SettlementsXmlPath);
					List<SettlementPositionScript.SettlementRecord> list2 = this.LoadSettlementData(xmlDocument);
					foreach (int num in invalidTerrainTypesForNavigationType)
					{
						base.Scene.SetAbilityOfFacesWithId(num, false);
					}
					SettlementPositionScript.SettlementPositionScriptNavigationCache settlementPositionScriptNavigationCache = new SettlementPositionScript.SettlementPositionScriptNavigationCache(list2, base.Scene, this._mapDistanceModel, this._partyNavigationModel, navigationType);
					settlementPositionScriptNavigationCache.GenerateCacheData();
					string text;
					this.GetSettlementsDistanceCacheFileForCapability(this.GetMapModuleId(), navigationType, out text);
					settlementPositionScriptNavigationCache.Serialize(text);
				}
				catch
				{
				}
				finally
				{
					foreach (int num2 in invalidTerrainTypesForNavigationType)
					{
						base.Scene.SetAbilityOfFacesWithId(num2, true);
					}
				}
			}
		}

		// Token: 0x040001DB RID: 475
		private const string SandBoxModuleId = "Sandbox";

		// Token: 0x040001DC RID: 476
		private const string NavalDLCModuleId = "NavalDLC";

		// Token: 0x040001DD RID: 477
		private const string NavalPartyNavigationModelName = "NavalPartyNavigationModel";

		// Token: 0x040001DE RID: 478
		private const string NavalMapDistanceModelName = "NavalDLCMapDistanceModel";

		// Token: 0x040001DF RID: 479
		private bool _mapIsSandBox;

		// Token: 0x040001E0 RID: 480
		private bool _mapIsNavalDLC;

		// Token: 0x040001E1 RID: 481
		[EditableScriptComponentVariable(true, "")]
		private string _partyNavigationModelOverriddenClassName;

		// Token: 0x040001E2 RID: 482
		[EditableScriptComponentVariable(true, "")]
		private string _distanceModelOverridenClassName;

		// Token: 0x040001E3 RID: 483
		private PartyNavigationModel _partyNavigationModel;

		// Token: 0x040001E4 RID: 484
		private MapDistanceModel _mapDistanceModel;

		// Token: 0x040001E5 RID: 485
		public SimpleButton CheckPositions;

		// Token: 0x040001E6 RID: 486
		public SimpleButton SavePositions;

		// Token: 0x040001E7 RID: 487
		public SimpleButton ComputeAndSaveSettlementDistanceCache;

		// Token: 0x020000B0 RID: 176
		private sealed class SettlementRecord : ISettlementDataHolder
		{
			// Token: 0x0600060B RID: 1547 RVA: 0x00029184 File Offset: 0x00027384
			public SettlementRecord(string settlementId, Vec2 position, Vec2 gatePosition, XmlNode node, bool hasGate, Vec2 portPosition, bool hasPort, bool isFortification)
			{
				this.SettlementId = settlementId;
				this.Position = position;
				this.GatePosition = gatePosition;
				this.Node = node;
				this.HasGate = hasGate;
				this.PortPosition = portPosition;
				this.HasPort = hasPort;
				this.IsFortification = isFortification;
			}

			// Token: 0x170000B8 RID: 184
			// (get) Token: 0x0600060C RID: 1548 RVA: 0x000291D4 File Offset: 0x000273D4
			public string StringId
			{
				get
				{
					return this.SettlementId;
				}
			}

			// Token: 0x170000B9 RID: 185
			// (get) Token: 0x0600060D RID: 1549 RVA: 0x000291DC File Offset: 0x000273DC
			CampaignVec2 ISettlementDataHolder.GatePosition
			{
				get
				{
					return new CampaignVec2(this.GatePosition, true);
				}
			}

			// Token: 0x170000BA RID: 186
			// (get) Token: 0x0600060E RID: 1550 RVA: 0x000291EA File Offset: 0x000273EA
			CampaignVec2 ISettlementDataHolder.PortPosition
			{
				get
				{
					return new CampaignVec2(this.PortPosition, false);
				}
			}

			// Token: 0x170000BB RID: 187
			// (get) Token: 0x0600060F RID: 1551 RVA: 0x000291F8 File Offset: 0x000273F8
			bool ISettlementDataHolder.IsFortification
			{
				get
				{
					return this.IsFortification;
				}
			}

			// Token: 0x170000BC RID: 188
			// (get) Token: 0x06000610 RID: 1552 RVA: 0x00029200 File Offset: 0x00027400
			bool ISettlementDataHolder.HasPort
			{
				get
				{
					return this.HasPort;
				}
			}

			// Token: 0x04000374 RID: 884
			public readonly string SettlementId;

			// Token: 0x04000375 RID: 885
			public readonly XmlNode Node;

			// Token: 0x04000376 RID: 886
			public readonly Vec2 Position;

			// Token: 0x04000377 RID: 887
			public readonly Vec2 GatePosition;

			// Token: 0x04000378 RID: 888
			public readonly bool HasGate;

			// Token: 0x04000379 RID: 889
			public readonly Vec2 PortPosition;

			// Token: 0x0400037A RID: 890
			public readonly bool HasPort;

			// Token: 0x0400037B RID: 891
			public readonly bool IsFortification;
		}

		// Token: 0x020000B1 RID: 177
		private sealed class SettlementPositionScriptNavigationCache : NavigationCache<SettlementPositionScript.SettlementRecord>
		{
			// Token: 0x06000611 RID: 1553 RVA: 0x00029208 File Offset: 0x00027408
			public SettlementPositionScriptNavigationCache(List<SettlementPositionScript.SettlementRecord> settlementRecords, Scene scene, MapDistanceModel mapDistanceModel, PartyNavigationModel partyNavigationModel, MobileParty.NavigationType navigationType)
				: base(navigationType)
			{
				this.Scene = scene;
				this._settlementRecords = settlementRecords;
				this._excludedFaceIds = partyNavigationModel.GetInvalidTerrainTypesForNavigationType(base._navigationType);
				this._regionSwitchCostTo0 = mapDistanceModel.RegionSwitchCostFromLandToSea;
				this._regionSwitchCostTo1 = mapDistanceModel.RegionSwitchCostFromSeaToLand;
			}

			// Token: 0x06000612 RID: 1554 RVA: 0x00029256 File Offset: 0x00027456
			protected override NavigationCacheElement<SettlementPositionScript.SettlementRecord> GetCacheElement(SettlementPositionScript.SettlementRecord settlement, bool isPortUsed)
			{
				return new NavigationCacheElement<SettlementPositionScript.SettlementRecord>(settlement, isPortUsed);
			}

			// Token: 0x06000613 RID: 1555 RVA: 0x00029260 File Offset: 0x00027460
			protected override SettlementPositionScript.SettlementRecord GetCacheElement(string settlementId)
			{
				return this._settlementRecords.Single<SettlementPositionScript.SettlementRecord>((SettlementPositionScript.SettlementRecord x) => x.SettlementId == settlementId);
			}

			// Token: 0x06000614 RID: 1556 RVA: 0x00029291 File Offset: 0x00027491
			public override void GetSceneXmlCrcValues(out uint sceneXmlCrc, out uint sceneNavigationMeshCrc)
			{
				sceneXmlCrc = this.Scene.GetSceneXMLCRC();
				sceneNavigationMeshCrc = this.Scene.GetNavigationMeshCRC();
			}

			// Token: 0x06000615 RID: 1557 RVA: 0x000292AD File Offset: 0x000274AD
			protected override int GetNavMeshFaceCount()
			{
				return this.Scene.GetNavMeshFaceCount();
			}

			// Token: 0x06000616 RID: 1558 RVA: 0x000292BC File Offset: 0x000274BC
			protected override Vec2 GetNavMeshFaceCenterPosition(int faceIndex)
			{
				Vec3 zero = Vec3.Zero;
				this.Scene.GetNavMeshCenterPosition(faceIndex, ref zero);
				return zero.AsVec2;
			}

			// Token: 0x06000617 RID: 1559 RVA: 0x000292E4 File Offset: 0x000274E4
			protected override PathFaceRecord GetFaceRecordAtIndex(int faceIndex)
			{
				return this.Scene.GetNavMeshPathFaceRecord(faceIndex);
			}

			// Token: 0x06000618 RID: 1560 RVA: 0x000292F2 File Offset: 0x000274F2
			protected override int[] GetExcludedFaceIds()
			{
				return this._excludedFaceIds;
			}

			// Token: 0x06000619 RID: 1561 RVA: 0x000292FA File Offset: 0x000274FA
			protected override int GetRegionSwitchCostTo0()
			{
				return this._regionSwitchCostTo0;
			}

			// Token: 0x0600061A RID: 1562 RVA: 0x00029302 File Offset: 0x00027502
			protected override int GetRegionSwitchCostTo1()
			{
				return this._regionSwitchCostTo1;
			}

			// Token: 0x0600061B RID: 1563 RVA: 0x0002930C File Offset: 0x0002750C
			protected override IEnumerable<SettlementPositionScript.SettlementRecord> GetClosestSettlementsToPositionInCache(Vec2 checkPosition, List<SettlementPositionScript.SettlementRecord> settlements)
			{
				if (base._navigationType == MobileParty.NavigationType.Naval)
				{
					return from x in settlements
						where x.HasPort
						orderby checkPosition.DistanceSquared(x.PortPosition)
						select x;
				}
				if (base._navigationType == MobileParty.NavigationType.Default)
				{
					return settlements.OrderBy<SettlementPositionScript.SettlementRecord, float>((SettlementPositionScript.SettlementRecord x) => checkPosition.DistanceSquared(x.GatePosition));
				}
				return settlements.OrderBy<SettlementPositionScript.SettlementRecord, float>(delegate(SettlementPositionScript.SettlementRecord x)
				{
					if (!x.HasPort)
					{
						return checkPosition.DistanceSquared(x.GatePosition);
					}
					return MathF.Min(checkPosition.DistanceSquared(x.GatePosition), checkPosition.DistanceSquared(x.PortPosition));
				});
			}

			// Token: 0x0600061C RID: 1564 RVA: 0x00029394 File Offset: 0x00027594
			protected override float GetRealPathDistanceFromPositionToSettlement(Vec2 checkPosition, PathFaceRecord currentFaceRecord, float maxDistanceToLookForPathDetection, SettlementPositionScript.SettlementRecord currentSettlementToLook, out bool isPort)
			{
				float num = float.MaxValue;
				isPort = false;
				PathFaceRecord nullFaceRecord = PathFaceRecord.NullFaceRecord;
				switch (base._navigationType)
				{
				case MobileParty.NavigationType.Default:
				{
					this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, currentSettlementToLook.GatePosition, true, false, true);
					float num2;
					if (this.Scene.GetPathDistanceBetweenAIFaces(currentFaceRecord.FaceIndex, nullFaceRecord.FaceIndex, checkPosition, currentSettlementToLook.GatePosition, 0.3f, maxDistanceToLookForPathDetection, out num2, this._excludedFaceIds, this._regionSwitchCostTo0, this._regionSwitchCostTo1))
					{
						num = num2;
					}
					break;
				}
				case MobileParty.NavigationType.Naval:
				{
					this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, currentSettlementToLook.PortPosition, false, false, true);
					float num3;
					if (this.Scene.GetPathDistanceBetweenAIFaces(currentFaceRecord.FaceIndex, nullFaceRecord.FaceIndex, checkPosition, currentSettlementToLook.PortPosition, 0.3f, maxDistanceToLookForPathDetection, out num3, this._excludedFaceIds, this._regionSwitchCostTo0, this._regionSwitchCostTo1))
					{
						num = num3;
						isPort = true;
					}
					break;
				}
				case MobileParty.NavigationType.All:
				{
					this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, currentSettlementToLook.GatePosition, true, false, true);
					float num4;
					if (this.Scene.GetPathDistanceBetweenAIFaces(currentFaceRecord.FaceIndex, nullFaceRecord.FaceIndex, checkPosition, currentSettlementToLook.GatePosition, 0.3f, maxDistanceToLookForPathDetection, out num4, this._excludedFaceIds, this._regionSwitchCostTo0, this._regionSwitchCostTo1))
					{
						num = num4;
					}
					if (currentSettlementToLook.HasPort)
					{
						this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, currentSettlementToLook.PortPosition, false, false, true);
						float num5;
						if (this.Scene.GetPathDistanceBetweenAIFaces(currentFaceRecord.FaceIndex, nullFaceRecord.FaceIndex, checkPosition, currentSettlementToLook.PortPosition, 0.3f, maxDistanceToLookForPathDetection, out num5, this._excludedFaceIds, this._regionSwitchCostTo0, this._regionSwitchCostTo1) && num5 < num4)
						{
							num = num5;
							isPort = true;
						}
					}
					break;
				}
				}
				return num;
			}

			// Token: 0x0600061D RID: 1565 RVA: 0x0002954C File Offset: 0x0002774C
			protected override float GetRealDistanceAndLandRatioBetweenSettlements(NavigationCacheElement<SettlementPositionScript.SettlementRecord> settlement1, NavigationCacheElement<SettlementPositionScript.SettlementRecord> settlement2, out float landRatio)
			{
				Vec2 vec = (settlement1.IsPortUsed ? settlement1.PortPosition.ToVec2() : settlement1.GatePosition.ToVec2());
				Vec2 vec2 = (settlement2.IsPortUsed ? settlement2.PortPosition.ToVec2() : settlement2.GatePosition.ToVec2());
				PathFaceRecord nullFaceRecord = PathFaceRecord.NullFaceRecord;
				this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, vec, !settlement1.IsPortUsed, false, true);
				PathFaceRecord nullFaceRecord2 = PathFaceRecord.NullFaceRecord;
				this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord2, vec2, !settlement2.IsPortUsed, false, true);
				landRatio = 1f;
				if (base._navigationType == MobileParty.NavigationType.Naval)
				{
					landRatio = 0f;
				}
				else if (base._navigationType == MobileParty.NavigationType.All)
				{
					NavigationPath navigationPath = new NavigationPath();
					this.Scene.GetPathBetweenAIFaces(nullFaceRecord.FaceIndex, nullFaceRecord2.FaceIndex, vec, vec2, 0.3f, navigationPath, this._excludedFaceIds, 1f, this._regionSwitchCostTo0, this._regionSwitchCostTo1);
					landRatio = base.GetLandRatioOfPath(navigationPath, vec);
				}
				float num;
				this.Scene.GetPathDistanceBetweenAIFaces(nullFaceRecord.FaceIndex, nullFaceRecord2.FaceIndex, vec, vec2, 0.3f, float.PositiveInfinity, out num, this._excludedFaceIds, this._regionSwitchCostTo0, this._regionSwitchCostTo1);
				return num;
			}

			// Token: 0x0600061E RID: 1566 RVA: 0x00029694 File Offset: 0x00027894
			protected override void GetFaceRecordForPoint(Vec2 position, out bool isOnRegion1)
			{
				isOnRegion1 = true;
				PathFaceRecord nullFaceRecord = PathFaceRecord.NullFaceRecord;
				this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, position, isOnRegion1, false, true);
				if (!nullFaceRecord.IsValid())
				{
					isOnRegion1 = false;
					this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, position, isOnRegion1, false, true);
				}
				if (!nullFaceRecord.IsValid())
				{
					Debug.Print(string.Format("{0} has no region data.", position), 0, Debug.DebugColor.Red, 17592186044416UL);
				}
			}

			// Token: 0x0600061F RID: 1567 RVA: 0x00029704 File Offset: 0x00027904
			protected override bool CheckBeingNeighbor(List<SettlementPositionScript.SettlementRecord> settlementsToConsider, SettlementPositionScript.SettlementRecord settlement1, SettlementPositionScript.SettlementRecord settlement2, bool useGate1, bool useGate2, out float distance)
			{
				Vec2 vec = (useGate1 ? settlement1.GatePosition : settlement1.PortPosition);
				Vec2 vec2 = (useGate2 ? settlement2.GatePosition : settlement2.PortPosition);
				PathFaceRecord nullFaceRecord = PathFaceRecord.NullFaceRecord;
				this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, vec, useGate1, false, true);
				PathFaceRecord nullFaceRecord2 = PathFaceRecord.NullFaceRecord;
				this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord2, vec2, useGate2, false, true);
				if (!nullFaceRecord.IsValid() || !nullFaceRecord2.IsValid())
				{
					Debug.FailedAssert("Settlement navFace index should not be -1, check here", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.View\\Map\\SettlementPositionScript.cs", "CheckBeingNeighbor", 392);
				}
				NavigationPath navigationPath = new NavigationPath();
				float num = (((float)(this._regionSwitchCostTo0 + this._regionSwitchCostTo1) > 0f) ? 2f : 0f);
				if (num > 0f)
				{
					this.Scene.GetPathBetweenAIFaces(nullFaceRecord.FaceIndex, nullFaceRecord2.FaceIndex, vec, vec2, 0.3f, navigationPath, this._excludedFaceIds, num, this._regionSwitchCostTo0, this._regionSwitchCostTo1);
				}
				else
				{
					this.Scene.GetPathBetweenAIFaces(nullFaceRecord.FaceIndex, nullFaceRecord2.FaceIndex, vec, vec2, 0.3f, navigationPath, this._excludedFaceIds, 0f);
				}
				bool flag = navigationPath.Size > 0 || nullFaceRecord.FaceIndex == nullFaceRecord2.FaceIndex;
				bool flag2 = useGate1;
				if (!this.Scene.GetPathDistanceBetweenAIFaces(nullFaceRecord.FaceIndex, nullFaceRecord2.FaceIndex, vec, vec2, 0.3f, 1784684f, out distance, this.GetExcludedFaceIds(), this._regionSwitchCostTo0, this._regionSwitchCostTo1))
				{
					distance = 1784684f;
				}
				int num2 = 0;
				while (num2 < navigationPath.Size && flag)
				{
					Vec2 vec3 = navigationPath[num2] - ((num2 == 0) ? vec : navigationPath[num2 - 1]);
					float num3 = vec3.Length / 1f;
					vec3.Normalize();
					int num4 = 0;
					while ((float)num4 < num3)
					{
						Vec2 vec4 = ((num2 == 0) ? vec : navigationPath[num2 - 1]) + vec3 * 1f * (float)num4;
						if (vec4 != vec && vec4 != vec2)
						{
							PathFaceRecord nullFaceRecord3 = PathFaceRecord.NullFaceRecord;
							this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord3, vec4, flag2, false, true);
							if (nullFaceRecord3.FaceIndex == -1)
							{
								flag2 = !flag2;
								this.Scene.GetNavMeshFaceIndex(ref nullFaceRecord3, vec4, flag2, false, true);
							}
							bool flag3;
							float realPathDistanceFromPositionToSettlement = this.GetRealPathDistanceFromPositionToSettlement(vec4, nullFaceRecord3, distance, settlement1, out flag3);
							float realPathDistanceFromPositionToSettlement2 = this.GetRealPathDistanceFromPositionToSettlement(vec4, nullFaceRecord3, distance, settlement2, out flag3);
							float num5 = ((realPathDistanceFromPositionToSettlement < realPathDistanceFromPositionToSettlement2) ? realPathDistanceFromPositionToSettlement : realPathDistanceFromPositionToSettlement2);
							if (nullFaceRecord3.FaceIndex != -1)
							{
								SettlementPositionScript.SettlementRecord closestSettlementToPosition = base.GetClosestSettlementToPosition(vec4, nullFaceRecord3, this._excludedFaceIds, settlementsToConsider, this._regionSwitchCostTo0, this._regionSwitchCostTo1, num5 * 0.8f, out flag3);
								if (closestSettlementToPosition != null && closestSettlementToPosition != settlement1 && closestSettlementToPosition != settlement2)
								{
									flag = false;
									break;
								}
							}
						}
						num4++;
					}
					num2++;
				}
				return flag;
			}

			// Token: 0x06000620 RID: 1568 RVA: 0x000299FB File Offset: 0x00027BFB
			protected override List<SettlementPositionScript.SettlementRecord> GetAllRegisteredSettlements()
			{
				return this._settlementRecords;
			}

			// Token: 0x0400037C RID: 892
			private readonly Scene Scene;

			// Token: 0x0400037D RID: 893
			private readonly List<SettlementPositionScript.SettlementRecord> _settlementRecords;

			// Token: 0x0400037E RID: 894
			private readonly int[] _excludedFaceIds;

			// Token: 0x0400037F RID: 895
			private readonly int _regionSwitchCostTo0;

			// Token: 0x04000380 RID: 896
			private readonly int _regionSwitchCostTo1;
		}
	}
}
