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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessFlow
{
    internal static class OneTimeAction<TParam0, TParam1>
    {
        public static Action<TParam0, TParam1> Create(
            Action<TParam0, TParam1> action)
        {
            int invoked = 0;
            return (p0, p1) =>
            {
                if (Interlocked.CompareExchange(ref invoked, 1, 0) == 0)
                    action(p0, p1);
            };
        }
    }
}
