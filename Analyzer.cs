using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
public class Analyzer
{
    private static IConfiguration _config;
    private static Dictionary<string, HashSet<string>> classDependencies = new Dictionary<string, HashSet<string>>();
    private static Dictionary<string, int> complexityScores = new Dictionary<string, int>();

    public static async Task RunAnalysis(string solutionPath, string fileName, string outputFormat)
    {
        LoadConfiguration();

        var workspace = MSBuildWorkspace.Create();
        var solution = await workspace.OpenSolutionAsync(solutionPath);
        var report = new Dictionary<string, object>();

        foreach (var project in solution.Projects)
        {
            var projectReport = new List<object>();
            foreach (var document in project.Documents)
            {
                var syntaxTree = await document.GetSyntaxTreeAsync();
                var semanticModel = await document.GetSemanticModelAsync();
                if (syntaxTree == null || semanticModel == null) 
                    continue;

                foreach (var classNode in syntaxTree.GetRoot().DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax>())
                {
                    Console.WriteLine($"- Analyzing Class: {classNode.Identifier.Text}");

                    var classReport = new Dictionary<string, object>();
                    string className = classNode.Identifier.Text;
                    classReport["ClassName"] = className;

                    var dependencies = new HashSet<string>();

                    // Extract dependencies using semantic analysis
                    foreach (var identifier in classNode.DescendantNodes().OfType<IdentifierNameSyntax>())
                    {
                        var symbol = semanticModel.GetSymbolInfo(identifier).Symbol;
                        if (symbol != null && symbol.Kind == SymbolKind.NamedType)
                        {
                            string dependencyName = symbol.Name;
                            if (dependencyName != className) // Avoid self-referencing dependencies
                            {
                                dependencies.Add(dependencyName);
                            }
                        }
                    }

                    // Store dependencies in classDependencies dictionary
                    if (_config.GetValue<bool>("Features:EnableDependencyGraph"))
                    {
                        if (!classDependencies.ContainsKey(className))
                            classDependencies[className] = new HashSet<string>();

                        foreach (var dep in dependencies)
                        {
                            classDependencies[className].Add(dep);
                        }
                    }

                    if (_config.GetValue<bool>("Features:EnableComplexityAnalysis"))
                        classReport["ComplexityScore"] = ComplexityAnalyzer.CalculateComplexity(classNode);

                    if (_config.GetValue<bool>("Features:EnableInsufficientComments"))
                    {
                        int insufficientCommentsThreshold = _config.GetValue<int>("Configurations:InsufficientCommentsThreshold");

                        if (CodeCommentQualityAnalyzer.HasInsufficientComments(classNode, insufficientCommentsThreshold))
                        {
                            classReport["InsufficientComments"] = true;
                        }
                    }

                    if (_config.GetValue<bool>("Features:EnableGodClassDetection"))
                    {
                        int godClassMethodThreshold = _config.GetValue<int>("Configurations:GodClassMethodThreshold");
                        int godClassFieldThreshold = _config.GetValue<int>("Configurations:GodClassFieldThreshold");

                        if (GodClassDetector.IsGodClass(classNode, godClassMethodThreshold, godClassFieldThreshold))
                        {
                            classReport["GodClass"] = true;
                        }
                    }

                    if (_config.GetValue<bool>("Features:EnableBigBallOfMudDetection"))
                    {
                        int bigBallOfMudThreshold = _config.GetValue<int>("Configurations:BigBallOfMudThreshold");

                        if (BigBallOfMudDetector.IsBigBallOfMud(classNode, bigBallOfMudThreshold))
                        {
                            classReport["BigBallOfMud"] = true;
                        }
                    }

                    if (_config.GetValue<bool>("Features:EnableHighCouplingDetection"))
                    {
                        int highCouplingThreshold = _config.GetValue<int>("Configurations:HighCouplingThreshold");

                        if (HighCouplingDetector.HasHighCoupling(classNode, highCouplingThreshold))
                        {
                            classReport["HighCoupling"] = true;
                        }
                    }

                    if (_config.GetValue<bool>("Features:EnableSingletonDetection") && SingletonStaticDetector.UsesExcessiveSingletons(classNode))
                        classReport["SingletonOveruse"] = true;

                    if (_config.GetValue<bool>("Features:EnableThreadSafetyAnalysis") && ThreadSafetyAnalyzer.HasSharedState(classNode))
                        classReport["ThreadSafety"] = true;

                    if (_config.GetValue<bool>("Features:EnableDIComplianceCheck") && DIComplianceChecker.UsesManualInstantiation(classNode))
                        classReport["DIViolation"] = true;

                    // Detect Unused Methods
                    if (_config.GetValue<bool>("Features:EnableUnusedMethods"))
                    {
                        var unusedMethods = DeadCodeDetector.FindUnusedMethods(null, syntaxTree.GetRoot());
                        classReport["UnusedMethods"] = unusedMethods;
                    }

                    // Detect Duplicated Methods
                    if (_config.GetValue<bool>("Features:EnableDuplicatedMethods"))
                    {
                        var allMethods = classNode.Members.OfType<MethodDeclarationSyntax>().ToList();
                        var duplicatedMethods = DuplicatedCodeDetector.DetectDuplicatedMethods(allMethods);
                        classReport["DuplicatedMethods"] = duplicatedMethods;
                    }

                    // Store complexity for heatmap
                    if (_config.GetValue<bool>("Features:EnableComplexityHeatmap"))
                        complexityScores[classNode.Identifier.Text] = ComplexityAnalyzer.CalculateComplexity(classNode); ;

                    // Process methods inside each class
                    foreach (var methodNode in classNode.Members.OfType<MethodDeclarationSyntax>())
                    {
                        var methodReport = new Dictionary<string, object>();
                        methodReport["MethodName"] = methodNode.Identifier.Text;

                        if (_config.GetValue<bool>("Features:EnableFeatureEnvy"))
                        {
                            int featureEnvyThreshold = _config.GetValue<int>("Configurations:FeatureEnvyThreshold");

                            if (FeatureEnvyDetector.IsFeatureEnvy(methodNode, featureEnvyThreshold))
                            {
                                methodReport["FeatureEnvy"] = true;
                            }
                        }

                        if (_config.GetValue<bool>("Features:EnableLongMethod"))
                        {
                            int LongMethodthreshold = _config.GetValue<int>("Configurations:LongMethodThreshold");

                            if (LongMethodDetector.IsLongMethod(methodNode, LongMethodthreshold))
                            {
                                methodReport["LongMethod"] = true;
                            }
                        }

                        if (_config.GetValue<bool>("Features:EnablePureFunction") && FunctionPurityDetector.IsPureFunction(methodNode))
                            methodReport["PureFunction"] = true;

                        if (_config.GetValue<bool>("Features:EnableUnitTestCheck") && UnitTestCoverageAnalyzer.HasUnitTest(methodNode))
                            methodReport["HasUnitTest"] = true;

                        // Add method-level findings to class report
                        if (methodReport.Count > 1)  // Ensure at least one check was true
                            classReport[methodNode.Identifier.Text] = methodReport;
                    }

                    projectReport.Add(classReport);
                }
            }

            report[project.Name] = projectReport;
        }

        SaveReport(report, fileName, outputFormat);

        if (_config.GetValue<bool>("Features:EnableDependencyGraph"))
        {
            var dependencyGraphFileName = _config.GetValue<string>("Configurations:DependencyGraphFile");

            // default file name if not present
            if (string.IsNullOrEmpty(dependencyGraphFileName))
            {
                dependencyGraphFileName = "DependencyGraph.dot";
            }

            var expandedDependencies = ExpandDependencies(classDependencies);
            var readableGraphFileName = $"Readable{dependencyGraphFileName}";

            DependencyGraphGenerator.GenerateGraph(dependencyGraphFileName, expandedDependencies);
            DependencyGraphGenerator.GenerateReadableGraph(readableGraphFileName, expandedDependencies);
        }

        // Call GenerateHeatmap
        if (_config.GetValue<bool>("Features:EnableComplexityHeatmap"))
        {
            var heatMapFileName = _config.GetValue<string>("Configurations:HeatmapFile");

            // default file name if not present
            if (string.IsNullOrEmpty(heatMapFileName))
            {
                heatMapFileName = "ComplexityHeatmap.csv";
            }

            CodeComplexityHeatmap.GenerateHeatmap(heatMapFileName, complexityScores);
        }
    }

