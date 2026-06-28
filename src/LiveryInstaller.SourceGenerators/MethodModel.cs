namespace LiveryInstaller.SourceGenerators;

internal record MethodModel(
    string ReturnType,
    ReturnKind ReturnKind,
    string Name,
    EquatableArray<ParameterModel> Parameters);