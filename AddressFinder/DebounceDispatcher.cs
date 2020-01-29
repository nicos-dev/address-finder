using System;
using System.Threading.Tasks;

/*
    -------------------------------------------------------------------------------
    MIT License

    Copyright (c) 2019 CoddiCat

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    --------------------------------------------------------------------------------

    File: https://github.com/coddicat/DebounceThrottle/blob/master/DebounceThrottle/DebounceDispatcher.cs
    
    Code has been slightly modified.
 */

namespace FormValidator
{
    /// <summary>
    /// The debounce dispatcher delays the invoking of Action until the calls stopped for a predetermined interval
    /// </summary>
    public class DebounceDispatcher : DebounceDispatcher<bool>
    {
        #region --- ctor ---
        /// <summary>
        /// The debounce dispatcher delays the invoking of Action until the calls stopped for a predetermined interval
        /// </summary>
        /// <param name="interval">The interval in milliseconds that must elapse after the last call so that a Action to be invoked</param>
        public DebounceDispatcher(int interval) : base(interval)
        {
        }
        #endregion

        #region --- public methods ---

        /// <summary>
        /// The call of the Action that will be invoked after waiting for the time interval if there are no more calls in this interval
        /// </summary>
        /// <param name="action">Action that will be invoked after waiting for the time interval</param>
        /// <returns>Task that will complete when Action will be invoked</returns>
        private void DebounceAsync(Func<Task> action)
        {
            base.DebounceAsync(async () =>
            {
                await action.Invoke();
                return true;
            });
        }
        /// <summary>
        /// The call of the Action that will be invoked after waiting for the time interval if there are no more calls in this interval
        /// </summary>
        /// <param name="action">Action that will be invoked after waiting for the time interval</param>
        public void Debounce(Action action)
        {
            Task<bool> ActionAsync() =>
                Task.Run(() =>
                {
                    action.Invoke();
                    return true;
                });

            DebounceAsync((Func<Task<bool>>) ActionAsync);
        }
        #endregion
    }

    /// <summary>
    /// The debounce dispatcher delays the invoking of Function until the calls stopped for a predetermined interval
    /// </summary>
    /// <typeparam name="T">The return Type of the Tasks. All tasks will return the same value if the invoking occurs once</typeparam>
    public class DebounceDispatcher<T>
    {
        #region --- private fields ---
        private DateTime _lastInvokeTime;
        private readonly int _interval;
        private Func<Task<T>> _functToInvoke;
        private readonly object _locker = new object();
        private bool _busy;
        private Task<T> _waitingTask;
        #endregion

        #region --- ctor ---
        /// <summary>
        /// The debounce dispatcher delays the invoking of Function until the calls stopped for a predetermined interval
        /// </summary>
        /// <param name="interval">The interval in milliseconds that must elapse after the last call so that a Function to be invoked</param>
        protected DebounceDispatcher(int interval)
        {
            this._interval = interval;
        }
        #endregion

        #region --- public methods ---

        /// <summary>
        /// The call of the Function that will be invoked after waiting for the time interval if there are no more calls in this interval
        /// </summary>
        /// <param name="functToInvoke">Function that will be invoked after waiting for the time interval</param>
        /// <returns>Task with a result that will complete when Function will be invoked. All tasks will return the same value if the invoking occurs once</returns>
        protected void DebounceAsync(Func<Task<T>> functToInvoke)
        {
            lock (_locker)
            {
                this._functToInvoke = functToInvoke;
                this._lastInvokeTime = DateTime.UtcNow;
                if (_busy)
                {
                    return;
                }

                _busy = true;
                _waitingTask = Task.Run(() =>
                {
                    do
                    {
                        int delay = (int)(_interval - (DateTime.UtcNow - _lastInvokeTime).TotalMilliseconds);
                        Task.Delay(delay).Wait();
                    } while ((DateTime.UtcNow - _lastInvokeTime).TotalMilliseconds < _interval);

                    T res;
                    try
                    {
                        res = this._functToInvoke.Invoke().Result;
                    }
                    finally
                    {
                        lock (_locker)
                        {
                            _busy = false;
                        }
                    }

                    return res;
                });
            }
        }
        #endregion
    }
}
