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
    /// <see cref="IChildContextualFactory{TScope,TParent,TChildContextual}"/>
    /// based on a <see cref="Func{TParent,IAccessContext,TChildContextual}"/>.
    /// </summary>
    public static class LambdaSubContextualFactory
    {
        /// <summary>
        /// Creates an
        /// <see cref="IChildContextualFactory{TScope,TParent,TChildContextual}"/>
        /// based on a
        /// <see cref="Func{TParent,IAccessContext,TChildContextual}"/>.
        /// </summary>
        /// <typeparam name="TScope">The type of the access scopes.</typeparam>
        /// <typeparam name="TParent">
        /// The type of the parent access contextual.
        /// </typeparam>
        /// <typeparam name="TChildContextual">
        /// The type of the created access contextual.
        /// </typeparam>
        /// <param name="creator">
        /// The function the new
        /// <see cref="IChildContextualFactory{TScope,TParent,TChildContextual}"/>
        /// is implemented with.
        /// </param>
        /// <returns>
        /// The new
        /// <see cref="IChildContextualFactory{TScope,TParent,TChildContextual}"/>.
        /// </returns>
        public static IChildContextualFactory<TScope, TParent, TChildContextual>
            Create<TScope, TParent, TChildContextual>(
            Func<TParent, IAccessContext<TScope>, TChildContextual> creator)
        {
            return new lambdaSubContextualFactory<
                TScope, TParent, TChildContextual>(creator);
        }

        #region private
        private class lambdaSubContextualFactory<TScope, TParent, TContextual>
            : IChildContextualFactory<TScope, TParent, TContextual>
        {
            public TContextual Create(
                TParent parentContextual, IAccessContext<TScope> context)
            {
                return creator(parentContextual, context);
            }

            public lambdaSubContextualFactory(
                Func<TParent, IAccessContext<TScope>, TContextual> creator)
            {
                this.creator = creator;
            }

            #region private
            private readonly Func<TParent, IAccessContext<TScope>, TContextual>
                creator;
            #endregion
        }
        #endregion
    }
}