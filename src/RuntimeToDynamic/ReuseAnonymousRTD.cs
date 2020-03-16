using System;
using System.Collections.Concurrent;

namespace Natasha.RuntimeToDynamic
{
    public class ReuseAnonymousRTD : AnonymousRTD
    {

        private readonly ConcurrentDictionary<object, string> _cache;
        public ReuseAnonymousRTD()
        {
            _cache = new ConcurrentDictionary<object, string>();
        }




        public string GetScript(object value)
        {

            if (_cache.ContainsKey(value))
            {
                return  TypeName + "." + _cache[value];
            }
            return default;

        }




        public override string AddValue(object value, Type type = null)
        {

            string name = default;
            if (!_cache.ContainsKey(value))
            {

                name = base.AddValue(value, type);
                _cache[value] = name;

            }
            return name;
            
        }




        public override void AddValue(string name, object value, Type type = null)
        {

            //删除之前的
            if (_cache.ContainsKey(value))
            {

                var temp = _cache[value];
                Remove(temp);

            }


            //保存最新的
            _cache[value] = name;
            base.AddValue(name, value, type);

        }

    }

}
