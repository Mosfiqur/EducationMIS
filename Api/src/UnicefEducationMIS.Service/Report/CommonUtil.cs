using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnicefEducationMIS.Service.Report
{
    internal class CommonUtil
    {
        public static string GetExceptionDetail(Exception exception)
        {
            var stringBuilder = new StringBuilder();
            var newLine = Environment.NewLine;
            stringBuilder.Append(String.Format("Exception Type: {0}{1}", exception.GetType().AssemblyQualifiedName,
                newLine));
            stringBuilder.Append(String.Format("Message: {0}{1}", exception.Message, newLine));

            Exception innerException = exception.InnerException;
            var counter = 1;
            while (null != innerException)
            {
                stringBuilder.Append(String.Format("Inner Exception {0} Type: {1}{2}", counter,
                    innerException.GetType().AssemblyQualifiedName, newLine));
                stringBuilder.Append(String.Format("Inner Exception {0} Message: {1}{2}", counter,
                    innerException.Message, newLine));

                innerException = innerException.InnerException;
                counter++;
            }
            stringBuilder.Append(String.Format("Stack Trace: {0}", exception.StackTrace));
            return stringBuilder.ToString();
        }

        public static MemoryStream GetDisconnectedMemoryStream(string filePath)
        {
            var bytesArray = File.ReadAllBytes(filePath);
            var ms = new MemoryStream();
            ms.Write(bytesArray, 0, bytesArray.Length);
            ms.Position = 0;
            return ms;
        }
    }
}
