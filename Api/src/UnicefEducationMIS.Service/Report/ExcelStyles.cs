using System;
using System.Collections.Generic;

namespace UnicefEducationMIS.Service.Report
{
    public class ExcelStyles
    {
        private static ExcelStyles _instance;
        private static readonly Object _lockObj = new object();
        private readonly Dictionary<string, UInt32> _styleIndexes;

        private ExcelStyles()
        {
            _styleIndexes = new Dictionary<string, uint>();
        }

        public static ExcelStyles Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (_lockObj)
                    {
                        if (null == _instance)
                        {
                            _instance = new ExcelStyles();
                        }
                    }
                }
                return _instance;
            }
        }

        public void CleareStyles()
        {
            _styleIndexes.Clear();
        }

        public void Add(string key, UInt32 value)
        {
            if (!_styleIndexes.ContainsKey(key))
            {
                _styleIndexes.Add(key, value);
            }
        }

        public UInt32 Get(string key)
        {
            if (_styleIndexes.ContainsKey(key))
                return _styleIndexes[key];
            return UInt32.MinValue;
        }
    }
}
