public static class CodeComplexityHeatmap
{
    public static void GenerateHeatmap(string outputPath, Dictionary<string, int> complexities)
    {
        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            writer.WriteLine("File,Complexity");
            foreach (var entry in complexities)
            {
                writer.WriteLine($"{entry.Key},{entry.Value}");
            }
        }
        Console.WriteLine($"Complexity heatmap saved to {outputPath}");
    }
}
