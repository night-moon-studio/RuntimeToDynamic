using Natasha;
using Natasha.RuntimeToDynamic;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UTProject
{
    [Trait("复用构造","")]
    public class TestReuseAnonymousRTD
    {

        [Fact(DisplayName = "传值")]
        public void TestValue()
        {

            ReuseAnonymousRTD runtime = new ReuseAnonymousRTD();
            runtime.AddValue("小明");
            runtime.AddValue("小明");
            runtime.AddValue("小明1");
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");
            runtime.Complie();


            string result = runtime.NDomainHandler.Func<string>($"return {runtime.GetScript("小明")};", runtime.Namespace)();
            string result3 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}.name2;", runtime.Namespace)();
            string result4 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}.name2;", runtime.Namespace)();
            Assert.Equal("小明", result);
            Assert.Equal("abc", result3);
            Assert.Equal("abc", result4);

        }


        [Fact(DisplayName = "传委托")]
        public void TestDelegate()
        {

            Func<string, int> ageFunc = item => item.Length;
            ReuseAnonymousRTD runtime = new ReuseAnonymousRTD();
            runtime.AddValue(ageFunc);
            runtime.AddValue(ageFunc);
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");
            runtime.Complie();


            var result = runtime.NDomainHandler.Func<string,int>($"return {runtime.GetScript(ageFunc)}(arg);", runtime.Namespace)("Hello");
            string result3 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}.name2;", runtime.Namespace)();
            string result4 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}.name2;", runtime.Namespace)();
            Assert.Equal("abc", result3);
            Assert.Equal("abc", result4);
            Assert.Equal(5, result);

        }

    }
}
