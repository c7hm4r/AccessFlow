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
    /// Creator for an access contextual based on a parent.
    /// </summary>
    /// <typeparam name="TScope">The type of the access scopes.</typeparam>
    /// <typeparam name="TParent">
    /// The type of the parent.
    /// </typeparam>
    /// <typeparam name="TChildContextual">
    /// The type of the child access contextual to create.
    /// </typeparam>
    public interface IChildContextualFactory<
        TScope, in TParent, out TChildContextual>
    {
        /// <summary>
        /// Creates a new contextual based on the parent access contextual
        /// <paramref name="parent"/>.
        /// The new contextual has <paramref name="context"/> as access scope.
        /// </summary>
        /// <param name="parent">The parent access contextual.</param>
        /// <param name="context">
        /// The access context of the child contextual.
        /// </param>
        /// <returns>The new child contextual.</returns>
        TChildContextual Create(
            TParent parent, IAccessContext<TScope> context);
    }
}
