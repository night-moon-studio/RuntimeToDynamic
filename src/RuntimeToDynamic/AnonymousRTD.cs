using System;
using System.Threading;

namespace RuntimeToDynamic
{

    public class AnonymousRTD : AnonymousRTD<AnonymousRTD>
    {

        public AnonymousRTD()
        {

            Link = this;

        }

    }


    public class AnonymousRTD<T> : BaseRTD<T> where T : BaseRTD<T>, new()
    {

        private string _prefix;
        private int _counter;

        public AnonymousRTD()
        {
            _prefix = "_anonymous_";
            _counter = 0;
        }



        /// <summary>
        /// 设置自己的前缀
        /// </summary>
        /// <param name="name">前缀名</param>
        public virtual void SetPrefix(string name)
        {
            _prefix = name;
        }




        /// <summary>
        /// 匿名添加值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="type">类型（选填）</param>
        /// <returns>返回匿名名称</returns>
        public virtual string AddValue(object value, Type type = default)
        {

            string name = _prefix + Interlocked.Increment(ref _counter);
            AddValue(name, value, type);
            return name;

        }

    }

}
