using System;
using BenchmarkDotNet.Attributes;
using static OperationResult.Helpers;

namespace OperationResult.Benchmarks
{
    public class Benchmark
    {
        [Params(50, 90, 99)]
        public int SuccessRate { get; set; }

        private int GetException(int arg)
        {
            if (arg < SuccessRate)
            {
                return arg;
            }
            throw new Exception("Invalid Operation");
        }

        [Benchmark(Description = "TResult Operation() + Exception")]
        public int ExceptionTest()
        {
            int acc = 0;
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    acc += GetException(i);
                }
                catch { }
            }
            return acc;
        }

        private Result<int, string> GetResult(int arg)
        {
            if (arg < SuccessRate)
            {
                return Ok(arg);
            }
            return Error("Invalid Operation");
        }

        [Benchmark(Description = "Result<TResult, TError> Operation()")]
        public int ResultTest()
        {
            int acc = 0;
            for (int i = 0; i < 100; i++)
            {
                var res = GetResult(i);
                if (res)
                {
                    acc += res.Value;
                }
            }
            return acc;
        }

        private Tuple<int, string> GetTuple(int arg)
        {
            if (arg < SuccessRate)
            {
                return Tuple.Create(arg, (string)null);
            }
            return Tuple.Create(0, "Invalid Operation");
        }

        [Benchmark(Description = "Tuple<TResult, TError> Operation()")]
        public int TupleTest()
        {
            int acc = 0;
            for (int i = 0; i < 100; i++)
            {
                var res = GetTuple(i);
                if (res.Item2 == null)
                {
                    acc += res.Item1;
                }
            }
            return acc;
        }

        private bool GetOutParameters(int arg, out int value, out string error)
        {
            if (arg < SuccessRate)
            {
                value = arg;
                error = null;
                return true;
            }
            value = 0;
            error = "Invalid Operation";
            return false;
        }

        [Benchmark(Description = "bool Operation(out TResult value, out TError error)")]
        public int OutParametersTest()
        {
            int acc = 0;
            for (int i = 0; i < 100; i++)
            {
                int value;
                string error;
                if (GetOutParameters(i, out value, out error))
                {
                    acc += value;
                }
            }
            return acc;
        }
    }
}
