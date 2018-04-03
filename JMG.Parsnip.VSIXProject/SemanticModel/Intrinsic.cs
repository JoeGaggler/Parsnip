using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.SemanticModel
{
	internal enum IntrinsicType
	{
		EndOfStream,
		EndOfLine,
		EndOfLineOrStream,
		AnyCharacter,
		AnyLetter,
		CString,
		OptionalHorizontalWhitespace,
		AnyDigit
	}

	internal class Intrinsic : IParseFunction
	{
		public Intrinsic(IntrinsicType type, InterfaceMethod interfaceMethod)
		{
			this.Type = type;
			this.InterfaceMethod = interfaceMethod;
		}

		public InterfaceMethod InterfaceMethod { get; }

		public IntrinsicType Type { get; }

		public INodeType ReturnType
		{
			get
			{
				switch (Type)
				{
					case IntrinsicType.AnyDigit:
					case IntrinsicType.AnyLetter:
					case IntrinsicType.AnyCharacter:
					case IntrinsicType.CString:
					case IntrinsicType.EndOfLine:
					case IntrinsicType.OptionalHorizontalWhitespace:
					return new SingleNodeType("String");

					case IntrinsicType.EndOfStream:
					case IntrinsicType.EndOfLineOrStream:
					return EmptyNodeType.Instance;

					default: throw new NotImplementedException();
				}

			}
		}

		public void ApplyVisitor(IParseFunctionActionVisitor visitor) => visitor.Visit(this);

		public void ApplyVisitor<TInput>(IParseFunctionActionVisitor<TInput> visitor, TInput input) => visitor.Visit(this, input);

		public void ApplyVisitor<TInput1, TInput2>(IParseFunctionActionVisitor<TInput1, TInput2> visitor, TInput1 input1, TInput2 input2) => visitor.Visit(this, input1, input2);

		public TOutput ApplyVisitor<TOutput>(IParseFunctionFuncVisitor<TOutput> visitor) => visitor.Visit(this);

		public TOutput ApplyVisitor<TInput, TOutput>(IParseFunctionFuncVisitor<TInput, TOutput> visitor, TInput input) => visitor.Visit(this, input);
	}
}
