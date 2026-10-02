using System;
using System.Reflection;
using System.Xml;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000EB RID: 235
	public class WaitMenuOption
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x000629C1 File Offset: 0x00060BC1
		// (set) Token: 0x060015D0 RID: 5584 RVA: 0x000629C9 File Offset: 0x00060BC9
		public int Priority { get; private set; }

		// Token: 0x060015D1 RID: 5585 RVA: 0x000629D2 File Offset: 0x00060BD2
		internal WaitMenuOption()
		{
			this.Priority = 100;
			this._text = null;
			this._tooltip = "";
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x000629F4 File Offset: 0x00060BF4
		internal WaitMenuOption(string idString, TextObject text, WaitMenuOption.OnConditionDelegate condition, WaitMenuOption.OnConsequenceDelegate consequence, int priority = 100, string tooltip = "")
		{
			this._idString = idString;
			this._text = text;
			this.OnCondition = condition;
			this.OnConsequence = consequence;
			this.Priority = priority;
			this._tooltip = tooltip;
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x00062A2C File Offset: 0x00060C2C
		public bool GetConditionsHold(Game game, MapState mapState)
		{
			if (this.OnCondition != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(mapState, this.Text);
				return this.OnCondition(menuCallbackArgs);
			}
			return true;
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x00062A5C File Offset: 0x00060C5C
		public TextObject Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060015D5 RID: 5589 RVA: 0x00062A64 File Offset: 0x00060C64
		public string IdString
		{
			get
			{
				return this._idString;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x00062A6C File Offset: 0x00060C6C
		public string Tooltip
		{
			get
			{
				return this._tooltip;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060015D7 RID: 5591 RVA: 0x00062A74 File Offset: 0x00060C74
		public bool IsLeave
		{
			get
			{
				return this._isLeave;
			}
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00062A7C File Offset: 0x00060C7C
		public void RunConsequence(Game game, MapState mapState)
		{
			if (this.OnConsequence != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(mapState, this.Text);
				this.OnConsequence(menuCallbackArgs);
			}
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00062AAC File Offset: 0x00060CAC
		public void Deserialize(XmlNode node, Type typeOfWaitMenusCallbacks)
		{
			if (node.Attributes == null)
			{
				throw new TWXmlLoadException("node.Attributes != null");
			}
			this._idString = node.Attributes["id"].Value;
			XmlNode xmlNode = node.Attributes["text"];
			if (xmlNode != null)
			{
				this._text = new TextObject(xmlNode.InnerText, null);
			}
			if (node.Attributes["is_leave"] != null)
			{
				this._isLeave = true;
			}
			XmlNode xmlNode2 = node.Attributes["on_condition"];
			if (xmlNode2 != null)
			{
				string innerText = xmlNode2.InnerText;
				this._methodOnCondition = typeOfWaitMenusCallbacks.GetMethod(innerText);
				if (this._methodOnCondition == null)
				{
					throw new MBNotFoundException("Can not find WaitMenuOption condition:" + innerText);
				}
				this.OnCondition = (WaitMenuOption.OnConditionDelegate)Delegate.CreateDelegate(typeof(WaitMenuOption.OnConditionDelegate), null, this._methodOnCondition);
			}
			XmlNode xmlNode3 = node.Attributes["on_consequence"];
			if (xmlNode3 != null)
			{
				string innerText2 = xmlNode3.InnerText;
				this._methodOnConsequence = typeOfWaitMenusCallbacks.GetMethod(innerText2);
				if (this._methodOnConsequence == null)
				{
					throw new MBNotFoundException("Can not find WaitMenuOption consequence:" + innerText2);
				}
				this.OnConsequence = (WaitMenuOption.OnConsequenceDelegate)Delegate.CreateDelegate(typeof(WaitMenuOption.OnConsequenceDelegate), null, this._methodOnConsequence);
			}
		}

		// Token: 0x04000737 RID: 1847
		private string _idString;

		// Token: 0x04000738 RID: 1848
		private TextObject _text;

		// Token: 0x04000739 RID: 1849
		private string _tooltip;

		// Token: 0x0400073A RID: 1850
		private MethodInfo _methodOnCondition;

		// Token: 0x0400073B RID: 1851
		public WaitMenuOption.OnConditionDelegate OnCondition;

		// Token: 0x0400073C RID: 1852
		private MethodInfo _methodOnConsequence;

		// Token: 0x0400073D RID: 1853
		public WaitMenuOption.OnConsequenceDelegate OnConsequence;

		// Token: 0x0400073E RID: 1854
		private bool _isLeave;

		// Token: 0x02000569 RID: 1385
		// (Invoke) Token: 0x06004DC3 RID: 19907
		public delegate bool OnConditionDelegate(MenuCallbackArgs args);

		// Token: 0x0200056A RID: 1386
		// (Invoke) Token: 0x06004DC7 RID: 19911
		public delegate void OnConsequenceDelegate(MenuCallbackArgs args);
	}
}
