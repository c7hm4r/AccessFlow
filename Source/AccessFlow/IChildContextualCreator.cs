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
    /// <summary>
    /// Creator of childs within access contexts.
    /// </summary>
    /// <typeparam name="TScope">
    /// The type of the access contexts.
    /// </typeparam>
    /// <typeparam name="TChild">
    /// The type of the created child contextuals.
    /// </typeparam>
    public interface IChildContextualCreator<in TScope, out TChild>
    {
        /// <summary>
        /// Creates a child contextual with an access scope of
        /// <paramref name="accessScope"/>.
        /// </summary>
        /// <param name="accessScope">
        /// The access scope of the created child contextual.
        /// </param>
        /// <returns>The new child contextual.</returns>
        TChild CreateChild(TScope accessScope);

        /// <summary>
        /// Creates a child contextual with
        /// the same access scope as the parent.
        /// </summary>
        /// <returns>The new child contextual.</returns>
        TChild CreateChildWithin();

        /// <summary>
        /// Creates a child contextual with
        /// same access scope as the intersection of 
        /// the access scope of the parent and
        /// <paramref name="maximumAccessScope"/>.
        /// </summary>
        /// <param name="maximumAccessScope">
        /// The access scope to intersect the parent scope with.
        /// </param>
        /// <returns>The new child contextual.</returns>
        TChild CreateChildWithin(TScope maximumAccessScope);
    }
}
