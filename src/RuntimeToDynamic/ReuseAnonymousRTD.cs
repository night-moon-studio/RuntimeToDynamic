using System;
using System.Collections.Concurrent;

namespace RuntimeToDynamic
{

    public class ReuseAnonymousRTD : ReuseAnonymousRTD<ReuseAnonymousRTD>
    {

        public ReuseAnonymousRTD()
        {

            Link = this;

        }

    }


    public class ReuseAnonymousRTD<T> : AnonymousRTD<T> where T : BaseRTD<T>, new()
    {

        private readonly ConcurrentDictionary<object, string> _cache;
        public ReuseAnonymousRTD()
        {
            _cache = new ConcurrentDictionary<object, string>();
        }



        /// <summary>
        /// 通过值获取匿名名字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetFieldName(object value)
        {

            if (_cache.ContainsKey(value))
            {
                return _cache[value];
            }
            return default;

        }




        public override string AddValue(object value, Type type = null)
        {

            if (!_cache.ContainsKey(value))
            {

                _cache[value] = base.AddValue(value, type);

            }
            return _cache[value];

        }



        /// <summary>
        /// 指定名字更新缓存
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="value">值</param>
        /// <param name="type">指定的类型</param>
        public override void AddValue(string name, object value, Type type = null)
        {

            if (!_cache.ContainsKey(value))
            {

                //保存最新的
                _cache[value] = name;
                base.AddValue(name, value, type);

            }
            

        }

    }

   


}
