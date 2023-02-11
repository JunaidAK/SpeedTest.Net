using SpeedTest.Net.Enums;
using SpeedTest.Net.Helpers;
using SpeedTest.Net.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpeedTest.Net
{
    internal abstract class BaseHttpClient : HttpClient
    {
        public BaseHttpClient(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler) { }
        public BaseHttpClient(HttpMessageHandler handler) : base(handler) { }
        public BaseHttpClient() : base() { }

        internal async Task<DownloadSpeed> GetDownloadSpeed(IEnumerable<string> downloadUrls, SpeedTestUnit unit, int timeout = 5000)
        {
            var bytes = 0D;
            var startTime = DateTime.Now;

            foreach (var url in downloadUrls)
            {
                bytes += await GetDownloadedBytes(url, timeout);
            }

            var elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
            var bytesPerSecond = bytes / elapsedSeconds;

            return new DownloadSpeed
            {
                Speed = bytesPerSecond.FromBytesPerSecondTo(unit),
                Unit = unit.ToShortIdentifier()
            };
        }

        private async Task<double> GetDownloadedBytes(string downloadUrl, int timeout)
        {
            var from = this.GetType().Name;

            var totalRead = 0L;
            var totalReads = 0L;
            var buffer = new byte[8192];
            var isMoreToRead = true;
            var tempFile = Path.Combine(Path.GetTempPath(), "TempFile" + from);
            var kiloBytes = 0D;

            try
            {
                DeleteFile(tempFile);

                DefaultRequestHeaders.Add("User-Agent", "SpeedTest.Net"); 

                using (HttpResponseMessage response = await GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    cancellationTokenSource.CancelAfter(timeout);
                    CancellationToken cancellationToken = cancellationTokenSource.Token;

                    response.EnsureSuccessStatusCode();

                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(), fileStream = new FileStream(tempFile, FileMode.Append, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        do
                        {
                            if (cancellationToken.IsCancellationRequested)
                                break;

                            var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                            if (read == 0)
                            {
                                isMoreToRead = false;
                            }
                            else
                            {
                                await fileStream.WriteAsync(buffer, 0, read);

                                totalRead += read;
                                totalReads += 1;

                                if (totalReads % 2000 == 0)
                                {
                                    Console.WriteLine(from + string.Format(" Total bytes downloaded so far: {0:n0}", totalRead));
                                }
                            }
                        }
                        while (isMoreToRead);
                    }
                }

                kiloBytes = new FileInfo(tempFile).Length;

                DeleteFile(tempFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return kiloBytes;
        }

        private static void DeleteFile(string tempFile)
        {
            try
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
            catch { /**/ }
        }
    }
}