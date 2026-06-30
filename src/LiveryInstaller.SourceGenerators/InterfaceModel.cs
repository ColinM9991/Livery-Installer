namespace LiveryInstaller.SourceGenerators;

internal record InterfaceModel(
    string Namespace,
    string Name,
    string FullyQualifiedName,
    EquatableArray<MethodModel> Methods);