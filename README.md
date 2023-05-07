# ApiBenchmark
The Web application can measure the performance of three popular libraries for API. For API, I used https://www.freeforexapi.com/. I implemented three HTTP clients: HttpClient, Refit, and RestSharp. All clients send identical data and receive the same response. For measuring, I used the BenchmarkDotNet library. This application's main idea is to launch tests without the console and get reports.

I implemented launching tests from Web UI. First, you can request forex API with different clients, catch sight of results and execute timing. The following functionality, You can choose the host, the runtimes, and the client and run a test. The Web UI launches the bash script under .NET, and further, it launches tests. The main peculiarity of this test is that it has optionality. You can choose different frameworks. Unfortunately, I couldn't make the tests for the NET8. There are some issues with packages and Docker. Next feature, You can see test reports, choosing client or all clients. And finally, I packed it to Docker.

## Usings:
First of all, You should clone the project. Then, if you want to use VS or Rider, you can run MVC and tests. Finally, if you don't want to use IDE, you can compose Docker and open it through your web browser.


## Measures:

### **Legends:**

**Mean:** Arithmetic mean of all measurements

**Error:** Half of 99.9% confidence interval

**StdDev:** Standard deviation of all measurements

**Gen 0:** GC Generation 0 collects per 1000 operations

**Allocated:** Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)

**1 Us:** 1 Microsecond (0.000001 sec)



### HttpClient
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





### Refit
``` ini

BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.3.1 (a) (22E772610a) [Darwin 22.4.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=8.0.100-preview.3.23178.7
  [Host]   : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2
  ShortRun : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                       Method |  Runtime |      Mean |    Error |   StdDev |   Gen0 |   Gen1 | Allocated |
|----------------------------- |--------- |----------:|---------:|---------:|-------:|-------:|----------:|
| BenchMark_RefitClientHandler | .NET 6.0 |  94.90 μs | 42.84 μs | 2.348 μs | 1.9531 | 0.4883 |  11.73 KB |
| BenchMark_RefitClientHandler | .NET 7.0 | 116.57 μs | 31.83 μs | 1.745 μs | 4.3945 | 1.4648 |  22.45 KB |

### RestSharp
``` ini

BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.3.1 (a) (22E772610a) [Darwin 22.4.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=8.0.100-preview.3.23178.7
  [Host]   : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2
  ShortRun : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                           Method |  Runtime |     Mean |    Error |    StdDev |   Gen0 | Allocated |
|--------------------------------- |--------- |---------:|---------:|----------:|-------:|----------:|
| BenchMark_RestsharpClientHandler | .NET 6.0 | 1.177 ms | 1.035 ms | 0.0567 ms | 7.8125 |  53.52 KB |
| BenchMark_RestsharpClientHandler | .NET 7.0 | 1.556 ms | 5.196 ms | 0.2848 ms | 7.8125 |  46.03 KB |
