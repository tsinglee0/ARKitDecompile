using System;
using UnityEngine;

namespace Vuforia
{
	internal static class DelegateHelper
	{
		public static void InvokeWithExceptionHandling(this Action action)
		{
			DelegateHelper.InvokeDelegate(action, new object[0]);
		}

		public static void InvokeWithExceptionHandling<T>(this Action<T> action, T arg)
		{
			DelegateHelper.InvokeDelegate(action, new object[]
			{
				arg
			});
		}

		public static void InvokeWithExceptionHandling<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			DelegateHelper.InvokeDelegate(action, new object[]
			{
				arg1,
				arg2
			});
		}

		private static void InvokeDelegate(Delegate action, params object[] args)
		{
			Delegate[] invocationList = action.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				Delegate @delegate = invocationList[i];
				try
				{
					@delegate.DynamicInvoke(args);
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in callback: " + ex.ToString());
				}
			}
		}
	}
}
