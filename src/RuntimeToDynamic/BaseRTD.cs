using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha.RuntimeToDynamic
{

    public class BaseRTD
    {

        public NClass Builder;
        private readonly ConcurrentDictionary<string,object> _name_value_mapping;
        private readonly ConcurrentDictionary<string, Type> _name_type_mapping;
        public BaseRTD()
        {

            Builder = new NClass()
                .ChangeToClass()
                .UseRandomOopName()
                .Namespace("NScriptPacking")
                .Public;
            _name_value_mapping = new ConcurrentDictionary<string, object>();
            _name_type_mapping = new ConcurrentDictionary<string, Type>();

        }




        public AssemblyDomain Domain
        {
            get { return Builder.Complier.Domain; }
            set { Builder.Complier.Domain = value; }
        }




        public string TypeName
        {
            get { return Builder.OopNameScript; }
        }




        public string Namespace
        {
            get { return Builder.NamespaceScript; }
            set { Builder.Namespace(value); }
        }




        public object this[string key]
        {
            set { AddValue(key, value); }
        }




        public BaseRTD Static()
        {

            Builder.OopModifier( Natasha.Reverser.Model.Modifiers.Static);
            return this;

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


                Builder.PublicStaticField(type, name);
                methodBuilder.AppendLine($"{name} = ({type.GetDevelopName()})objs[\"{name}\"];");


            }
            methodBuilder.AppendLine("}");


            var result = Builder.OopBody(methodBuilder).GetType();
            var action = NDomain.Create(Domain).Action<ConcurrentDictionary<string, object>>($"{Builder.OopNameScript}.SetObject(obj);", Builder.NamespaceScript);
            action(_name_value_mapping);


            return result;

        }


    }



}
