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
    /// A context for accesses to side effects within an access scope of
    /// <see cref="RequiredScope"/>.
    /// </summary>
    /// <typeparam name="TScope">The type of the access scopes.</typeparam>
    public interface IAccessContext<TScope> :
        IDisposable, IChildContextualCreator<TScope, IAccessContext<TScope>>
    {
        /// <summary>
        /// The set calculus applied on the access scopes.
        /// </summary>
        IAccessScopeLogic<TScope> AccessScopeLogic { get; }

        /// <summary>
        /// If <see cref="RequiredScope"/> is completely owned by the current
        /// instance at the current time.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// The access scope which shall be awailable for the current
        /// <see cref="IAccessContext{TScope}"/>.
        /// It may only be reduced during the live time of each
        /// <see cref="IAccessContext{TScope}"/> instance.
        /// </summary>
        TScope RequiredScope { get; set; }

        /// <summary>
        /// If the current instance has been disposed so that
        /// no new access may be scheduled.
        /// </summary>
        bool Disposed { get; }

        /// <summary>
        /// Reduces the access scope of the current instance to
        /// the intersection of the current <see cref="RequiredScope"/>
        /// with <paramref name="maximumAccessScope"/>.
        /// </summary>
        /// <param name="maximumAccessScope">
        /// The access scope <see cref="RequiredScope"/> is intersected with.
        /// </param>
        void IntersectRequiredScopeWith(TScope maximumAccessScope);

        /// <summary>
        /// Returns a Task
        /// which completes as soon as the whole RequiredAcessScope
        /// is owned by the current AccessContext
        /// so that subsequent operations may actually modify
        /// the resource managed by AccessFlow.
        /// </summary>
        /// <returns>
        /// A Task
        /// which completes as soon as the whole RequiredAcessScope
        /// is owned by the current AccessContext.
        /// </returns>
        /// <remarks>
        /// If this method is used the current
        /// <see cref="IAccessContext{TScope}"/> must not have
        /// any child contexts, 
        /// neither before nor after the invocation of this method.
        /// The method should only be invoked sequentially
        /// (usually via <c>await</c>)from the thread “owning”
        /// the <see cref="IAccessContext{TScope}"/>.
        /// </remarks>
        Task UntilAvailable();

        /// <summary>
        /// Returns a Task
        /// which completes as soon as the <paramref name="scopeToOwn"/>
        /// is owned by the current instance
        /// so that subsequent operations may actually modify
        /// the resource managed by AccessFlow.
        /// </summary>
        /// <param name="scopeToOwn">
        /// The access scope to own by the current instance.
        /// </param>
        /// <returns>
        /// A Task which completes as soon as
        /// the given access scope is owned
        /// by the current AccessContext.
        /// </returns>
        /// <remarks>
        /// If this method is used the current
        /// <see cref="IAccessContext{TScope}"/> must not have
        /// any child contexts, 
        /// neither before nor after the invocation of this method.
        /// The method should only be invoked sequentially
        /// (usually via <c>await</c>) from the thread “owning”
        /// the <see cref="IAccessContext{TScope}"/>.
        /// </remarks>
        Task UntilAvailable(TScope scopeToOwn);
    }
}