using Natasha;
using Natasha.RuntimeToDynamic;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UTProject
{
    [Trait("匿名构造","")]
    public class TestAnonymousRTD
    {

        [Fact(DisplayName = "传值")]
        public void TestValue()
        {

            AnonymousRTD runtime = new AnonymousRTD();
            runtime.AddValue("小明");
            runtime.AddValue("小明1");
            runtime.AddValue("小明");
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");
            runtime.Complie();


            string result0 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}._anonymous_0;", runtime.Namespace)();
            string result1 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}._anonymous_1;", runtime.Namespace)();
            string result2 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}._anonymous_2;", runtime.Namespace)();
            string result3 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}.name;", runtime.Namespace)();
            string result4 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}.name2;", runtime.Namespace)();
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
            AnonymousRTD runtime = new AnonymousRTD();
            runtime.AddValue(func);
            runtime.AddValue(func1);
            runtime.AddValue(func);
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");
            runtime.Complie();


            int result0 = runtime.NDomainHandler.Func<string,int>($"return {runtime.TypeName}._anonymous_0(arg);", runtime.Namespace)("hello");
            int result1 = runtime.NDomainHandler.Func<string, int>($"return {runtime.TypeName}._anonymous_1(arg);", runtime.Namespace)("hello");
            int result2 = runtime.NDomainHandler.Func<string, int>($"return {runtime.TypeName}._anonymous_2(arg);", runtime.Namespace)("hello");
            string result3 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}.name;", runtime.Namespace)();
            string result4 = runtime.NDomainHandler.Func<string>($"return {runtime.TypeName}.name2;", runtime.Namespace)();
            Assert.Equal(6, result1);
            Assert.Equal(5, result0);
            Assert.Equal(5, result2);
            Assert.Equal("abc", result3);
            Assert.Equal("abc", result4);

        }

    }
}