    private static Dictionary<string, HashSet<string>> ExpandDependencies(Dictionary<string, HashSet<string>> dependencies)
    {
        var expandedGraph = new Dictionary<string, HashSet<string>>();

        foreach (var entry in dependencies)
        {
            var fullDependencies = new HashSet<string>();
            ResolveDependencies(entry.Key, dependencies, fullDependencies);
            expandedGraph[entry.Key] = fullDependencies;
        }

        return expandedGraph;
    }

    private static void ResolveDependencies(string className, Dictionary<string, HashSet<string>> dependencies, HashSet<string> resolved)
    {
        if (!dependencies.ContainsKey(className)) return;

        foreach (var dep in dependencies[className])
        {
            if (!resolved.Contains(dep))
            {
                resolved.Add(dep);
                ResolveDependencies(dep, dependencies, resolved); // Recursively resolve deeper dependencies
            }
        }
    }

    private static void LoadConfiguration()
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        _config = configBuilder.Build();
    }

    private static void SaveReport(Dictionary<string, object> report, string fileName, string format)
    {
        string outputFile = $"{fileName}.{format}";

        switch (format.ToLower())
        {
            case "json":
                File.WriteAllText(outputFile, JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
                break;

            case "xml":
                var xmlReport = new XElement("AnalysisReport",
                    report.Select(project => new XElement("Project",
                        new XAttribute("Name", project.Key),
                        ((List<object>)project.Value).Select(cls =>
                            new XElement("Class",
                                ((Dictionary<string, object>)cls).Select(kv =>
                                    new XElement(kv.Key, kv.Value.ToString())))))));
                xmlReport.Save(outputFile);
                break;

            case "txt":
                var textReport = new StringBuilder();
                foreach (var project in report)
                {
                    textReport.AppendLine($"Project: {project.Key}");
                    foreach (var cls in (List<object>)project.Value)
                    {
                        var classDict = (Dictionary<string, object>)cls;
                        textReport.AppendLine($"  Class: {classDict["ClassName"]}");
                        foreach (var kv in classDict.Where(kv => kv.Key != "ClassName"))
                        {
                            textReport.AppendLine($"    {kv.Key}: {kv.Value}");
                        }
                    }
                }
                File.WriteAllText(outputFile, textReport.ToString());
                break;

            case "md":
                var markdownReport = new StringBuilder();
                markdownReport.AppendLine("# Code Analysis Report");
                foreach (var project in report)
                {
                    markdownReport.AppendLine($"## Project: {project.Key}");
                    foreach (var cls in (List<object>)project.Value)
                    {
                        var classDict = (Dictionary<string, object>)cls;
                        markdownReport.AppendLine($"### Class: {classDict["ClassName"]}");
                        foreach (var kv in classDict.Where(kv => kv.Key != "ClassName"))
                        {
                            markdownReport.AppendLine($"- **{kv.Key}**: {kv.Value}");
                        }
                    }
                }
                File.WriteAllText(outputFile, markdownReport.ToString());
                break;

            case "yaml":
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                string yamlOutput = serializer.Serialize(report);
                File.WriteAllText(outputFile, yamlOutput);
                break;

            default:
                Console.WriteLine("Unsupported format. Please use JSON, XML, TXT, MD, or YAML.");
                return;
        }

        Console.WriteLine($"Report saved as {outputFile}");
    }
}
