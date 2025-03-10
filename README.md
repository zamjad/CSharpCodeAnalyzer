# CSharpCodeAnalyzer

This small progarm is doing the C# code analysis using Microsoft.CodeAnalysis. It is another example of using AI help to solve the problem. The basic structure of the program developed by AI, followed by customizing and adding more features manually. 

This program accepts three command line argument. The solution file name, the output file name and output format. 

```
CSharpCodeAnalyzer "C:\Projects\MyTestProject.sln" report json
```

The output file can be in JSON, XML, TXT, MD or YAML format. 

Specific features of the program can be enabled or disable from the appsettings.json file. This file also defined the threashold to decide insufficient code comment, god class, big ball of mud, high coupling, long methods etc. This program can also generate Heatmap and DependencyGraph in a seperate file, which can also be define in the appsettings. Here is a default example of appsettings.json file
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

