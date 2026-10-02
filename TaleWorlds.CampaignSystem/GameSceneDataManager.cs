using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200008D RID: 141
	public class GameSceneDataManager
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x00053661 File Offset: 0x00051861
		// (set) Token: 0x0600123E RID: 4670 RVA: 0x00053668 File Offset: 0x00051868
		public static GameSceneDataManager Instance { get; private set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00053670 File Offset: 0x00051870
		public MBReadOnlyList<SingleplayerBattleSceneData> SingleplayerBattleScenes
		{
			get
			{
				return this._singleplayerBattleScenes;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x00053678 File Offset: 0x00051878
		public MBReadOnlyList<ConversationSceneData> ConversationScenes
		{
			get
			{
				return this._conversationScenes;
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x00053680 File Offset: 0x00051880
		public MBReadOnlyList<MeetingSceneData> MeetingScenes
		{
			get
			{
				return this._meetingScenes;
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00053688 File Offset: 0x00051888
		public GameSceneDataManager()
		{
			this._singleplayerBattleScenes = new MBList<SingleplayerBattleSceneData>();
			this._conversationScenes = new MBList<ConversationSceneData>();
			this._meetingScenes = new MBList<MeetingSceneData>();
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x000536B1 File Offset: 0x000518B1
		internal static void Initialize()
		{
			GameSceneDataManager.Instance = new GameSceneDataManager();
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x000536BD File Offset: 0x000518BD
		internal static void Destroy()
		{
			GameSceneDataManager.Instance = null;
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000536C8 File Offset: 0x000518C8
		public void LoadSPBattleScenes(string path)
		{
			XmlDocument xmlDocument = this.LoadXmlFile(path);
			this.LoadSPBattleScenes(xmlDocument);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x000536E4 File Offset: 0x000518E4
		public void LoadConversationScenes(string path)
		{
			XmlDocument xmlDocument = this.LoadXmlFile(path);
			this.LoadConversationScenes(xmlDocument);
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x00053700 File Offset: 0x00051900
		public void LoadMeetingScenes(string path)
		{
			XmlDocument xmlDocument = this.LoadXmlFile(path);
			this.LoadMeetingScenes(xmlDocument);
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0005371C File Offset: 0x0005191C
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

		// Token: 0x06001249 RID: 4681 RVA: 0x00053768 File Offset: 0x00051968
		private void LoadSPBattleScenes(XmlDocument doc)
		{
			Debug.Print("loading sp_battles.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least 2 child nodes.");
			}
			XmlNode xmlNode = doc.ChildNodes[1];
			if (xmlNode.Name != "SPBattleScenes")
			{
				throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be SPBattleScenes.");
			}
			if (xmlNode.Name == "SPBattleScenes")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != XmlNodeType.Comment)
					{
						string text = null;
						List<int> list = new List<int>();
						TerrainType terrainType = TerrainType.Plain;
						ForestDensity forestDensity = ForestDensity.None;
						bool flag = false;
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes[i].Name == "id")
							{
								text = xmlNode2.Attributes[i].InnerText;
							}
							else if (xmlNode2.Attributes[i].Name == "map_indices")
							{
								foreach (string text2 in xmlNode2.Attributes[i].InnerText.Replace(" ", "").Split(new char[] { ',' }))
								{
									list.Add(int.Parse(text2));
								}
							}
							else if (xmlNode2.Attributes[i].Name == "terrain")
							{
								if (!Enum.TryParse<TerrainType>(xmlNode2.Attributes[i].InnerText, out terrainType))
								{
									terrainType = TerrainType.Plain;
								}
							}
							else if (xmlNode2.Attributes[i].Name == "forest_density")
							{
								char[] array2 = xmlNode2.Attributes[i].InnerText.ToLower().ToCharArray();
								array2[0] = char.ToUpper(array2[0]);
								if (!Enum.TryParse<ForestDensity>(new string(array2), out forestDensity))
								{
									forestDensity = ForestDensity.None;
								}
							}
							else if (xmlNode2.Attributes[i].Name == "is_naval")
							{
								bool.TryParse(xmlNode2.Attributes[i].Value, out flag);
							}
						}
						XmlNodeList childNodes = xmlNode2.ChildNodes;
						List<TerrainType> list2 = new List<TerrainType>();
						foreach (object obj2 in childNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.NodeType != XmlNodeType.Comment && xmlNode3.Name == "TerrainTypes")
							{
								foreach (object obj3 in xmlNode3.ChildNodes)
								{
									XmlNode xmlNode4 = (XmlNode)obj3;
									TerrainType terrainType2;
									if (xmlNode4.Name == "TerrainType" && Enum.TryParse<TerrainType>(xmlNode4.Attributes["name"].InnerText, out terrainType2) && !list2.Contains(terrainType2))
									{
										list2.Add(terrainType2);
									}
								}
							}
						}
						this._singleplayerBattleScenes.Add(new SingleplayerBattleSceneData(text, terrainType, list2, forestDensity, list, flag));
					}
				}
			}
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00053B48 File Offset: 0x00051D48
		private void LoadConversationScenes(XmlDocument doc)
		{
			Debug.Print("loading conversation_scenes.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least 2 child nodes.");
			}
			XmlNode xmlNode = doc.ChildNodes[1];
			if (xmlNode.Name != "ConversationScenes")
			{
				throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be ConversationScenes.");
			}
			if (xmlNode.Name == "ConversationScenes")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != XmlNodeType.Comment)
					{
						string text = null;
						TerrainType terrainType = TerrainType.Plain;
						ForestDensity forestDensity = ForestDensity.None;
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes[i].Name == "id")
							{
								text = xmlNode2.Attributes[i].InnerText;
							}
							else if (xmlNode2.Attributes[i].Name == "terrain")
							{
								if (!Enum.TryParse<TerrainType>(xmlNode2.Attributes[i].InnerText, out terrainType))
								{
									terrainType = TerrainType.Plain;
								}
							}
							else if (xmlNode2.Attributes[i].Name == "forest_density")
							{
								char[] array = xmlNode2.Attributes[i].InnerText.ToLower().ToCharArray();
								array[0] = char.ToUpper(array[0]);
								if (!Enum.TryParse<ForestDensity>(new string(array), out forestDensity))
								{
									forestDensity = ForestDensity.None;
								}
							}
						}
						XmlNodeList childNodes = xmlNode2.ChildNodes;
						List<TerrainType> list = new List<TerrainType>();
						foreach (object obj2 in childNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.NodeType != XmlNodeType.Comment && xmlNode3.Name == "flags")
							{
								foreach (object obj3 in xmlNode3.ChildNodes)
								{
									XmlNode xmlNode4 = (XmlNode)obj3;
									TerrainType terrainType2;
									if (xmlNode4.NodeType != XmlNodeType.Comment && xmlNode4.Attributes["name"].InnerText == "TerrainType" && Enum.TryParse<TerrainType>(xmlNode4.Attributes["value"].InnerText, out terrainType2) && !list.Contains(terrainType2))
									{
										list.Add(terrainType2);
									}
								}
							}
						}
						this._conversationScenes.Add(new ConversationSceneData(text, terrainType, list, forestDensity));
					}
				}
			}
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00053E74 File Offset: 0x00052074
		private void LoadMeetingScenes(XmlDocument doc)
		{
			Debug.Print("loading meeting_scenes.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least 2 child nodes.");
			}
			XmlNode xmlNode = doc.ChildNodes[1];
			if (xmlNode.Name != "MeetingScenes")
			{
				throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be MeetingScenes.");
			}
			if (xmlNode.Name == "MeetingScenes")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != XmlNodeType.Comment)
					{
						string text = null;
						string text2 = null;
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes[i].Name == "id")
							{
								text = xmlNode2.Attributes[i].InnerText;
							}
							if (xmlNode2.Attributes[i].Name == "culture")
							{
								text2 = xmlNode2.Attributes[i].InnerText.Split(new char[] { '.' })[1];
							}
						}
						this._meetingScenes.Add(new MeetingSceneData(text, text2));
					}
				}
			}
		}

		// Token: 0x04000605 RID: 1541
		private MBList<SingleplayerBattleSceneData> _singleplayerBattleScenes;

		// Token: 0x04000606 RID: 1542
		private MBList<ConversationSceneData> _conversationScenes;

		// Token: 0x04000607 RID: 1543
		private MBList<MeetingSceneData> _meetingScenes;

		// Token: 0x04000608 RID: 1544
		private const TerrainType DefaultTerrain = TerrainType.Plain;

		// Token: 0x04000609 RID: 1545
		private const ForestDensity DefaultForestDensity = ForestDensity.None;
	}
}
