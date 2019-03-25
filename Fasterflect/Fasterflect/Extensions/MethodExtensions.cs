#region License
// Copyright 2010 Buu Nguyen, Morten Mertner
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://fasterflect.codeplex.com/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect.Emitter;
using Fasterflect.Extensions.Utilities;

namespace Fasterflect.Extensions
{
	/// <summary>
	/// Extension methods for locating, inspecting and invoking methods.
	/// </summary>
	public static class MethodExtensions
	{
		#region Method Invocation
		/// <summary>
		/// Creates a delegate which can invoke the method <paramref name="name"/> with arguments matching
		/// <paramref name="parameterTypes"/> on the given <paramref name="type"/>.
		/// Leave <paramref name="parameterTypes"/> empty if the method has no arguments.
		/// </summary>
		public static MethodInvoker DelegateForCallMethod(this Type type, string name, params Type[] parameterTypes)
		{
			return Reflect.Method(type, name, parameterTypes);
		}

		/// <summary>
		/// Create a delegate to invoke a generic method.  See the overload with same parameters except for <paramref name="genericTypes"/>.
		/// </summary>
		/// <seealso cref="DelegateForCallMethod(Type,string,Type[])"/>
		public static MethodInvoker DelegateForCallMethod(this Type type, Type[] genericTypes, string name, params Type[] parameterTypes)
		{
			return Reflect.Method(type, genericTypes, name, parameterTypes);
		}

		/// <summary>
		/// Creates a delegate which can invoke the method <paramref name="name"/> with arguments matching
		/// <paramref name="parameterTypes"/> and matching <paramref name="bindingFlags"/> on the given <paramref name="type"/>.
		/// Leave <paramref name="parameterTypes"/> empty if the method has no arguments.
		/// </summary>
		public static MethodInvoker DelegateForCallMethod(this Type type, string name, Flags bindingFlags, params Type[] parameterTypes)
		{
			return Reflect.Method(type, name, bindingFlags, parameterTypes);
		}

		/// <summary>
		/// Create a delegate to invoke a generic method.  See the overload with same parameters except for <paramref name="genericTypes"/>.
		/// </summary>
		/// <seealso cref="DelegateForCallMethod(Type,string,Flags,Type[])"/>
		public static MethodInvoker DelegateForCallMethod(this Type type, Type[] genericTypes, string name, Flags bindingFlags,
			params Type[] parameterTypes)
		{
			return Reflect.Method(type, genericTypes, name, bindingFlags, parameterTypes);
		}
		#endregion

		#region Method Lookup (Single)
		/// <summary>
		/// Gets the public or non-public instance method with the given <paramref name="name"/> on the
		/// given <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type on which to reflect.</param>
		/// <param name="name">The name of the method to search for. This argument must be supplied. The 
		/// default behavior is to check for an exact, case-sensitive match. Pass <see href="Flags.ExplicitNameMatch"/> 
		/// to include explicitly implemented interface members, <see href="Flags.PartialNameMatch"/> to locate
		/// by substring, and <see href="Flags.IgnoreCase"/> to ignore case.</param>
		/// <returns>The specified method or null if no method was found. If there are multiple matches
		/// due to method overloading the first found match will be returned.</returns>
		public static MethodInfo Method(this Type type, string name)
		{
			return Reflect.Lookup.Method(type, name);
		}

		/// <summary>
		/// Gets a generic method.  See the overload with same arguments exception for <paramref name="genericTypes"/>.
		/// </summary>
		/// <seealso cref="Method(Type,string)"/>
		public static MethodInfo Method(this Type type, Type[] genericTypes, string name)
		{
			return Reflect.Lookup.Method(type, genericTypes, name);
		}

