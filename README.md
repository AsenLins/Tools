# YaHooExRateTool
#### 用于调用雅虎汇率Api工具。

#### 实现功能：
1. 实现了对string,deciaml,int的金额进行指定币种与币种之间的汇率转换。
2. 可查询币种与币种之前的汇率信息,查询结果分为Xml字符串与对象。

---

##如何使用
**添加引用**
```
using YaHooRate;
```


**把金额转换为指定币种汇率之间的金额**
```
 /*支持转换类型
  * string
  * int
  * deciaml
 */ 
 string _SMoeny = "10".ConverterRate("CNY","HKD"); 
 decimal _DMoeny = 10.ConverterRate("HKD","CNY");
 int _IMoeny = 10.ConverterRate("USD","HKD");
```
**查询指定币种之间的汇率**
```
  /*查询汇率（返回XML字符串）*/
  string Xml = Rate.QueryRateToXml("CNY","HKD");
  
  /*查询汇率（返回Obj对象）*/
  RateObj _RateObj=Rate.QueryRateToObj("CNY","HKD");
```


