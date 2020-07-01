using Natasha;
using Natasha.RuntimeToDynamic;
using NatashaUT;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UTProject
{
    [Trait("匿名构造","")]
    public class TestAnonymousRTD:PrepareTest
    {

        [Fact(DisplayName = "传值")]
        public void TestValue()
        {

            var runtime = AnonymousRTD.RandomDomain();
            runtime.AddValue("小明");
            runtime.AddValue("小明1");
            runtime.AddValue("小明");
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");
            runtime.Complie();


            string result0 = runtime.DelegateHandler.Func<string>($"return {runtime.TypeName}._anonymous_1;")();
            string result1 = runtime.DelegateHandler.Func<string>($"return {runtime.TypeName}._anonymous_2;")();
            string result2 = runtime.DelegateHandler.Func<string>($"return {runtime.TypeName}._anonymous_3;")();
            string result3 = runtime.DelegateHandler.Func<string>($"return {runtime.TypeName}.name;")();
            string result4 = runtime.DelegateHandler.Func<string>($"return {runtime.TypeName}.name2;")();
            Assert.Equal("小明1", result1);
            Assert.Equal("小明", result0);
            Assert.Equal("小明", result2);
            Assert.Equal("abc", result3);
            Assert.Equal("abc", result4);

        }


        [Fact(DisplayName = "传委托")]
        public void TestDelegate()
        {

            Func<string, int> func = item => item.Length;
            Func<string, int> func1 = item => item.Length+1;
            var runtime = AnonymousRTD.RandomDomain();
            runtime.AddValue(func);
            runtime.AddValue(func1);
            runtime.AddValue(func);
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");
            runtime.Complie();


            int result0 = runtime.DelegateHandler.Func<string,int>($"return {runtime.TypeName}._anonymous_1(arg);")("hello");
            int result1 = runtime.DelegateHandler.Func<string, int>($"return {runtime.TypeName}._anonymous_2(arg);")("hello");
            int result2 = runtime.DelegateHandler.Func<string, int>($"return {runtime.TypeName}._anonymous_3(arg);")("hello");
            string result3 = runtime.DelegateHandler.Func<string>($"return {runtime.TypeName}.name;")();
            string result4 = runtime.DelegateHandler.Func<string>($"return {runtime.TypeName}.name2;")();
            Assert.Equal(6, result1);
            Assert.Equal(5, result0);
            Assert.Equal(5, result2);
            Assert.Equal("abc", result3);
            Assert.Equal("abc", result4);

        }

    }
}
