# RuntimeToDynamic
将运行时数据存储在动态生成的静态代码中

该库旨在将运行时数据转储到动态构建的静态类中，以供动态代码使用运行时数据，比较常用的场景是在封装类库写配置相关的API的时候，可以直接使用本库进行运行时数据转储方面的构建。

<br/>  
<br/>

### AnonymousRTD 类


该类属于匿名构建，动态生成的字段将会以 “前缀”+“自增号” 形式被动态定义。  
匿名字段前缀默认为 `_anonymous_`，因此生成的代码为：`pubilc (static/readonly) string _anonymous_1;`  

#### 设置构造类型
```C#
var arst = new AnonymousRTD();
arst.UseReadonlyFields(); //生成的字段是 public readonly {type} _anonyous_{count};
arst.UseStaticFields(); //生成的字段是 public static {type} _anonyous_{count};
arst.UseStaticReadonlyFields(); //生成的字段是 public static readonly {type} _anonyous_{count};
```

#### 获取字段和方法脚本
```C#
//通过以下代码获取生成的脚本
arst.FieldsScript;
arst.MethodScript;
```

#### 初始化赋值
```C#
//初始化赋值
arst.GetInitMethod(nclass);
arst.GetInitMethod<T>(nclass);
```

生成的脚本可以添加到 Natasha 中进行编译使用。

<br/>  
<br/>


### ReuseAnonymousRTD 类

同样具有上述的静态构造法
该类属于对象复用构建，当一个值对应多个名字时，将只合并到同一个字段(以最开始赋值的名字为准)。  
该类继承自 AnonymousRTD 类，因此允许无脑添加值，之后通过 `GetFieldName(obj)` 方法获取动态的字段名。


<br/>  
<br/>


### 使用方法：

 <br/>  
 
 - 引入 动态构建库： NMS.RuntimeToDynamic

 - 初始化 Natasha： NatashaInitializer.InitializeAndPreheating();  

 - 敲代码  
 
<br/>  

```C#

//随你喜欢添加什么类型都行，最后会统一强转到强类型字段
int age = 10;
string name = "xiaoming";
Func<string, int> func = item => item.Length;
Student stu = new Student();

//使用随即域进行动态构造
var runtime = new ReuseAnonymousRTD();
var runtime = new AnonymousRTD();


//这里的 age 是将是同一个对象;
//如果使用了 ReuseAnonymousRTD 将只生成 _anonymous_1 一个字段；
//如果使用了 AnonymousRTD 将生成 _anonymous_1,_anonymous_2,_anonymous_3 三个字段。
runtime.AddValue(age);
runtime.AddValue(age);
runtime.AddValue(age);


//由于 abc 为同一个对象，且使用了匿名映射，
//如果使用了 ReuseAnonymousRTD 将只生成 name 一个字段；
//如果使用了 AnonymousRTD 将生成 name,name2 两个字段。
runtime.AddValue("name", "abc");
runtime.AddValue("name2", "abc");


//当您构造一段字符串代码时，就可这样使用：

例1：
runtime.AddValue("name", "abc");
runtime.AddValue("name2", "abc");
$@"public string GetName(){{
      return {runtime.TypeName}.{runtime.GetFieldScript("abc")};
}}"


例2：
Func<string, int> func = item => item.Length;
runtime.AddValue("test",func):
$@"public int GetName(string name){{
      return test(name);
}}"
            
```

<br/>  
<br/>

### 封装

如果您想在此基础上继续封装，可以使用以下方法：

```C#
//在 ReuseAnonymousRTD 功能的基础上加自己的功能
public class MyTemplate : ReuseAnonymousRTD<MyTemplate>


// 继承 BaseRTD<T> 并须使用 where T : BaseRTD<T>, new() 约束，将会默认得到上述 Random / Default / Create 静态构造域的 API 支持
// 您也可以继承 ReuseAnonymousRTD<T> 或  AnonymousRTD<T>  where T : BaseRTD<T>, new() 
public class MyDevelopTemplate<T> : (BaseRTD<T> / AnonymousRTD<T> / ReuseAnonymousRTD<T>) where T : BaseRTD<T>, new(){}

public class MyTest : MyDevelopTemplate<MyTest> { .... }
var test = new MyTest();
```

自带的模板中的方法均采用虚方法，以便开发者重载。

<br/>  
<br/>

### 关联动态委托

当您使用完上述代码之后，若想继续构造动态的方法 / 类 /  枚举 / 接口 / 结构体等工作可以如下：

```C# 

var runtime = new ReuseAnonymousRTD();
Func<string, int> func = item => item.Length;
runtime.AddValue("test",func):

var func = runtime.DelegateHandler.Func<string,int>(

@"if( arg = \"小明\" )
{ 
      return 100; 
}
else 
{
      return test(arg);
}"

);

int age = func("小明");
//age = 100;
```
