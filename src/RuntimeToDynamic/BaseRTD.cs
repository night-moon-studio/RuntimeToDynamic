using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp;
using Natasha.Framework;
using Natasha.Reverser;
using Natasha.Reverser.Model;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha.RuntimeToDynamic
{

    public class BaseRTD<T> : NHandler<T> where T : BaseRTD<T>, new()
    {

        public readonly ConcurrentDictionary<string, object> NameValueMapping;
        private readonly ConcurrentDictionary<string, Type> _name_type_mapping;
        private AccessFlags _fieldAccess;
        private string _template;

        public T FieldAccess(AccessFlags accessType)
        {
            _fieldAccess = accessType;
            return Link;
        }
        public T UseStaticReadonlyField()
        {
            _template = "static readonly ";
            return Link;
        }
        public T UseStaticField()
        {
            _template = "static ";
            return Link;
        }
        public T UseReadonlyField()
        {
            _template = "readonly ";
            return Link;
        }


        public BaseRTD()
        {
            FieldAccess(AccessFlags.Public);
            UseStaticField();
            this.Class();
            UseRandomName();
            Namespace("NScriptPacking");
            Public();
            NameValueMapping = new ConcurrentDictionary<string, object>();
            _name_type_mapping = new ConcurrentDictionary<string, Type>();

        }




        public DomainBase Domain
        {
            get { return AssemblyBuilder.Compiler.Domain; }
            set { AssemblyBuilder.Compiler.Domain = value; }
        }




        public string TypeName
        {
            get { return NameScript; }
        }




        public object this[string key]
        {
            set { AddValue(key, value); }
        }




        public virtual void AddValue(string name, object value, Type type = default)
        {

            NameValueMapping[name] = value;
            if (type != default)
            {
                _name_type_mapping[name] = type;
            }

        }




        public virtual void Remove(string name)
        {

            if (_name_type_mapping.ContainsKey(name))
            {
                while (!_name_type_mapping.TryRemove(name, out _)) ;
            }
            if (NameValueMapping.ContainsKey(name))
            {
                while (!NameValueMapping.TryRemove(name, out _)) ;
            }

        }




        public virtual Type Complie()
        {

            var fieldTemplate = AccessReverser.GetAccess(_fieldAccess) + _template;
            StringBuilder methodBuilder = new StringBuilder();
            if (_template.Contains("static"))
            {
                methodBuilder.AppendLine(@"public static void SetObject(ConcurrentDictionary<string,object> objs){");
            }
            else
            {
                methodBuilder.AppendLine($"public {NameScript}(ConcurrentDictionary<string,object> objs){{");
            }
            foreach (var item in NameValueMapping)
            {

                string name = item.Key;
                Type type;
                if (_name_type_mapping.ContainsKey(item.Key))
                {
                    type = _name_type_mapping[item.Key];
                }
                else
                {
                    type = NameValueMapping[item.Key].GetType();
                }

                BodyAppend(fieldTemplate + $"{type.GetDevelopName()} {name};");
                methodBuilder.AppendLine($"{name} = ({type.GetDevelopName()})objs[\"{name}\"];");


            }
            methodBuilder.AppendLine("}");
            var result = BodyAppend(methodBuilder.ToString()).GetType();

            if (_template.Contains("static"))
            {
                var action = DelegateHandler.Action<ConcurrentDictionary<string, object>>($"{NameScript}.SetObject(obj);", NamespaceScript);
                action(NameValueMapping);
            }

            return result;

        }


    }



}
