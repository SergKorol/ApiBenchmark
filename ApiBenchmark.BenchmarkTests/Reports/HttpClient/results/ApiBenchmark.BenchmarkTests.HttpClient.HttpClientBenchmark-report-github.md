``` ini

BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.3.1 (22E261) [Darwin 22.4.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=8.0.100-preview.2.23157.25
  [Host]   : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2
  ShortRun : .NET 6.0.14 (6.0.1423.7309), X64 RyuJIT AVX2

Job=ShortRun  Runtime=.NET 6.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                      Method |     Mean |    Error |   StdDev |   Gen0 |   Gen1 | Allocated |
|---------------------------- |---------:|---------:|---------:|-------:|-------:|----------:|
| BenchMark_HttpClientHandler | 44.24 μs | 3.158 μs | 0.173 μs | 0.8545 | 0.3662 |   5.03 KB |
