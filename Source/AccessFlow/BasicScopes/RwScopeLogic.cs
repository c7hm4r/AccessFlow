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

namespace AccessFlow.BasicScopes
{
    /// <summary>
    /// Provides an implementation for an
    /// <see cref="IAccessScopeLogic{T}"/>&lt;<see cref="RWScope"/>&gt;.
    /// </summary>
    public static class RWScopeLogic
    {
        /// <summary>
        /// An instance of an <see cref="IAccessScopeLogic{TScope}"/>&lt;
        /// <see cref="RWScope"/>&gt;.
        /// </summary>
        public static readonly IAccessScopeLogic<RWScope> Instance =
            new rwScopeLogic();

        /// <summary>
        /// The access scope which contains any <see cref="RWScope"/>
        /// in terms of this implementation of an
        /// <see cref="IAccessScopeLogic{T}"/>&lt;<see cref="RWScope"/>&gt;.
        /// </summary>
        public static readonly RWScope MaximumScope = RWScope.ReadWrite;

        #region private
        private class rwScopeLogic : IAccessScopeLogic<RWScope>
        {
            public bool Contains(RWScope outer, RWScope inner)
            {
                return ((byte)inner & ~(byte)outer) == (byte)RWScope.None;
            }

            public bool Influences(RWScope first, RWScope second)
            {
                return ((byte)first & (byte)second) != (byte)RWScope.None;
            }

            public RWScope Intersect(RWScope first, RWScope second)
            {
                return (RWScope)((byte)first & (byte)second);
            }
        }
        #endregion
    }
}