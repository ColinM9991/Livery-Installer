namespace LiveryInstaller.SourceGenerators;

enum ReturnKind
{
    Void,
    Task,
    ValueTask,
    GenericTask,
    GenericValueTask,
    NonVoid
}