# CSharpCodeAnalyzer

This small program analyzes C# code using Microsoft CodeAnalysis. It is another example of using AI to solve problems. The basic structure of the program was developed by AI, followed by customizing and adding more features manually. 

This program accepts three command-line arguments: the solution file name, the output file name, and the output format. 

```
CSharpCodeAnalyzer "C:\Projects\MyTestProject.sln" report json
```

The output file can be in JSON, XML, TXT, MD or YAML format. 

Specific features of the program can be enabled or disabled from the appsettings.json file. This file also defined the threshold to decide insufficient code comment, god class, big ball of mud, high coupling, long methods, etc. This program can also generate a Heatmap and DependencyGraph in a separate file, which can also be defined in the appsettings. Here is a default example of appsettings.json file
```
{
  "Features": {
    "EnableGodClassDetection": true,
    "EnableBigBallOfMudDetection": true,
    "EnableHighCouplingDetection": true,
    "EnableSingletonDetection": true,
    "EnableComplexityAnalysis": true,
    "EnableThreadSafetyAnalysis": true,
    "EnableDIComplianceCheck": true,
    "EnableFeatureEnvy": true,
    "EnableLongMethod": true,
    "EnablePureFunction": true,
    "EnableUnitTestCheck": false,
    "EnableUnusedMethods": false,
    "EnableDuplicatedMethods": false,
    "EnableDependencyGraph": true,
    "EnableInsufficientComments": false,
    "EnableComplexityHeatmap": true
  },
  "Configurations": {
    "InsufficientCommentsThreshold": 5,
    "GodClassMethodThreshold": 40,
    "GodClassFieldThreshold": 20,
    "BigBallOfMudThreshold": 100,
    "LongMethodThreshold": 100,
    "HighCouplingThreshold": 10,
    "FeatureEnvyThreshold": 5,
    "DependencyGraphFile": "DependencyGraph.dot",
    "HeatmapFile": "ComplexityHeatmap.csv"
  }
}
```

