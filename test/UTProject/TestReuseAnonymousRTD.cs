using Natasha;
using Natasha.RuntimeToDynamic;
using NatashaUT;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UTProject
{
    [Trait("复用构造","")]
    public class TestReuseAnonymousRTD : PrepareTest
    {

        [Fact(DisplayName = "传值")]
        public void TestValue()
        {

            var runtime = ReuseAnonymousRTD.RandomDomain();
            runtime.AddValue("小明");
            runtime.AddValue("小明");
            runtime.AddValue("小明1");
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");
            runtime.Complie();


            string result = runtime.DelegateHandler.Func<string>($"return {runtime.TypeName}.{runtime.GetFieldScript("小明")};")();
            string result3 = runtime.DelegateHandler.Func<string>($"return {runtime.TypeName}.name;")();
            Assert.Equal("小明", result);
            Assert.Equal("abc", result3);

        }


        [Fact(DisplayName = "传委托")]
        public void TestDelegate()
        {

            Func<string, int> ageFunc = item => item.Length;
            var runtime = ReuseAnonymousRTD.RandomDomain();
            runtime.AddValue(ageFunc);
            runtime.AddValue(ageFunc);
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");
            runtime.Complie();


            var result = runtime.DelegateHandler.Func<string,int>($"return {runtime.TypeName}.{runtime.GetFieldScript(ageFunc)}(arg);")("Hello");
            string result3 = runtime.DelegateHandler.Func<string>($"return {runtime.TypeName}.name;")();
            Assert.Equal("abc", result3);
            Assert.Equal(5, result);

        }

    }
}
