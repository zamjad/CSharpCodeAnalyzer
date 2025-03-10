class Program
{
    static async Task<int> Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: CSharpCodeAnalyzer <solution.sln> <output file name> <output format (json/xml/txt/md/yaml)>");
            return 1;
        }

        string solutionPath = args[0];
        string outputFileName = args[1];
        string outputFormat = args[2];

        await Analyzer.RunAnalysis(solutionPath, outputFileName, outputFormat);

        return 0;
    }
}
