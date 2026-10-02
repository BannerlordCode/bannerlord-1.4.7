using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000086 RID: 134
	public struct ExplainedNumber
	{
		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x00052160 File Offset: 0x00050360
		public float ResultNumber
		{
			get
			{
				return MathF.Clamp(this._unclampedResultNumber, this.LimitMinValue, this.LimitMaxValue);
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x00052179 File Offset: 0x00050379
		public int RoundedResultNumber
		{
			get
			{
				return MathF.Round(this.ResultNumber);
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001114 RID: 4372 RVA: 0x00052186 File Offset: 0x00050386
		// (set) Token: 0x06001115 RID: 4373 RVA: 0x0005218E File Offset: 0x0005038E
		public float BaseNumber { get; private set; }

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x00052197 File Offset: 0x00050397
		public bool IncludeDescriptions
		{
			get
			{
				return this._explainer != null;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x000521A2 File Offset: 0x000503A2
		public float LimitMinValue
		{
			get
			{
				if (this._limitMinValue == null)
				{
					return float.MinValue;
				}
				return this._limitMinValue.Value;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x000521C3 File Offset: 0x000503C3
		public float LimitMaxValue
		{
			get
			{
				if (this._limitMaxValue == null)
				{
					return float.MaxValue;
				}
				return this._limitMaxValue.Value;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x000521E4 File Offset: 0x000503E4
		// (set) Token: 0x0600111A RID: 4378 RVA: 0x000521EC File Offset: 0x000503EC
		public float SumOfFactors { get; private set; }

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x000521F5 File Offset: 0x000503F5
		private float _unclampedResultNumber
		{
			get
			{
				return this.BaseNumber + this.BaseNumber * this.SumOfFactors;
			}
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0005220C File Offset: 0x0005040C
		public ExplainedNumber(float baseNumber = 0f, bool includeDescriptions = false, TextObject baseText = null)
		{
			this.BaseNumber = baseNumber;
			this._explainer = (includeDescriptions ? new ExplainedNumber.StatExplainer() : null);
			this.SumOfFactors = 0f;
			this._limitMinValue = new float?(float.MinValue);
			this._limitMaxValue = new float?(float.MaxValue);
			if (this._explainer != null && !this.BaseNumber.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this._explainer.AddLine((baseText ?? ExplainedNumber.BaseText).ToString(), this.BaseNumber, ExplainedNumber.StatExplainer.OperationType.Base);
			}
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0005229C File Offset: 0x0005049C
		public string GetExplanations()
		{
			if (this._explainer == null)
			{
				return "";
			}
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "GetExplanations");
			foreach (ValueTuple<string, float> valueTuple in this._explainer.GetLines(this.BaseNumber, this._unclampedResultNumber, null, null, null))
			{
				string text = string.Format("{0} : {1}{2:0.##}\n", valueTuple.Item1, (valueTuple.Item2 > 0.001f) ? "+" : "", valueTuple.Item2);
				mbstringBuilder.Append<string>(text);
			}
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00052368 File Offset: 0x00050568
		[return: TupleElementNames(new string[] { "name", "number" })]
		public List<ValueTuple<string, float>> GetLines()
		{
			if (this._explainer == null)
			{
				return new List<ValueTuple<string, float>>();
			}
			return this._explainer.GetLines(this.BaseNumber, this._unclampedResultNumber, null, null, null);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00052394 File Offset: 0x00050594
		public void AddFromExplainedNumber(ExplainedNumber explainedNumber, TextObject baseText)
		{
			if (explainedNumber._explainer != null && this._explainer != null)
			{
				TextObject textObject = new TextObject("{=HKoLNyIm}{BASE} Maximum", null);
				TextObject textObject2 = new TextObject("{=0Fliz2vk}{BASE} Minimum", null);
				textObject.SetTextVariable("BASE", baseText);
				textObject2.SetTextVariable("BASE", baseText);
				foreach (ValueTuple<string, float> valueTuple in explainedNumber._explainer.GetLines(explainedNumber.BaseNumber, explainedNumber._unclampedResultNumber, baseText, textObject, textObject2))
				{
					this._explainer.AddLine(valueTuple.Item1, valueTuple.Item2, ExplainedNumber.StatExplainer.OperationType.Add);
				}
			}
			this.BaseNumber += explainedNumber.ResultNumber;
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0005246C File Offset: 0x0005066C
		public void SubtractFromExplainedNumber(ExplainedNumber explainedNumber, TextObject baseText)
		{
			if (explainedNumber._explainer != null && this._explainer != null)
			{
				TextObject textObject = new TextObject("{=HKoLNyIm}{BASE} Maximum", null);
				TextObject textObject2 = new TextObject("{=0Fliz2vk}{BASE} Minimum", null);
				textObject.SetTextVariable("BASE", baseText);
				textObject2.SetTextVariable("BASE", baseText);
				foreach (ValueTuple<string, float> valueTuple in explainedNumber._explainer.GetLines(explainedNumber.BaseNumber, explainedNumber._unclampedResultNumber, baseText, textObject, textObject2))
				{
					this._explainer.AddLine(valueTuple.Item1, -valueTuple.Item2, ExplainedNumber.StatExplainer.OperationType.Add);
				}
			}
			this.BaseNumber -= explainedNumber.ResultNumber;
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00052544 File Offset: 0x00050744
		public void Add(float value, TextObject description = null, TextObject variable = null)
		{
			if (value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return;
			}
			this.BaseNumber += value;
			if (this._explainer != null && description != null && !value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				if (variable != null)
				{
					description.SetTextVariable("A0", variable);
				}
				this._explainer.AddLine(description.ToString(), value, ExplainedNumber.StatExplainer.OperationType.Add);
			}
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x000525C0 File Offset: 0x000507C0
		public void AddFactor(float value, TextObject description = null)
		{
			if (value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return;
			}
			this.SumOfFactors += value;
			if (description != null && this._explainer != null && !value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this._explainer.AddLine(description.ToString(), MathF.Round(value, 3) * 100f, ExplainedNumber.StatExplainer.OperationType.Multiply);
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00052630 File Offset: 0x00050830
		public void LimitMin(float minValue)
		{
			this._limitMinValue = new float?(minValue);
			if (this._explainer != null)
			{
				this._explainer.AddLine(ExplainedNumber.LimitMinText.ToString(), minValue, ExplainedNumber.StatExplainer.OperationType.LimitMin);
			}
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0005265D File Offset: 0x0005085D
		public void LimitMax(float maxValue, TextObject description = null)
		{
			this._limitMaxValue = new float?(maxValue);
			if (this._explainer != null)
			{
				this._explainer.AddLine((description ?? ExplainedNumber.LimitMaxText).ToString(), maxValue, ExplainedNumber.StatExplainer.OperationType.LimitMax);
			}
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0005268F File Offset: 0x0005088F
		public void Clamp(float minValue, float maxValue)
		{
			this.LimitMin(minValue);
			this.LimitMax(maxValue, null);
		}

		// Token: 0x04000553 RID: 1363
		private static readonly TextObject LimitMinText = new TextObject("{=GNalaRaN}Minimum", null);

		// Token: 0x04000554 RID: 1364
		private static readonly TextObject LimitMaxText = new TextObject("{=cfjTtxWv}Maximum", null);

		// Token: 0x04000555 RID: 1365
		private static readonly TextObject BaseText = new TextObject("{=basevalue}Base", null);

		// Token: 0x04000557 RID: 1367
		private float? _limitMinValue;

		// Token: 0x04000558 RID: 1368
		private float? _limitMaxValue;

		// Token: 0x04000559 RID: 1369
		private ExplainedNumber.StatExplainer _explainer;

		// Token: 0x02000541 RID: 1345
		private class StatExplainer
		{
			// Token: 0x17000EF9 RID: 3833
			// (get) Token: 0x06004D01 RID: 19713 RVA: 0x0017F866 File Offset: 0x0017DA66
			// (set) Token: 0x06004D02 RID: 19714 RVA: 0x0017F86E File Offset: 0x0017DA6E
			public List<ExplainedNumber.StatExplainer.ExplanationLine> Lines { get; private set; } = new List<ExplainedNumber.StatExplainer.ExplanationLine>();

			// Token: 0x17000EFA RID: 3834
			// (get) Token: 0x06004D03 RID: 19715 RVA: 0x0017F877 File Offset: 0x0017DA77
			// (set) Token: 0x06004D04 RID: 19716 RVA: 0x0017F87F File Offset: 0x0017DA7F
			public ExplainedNumber.StatExplainer.ExplanationLine? BaseLine { get; private set; }

			// Token: 0x17000EFB RID: 3835
			// (get) Token: 0x06004D05 RID: 19717 RVA: 0x0017F888 File Offset: 0x0017DA88
			// (set) Token: 0x06004D06 RID: 19718 RVA: 0x0017F890 File Offset: 0x0017DA90
			public ExplainedNumber.StatExplainer.ExplanationLine? LimitMinLine { get; private set; }

			// Token: 0x17000EFC RID: 3836
			// (get) Token: 0x06004D07 RID: 19719 RVA: 0x0017F899 File Offset: 0x0017DA99
			// (set) Token: 0x06004D08 RID: 19720 RVA: 0x0017F8A1 File Offset: 0x0017DAA1
			public ExplainedNumber.StatExplainer.ExplanationLine? LimitMaxLine { get; private set; }

			// Token: 0x06004D09 RID: 19721 RVA: 0x0017F8AC File Offset: 0x0017DAAC
			[return: TupleElementNames(new string[] { "name", "number" })]
			public List<ValueTuple<string, float>> GetLines(float baseNumber, float unclampedResultNumber, TextObject overrideBaseLineText = null, TextObject overrideMaximumLineText = null, TextObject overrideMinimumLineText = null)
			{
				List<ValueTuple<string, float>> list = new List<ValueTuple<string, float>>();
				if (this.BaseLine != null)
				{
					list.Add(new ValueTuple<string, float>((overrideBaseLineText != null) ? overrideBaseLineText.ToString() : this.BaseLine.Value.Name, this.BaseLine.Value.Number));
				}
				foreach (ExplainedNumber.StatExplainer.ExplanationLine explanationLine in this.Lines)
				{
					float num = explanationLine.Number;
					if (explanationLine.OperationType == ExplainedNumber.StatExplainer.OperationType.Multiply)
					{
						num = baseNumber * num * 0.01f;
					}
					list.Add(new ValueTuple<string, float>(explanationLine.Name, num));
				}
				if (this.LimitMinLine != null && this.LimitMinLine.Value.Number > unclampedResultNumber)
				{
					list.Add(new ValueTuple<string, float>((overrideMinimumLineText != null) ? overrideMinimumLineText.ToString() : this.LimitMinLine.Value.Name, this.LimitMinLine.Value.Number - unclampedResultNumber));
				}
				if (this.LimitMaxLine != null && this.LimitMaxLine.Value.Number < unclampedResultNumber)
				{
					list.Add(new ValueTuple<string, float>((overrideMaximumLineText != null) ? overrideMaximumLineText.ToString() : this.LimitMaxLine.Value.Name, this.LimitMaxLine.Value.Number - unclampedResultNumber));
				}
				return list;
			}

			// Token: 0x06004D0A RID: 19722 RVA: 0x0017FA5C File Offset: 0x0017DC5C
			public void AddLine(string name, float number, ExplainedNumber.StatExplainer.OperationType opType)
			{
				ExplainedNumber.StatExplainer.ExplanationLine explanationLine = new ExplainedNumber.StatExplainer.ExplanationLine(name, number, opType);
				if (opType == ExplainedNumber.StatExplainer.OperationType.Add || opType == ExplainedNumber.StatExplainer.OperationType.Multiply)
				{
					int num = -1;
					for (int i = 0; i < this.Lines.Count; i++)
					{
						if (this.Lines[i].Name.Equals(name) && this.Lines[i].OperationType == opType)
						{
							num = i;
							break;
						}
					}
					if (num < 0)
					{
						this.Lines.Add(explanationLine);
						return;
					}
					explanationLine = new ExplainedNumber.StatExplainer.ExplanationLine(name, number + this.Lines[num].Number, opType);
					this.Lines[num] = explanationLine;
					return;
				}
				else
				{
					if (opType == ExplainedNumber.StatExplainer.OperationType.Base)
					{
						this.BaseLine = new ExplainedNumber.StatExplainer.ExplanationLine?(explanationLine);
						return;
					}
					if (opType == ExplainedNumber.StatExplainer.OperationType.LimitMin)
					{
						this.LimitMinLine = new ExplainedNumber.StatExplainer.ExplanationLine?(explanationLine);
						return;
					}
					if (opType == ExplainedNumber.StatExplainer.OperationType.LimitMax)
					{
						this.LimitMaxLine = new ExplainedNumber.StatExplainer.ExplanationLine?(explanationLine);
					}
					return;
				}
			}

			// Token: 0x020008B0 RID: 2224
			public enum OperationType
			{
				// Token: 0x04002503 RID: 9475
				Base,
				// Token: 0x04002504 RID: 9476
				Add,
				// Token: 0x04002505 RID: 9477
				Multiply,
				// Token: 0x04002506 RID: 9478
				LimitMin,
				// Token: 0x04002507 RID: 9479
				LimitMax
			}

			// Token: 0x020008B1 RID: 2225
			public readonly struct ExplanationLine
			{
				// Token: 0x060068EF RID: 26863 RVA: 0x001CAEFA File Offset: 0x001C90FA
				public ExplanationLine(string name, float number, ExplainedNumber.StatExplainer.OperationType operationType)
				{
					this.Name = name;
					this.Number = number;
					this.OperationType = operationType;
				}

				// Token: 0x04002508 RID: 9480
				public readonly float Number;

				// Token: 0x04002509 RID: 9481
				public readonly string Name;

				// Token: 0x0400250A RID: 9482
				public readonly ExplainedNumber.StatExplainer.OperationType OperationType;
			}
		}
	}
}
