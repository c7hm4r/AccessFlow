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
    /// Represents three basic acces types None, Read and ReadWrite.
    /// </summary>
    public enum RWScope : byte
    {
        /// <summary>
        /// Represents no access on the entity.
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Represents the capability to read information from the entity.
        /// </summary>
        Read = 0x1,
        /// <summary>
        /// Represents the capability to read information from the entity
        /// and modify the entity.
        /// </summary>
        ReadWrite = 0x3
    }
}
