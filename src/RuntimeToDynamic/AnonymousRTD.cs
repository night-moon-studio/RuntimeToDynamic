using System;

namespace Natasha.RuntimeToDynamic
{
    public class AnonymousRTD : BaseRTD
    {

        private string _prefix;
        private int _counter;

        public AnonymousRTD()
        {
            _prefix = "_anonymous_";
            _counter = 0;
        }




        public void SetPrefix(string value)
        {
            _prefix = value;
        }




        public virtual string AddValue(object value,Type type = default)
        {

            string name = _prefix + _counter;
            AddValue(_prefix + _counter, value, type);
            _counter += 1;
            return name;

        }

    }

}
