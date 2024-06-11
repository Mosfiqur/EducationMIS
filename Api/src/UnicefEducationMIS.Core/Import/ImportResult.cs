using System.Collections.Generic;

namespace UnicefEducationMIS.Core.Import
{
    public class ImportResult<T>
    {
        public ImportResult()
        {
            RowErrors = new List<RowError>(); 
            ImportedObjects = new List<T>();
        }

        public bool InvalidFile { get; set; }
        public int TotalImported { get; set; }
        public List<T> ImportedObjects { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<RowError> RowErrors { get; set; }

        public void Succeed()
        {
            TotalImported = ImportedObjects.Count;
            ImportedObjects.Clear();
            IsSuccess = true;
        }

        public void Fail()
        {
            TotalImported = ImportedObjects.Count;
            ImportedObjects.Clear();
            IsSuccess = false;           
        }

        public void Fail(string errorMsg)
        {
            Fail();
            ErrorMessage = errorMsg;
        }

        public void AddRowError(int rowIndex, string message)
        {
            this.RowErrors.Add(new RowError()
            {
                RowNumber = rowIndex,
                ErrorMessage = message
            });
        }
    }
}
