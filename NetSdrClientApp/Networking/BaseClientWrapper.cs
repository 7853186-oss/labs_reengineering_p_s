using System;
using System.Threading;

namespace NetSdrClientApp.Networking
{
    public abstract class BaseClientWrapper : IDisposable
    {
        protected CancellationTokenSource? _cts;
        protected CancellationToken Token => _cts?.Token ?? CancellationToken.None;

        protected void ResetCancellationToken()
        {
            Cancel(); 
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }

        protected void Cancel()
        {
            if (_cts != null)
            {
                try
                {
                    if (!_cts.IsCancellationRequested)
                    {
                        _cts.Cancel();
                    }
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        protected void StopCancellationToken()
        {
            Cancel();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Cancel();
                _cts?.Dispose();
                _cts = null;
            }
        }
    }
}
