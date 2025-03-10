public static class DependencyGraphGenerator
{
    public static void GenerateGraph(string outputPath, Dictionary<string, HashSet<string>> dependencies)
    {
        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            writer.WriteLine("digraph G {");
            foreach (var entry in dependencies)
            {
                foreach (var dep in entry.Value)
                {
                    writer.WriteLine($"    \"{entry.Key}\" -> \"{dep}\";");
                }
            }
            writer.WriteLine("}");
        }
        Console.WriteLine($"Dependency graph saved to {outputPath}");
    }

    public static void GenerateReadableGraph(string outputPath, Dictionary<string, HashSet<string>> dependencies)
    {
        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            writer.WriteLine("Class Dependencies:");
            foreach (var entry in dependencies)
            {
                writer.WriteLine($"{entry.Key} depends on: {string.Join(", ", entry.Value)}");
            }
        }

        Console.WriteLine($"Readable dependency graph saved to {outputPath}");
    }
}
