using Natasha.CSharp;
using NatashaUT;
using RuntimeToDynamic;
using System;
using Xunit;

namespace UTProject
{
    [Trait("匿名构造","")]
    public class TestAnonymousRTD:PrepareTest
    {

        [Fact(DisplayName = "传值")]
        public void TestValue()
        {

            var runtime = new AnonymousRTD();
            runtime.UseStaticReadonlyFields();
            runtime.AddValue("小明");
            runtime.AddValue("小明1");
            runtime.AddValue("小明");
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");

            var nClass = NClass.RandomDomain()
                .Public()
                .BodyAppendLine(runtime.FieldsScript)
                .BodyAppendLine(runtime.MethodScript);
            var type = nClass.GetType();
            var action = runtime.GetInitMethod(nClass);
            action();
            string result0 = nClass.DelegateHandler.Func<string>($"return {type.Name}._anonymous_1;")();
            string result1 = nClass.DelegateHandler.Func<string>($"return {type.Name}._anonymous_2;")();
            string result2 = nClass.DelegateHandler.Func<string>($"return {type.Name}._anonymous_3;")();
            string result3 = nClass.DelegateHandler.Func<string>($"return {type.Name}.name;")();
            string result4 = nClass.DelegateHandler.Func<string>($"return {type.Name}.name2;")();
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
            var runtime = new AnonymousRTD();
            runtime.UseStaticFields();
            var key = runtime.AddValue(func);
            runtime.AddValue(func1);
            runtime.AddValue(func);
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");

            var nClass = NClass.RandomDomain()
                .Public()
                .BodyAppendLine(runtime.FieldsScript)
                .BodyAppendLine(runtime.MethodScript);
            var type = nClass.GetType();

            var action = runtime.GetInitMethod(nClass);
            action();


            int result0 = nClass.DelegateHandler.Func<string,int>($"return {type.Name}.{key}(arg);")("hello");
            int result1 = nClass.DelegateHandler.Func<string, int>($"return {type.Name}._anonymous_2(arg);")("hello");
            int result2 = nClass.DelegateHandler.Func<string, int>($"return {type.Name}._anonymous_3(arg);")("hello");
            string result3 = nClass.DelegateHandler.Func<string>($"return {type.Name}.name;")();
            string result4 = nClass.DelegateHandler.Func<string>($"return {type.Name}.name2;")();
            Assert.Equal(6, result1);
            Assert.Equal(5, result0);
            Assert.Equal(5, result2);
            Assert.Equal("abc", result3);
            Assert.Equal("abc", result4);

        }

    }
}