		/// <summary>
		/// Gets the public or non-public instance method with the given <paramref name="name"/> on the 
		/// given <paramref name="type"/> where the parameter types correspond in order with the
		/// supplied <paramref name="parameterTypes"/>.
		/// </summary>
		/// <param name="type">The type on which to reflect.</param>
		/// <param name="name">The name of the method to search for. This argument must be supplied. The 
		/// default behavior is to check for an exact, case-sensitive match.</param>
		/// <param name="parameterTypes">If this parameter is not null then only methods with the same 
		/// parameter signature will be included in the result.</param>
		/// <returns>The specified method or null if no method was found. If there are multiple matches
		/// due to method overloading the first found match will be returned.</returns>
		public static MethodInfo Method(this Type type, string name, Type[] parameterTypes)
		{
			return Reflect.Lookup.Method(type, name, parameterTypes);
		}

		/// <summary>
		/// Gets a generic method.  See the overload with same arguments exception for <paramref name="genericTypes"/>.
		/// </summary>
		/// <seealso cref="Method(Type,string,Type[])"/>
		public static MethodInfo Method(this Type type, Type[] genericTypes, string name, Type[] parameterTypes)
		{
			return Reflect.Lookup.Method(type, genericTypes, name, parameterTypes);
		}

		/// <summary>
		/// Gets the method with the given <paramref name="name"/> and matching <paramref name="bindingFlags"/>
		/// on the given <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type on which to reflect.</param>
		/// <param name="name">The name of the method to search for. This argument must be supplied. The 
		/// default behavior is to check for an exact, case-sensitive match. Pass <see href="Flags.ExplicitNameMatch"/> 
		/// to include explicitly implemented interface members, <see href="Flags.PartialNameMatch"/> to locate
		/// by substring, and <see href="Flags.IgnoreCase"/> to ignore case.</param>
		/// <param name="bindingFlags">The <see cref="BindingFlags"/> or <see cref="Flags"/> combination used to define
		/// the search behavior and result filtering.</param>
		/// <returns>The specified method or null if no method was found. If there are multiple matches
		/// due to method overloading the first found match will be returned.</returns>
		public static MethodInfo Method(this Type type, string name, Flags bindingFlags)
		{
			return Reflect.Lookup.Method(type, name, bindingFlags);
		}

		/// <summary>
		/// Gets a generic method.  See the overload with same arguments exception for <paramref name="genericTypes"/>.
		/// </summary>
		/// <seealso cref="Method(Type,string,Flags)"/>
		public static MethodInfo Method(this Type type, Type[] genericTypes, string name, Flags bindingFlags)
		{
			return Reflect.Lookup.Method(type, genericTypes, name, bindingFlags);
		}

		/// <summary>
		/// Gets the method with the given <paramref name="name"/> and matching <paramref name="bindingFlags"/>
		/// on the given <paramref name="type"/> where the parameter types correspond in order with the
		/// supplied <paramref name="parameterTypes"/>.
		/// </summary>
		/// <param name="type">The type on which to reflect.</param>
		/// <param name="name">The name of the method to search for. This argument must be supplied. The 
		///   default behavior is to check for an exact, case-sensitive match. Pass <see href="Flags.ExplicitNameMatch"/> 
		///   to include explicitly implemented interface members, <see href="Flags.PartialNameMatch"/> to locate
		///   by substring, and <see href="Flags.IgnoreCase"/> to ignore case.</param>
		/// <param name="parameterTypes">If this parameter is supplied then only methods with the same parameter signature
		///   will be included in the result. The default behavior is to check only for assignment compatibility,
		///   but this can be changed to exact matching by passing <see href="Flags.ExactBinding"/>.</param>
		/// <param name="bindingFlags">The <see cref="BindingFlags"/> or <see cref="Flags"/> combination used to define
		///   the search behavior and result filtering.</param>
		/// <returns>The specified method or null if no method was found. If there are multiple matches
		/// due to method overloading the first found match will be returned.</returns>
		public static MethodInfo Method(this Type type, string name, Type[] parameterTypes, Flags bindingFlags)
		{
			return Reflect.Lookup.Method(type, name, parameterTypes, bindingFlags);
		}

