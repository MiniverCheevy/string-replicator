using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Voodoo;

namespace StringReplicator.Core.CodeGeneration
{
    public class CodeWalker
    {
        public const string Namespace = "StringReplicator.Core.Controllers";
        private Type[] types;

        public CodeWalker()
        {
            Resources = new List<Resource>();
            var assembly = typeof (CodeWalker).Assembly;
            types = assembly.GetTypes();
            var interestingTypes =
                types.Where(c => c.GetCustomAttributes(typeof (RestAttribute), false).Any()).ToArray();

            var resources =
                interestingTypes.ToLookup(
                    c => c.GetCustomAttributes(typeof (RestAttribute), false).First().To<RestAttribute>().Resource,
                    c =>
                    buildVerb(
                        c.GetCustomAttributes(typeof (RestAttribute), false).First(), c));
            foreach (var key in resources.Select(c => c.Key).ToArray())
            {
                var resource = new Resource()
                    {
                        Name = key,
                        Namespace = Namespace,
                        ClassName = string.Format("{0}Controller", key)
                    };
                resource.Verbs.AddRange(resources[key]);
                Resources.Add(resource);
            }
        }

        public List<Resource> Resources { get; set; }

        public static Dictionary<Verb, RestMethod> Methods
        {
            get
            {
                return new Dictionary<Verb, RestMethod>()
                    {
                        {Verb.Get, new RestMethod() {Attribute = "[HttpGet]", Name = "Get", Parameter = "[FromUri]"}},
                        {Verb.Post, new RestMethod() {Attribute = "[HttpPost]", Name = "Post", Parameter = "[FromUri]"}},
                        {Verb.Put, new RestMethod() {Attribute = "[HttpPut]", Name = "Put", Parameter = "[FromUri]"}},
                        {Verb.Delete,new RestMethod() {Attribute = "[HttpDelete]", Name = "Delete", Parameter = "[FromUri]"}},
                    };
            }
        }

        public Field[] GetProperties(string typeFullName, bool instanceOnly = true)
        {
            var result = new List<Field>();
            var type = types.FirstOrDefault(c => c.FullName == typeFullName);
            if (type == null)
                throw new Exception(string.Format("Could not find type {0}", typeFullName));
            var flags = BindingFlags.Public | BindingFlags.Instance;
            if (instanceOnly)
                flags = flags | BindingFlags.DeclaredOnly;
            var properties = type.GetProperties(flags).ToArray();
            foreach (var property in properties.Where(c => c.PropertyType.IsScalar()).ToArray())
            {
                var field = new Field();
                var name = property.Name;
                field.Name = name;
                field.FriendlyName = name.ToFriendlyString();
                field.CamelCaseName = name.Substring(0, 1).ToLower() + name.Substring(1);
                field.TypeName = property.PropertyType.Name;

                var stringLength = property.GetCustomAttribute<StringLengthAttribute>();
                if (stringLength != null)
                    field.MaxLength = stringLength.MaximumLength;

                result.Add(field);
            }
            return result.ToArray();
        }

        private RestMethod buildVerb(object attributeObject, Type operationType)
        {
            var attribute = attributeObject.To<RestAttribute>();
            var type = operationType;

            while (type.BaseType != null && type.GetGenericArguments().Count() != 2)
            {
                type = type.BaseType;
            }

            var typeArguments = type.GetGenericArguments();
            var verb = Methods[attribute.Verb];
            verb.RequestTypeName = FixUpTypeNamex(typeArguments[0]);
            verb.ResponseTypeName = FixUpTypeNamex(typeArguments[1]);
            verb.OperationTypeName = FixUpTypeNamex(operationType);
            return verb;
        }


        public static string FixUpTypeNamex(Type type)
        {
            var result = type.FullName;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
                result = string.Format("{0}?", Nullable.GetUnderlyingType(type).FullName);
            }

            else if (type.IsGenericType)
            {
                var inner = string.Empty;
                foreach (var t in type.GetGenericArguments())
                {
                    if (t.IsGenericType)
                    {
                        var outer1 = t.GetGenericTypeDefinition().FullName;
                        var ary1 = outer1.Split(@"`".ToCharArray());
                        outer1 = ary1[0];

                        var inner1 = string.Empty;
                        foreach (var t1 in t.GetGenericArguments())
                        {
                            inner1 += t1.FullName;
                            inner1 += ",";
                        }
                        inner1 = inner1.TrimEnd(",".ToCharArray());
                        inner += string.Format("{1}<{0}>", inner1, outer1);
                    }
                    else
                    {
                        inner += t.FullName;
                        inner += ",";
                    }
                }
                inner = inner.TrimEnd(",".ToCharArray());
                var outer = type.GetGenericTypeDefinition().FullName;
                var types = outer.Split(@"`".ToCharArray());
                outer = types[0];
                result = string.Format("{1}<{0}>", inner, outer);
            }
            else
            {
                return result;
            }


            return result;
        }

        public class Field
        {
            public string Name { get; set; }
            public string FriendlyName { get; set; }
            public string CamelCaseName { get; set; }
            public string TypeName { get; set; }
            public int? MaxLength { get; set; }
        }

        [Serializable]
        public class Resource
        {
            public Resource()
            {
                Verbs = new List<RestMethod>();
            }

            public string Namespace { get; set; }
            public string ClassName { get; set; }
            public List<RestMethod> Verbs { get; set; }
            public string Name { get; set; }
        }

        [Serializable]
        public class RestMethod
        {
            public Verb Method { get; set; }
            public string RequestTypeName { get; set; }
            public string ResponseTypeName { get; set; }
            public string OperationTypeName { get; set; }
            public string Attribute { get; set; }
            public string Name { get; set; }
            public string Parameter { get; set; }
        }
    }
}