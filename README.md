# EasyLog

轻量级 C# 日志记录工具

## 1. 安装

```bash
PM> Install-Package ETool.EasyLog
```

## 2. 使用

> 引入命名空间

```c#
using EasyLog;
```

> 简单使用使用

```c#
LogUtil.Success("这是成功的日志【绿色】");
LogUtil.Success("这是成功的日志【绿色】");
LogUtil.Warn("这是警告的日志【黄色】");
LogUtil.Warn("这是警告的日志【黄色】");
LogUtil.Error("这是错误的日志【红色】");
LogUtil.Error("这是错误的日志【红色】");
```

> 自定义日志委托

```c#
LogUtil.LogHandler logHandler = (text, logLevel) =>
{
    if (logLevel == LogUtil.LogLevel.Success)
    {
        Console.WriteLine("这是成功的日志：" + text);
    }
    else if (logLevel == LogUtil.LogLevel.Warn)
    {
        Console.WriteLine("这是警告的日志：" + text);
    }
    else
    {
        Console.WriteLine("这是错误的日志：" + text);
    }
};

LogUtil.SetCurrentLogHandler(logHandler);

LogUtil.Success("这是成功的日志【绿色】");
LogUtil.Success("这是成功的日志【绿色】");
LogUtil.Warn("这是警告的日志【黄色】");
LogUtil.Warn("这是警告的日志【黄色】");
LogUtil.Error("这是错误的日志【红色】");
LogUtil.Error("这是错误的日志【红色】");
```