		/// <summary>
		/// Gets the method with the given <paramref name="name"/> and matching <paramref name="bindingFlags"/>
		/// on the given <paramref name="type"/> where the parameter types correspond in order with the
		/// supplied <paramref name="parameterTypes"/>.
		/// </summary>
		/// <param name="type">The type on which to reflect.</param>
		/// <param name="genericTypes">Type parameters if this is a generic method.</param>
		/// <param name="name">The name of the method to search for. This argument must be supplied. The 
		///   default behavior is to check for an exact, case-sensitive match. Pass <see href="Flags.ExplicitNameMatch"/> 
		///   to include explicitly implemented interface members, <see href="Flags.PartialNameMatch"/> to locate
		///   by substring, and <see href="Flags.IgnoreCase"/> to ignore case.</param>
		/// <param name="parameterTypes">If this parameter is supplied then only methods with the same parameter signature
		///   will be included in the result. The default behavior is to check only for assignment compatibility,
		///   but this can be changed to exact matching by passing <see href="Flags.ExactBinding"/>.</param>
		/// <param name="bindingFlags">The <see cref="BindingFlags"/> or <see cref="Flags"/> combination used to define
		///   the search behavior and result filtering.</param>
		/// <returns>The specified method or null if no method was found. If there are multiple matches
		/// due to method overloading the first found match will be returned.</returns>
		public static MethodInfo Method(this Type type, Type[] genericTypes, string name, Type[] parameterTypes, Flags bindingFlags)
		{
			return Reflect.Lookup.Method(type, genericTypes, name, parameterTypes, bindingFlags);
		}

		internal static MethodInfo MakeGeneric(this MethodInfo methodInfo, Type[] genericTypes)
		{
			if (methodInfo == null) {
				return null;
			}
			if (genericTypes == null ||
				genericTypes.Length == 0 ||
				genericTypes == Type.EmptyTypes) {
				return methodInfo;
			}
			return methodInfo.MakeGenericMethod(genericTypes);
		}
		#endregion

		#region Method Lookup (Multiple)
		/// <summary>
		/// Gets all public and non-public instance methods on the given <paramref name="type"/> that match the 
		/// given <paramref name="names"/>.
		/// </summary>
		/// <param name="type">The type on which to reflect.</param>
		/// <param name="names">The optional list of names against which to filter the result. If this parameter is
		/// <c>null</c> or empty no name filtering will be applied. The default behavior is to check for an exact, 
		/// case-sensitive match. Pass <see href="Flags.ExcludeExplicitlyImplemented"/> to exclude explicitly implemented 
		/// interface members, <see href="Flags.PartialNameMatch"/> to locate by substring, and 
		/// <see href="Flags.IgnoreCase"/> to ignore case.</param>
		/// <returns>A list of all matching methods. This value will never be null.</returns>
		public static IList<MethodInfo> Methods(this Type type, params string[] names)
		{
			return Reflect.Lookup.Methods(type, names);
		}

		/// <summary>
		/// Gets all public and non-public instance methods on the given <paramref name="type"/> that match the 
		/// given <paramref name="names"/>.
		/// </summary>
		/// <param name="type">The type on which to reflect.</param>
		/// <param name="bindingFlags">The <see cref="BindingFlags"/> or <see cref="Flags"/> combination used to define
		/// the search behavior and result filtering.</param>
		/// <param name="names">The optional list of names against which to filter the result. If this parameter is
		/// <c>null</c> or empty no name filtering will be applied. The default behavior is to check for an exact, 
		/// case-sensitive match. Pass <see href="Flags.ExcludeExplicitlyImplemented"/> to exclude explicitly implemented 
		/// interface members, <see href="Flags.PartialNameMatch"/> to locate by substring, and 
		/// <see href="Flags.IgnoreCase"/> to ignore case.</param>
		/// <returns>A list of all matching methods. This value will never be null.</returns>
		public static IList<MethodInfo> Methods(this Type type, Flags bindingFlags, params string[] names)
		{
			return Reflect.Lookup.Methods(type, bindingFlags, names);
		}

