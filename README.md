# AccessFlow

AccessFlow is a framework for .Net with the purpose of simplifying
the implementation of parallel processes with side effects using threads.
It does so by providing an abstraction of accesses to shared resources
so that algorithms may be written in sequential form
while the operations are executed in parallel.
The coordination between the threads is implemented without locks or semaphores.

## Concept

Instruction sequences are an intuitive way of expressing behaviour for many developers.
When writing sequential algorithms
it is generally assumed that there is a global state
which is step by step modified according to the instructions.
Parallelization however seems to require a relaxation of this concept
and introduces additional mechanisms of controlling the temporal execution of operations.
Applying each these means correctly is not a trivial task and
error-prone as the eventual order of execution usually depends
on the scheduling of the operating system and other parameters of the execution environment.

An important approach to these difficulties is
the model of [Linearizability](https://en.wikipedia.org/wiki/Linearizability).
It assumes that the modification of an operation takes place as a transaction at
a moment between the invocation and the return of the result.
Accordingly the computation of a `Task` of the Task Parallel Library in .Net
is done between its instantiation and the time when its `IsCompleted` property is true.
That way also any exceptions occurred can be handled when the `Task` is being awaited.

Nevertheless, parallelization of a sequential algorithm is still complicated.
Given that a thread safe component `component` comprises
two [asynchronous methods](https://msdn.microsoft.com/en-us/library/mt674882.aspx) `DoA` and `DoB`
these methods may safely be started in parallel.

```
Task<Result> taskA = component.DoA();
Task<Result> taskB = component.DoB();

var resultA = await taskA;
var resultB = await taskB;
```

That way an execution order is not specified and
`DoA` and `DoB` may be scheduled to run simultaneosly or in any order in sequence.
If the two methods access a shared state and
the results of `DoA` and `DoB` depend on the order in which the methods are executed
there is some non-determinism involved
(which is aimed to be avoided within the scope of this example).
The order could be specified explicitly by awaiting `DoA` before starting `DoB`
but this would defeat the purpose of parallelism.
So some knowledge is required to properly schedule `DoA` and `DoB`.
AccessFlow aims to move this knowledge about temporal dependencies between method execution from
the user of such a component to the component implementation itself.

The general assumption of AccessFlow is that the sequential order of operations matters.
But this does not imply that no instructions my be executed in parallel.
In AccessFlow the state transition by an operation (the linearization point) happens
logically between the invocation of a method and the return of the `Task`—not the completion of the `Task`.
In the above example this means that `DoA` is always executed before `DoB` if
a change in the order of the operations would substantially change the results of `DoA` and `DoB` or the side effects of them.
If that is not the case, the operations may execute in arbitrary order.
The time required for the creation of the `Task`s should be short as
it only requires the scheduling of the operation in AccessFlow.

### Access scopes

A key concept of AccessFlow is to divide a “resource” into
parts not influencing each other.
Examples:

  - a directory may comprise files which are independent of each other,
  - a file consists of bytes at different locations or
  - a web API manages several separate entities.

The term used here for (sets of) these parts is *access scope*.

„Influence“ is seen here with regard to operations performed on the resources.
Given that two access scopes A and B do not influence each other,
then for any operation performed on access scope A
it does not substantially matter for the results of those operations
how many and which operations are be performed on part B.
For details see the documentation of `IAccessScopeLogic`.

Based on this division of the resource,
operations need to specify which access scope is at most required for them.
The smaller the scope is,
the fewer other executions are possibly blocked by the operation.

### Access contexts

Operations may dynamically invoke other operations.
To enable sequential consistency and a fast return of operations,
it is necessary to provide some means of reservation for later invocation of sub operations.

That’s when access contexts (`IAccessContext`) come into play.
It has an access scope assigned which is available to operations scheduled “within” the access context.
The access scope may be reduced later during execution but it may not be exended.
The motive of this is to avoid dead locks
as succeeding operations with a conflicting access scope could have already been started.

### Access contextual

Access contextuals are components backed by AccessFlow.
The idea is that instead of working with access contexts on the usage side of such a component,
“child” instances of the access contextuals can be created
representing the access contextual within an sub access context
The class `AccessContext` is a base class to aid the implementation of this pattern
but it is not required to make use of AccessFlow.

## Examples

See https://github.com/2i/AccessFlowExamples.

## Obtaining

This module is available as an NuGet package at [NuGet.org](https://www.nuget.org/packages/AccessFlow/).
It may be downloaded manually from [GitHub](https://github.com/2i/AccessFlow/releases/).

## Build process

The project has been developed using

- [Visual Studio 2015 Community](https://www.visualstudio.com/products/visual-studio-community-vs)
- [ReSharper 2016.1](https://www.jetbrains.com/resharper/) (not required for the build)
- [NuGet.exe](https://dist.nuget.org/index.html)

To create the NuGet and symbol package follow these steps:

1. Build the project in Visual Studio using the `Release` configuration.
2. Open command line in the Source/AccessFlow folder.
3. Run `mkdir build`
4. Run `path/to/nuget.exe pack AccessFlow.csproj -Prop Configuration=Release -OutputDirectory build -Symbols`

## Links

- GitHub project https://www.github.com/2i/AccessFlow/
- Examples project https://www.github.com/2i/AccessFlowExamples/
- NuGet package site https://www.nuget.org/packages/AccessFlow/
