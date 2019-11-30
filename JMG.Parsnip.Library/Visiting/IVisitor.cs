﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMG.Parsnip.VSIXProject.Visiting
{
	public interface IActionVisitor<in TVisitee>
	{
		void Visit(TVisitee target);
	}

	public interface IActionVisitor<in TVisitee, in TInput>
	{
		void Visit(TVisitee target, TInput input);
	}

	public interface IActionVisitor<in TVisitee, in TInput1, in TInput2>
	{
		void Visit(TVisitee target, TInput1 input1, TInput2 input2);
	}

	public interface IFuncVisitor<in TVisitee, out TOutput>
	{
		TOutput Visit(TVisitee target);
	}

	public interface IFuncVisitor<in TVisitee, in TInput, out TOutput>
	{
		TOutput Visit(TVisitee target, TInput input);
	}
}