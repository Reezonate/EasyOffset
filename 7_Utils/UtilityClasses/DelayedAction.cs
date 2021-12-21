using System;
using System.Threading;

namespace EasyOffset {
    public class DelayedAction {
        private Thread _thread;

        public void InvokeLater(int delayMillis, Action action) {
            Abort();
            _thread = new Thread(() => {
                try {
                    Thread.Sleep(delayMillis);
                    action.Invoke();
                } catch (ThreadAbortException) { }
            });
            _thread.Start();
        }

        public void Abort() {
            if (_thread is { IsAlive: true }) _thread.Abort();
            _thread = null;
        }
    }
}