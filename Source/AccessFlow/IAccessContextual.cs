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
    /// An object operating within an <see cref="IAccessContext{TScope}"/>
    /// and which child access contextuals can be derived from.
    /// </summary>
    /// <typeparam name="TScope">The type of the access scope.</typeparam>
    /// <typeparam name="TThis">
    /// The type of this access contextual
    /// when it is used as the parent of a new child access contextual.
    /// This is usually an interface deriving from
    /// <see cref="IAccessContextual{TScope,TThis,TChildContextual}"/>.
    /// </typeparam>
    /// <typeparam name="TChildContextual">
    /// The type of child access contextuals.
    /// </typeparam>
    public interface IAccessContextual<
            TScope, out TThis, out TChildContextual> :
        IDisposable, IChildContextualCreator<TScope, TChildContextual>
    {
        /// <summary>
        /// Returns the acess contextual when it is used as the parent
        /// of a new child context derived from this access context.
        /// </summary>
        /// <returns>
        /// The acess contextual when it is used as the parent
        /// of a new child context derived from this access context.</returns>
        TThis ThisAsTParent();

        /// <summary>
        /// The <see cref="IAccessContext{TScope}"/> which
        /// the current instance operates on.
        /// </summary>
        IAccessContext<TScope> Context { get; }

        /// <summary>
        /// Returns if the current
        /// <see cref="IAccessContextual{TScope,TThis,TSubContextual}"/>
        /// has already been disposed.
        /// </summary>
        bool Disposed { get; }
    }
}