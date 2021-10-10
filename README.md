# Serilog.Sinks.RethinkDB [![build](https://github.com/erkanozmenli/serilog-sinks-rethinkdb/actions/workflows/main.yml/badge.svg?branch=master)](https://github.com/erkanozmenli/serilog-sinks-rethinkdb/actions/workflows/main.yml) [![NuGet version (Serilog.Sinks.RethinkDB2)](https://img.shields.io/nuget/v/Serilog.Sinks.RethinkDB2.svg?style=flat-square)](https://www.nuget.org/packages/Serilog.Sinks.RethinkDB2/)

A Serilog sink that writes events to RethinkDB.  

#### Topics
* [Quick Start](#quick-start)
* [Sink Configuration](#sink-configuration)
* [.WriteTo or AuditTo?](#writeto-or-auditto)
* [Additional Features](#additional-features)

### Quick Start
Basic sink configuration can be initialzed and used like below.  
  
```csharp
 Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RethinkDB()
                .CreateLogger();
  
 Log.Information("Shoryuken");
```
  
### Sink Configuration
  
I divided sink configuration in two sections which are .NET Framework and .NET Core.
  
* #### .NET Framework
  
  First add RethinkDB configuration section in your web or app config.  
```xml
<configuration>
	<configSections>
		<section name="RethinkDBSettingsSection" type="Serilog.Configuration.RethinkDBConfigurationSection, Serilog.Sinks.RethinkDB" />
	</configSections>
    
	<RethinkDBSettingsSection>
		<Host Value="192.168.1.75" />
		<Port Value="28015" />
		<Database Value="logging01" />
		<Table Value="table01" />
		<Index Value="idx" />
		<BatchSizeLimit Value="1" />
		<BatchPeriod Value="2" />
		<EagerlyEmitFirstEvent Value="true" />
		<!--<QueueLimit Value="10000" />-->
		<UseProps Value="false" />
	</RethinkDBSettingsSection>
</configuration>
```
  
Add required using references.
```csharp
using Serilog;
using Serilog.Sinks.RethinkDB;
```
  
Then you can initialize your RethinkDB sink to use it in your project.
```csharp
 Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RethinkDB()
                .CreateLogger();
  
 Log.Information("Log from .NET Framework");
```
  
* #### .NET Core
  
First add RethinkDB configuration section in your appsettings.json file. If you don't have an appsettings.json file in your project, create a new one and change it's properties like below.
  
**Build Action**: Content  
**Copy to Output Directory**: Copy if newer
  
```json
{
  "Serilog": {
    "SinkOptions": {
      "Host": "192.168.1.75",
      "Port": 28015,
      "Database": "logging02",
      "Table": "table02",
      "Index": "idx",
      "UseProps": false
    }
  }
}  
```
  
Add required using references.
```csharp
using Serilog;
using Serilog.Sinks.RethinkDB;
```
  
Then you can initialize your RethinkDB sink to use it in your project.
```csharp
 Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RethinkDB()
                .CreateLogger();
  
 Log.Information("Log from .NET Core");
```
  
In both .NET Framework and .NET Core, you can override any sink options by passing optional parameters to RethinkDB method like below.  
  
```csharp
 Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RethinkDB(database:"anotherDB", table: "anotherTBL")
                .CreateLogger();
  
 Log.Information("Log from .NET Core");
```
  
### .WriteTo or AuditTo?
RethinkDB sink has both .WriteTo and .AuditTo implemmentations. In your production environment I recommend you to use .WriteTo because it does not only work asynchronously but also have batching functionality. The other options is .AuditTo but it works synchronously and does not have bacthing functionality.
  
### Additional Features
Personally I do not use Serilog's Props feature (At least for now). Instead I implemented two additional properties which are **ContextData** amd **ContextName**.
  
**ContextName** property holds the name of the object that I associated with Serilog's ForContext feature.  
**ContextObject** property holds the object's itself.  

To be able to use it more practically I also added an extension method to **object** called **.Log**.  
  
```csharp
using Serilog;
using Serilog.Sinks.RethinkDB;

namespace net461ConsoleApp
{
    class Company 
    {
        public int CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .AuditTo.RethinkDB()
                .Enrich.With<SequentialIdEnricher>()
                .CreateLogger();

            var company = new Company { CustomerName = "XCompany", CustomerID = 112345, CustomerCode = "0022222" };
            var log = Log.ForContext(company.Log());

            for (int i = 0; i < 2; i++)
            {
                log.Information("Personel {0}", i);
            }
        }
    }
}
```  
  
The above code gives the following json output. Pay attention **.Log()** extention method passed in Log.ForContext. You can pass any object like this to save time.
  
```json
{
    "ContextData": {
        "CustomerCode": "0022222",
        "CustomerID": 112345,
        "CustomerName": "XCompany"
    },
    "ContextName": "Company",
    "Exception": null,
    "Level": 2,
    "Message": "Personel 0",
    "MessageTemplate": "Personel {0}",
    "Props": null,
    "SequentialId": 1,
    "Timestamp": Wed Oct 06 2021 20: 00: 11 GMT+03: 00,
    "id": "dcf1b8de-0494-4569-9da8-048bfd915bc1"
},
{
    "ContextData": {
        "CustomerCode": "0022222",
        "CustomerID": 112345,
        "CustomerName": "XCompany"
    },
    "ContextName": "Company",
    "Exception": null,
    "Level": 2,
    "Message": "Personel 1",
    "MessageTemplate": "Personel {0}",
    "Props": null,
    "SequentialId": 2,
    "Timestamp": Wed Oct 06 2021 20: 00: 11 GMT+03: 00,
    "id": "6d5b90ac-9f12-42fd-9a65-42a57f5cd16f"
}
```
  
Another additional feature I added is **SequentialId**.  
When you insert many data at the same time or in a loop, they might have the same milliseconds in Timestamp field. This might causes non-sequential output when you query your collection even with RQL **.orderBy** function. For this reason, I added **SequentialIdEnricher** to enrich the sink. Use this enricher if you want to add an auto-increment field to your logs. The sink already automaticallly creates a compound index (idx) with the combination of SequentialId and Timestamp fields to get right order.
  
In the last C# sample you may have noticed that we also used **SequentialIdEnricher>()**. As a result in json output we had auto-incremental values at SequentialId field. But the important thing to keep in mind is that unlike auto-increment fields in RDMS, <ins>SequentialId is reset when your session ends</ins>.
  
