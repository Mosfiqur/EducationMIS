using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace UnicefEducationMIS.Service.Report
{
    internal class SharedStrings
    {
        private readonly Dictionary<string, uint> _stringIndexes;
        private readonly Dictionary<uint, string> _indexToString;
        private static SpreadsheetDocument _document;
        private static SharedStringTable _sharedStringTable;


        public SharedStrings(SpreadsheetDocument document)
        {
            _document = document;
            _stringIndexes = new Dictionary<string, uint>();
            _indexToString = new Dictionary<uint, string>();
            PopulateStringTable();
        }

        private void PopulateStringTable()
        {
            _sharedStringTable = _document.WorkbookPart.SharedStringTablePart.SharedStringTable;
            for (int index = 0; index < _sharedStringTable.ChildElements.Count; index++)
            {
                var sharedString = (SharedStringItem)_sharedStringTable.ChildElements[index];
                var strValue = sharedString.InnerText;
                if (_stringIndexes.ContainsKey(strValue) == false)
                {
                    var uintIndex = (uint)index;
                    _stringIndexes.Add(strValue, uintIndex);
                    AddToIndexToString(strValue, uintIndex);
                }
            }
        }

        private void AddToIndexToString(string strValue, uint uintIndex)
        {
            if (_indexToString.ContainsKey(uintIndex) == false)
                _indexToString.Add(uintIndex, strValue);
        }

        public void Clear()
        {
            _stringIndexes.Clear();
        }

        public UInt32Value Get(string strValue)
        {
            if (_stringIndexes.ContainsKey(strValue))
                return _stringIndexes[strValue];
            var sharedString = new SharedStringItem(new Text(strValue) { Space = SpaceProcessingModeValues.Preserve });
            _sharedStringTable = _document.WorkbookPart.SharedStringTablePart.SharedStringTable;
            _sharedStringTable.AppendChild(sharedString);
            _sharedStringTable.UniqueCount++;
            var index = _sharedStringTable.UniqueCount - 1;
            _stringIndexes.Add(strValue, index);
            AddToIndexToString(strValue, index);
            return index;
        }
        public string GetString(uint index)
        {
            if (_indexToString.ContainsKey(index))
                return _indexToString[index];
            return null;
        }

        public void Save()
        {
            _sharedStringTable.Save();
        }
    }
}
