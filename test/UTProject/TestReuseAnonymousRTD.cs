using Natasha.CSharp;
using NatashaUT;
using RuntimeToDynamic;
using System;
using Xunit;

namespace UTProject
{
    [Trait("复用构造","")]
    public class TestReuseAnonymousRTD : PrepareTest
    {

        [Fact(DisplayName = "传值")]
        public void TestValue()
        {
            var runtime = new ReuseAnonymousRTD();
            runtime.UseReadonlyFields();
            runtime.AddValue("小明");
            runtime.AddValue("小明");
            runtime.AddValue("小明1");
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");

            var nClass = NClass.RandomDomain()
                .Public()
                .Inheritance<Test>()
               .BodyAppendLine(runtime.FieldsScript)
               .BodyAppendLine(runtime.MethodScript);
            var type = nClass.GetType();
            Test test = (Test)Activator.CreateInstance(type);
            test.Age = 100;
            var action = runtime.GetInitMethod<Test>(nClass);
            action(test);
            string result = nClass.DelegateHandler.Func<Test,string>($"return (({type.Name})arg).{runtime.GetFieldName("小明")};")(test);
            string result3 = nClass.DelegateHandler.Func<Test, string>($"return (({type.Name})arg).name;")(test);
            Assert.Equal("小明", result);
            Assert.Equal("abc", result3);
            Assert.Equal(100, test.Age);
        }


        [Fact(DisplayName = "传委托")]
        public void TestDelegate()
        {

            Func<string, int> ageFunc = item => item.Length;
            var runtime = new ReuseAnonymousRTD();
            runtime.AddValue(ageFunc);
            runtime.AddValue(ageFunc);
            runtime.AddValue("name", "abc");
            runtime.AddValue("name2", "abc");

            var nClass = NClass.RandomDomain()
                .Public()
                .Inheritance<Test>()
              .BodyAppendLine(runtime.FieldsScript)
              .BodyAppendLine(runtime.MethodScript);
            var type = nClass.GetType();
            Test test = (Test)Activator.CreateInstance(type);
            test.Age = 100;
            var action = runtime.GetInitMethod<Test>(nClass);
            action(test);

            var result = nClass.DelegateHandler.Func<Test,string, int>($"return (({type.Name})arg1).{runtime.GetFieldName(ageFunc)}(arg2);")(test,"Hello");
            string result3 = nClass.DelegateHandler.Func<Test,string>($"return (({type.Name})arg).name;")(test);
            Assert.Equal("abc", result3);
            Assert.Equal(5, result);
            Assert.Equal(100, test.Age);
        }

    }
}
