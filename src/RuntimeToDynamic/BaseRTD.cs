using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha.RuntimeToDynamic
{

    public class BaseRTD<T> : RTDHandler where T : BaseRTD<T>, new()
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





        #region 指定字符串域创建以及参数
        public static T Create(string domainName, ComplierResultTarget target = ComplierResultTarget.Stream, ComplierResultError error = ComplierResultError.None)
        {

            return Create(domainName, error, target);

        }

        public static T Create(string domainName, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            if (domainName == default || domainName.ToLower() == "default")
            {
                return Create(DomainManagment.Default, target, error);
            }
            else
            {
                return Create(DomainManagment.Create(domainName), target, error);
            }

        }
        #endregion
        #region 指定域创建以及参数
        public static T Create(AssemblyDomain domain, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(domain, target, error);

        }

        public static T Create(AssemblyDomain domain, ComplierResultTarget target = ComplierResultTarget.Stream, ComplierResultError error = ComplierResultError.None)
        {

            T instance = new T();
            instance.Builder.Complier.EnumCRError = error;
            instance.Builder.Complier.EnumCRTarget = target;
            instance.Builder.Complier.Domain = domain;
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static T Default()
        {

            return Create(DomainManagment.Default, ComplierResultTarget.Stream);

        }

        public static T Default(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Default, target, error);

        }

        public static T Default(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Default, target, error);

        }
        #endregion
        #region 随机域创建以及参数
        public static T Random()
        {

            return Create(DomainManagment.Random, ComplierResultTarget.Stream);

        }


        public static T Random(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Random, target, error);

        }


        public static T Random(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Random, target, error);

        }
        #endregion





        public override AssemblyDomain Domain
        {
            get { return Builder.Complier.Domain; }
            set { Builder.Complier.Domain = value; }
        }




        public string TypeName
        {
            get { return Builder.OopNameScript; }
        }




        public override string Namespace
        {
            get { return Builder.NamespaceScript; }
            set { Builder.Namespace(value); }
        }




        public object this[string key]
        {
            set { AddValue(key, value); }
        }




        public BaseRTD<T> Static()
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
            var action = DelegateHandler.Action<ConcurrentDictionary<string, object>>($"{Builder.OopNameScript}.SetObject(obj);", Builder.NamespaceScript);
            action(_name_value_mapping);


            return result;

        }


    }



}
