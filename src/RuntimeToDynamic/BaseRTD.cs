using Natasha.CSharp;
using Natasha.Framework;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha.RuntimeToDynamic
{

    public class BaseRTD<T> : NHandler<T> where T : BaseRTD<T>, new()
    {

        private readonly ConcurrentDictionary<string,object> _name_value_mapping;
        private readonly ConcurrentDictionary<string, Type> _name_type_mapping;
        public BaseRTD()
        {

            this.Class();
            UseRandomName();
            Namespace("NScriptPacking");
            Public();
            _name_value_mapping = new ConcurrentDictionary<string, object>();
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




        public virtual void AddValue(string name,object value, Type type = default)
        {

            _name_value_mapping[name] = value;
            if (type!=default)
            {
                _name_type_mapping[name] = type;
            }

        }




        public virtual void Remove(string name)
        {

            if (_name_type_mapping.ContainsKey(name))
            {
                while (!_name_type_mapping.TryRemove(name, out _));
            }
            if (_name_value_mapping.ContainsKey(name))
            {
                while (!_name_value_mapping.TryRemove(name, out _)) ;
            }

        }




        public virtual Type Complie()
        {

            StringBuilder methodBuilder = new StringBuilder();
            methodBuilder.AppendLine(@"public static void SetObject(ConcurrentDictionary<string,object> objs){");
            foreach (var item in _name_value_mapping)
            {
                
                string name = item.Key;


                Type type;
                if (_name_type_mapping.ContainsKey(item.Key))
                {
                    type = _name_type_mapping[item.Key];
                }
                else
                {
                    type = _name_value_mapping[item.Key].GetType();
                }


                PublicStaticField(type, name);
                methodBuilder.AppendLine($"{name} = ({type.GetDevelopName()})objs[\"{name}\"];");


            }
            methodBuilder.AppendLine("}");


            var result = BodyAppend(methodBuilder.ToString()).GetType();
            var action = DelegateHandler.Action<ConcurrentDictionary<string, object>>($"{NameScript}.SetObject(obj);", NamespaceScript);
            action(_name_value_mapping);


            return result;

        }


    }



}
