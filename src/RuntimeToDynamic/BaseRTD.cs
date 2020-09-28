using Natasha.CSharp;
using Natasha.CSharp.Template;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;

namespace RuntimeToDynamic
{

    public class BaseRTD<T> : ALinkTemplate<T> where T : BaseRTD<T>, new() 
    {

        public readonly ConcurrentDictionary<string, object> NameValueMapping;
        private readonly ConcurrentDictionary<string, Type> _name_type_mapping;
        private R2DBuildType _buildType;


        public BaseRTD()
        {

            NameValueMapping = new ConcurrentDictionary<string, object>();
            _name_type_mapping = new ConcurrentDictionary<string, Type>();

        }


        public T SetBuildType(R2DBuildType buildType)
        {
            _buildType = buildType;
            return Link;
        }


        public T UseReadonlyFields()
        {
            return SetBuildType(R2DBuildType.Readonly);
        }
        public T UseStaticFields()
        {
            return SetBuildType(R2DBuildType.Static);
        }
        public T UseStaticReadonlyFields()
        {
            return SetBuildType(R2DBuildType.Static | R2DBuildType.Readonly);
        }


        public object this[string key]
        {
            set { AddValue(key, value); }
        }



        /// <summary>
        /// 指定名字将值添加到缓存中
        /// </summary>
        /// <param name="name">字段名字</param>
        /// <param name="value">值</param>
        /// <param name="type">指定的类型</param>
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


        /// <summary>
        /// 获取字段定义字符串
        /// </summary>
        public string FieldsScript 
        { 
            get 
            {
                StringBuilder fieldsBuilder = new StringBuilder();
                string fieldsDefined = "public ";
                if ((_buildType & R2DBuildType.Static) != 0)
                {

                    fieldsDefined += "static ";

                }
                if ((_buildType & R2DBuildType.Readonly) != 0)
                {

                    fieldsDefined += "readonly ";

                }


                foreach (var item in NameValueMapping)
                {

                    string name = item.Key;
                    string typeName = NameValueMapping[item.Key].GetType().GetDevelopName();
                    if (_name_type_mapping.ContainsKey(item.Key))
                    {
                        typeName = _name_type_mapping[item.Key].GetDevelopName();
                    }
                    fieldsBuilder.AppendLine($"{fieldsDefined} {typeName} {name};");

                }
                return fieldsBuilder.ToString();
            } 
        }


        /// <summary>
        /// 获取方法字符串
        /// </summary>
        public string MethodScript
        {
            get
            {
                StringBuilder methodBuilder = new StringBuilder();
                string methodDefined = "public void SetObject(ConcurrentDictionary<string,object> objs){";
                if ((_buildType & R2DBuildType.Static) != 0)
                {

                    methodDefined =@"public static void SetObject(ConcurrentDictionary<string,object> objs){";

                }

                methodBuilder.AppendLine(methodDefined);

                    foreach (var item in NameValueMapping)
                    {

                        string name = item.Key;
                        string typeName = NameValueMapping[item.Key].GetType().GetDevelopName();
                        if (_name_type_mapping.ContainsKey(item.Key))
                        {
                            typeName = _name_type_mapping[item.Key].GetDevelopName();
                        }
                        methodBuilder.AppendLine($"{((_buildType & R2DBuildType.Readonly) != 0? name.ReadonlyScript() : name)} = ({typeName})objs[\"{name}\"];");

                    }

                
                methodBuilder.AppendLine("}");
                return methodBuilder.ToString();
            }
        }


        public Action GetInitMethod(NClass nClass)
        {
           var action = nClass.DelegateHandler.Action<ConcurrentDictionary<string, object>>($@"
{nClass.NameScript}.SetObject(obj);
");
            return () => { action(NameValueMapping); };
        }

        public Action<TInstance> GetInitMethod<TInstance>(NClass nClass)
        {
                var initMethod = nClass.DelegateHandler.Action<TInstance, ConcurrentDictionary<string, object>>($@"
var realInstance  = ({nClass.NameScript})arg1;
realInstance.SetObject(arg2);
");
                return (instance) => { initMethod(instance, NameValueMapping); };
           
        }
    }

}
