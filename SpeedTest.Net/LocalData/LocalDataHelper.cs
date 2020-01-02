using System.Reflection;

namespace SpeedTest.Net.LocalData
{
    internal static class LocalDataHelper
    {
        internal static string ReadLocalFile(string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                    return null;

                var assembly = typeof(LocalDataHelper).GetTypeInfo().Assembly;
                var resourceName = "SpeedTest.Net.LocalData." + filename;

                using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch { /**/ }
            return null;
        }
    }
}