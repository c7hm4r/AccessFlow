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

namespace AccessFlow
{
    internal enum AccessContextState
    {
        /// <summary>
        /// The access context is active,
        /// but its nextOverhangingNode property is not yet initialized.
        /// Every next modifying operation
        /// has to (internally) translate
        /// the current node to the real active state.
        /// </summary>
        AcitiveInitializing,
        /// <summary>
        /// The active state of the access context.
        /// Child contexts can be created.
        /// </summary>
        Active,
        ///// <summary>
        ///// The access context cannot create sub contexts
        ///// but it is kept to accelerate
        ///// the process of finding the next influencing node.
        ///// </summary>
        //Placeholder,
        /// <summary>
        /// The access context cannot create child contexts anymore.
        /// It is also going to be removed from the context graph.
        /// </summary>
        Ended,
    }
}
