using System;

namespace Natasha.RuntimeToDynamic
{

    public class AnonymousRTD : BaseRTD<AnonymousRTD>
    {

        private string _prefix;
        private int _counter;

        public AnonymousRTD()
        {
            _prefix = "_anonymous_";
            _counter = 1;
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
        /// <returns></returns>
        public virtual string AddValue(object value, Type type = default)
        {

            string name = _prefix + _counter;
            AddValue(_prefix + _counter, value, type);
            _counter += 1;
            return name;

        }

    }


    public class AnonymousRTD<T> : BaseRTD<T> where T : BaseRTD<T>, new()
    {

        private string _prefix;
        private int _counter;

        public AnonymousRTD()
        {
            _prefix = "_anonymous_";
            _counter = 1;
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
        /// <returns></returns>
        public virtual string AddValue(object value, Type type = default)
        {

            string name = _prefix + _counter;
            AddValue(_prefix + _counter, value, type);
            _counter += 1;
            return name;

        }

    }

}
