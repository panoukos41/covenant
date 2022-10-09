namespace Covenant.Analysis;

public sealed class AnalysisContext : DiagnosticContext
{
    private readonly Graph<BomComponent> _graph;
    private readonly Graph<BomComponent> _localGraph;

    public IReadOnlyGraph<BomComponent> Graph => _graph;
    public IReadOnlyGraph<BomComponent> Delta => _localGraph;
    public DirectoryPath Root { get; }
    public ICommandLineResolver Cli { get; }

    public AnalysisContext(DirectoryPath root, Graph<BomComponent> graph, ICommandLineResolver resolver)
    {
        _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        _localGraph = new Graph<BomComponent>(BomComponentComparer.Shared);

        Root = root ?? throw new ArgumentNullException(nameof(root));
        Cli = resolver ?? throw new ArgumentNullException(nameof(resolver));
    }

    public BomComponent AddComponent(BomComponent component)
    {
        // Already known?
        if (_graph.Exist(component))
        {
            return component;
        }

        // Add it to the local graph.
        _localGraph.Add(component);
        return _graph.Add(component);
    }

    public void Connect(BomComponent start, BomComponent end, string? metadata = null)
    {
        _localGraph.Connect(start, end, metadata);
    }
}