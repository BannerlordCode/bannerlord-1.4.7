using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000077 RID: 119
	public class GameTextManager
	{
		// Token: 0x06000835 RID: 2101 RVA: 0x0001B265 File Offset: 0x00019465
		public GameTextManager()
		{
			this._gameTexts = new Dictionary<string, GameText>();
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001B278 File Offset: 0x00019478
		public GameText GetGameText(string id)
		{
			GameText gameText;
			if (this._gameTexts.TryGetValue(id, out gameText))
			{
				return gameText;
			}
			return null;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001B298 File Offset: 0x00019498
		public GameText AddGameText(string id)
		{
			GameText gameText;
			if (!this._gameTexts.TryGetValue(id, out gameText))
			{
				gameText = new GameText(id);
				this._gameTexts.Add(gameText.Id, gameText);
			}
			return gameText;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001B2D0 File Offset: 0x000194D0
		public bool TryGetText(string id, string variation, out TextObject text)
		{
			text = null;
			GameText gameText;
			this._gameTexts.TryGetValue(id, out gameText);
			if (gameText != null)
			{
				if (variation == null)
				{
					text = gameText.DefaultText;
				}
				else
				{
					text = gameText.GetVariation(variation);
				}
				if (text != null)
				{
					text = text.CopyTextObject();
					text.AddIDToValue(id);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001B328 File Offset: 0x00019528
		public TextObject FindText(string id, string variation = null)
		{
			TextObject textObject;
			if (this.TryGetText(id, variation, out textObject))
			{
				return textObject;
			}
			TextObject textObject2;
			if (variation == null)
			{
				textObject2 = new TextObject("{=!}ERROR: Text with id " + id + " doesn't exist!", null);
			}
			else
			{
				textObject2 = new TextObject("{=!}ERROR: Text with id " + id + " doesn't exist! Variation: " + variation, null);
			}
			return textObject2;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001B378 File Offset: 0x00019578
		public IEnumerable<TextObject> FindAllTextVariations(string id)
		{
			GameText gameText;
			this._gameTexts.TryGetValue(id, out gameText);
			if (gameText != null)
			{
				foreach (GameText.GameTextVariation gameTextVariation in gameText.Variations)
				{
					yield return gameTextVariation.Text;
				}
				IEnumerator<GameText.GameTextVariation> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001B390 File Offset: 0x00019590
		public void LoadGameTexts()
		{
			Game game = Game.Current;
			bool flag = false;
			string text = "";
			if (game != null)
			{
				flag = game.GameType.IsDevelopment;
				text = game.GameType.GetType().Name;
			}
			XmlDocument mergedXmlForManaged = MBObjectManager.GetMergedXmlForManaged("GameText", false, flag, text);
			try
			{
				this.LoadFromXML(mergedXmlForManaged);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001B3F8 File Offset: 0x000195F8
		public void LoadDefaultTexts()
		{
			try
			{
				List<string> list = new List<string>();
				foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules(null))
				{
					string text = moduleInfo.FolderPath + "/ModuleData/global_strings.xml";
					if (File.Exists(text))
					{
						list.Add(text);
					}
				}
				string text2 = ModuleHelper.GetModuleFullPath("Native") + "ModuleData/consoles.xml";
				if (File.Exists(text2))
				{
					list.Add(text2);
				}
				else
				{
					Debug.FailedAssert("Cant find Native/consoles.xml", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\GameTextManager.cs", "LoadDefaultTexts", 177);
				}
				foreach (string text3 in list)
				{
					Debug.Print("opening " + text3, 0, Debug.DebugColor.White, 17592186044416UL);
					XmlDocument xmlDocument = new XmlDocument();
					StreamReader streamReader = new StreamReader(text3);
					string text4 = streamReader.ReadToEnd();
					xmlDocument.LoadXml(text4);
					streamReader.Close();
					this.LoadFromXML(xmlDocument);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001B55C File Offset: 0x0001975C
		private void LoadFromXML(XmlDocument doc)
		{
			XmlNode xmlNode = null;
			for (int i = 0; i < doc.ChildNodes.Count; i++)
			{
				XmlNode xmlNode2 = doc.ChildNodes[i];
				if (xmlNode2.NodeType != XmlNodeType.Comment && xmlNode2.Name == "strings" && xmlNode2.ChildNodes.Count > 0)
				{
					xmlNode = xmlNode2.ChildNodes[0];
					IL_01FF:
					while (xmlNode != null)
					{
						try
						{
							if (xmlNode.Name == "string" && xmlNode.NodeType != XmlNodeType.Comment)
							{
								if (xmlNode.Attributes == null)
								{
									throw new TWXmlLoadException("Node attributes are null.");
								}
								string[] array = xmlNode.Attributes["id"].Value.Split(new char[] { '.' });
								string text = array[0];
								GameText gameText = this.AddGameText(text);
								string text2 = "";
								if (array.Length > 1)
								{
									text2 = array[1];
								}
								TextObject textObject = new TextObject(xmlNode.Attributes["text"].Value, null);
								List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
								foreach (object obj in xmlNode.ChildNodes)
								{
									XmlNode xmlNode3 = (XmlNode)obj;
									if (xmlNode3.Name == "tags")
									{
										XmlNodeList childNodes = xmlNode3.ChildNodes;
										for (int j = 0; j < childNodes.Count; j++)
										{
											XmlAttributeCollection attributes = childNodes[j].Attributes;
											if (attributes != null)
											{
												int num = 1;
												if (attributes["weight"] != null)
												{
													int.TryParse(attributes["weight"].Value, out num);
												}
												GameTextManager.ChoiceTag choiceTag = new GameTextManager.ChoiceTag(attributes["tag_name"].Value, num);
												list.Add(choiceTag);
											}
										}
									}
								}
								textObject.CacheTokens();
								gameText.AddVariationWithId(text2, textObject, list);
							}
						}
						catch (Exception)
						{
						}
						finally
						{
							xmlNode = xmlNode.NextSibling;
						}
					}
					return;
				}
			}
			goto IL_01FF;
		}

		// Token: 0x0400041F RID: 1055
		private readonly Dictionary<string, GameText> _gameTexts;

		// Token: 0x0200011A RID: 282
		public struct ChoiceTag
		{
			// Token: 0x170003FC RID: 1020
			// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00026607 File Offset: 0x00024807
			// (set) Token: 0x06000BFB RID: 3067 RVA: 0x0002660F File Offset: 0x0002480F
			public string TagName { get; private set; }

			// Token: 0x170003FD RID: 1021
			// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00026618 File Offset: 0x00024818
			// (set) Token: 0x06000BFD RID: 3069 RVA: 0x00026620 File Offset: 0x00024820
			public uint Weight { get; private set; }

			// Token: 0x170003FE RID: 1022
			// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00026629 File Offset: 0x00024829
			// (set) Token: 0x06000BFF RID: 3071 RVA: 0x00026631 File Offset: 0x00024831
			public bool IsTagReversed { get; private set; }

			// Token: 0x06000C00 RID: 3072 RVA: 0x0002663A File Offset: 0x0002483A
			public ChoiceTag(string tagName, int weight)
			{
				this = default(GameTextManager.ChoiceTag);
				this.TagName = tagName;
				this.Weight = (uint)MathF.Abs(weight);
				this.IsTagReversed = weight < 0;
			}
		}
	}
}
