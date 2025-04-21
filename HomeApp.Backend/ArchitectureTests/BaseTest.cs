using System.Reflection;
using System.Reflection.Metadata;
using Domain;
using Infrastructure;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(AssemblyReference).Assembly;
    protected static readonly Assembly DomainAssembly = typeof(DomainReference).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(InfrastructureReference).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
