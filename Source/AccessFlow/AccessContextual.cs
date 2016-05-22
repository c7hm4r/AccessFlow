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
    /// Base class to facilitate the implementation of
    /// <see cref="IAccessContextual{TScope,TThis,TSubContextual}"/>.
    /// </summary>
    /// <typeparam name="TScope">The type of the access scope.</typeparam>
    /// <typeparam name="TParent">
    /// The type of the access contextual when it is used as the parent of
    /// a new child contextual.
    /// This is usually the type itself.
    /// </typeparam>
    /// <typeparam name="TChildContextual">
    /// The type of the child contexuals derived from this class.
    /// This is usually an interface the current class implements itself.
    /// </typeparam>
    public abstract class AccessContextual<TScope, TParent, TChildContextual> :
        IAccessContextual<TScope, TParent, TChildContextual>
    {
        /// <summary>
        /// Returns the acess contextual when it is used as the parent
        /// of a new child context derived from this access context.
        /// </summary>
        /// <returns>
        /// The acess contextual when it is used as the parent
        /// of a new child context derived from this access context.</returns>
        public abstract TParent ThisAsTParent();

        /// <summary>
        /// The <see cref="IAccessContext{TScope}"/> which
        /// the current instance operates on.
        /// </summary>
        public IAccessContext<TScope> Context { get; }

        /// <summary>
        /// Creates a child contextual with an access scope of
        /// <paramref name="childScope"/>.
        /// </summary>
        /// <param name="childScope">
        /// The access scope of the created child contextual.
        /// </param>
        /// <returns>The new child contextual.</returns>
        public TChildContextual CreateChild(TScope childScope)
        {
            return factory.Create(ThisAsTParent(),
                Context.CreateChild(childScope));
        }

        /// <summary>
        /// Creates a child contextual with
        /// the same access scope as the parent.
        /// </summary>
        /// <returns>The new child contextual.</returns>
        public TChildContextual CreateChildWithin()
        {
            return factory.Create(ThisAsTParent(),
                Context.CreateChildWithin());
        }

        /// <summary>
        /// Creates a child contextual with
        /// same access scope as the intersection of 
        /// the access scope of the parent and
        /// <paramref name="maximumChildScope"/>.
        /// </summary>
        /// <param name="maximumChildScope">
        /// The access scope to intersect the parent scope with.
        /// </param>
        /// <returns>The new child contextual.</returns>
        public TChildContextual CreateChildWithin(TScope maximumChildScope)
        {
            return factory.Create(ThisAsTParent(),
                Context.CreateChildWithin(maximumChildScope));
        }

        /// <summary>
        /// Returns if the current
        /// <see cref="IAccessContextual{TScope,TThis,TSubContextual}"/>
        /// has already been disposed.
        /// </summary>
        public bool Disposed => Context.Disposed;

        /// <summary>
        /// Disposes the <see cref="IAccessContext{TScope}"/>
        /// used by this access contextual.
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
        }

        /// <summary>
        /// Constructor for an
        /// <see cref="AccessContextual{TScope,TParent,TChildContextual}"/>
        /// for given <paramref name="context"/>.
        /// Child contextuals are created using <paramref name="factory"/>.
        /// </summary>
        /// <param name="context">The access context
        /// which all operations of this
        /// <see cref="AccessContextual{TScope,TParent,TChildContextual}"/>
        /// are executed in.
        /// </param>
        /// <param name="factory">
        /// A creator for new child contextuals.
        /// </param>
        protected AccessContextual(IAccessContext<TScope> context,
            IChildContextualFactory<TScope, TParent, TChildContextual> factory)
        {
            Context = context;
            this.factory = factory;
        }

        /// <summary>
        /// Constructor for an
        /// <see cref="AccessContextual{TScope,TParent,TChildContextual}"/>
        /// for given <paramref name="context"/>.
        /// Child contextuals are created using <paramref name="factory"/>.
        /// </summary>
        /// <param name="context">The access context
        /// which all operations of this
        /// <see cref="AccessContextual{TScope,TParent,TChildContextual}"/>
        /// are executed in.
        /// </param>
        /// <param name="factory">
        /// A creator for new child contextuals.
        /// </param>
        protected AccessContextual(IAccessContext<TScope> context,
            Func<TParent, IAccessContext<TScope>, TChildContextual> factory)
        {
            Context = context;
            this.factory = LambdaSubContextualFactory.Create(factory);
        }

        #region private
        private readonly IChildContextualFactory<
            TScope, TParent, TChildContextual> factory;
        #endregion
    }
}
