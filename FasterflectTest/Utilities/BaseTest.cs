﻿#region License

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

using Fasterflect;
using Fasterflect.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FasterflectTest.Common
{
	public abstract class BaseTest
	{
		protected readonly Type[] Types;

		protected BaseTest(Type[] types)
		{
			Types = types;
		}

		protected static void VerifyProperties(Type type, object sample)
		{
			Type sampleType = sample.GetType();
			IList<PropertyInfo> properties = sampleType.Properties();
			foreach (PropertyInfo propInfo in properties) {
				string name = propInfo.Name.FirstCharUpper();
				object value1 = propInfo.Get(sample);
				object value2 = type.GetPropertyValue(name);
				Assert.AreEqual(value1, value2);
			}
		}

		protected static void VerifyProperties(object obj, object sample)
		{
			Type sampleType = sample.GetType();
			IList<PropertyInfo> properties = sampleType.Properties();
			foreach (PropertyInfo propInfo in properties) {
				string name = propInfo.Name.FirstCharUpper();
				object value1 = propInfo.Get(sample);
				object value2 = obj.GetPropertyValue(name);
				Assert.AreEqual(value1, value2);
			}
		}

		protected static void VerifyFields(Type type, object sample)
		{
			Type sampleType = sample.GetType();
			IList<PropertyInfo> properties = sampleType.Properties();
			foreach (PropertyInfo propInfo in properties) {
				string name = propInfo.Name.FirstCharLower();
				object value1 = propInfo.Get(sample);
				object value2 = type.GetFieldValue(name);
				Assert.AreEqual(value1, value2);
			}
		}

		protected static void VerifyFields(object obj, object sample)
		{
			IList<PropertyInfo> properties = sample.GetType().Properties();
			foreach (PropertyInfo propInfo in properties) {
				string name = propInfo.Name.FirstCharLower();
				object value1 = propInfo.Get(sample);
				object value2 = obj.GetFieldValue(name);
				Assert.AreEqual(value1, value2);
			}
		}

		protected void RunWith(Action<Type> assertionAction)
		{
			foreach (Type type in Types) {
				assertionAction(type);
			}
		}

		protected void RunWith(Action<object> assertionAction)
		{
			foreach (Type type in Types) {
				object instance = type.CreateInstance();
				object wrapped = instance.WrapIfValueType();
				assertionAction(wrapped);
			}
		}
	}
}