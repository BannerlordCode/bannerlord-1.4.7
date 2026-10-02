using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000177 RID: 375
	public abstract class EncyclopediaPage
	{
		// Token: 0x06001B63 RID: 7011
		protected abstract IEnumerable<EncyclopediaListItem> InitializeListItems();

		// Token: 0x06001B64 RID: 7012
		protected abstract IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems();

		// Token: 0x06001B65 RID: 7013
		protected abstract IEnumerable<EncyclopediaSortController> InitializeSortControllers();

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001B66 RID: 7014 RVA: 0x0008DE5A File Offset: 0x0008C05A
		// (set) Token: 0x06001B67 RID: 7015 RVA: 0x0008DE62 File Offset: 0x0008C062
		public int HomePageOrderIndex { get; protected set; }

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001B68 RID: 7016 RVA: 0x0008DE6B File Offset: 0x0008C06B
		public EncyclopediaPage Parent { get; }

		// Token: 0x06001B69 RID: 7017 RVA: 0x0008DE74 File Offset: 0x0008C074
		public EncyclopediaPage()
		{
			this._filters = this.InitializeFilterItems();
			this._items = this.InitializeListItems();
			this._sortControllers = new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(new TextObject("{=koX9okuG}None", null), new EncyclopediaListItemNameComparer())
			};
			((List<EncyclopediaSortController>)this._sortControllers).AddRange(this.InitializeSortControllers());
			foreach (object obj in base.GetType().GetCustomAttributesSafe(typeof(EncyclopediaModel), true))
			{
				if (obj is EncyclopediaModel)
				{
					this._identifierTypes = (obj as EncyclopediaModel).PageTargetTypes;
					break;
				}
			}
			this._identifiers = new Dictionary<Type, string>();
			foreach (Type type in this._identifierTypes)
			{
				if (Game.Current.ObjectManager.HasType(type))
				{
					this._identifiers.Add(type, Game.Current.ObjectManager.FindRegisteredClassPrefix(type));
				}
				else
				{
					string text = type.Name.ToString();
					if (text == "Clan")
					{
						text = "Faction";
					}
					this._identifiers.Add(type, text);
				}
			}
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0008DFA9 File Offset: 0x0008C1A9
		public virtual bool IsRelevant()
		{
			return true;
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x0008DFAC File Offset: 0x0008C1AC
		public bool HasIdentifierType(Type identifierType)
		{
			return this._identifierTypes.Contains(identifierType);
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x0008DFBA File Offset: 0x0008C1BA
		internal bool HasIdentifier(string identifier)
		{
			return this._identifiers.ContainsValue(identifier);
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x0008DFC8 File Offset: 0x0008C1C8
		public string GetIdentifier(Type identifierType)
		{
			if (this._identifiers.ContainsKey(identifierType))
			{
				return this._identifiers[identifierType];
			}
			return "";
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x0008DFEA File Offset: 0x0008C1EA
		public string[] GetIdentifierNames()
		{
			return this._identifiers.Values.ToArray<string>();
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x0008DFFC File Offset: 0x0008C1FC
		public bool IsFiltered(object o)
		{
			using (IEnumerator<EncyclopediaFilterGroup> enumerator = this.GetFilterItems().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Predicate(o))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0008E058 File Offset: 0x0008C258
		public virtual string GetViewFullyQualifiedName()
		{
			return "";
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x0008E05F File Offset: 0x0008C25F
		public virtual string GetStringID()
		{
			return "";
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x0008E066 File Offset: 0x0008C266
		public virtual TextObject GetName()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0008E06D File Offset: 0x0008C26D
		public virtual MBObjectBase GetObject(string typeName, string stringID)
		{
			return MBObjectManager.Instance.GetObject(typeName, stringID);
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x0008E07B File Offset: 0x0008C27B
		public virtual bool IsValidEncyclopediaItem(object o)
		{
			return false;
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0008E07E File Offset: 0x0008C27E
		public virtual TextObject GetDescriptionText()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0008E085 File Offset: 0x0008C285
		public IEnumerable<EncyclopediaListItem> GetListItems()
		{
			return this._items;
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x0008E08D File Offset: 0x0008C28D
		public IEnumerable<EncyclopediaFilterGroup> GetFilterItems()
		{
			return this._filters;
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0008E095 File Offset: 0x0008C295
		public IEnumerable<EncyclopediaSortController> GetSortControllers()
		{
			return this._sortControllers;
		}

		// Token: 0x04000943 RID: 2371
		private readonly Type[] _identifierTypes;

		// Token: 0x04000944 RID: 2372
		private readonly Dictionary<Type, string> _identifiers;

		// Token: 0x04000945 RID: 2373
		private IEnumerable<EncyclopediaFilterGroup> _filters;

		// Token: 0x04000946 RID: 2374
		private IEnumerable<EncyclopediaListItem> _items;

		// Token: 0x04000947 RID: 2375
		private IEnumerable<EncyclopediaSortController> _sortControllers;
	}
}
