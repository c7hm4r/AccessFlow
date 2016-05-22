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
    /// Provides an implementation for an
    /// <see cref="IChildContextualCreator{TScope,TChild}"/>
    /// encapsulating an
    /// <see cref="IAccessContextual{TScope,TThis,TSubContextual}"/>.
    /// </summary>
    public static class ChildContextualCreator
    {
        /// <summary>
        /// Wraps the
        /// <see cref="IAccessContextual{TScope,TThis,TSubContextual}"/>
        /// <paramref name="parent"/>
        /// in a new <see cref="IChildContextualCreator{TScope,TChild}"/>,
        /// which is returned.
        /// </summary>
        /// <typeparam name="TScope">The type of the access scopes.</typeparam>
        /// <typeparam name="TParent">
        /// The type of the access contextual
        /// when it is used as parent for the creation of a child contextual.
        /// </typeparam>
        /// <typeparam name="TChildContext">
        /// The type of the child contextuals.
        /// </typeparam>
        /// <param name="parent">
        /// The access contextual being wrapped.
        /// </param>
        /// <returns>
        /// The newly created
        /// <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// wrapping <paramref name="parent"/>.
        /// </returns>
        public static IChildContextualCreator<TScope, TChildContext>
            Create<TScope, TParent, TChildContext>(
            IAccessContextual<TScope, TParent, TChildContext> parent)
        {
            return new childContextualCreator<TScope, TParent, TChildContext>(
                parent);
        }

        #region private
        private class childContextualCreator<TScope, TParent, TContext> :
            IChildContextualCreator<TScope, TContext>
        {
            public TContext CreateChild(TScope scope)
            {
                return parent.CreateChild(scope);
            }

            public TContext CreateChildWithin()
            {
                return parent.CreateChildWithin();
            }

            public TContext CreateChildWithin(TScope maximumScope)
            {
                return parent.CreateChildWithin(maximumScope);
            }

            public childContextualCreator(
                IAccessContextual<TScope, TParent, TContext> parent)
            {
                this.parent = parent;
            }

            #region private
            private readonly IAccessContextual<TScope, TParent, TContext>
                parent;
            #endregion
        }
        #endregion
    }
}