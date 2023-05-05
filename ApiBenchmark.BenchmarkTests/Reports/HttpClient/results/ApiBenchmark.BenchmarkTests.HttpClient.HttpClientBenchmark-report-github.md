``` ini

BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.3.1 (a) (22E772610a) [Darwin 22.4.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=8.0.100-preview.3.23178.7
  [Host]   : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2
  ShortRun : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                      Method |  Runtime |     Mean |    Error |   StdDev |   Gen0 |   Gen1 | Allocated |
|---------------------------- |--------- |---------:|---------:|---------:|-------:|-------:|----------:|
| BenchMark_HttpClientHandler | .NET 6.0 | 39.62 μs | 12.92 μs | 0.708 μs | 0.7935 | 0.4272 |   4.89 KB |
| BenchMark_HttpClientHandler | .NET 7.0 | 50.59 μs | 62.91 μs | 3.448 μs | 1.5869 | 0.4883 |   8.78 KB |