		/// <summary>
		/// Gets all public and non-public instance methods on the given <paramref name="type"/> that match the given 
		///  <paramref name="names"/>.
		/// </summary>
		/// <param name="type">The type on which to reflect.</param>
		/// <param name="parameterTypes">If this parameter is supplied then only methods with the same parameter 
		/// signature will be included in the result.</param>
		/// <param name="names">The optional list of names against which to filter the result. If this parameter is
		/// <c>null</c> or empty no name filtering will be applied. The default behavior is to check for an exact, 
		/// case-sensitive match.</param>
		/// <returns>A list of all matching methods. This value will never be null.</returns>
		public static IList<MethodInfo> Methods(this Type type, Type[] parameterTypes, params string[] names)
		{
			return Reflect.Lookup.Methods(type, parameterTypes, names);
		}

		/// <summary>
		/// Gets all methods on the given <paramref name="type"/> that match the given lookup criteria.
		/// </summary>
		/// <param name="type">The type on which to reflect.</param>
		/// <param name="parameterTypes">If this parameter is supplied then only methods with the same parameter signature
		/// will be included in the result. The default behavior is to check only for assignment compatibility,
		/// but this can be changed to exact matching by passing <see href="Flags.ExactBinding"/>.</param>
		/// <param name="bindingFlags">The <see cref="BindingFlags"/> or <see cref="Flags"/> combination used to define
		/// the search behavior and result filtering.</param>
		/// <param name="names">The optional list of names against which to filter the result. If this parameter is
		/// <c>null</c> or empty no name filtering will be applied. The default behavior is to check for an exact, 
		/// case-sensitive match. Pass <see href="Flags.ExcludeExplicitlyImplemented"/> to exclude explicitly implemented 
		/// interface members, <see href="Flags.PartialNameMatch"/> to locate by substring, and 
		/// <see href="Flags.IgnoreCase"/> to ignore case.</param>
		/// <returns>A list of all matching methods. This value will never be null.</returns>
		public static IList<MethodInfo> Methods(this Type type, Type[] parameterTypes, Flags bindingFlags, params string[] names)
		{
			return Reflect.Lookup.Methods(type, parameterTypes, bindingFlags, names);
		}

		/// <summary>
		/// Gets all methods on the given <paramref name="type"/> that match the given lookup criteria.
		/// </summary>
		/// <param name="type">The type on which to reflect.</param>
		/// <param name="genericTypes">If this parameter is supplied then only methods with the same generic parameter 
		/// signature will be included in the result. The default behavior is to check only for assignment compatibility,
		/// but this can be changed to exact matching by passing <see href="Flags.ExactBinding"/>.</param>
		/// <param name="parameterTypes">If this parameter is supplied then only methods with the same parameter signature
		/// will be included in the result. The default behavior is to check only for assignment compatibility,
		/// but this can be changed to exact matching by passing <see href="Flags.ExactBinding"/>.</param>
		/// <param name="bindingFlags">The <see cref="BindingFlags"/> or <see cref="Flags"/> combination used to define
		/// the search behavior and result filtering.</param>
		/// <param name="names">The optional list of names against which to filter the result. If this parameter is
		/// <c>null</c> or empty no name filtering will be applied. The default behavior is to check for an exact, 
		/// case-sensitive match. Pass <see href="Flags.ExcludeExplicitlyImplemented"/> to exclude explicitly implemented 
		/// interface members, <see href="Flags.PartialNameMatch"/> to locate by substring, and 
		/// <see href="Flags.IgnoreCase"/> to ignore case.</param>
		/// <returns>A list of all matching methods. This value will never be null.</returns>
		public static IList<MethodInfo> Methods(this Type type, Type[] genericTypes, Type[] parameterTypes, Flags bindingFlags,
			params string[] names)
		{
			return Reflect.Lookup.Methods(type, genericTypes, parameterTypes, bindingFlags, names);
		}
		#endregion
	}
}