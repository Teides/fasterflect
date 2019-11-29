﻿#region License

// Copyright 2010 Buu Nguyen, Morten Mertner
// Copyright 2018 Wesley Hamilton
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
// The latest version of this file can be found at https://github.com/ffhighwind/fasterflect

#endregion

using Fasterflect.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Fasterflect.Emitter
{
	internal class MultiSetEmitter : BaseEmitter
	{
		public MultiSetEmitter(Type targetType, FasterflectFlags flags, string[] memberNames)
			: base(new MultiSetCallInfo(targetType, flags, memberNames))
		{
		}

		protected internal override DynamicMethod CreateDynamicMethod()
		{
			return CreateDynamicMethod("multisetter", CallInfo.TargetType, null,
				new[] { typeof(object), typeof(object).MakeArrayType() });
		}

		protected internal override Delegate CreateDelegate()
		{
			MultiSetCallInfo callinfo = (MultiSetCallInfo)CallInfo;
			MemberInfo[] members = callinfo.members;
			callinfo.IsStatic = true;
			for (int i = 0, count = members.Length; i < count; i++) {
				bool isstatic = members[i].IsStatic();
				if (!isstatic) {
					callinfo.IsStatic = false;
					break;
				}
			}
			bool handleInnerStruct = CallInfo.ShouldHandleInnerStruct;
			if (!callinfo.IsStatic) {
				Generator.ldarg_0.end();                            // load arg-0 (this)
				if (handleInnerStruct) {
					Generator.DeclareLocal(CallInfo.TargetType);    // TargetType tmpStr
					Generator.castclass(typeof(ValueTypeHolder)) // (ValueTypeHolder)wrappedStruct
						.callvirt(StructGetMethod) // <stack>.get_Value()
						.unbox_any(CallInfo.TargetType) // unbox <stack>
						.stloc(0); // localStr = <stack>
				}
				else {
					Generator.castclass(CallInfo.TargetType);      // (TargetType)this
				}
			}

			for (int i = 0, count = members.Length; i < count; i++) {
				MemberInfo method = members[i];
				if (method == null)
					continue;
				if (method is FieldInfo field) {
					if (!field.IsStatic) {
						if (handleInnerStruct) {
							Generator.ldloca_s(0);
						}
						else if (i != count - 1) {
							Generator.dup.end();
						}
					}
					Generator.ldarg_1.ldc_i4(i).ldelem_ref.end();
					Generator.CastFromObject(field.FieldType);
					Generator.stfld(field.IsStatic, field);             // (this|tmpStr).field = value-to-be-set;
				}
				else {
					PropertyInfo property = (PropertyInfo)method;
					MethodInfo setMethod = property.GetSetMethod(true);
					if (!setMethod.IsStatic) {
						if (handleInnerStruct) {
							Generator.ldloca_s(0);
						}
						else if (i != count - 1) {
							Generator.dup.end();
						}
					}
					Generator.ldarg_1.ldc_i4(i).ldelem_ref.end();
					Generator.CastFromObject(property.PropertyType);
					Generator.call(setMethod.IsStatic || CallInfo.IsTargetTypeStruct, setMethod); // (this|tmpStr).set_Prop(value-to-be-set);
				}
			}
			if (handleInnerStruct) {
				StoreLocalToInnerStruct(0); // ((ValueTypeHolder)this)).Value = tmpStr
			}
			//Generator.ldnull.end(); // load null
			Generator.ret();
			return Method.CreateDelegate(typeof(MultiSetter));
		}
	}
}
