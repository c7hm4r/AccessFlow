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
using System.Threading.Tasks;
using LockFreeDoublyLinkedLists;

namespace AccessFlow
{
    internal class AccessContextInfo<T>
    {
        public T AccessScope { get; }
        public AccessContextState State { get; }

        public ILockFreeDoublyLinkedListNode<AccessContextInfo<T>>
            NextOverhangingNode
        { get; }

        public MultiEvent<ILockFreeDoublyLinkedListNode<AccessContextInfo<T>>,
            AccessContextInfo<T>>
            RequiredAccessScopeReduced
        { get; }

        public bool IsActive => State != AccessContextState.Ended;

        public AccessContextInfo(T accessScope, AccessContextState state,
            ILockFreeDoublyLinkedListNode<AccessContextInfo<T>>
                nextOverhangingNode,
            MultiEvent<ILockFreeDoublyLinkedListNode<AccessContextInfo<T>>,
                    AccessContextInfo<T>>
                requiredAccessScopeReduced)
        {
            AccessScope = accessScope;
            State = state;
            NextOverhangingNode = nextOverhangingNode;
            RequiredAccessScopeReduced = requiredAccessScopeReduced;
        }
    }
}
