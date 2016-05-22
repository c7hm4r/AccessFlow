#region copyright
// Copyright 2016 Christoph Müller
// 
// This file is part of AccessFlow.
// 
// AccessFlow is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// AccessFlow is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with AccessFlow.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessFlow
{
    ///// <summary>
    ///// It is guarateed that every event inserted before
    ///// the last invocation of <see cref="InvokeAll"/> will be called.
    ///// It may happen that events enqueued
    ///// after the last invocation of <see cref="InvokeAll"/>
    ///// are being invoked.
    ///// Every event is only invoked once.
    ///// </summary>
    ///// <typeparam name="TEventArg">The type of the event argument.</typeparam>
    //internal class MultiEvent<TEventArg>
    //{
    //    public void Enqueue(Action<TEventArg> handler)
    //    {
    //        handles.Enqueue(handler);
    //    }

    //    public void InvokeAll(TEventArg arg)
    //    {
    //        int handleCount = handles.ToList().Count;
    //        for (int i = 0; i < handleCount; i++)
    //        {
    //            Action<TEventArg> dequeued;
    //            if (!handles.TryDequeue(out dequeued))
    //                break;
    //            dequeued(arg);
    //        }
    //    }

    //    #region private
    //    private readonly ConcurrentQueue<Action<TEventArg>> handles
    //        = new ConcurrentQueue<Action<TEventArg>>();
    //    #endregion
    //}

    /// <summary>
    /// It is guarateed that every event inserted before
    /// the last invocation of <see cref="InvokeAll"/> will be called.
    /// It may happen that events enqueued
    /// after the last invocation of <see cref="InvokeAll"/>
    /// are being invoked.
    /// Every event is only invoked once.
    /// </summary>
    /// <typeparam name="TEventArg0">
    /// The type of the first event argument.
    /// </typeparam>
    /// <typeparam name="TEventArg1">
    /// The type of the second event argument.
    /// </typeparam>
    internal class MultiEvent<TEventArg0, TEventArg1>
    {
        public void Enqueue(Action<TEventArg0, TEventArg1> handler)
        {
            handles.Enqueue(handler);
        }

        public void InvokeAll(TEventArg0 arg0, TEventArg1 arg1)
        {
            int handleCount = handles.ToList().Count;
            for (int i = 0; i < handleCount; i++)
            {
                Action<TEventArg0, TEventArg1> dequeued;
                if (!handles.TryDequeue(out dequeued))
                    break;
                dequeued(arg0, arg1);
            }
        }

        #region private
        private readonly ConcurrentQueue<Action<TEventArg0, TEventArg1>>
            handles = new ConcurrentQueue<Action<TEventArg0, TEventArg1>>();
        #endregion
    }
}
