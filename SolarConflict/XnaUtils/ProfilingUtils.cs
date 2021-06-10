using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using XnaUtils.Framework.Graphics;

namespace XnaUtils
{

    public class ProfilingUtils
    {
        private class StatisticsBlock
        {
            //public string Name { get; private set; }    
            public Stopwatch Watch { get; private set; }
            public long TotalExecutionTime { get; private set; }
            public long AverageExecutionTime { get { return TotalExecutionTime / TotalInvocationCount; }  }
            public long MaximumExecutionTime { get; private set; }            
            public int TotalInvocationCount { get; private set; }            

            public StatisticsBlock()
            {
                Watch = new Stopwatch();
            }

            public void Reset()
            {
                Watch.Reset();
                TotalExecutionTime = 0;
                TotalInvocationCount = 0;
                MaximumExecutionTime = 0;
            }
            
            public void Tic()
            {
                Watch.Restart();
            }

            public void Toc()
            {
                Watch.Stop();
                TotalInvocationCount++;
                long time = Watch.ElapsedTicks; //Add if ticks or MS
                TotalExecutionTime += time; 
                MaximumExecutionTime = Math.Max(MaximumExecutionTime, time);
            }

            public ProfilingData GetProfilingData()
            {
                return new ProfilingData(TotalExecutionTime, AverageExecutionTime, MaximumExecutionTime, TotalInvocationCount);
            }
        }

        private struct ProfilingData
        {
            public long TotalExecutionTime { get; set; }
            public long AverageExecutionTime { get; set; }
            public long MaximumExecutionTime { get; set; }
            public int TotalInvocationCount { get; set; }

            public ProfilingData(long totalExecutionTime, long averageExecutionTime, long maximumExecutionTime, int totalInvocationCount)
            {
                TotalExecutionTime = totalExecutionTime;
                AverageExecutionTime = averageExecutionTime;
                MaximumExecutionTime = maximumExecutionTime;
                TotalInvocationCount = totalInvocationCount;
            }

        }

        private Dictionary<string, List<ProfilingData>> _dataDictionary;
        private Dictionary<string, StatisticsBlock> _stopwatchDictionary;
        private ProfilingData _emptyProfilingData;
        public int InvocationBlockSize { get; set; }

        public ProfilingUtils(int invocationBlockSize = 600)
        {
            InvocationBlockSize = invocationBlockSize;
            _stopwatchDictionary = new Dictionary<string, StatisticsBlock>();
            _dataDictionary = new Dictionary<string, List<ProfilingData>>();
            _emptyProfilingData = new ProfilingData();
        }

        public void Tic(string name)
        {
            StatisticsBlock block;
            if(_stopwatchDictionary.TryGetValue(name, out block))
            {
                block.Tic();
            }
            else
            {
                _dataDictionary.Add(name, new List<ProfilingData>());
                block = new StatisticsBlock();
                _stopwatchDictionary.Add(name, block);
                block.Tic();
            }
        }

        public void Toc(string name)
        {
            StatisticsBlock block = _stopwatchDictionary[name];
            block.Toc();
            if (block.TotalInvocationCount >= InvocationBlockSize)
            {
                _dataDictionary[name].Add(block.GetProfilingData());
                block.Reset();
            }            
        }

        public long GetTime(string name)
        {
            List<ProfilingData> item = _dataDictionary[name];
            ProfilingData data = _emptyProfilingData;
            if (item.Count > 0)
            {
                data = item.Last();
            }
            return data.MaximumExecutionTime;
        }


        public string GetTimes(int length)
        {
            List<Tuple<string,long>> times = new List<Tuple<string, long>>();
            foreach (var item in _dataDictionary)
            {
                ProfilingData data = _emptyProfilingData;
                if(item.Value.Count > 0)
                {
                    data = item.Value.Last();
                }
                times.Add(new Tuple<string, long>(item.Key,data.MaximumExecutionTime));
            }

            StringBuilder sb = new StringBuilder();
            long sum = 0;
            for (int i = 0; i < times.Count; i++)
            {
                sum += times[i].Item2;
            }
            sum = Math.Max(sum, 1);
            for (int i = 0; i < times.Count; i++)
            {
                Color col = GraphicsUtils.HsvToRgb(i / (float)times.Count, 1, 1);
                
                sb.AppendLine(times[i].Item1 + " : " + (100*times[i].Item2 /sum));
            }
            sb.AppendLine(" -= Total : " + sum + " =-");
            return sb.ToString();
        }

    }
}
