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
    /// Provides an implementation for 
    /// <see cref="IAccessScopeLogic{TScope}"/>
    /// &lt;<see cref="IInfiniteIntervalScope{TSubScope}"/>&gt;.
    /// </summary>
    public static class InfiniteIntervalScopeLogic
    {
        /// <summary>
        /// Creates a
        /// <see cref="IAccessScopeLogic{TScope}"/>
        /// &lt;<see cref="IInfiniteIntervalScope{TSubScope}"/>&gt;
        /// based on a given access scope logic
        /// <paramref name="subScopeLogic"/>
        /// for the access scope of each element.
        /// </summary>
        /// <typeparam name="TSubScope">
        /// The type of the access scope of each element.
        /// </typeparam>
        /// <param name="subScopeLogic">
        /// The logic for the access scope of each element.
        /// </param>
        /// <returns>
        /// A new
        /// <see cref="IAccessScopeLogic{TScope}"/>
        /// &lt;<see cref="IInfiniteIntervalScope{TSubScope}"/>&gt;
        /// based on a given access scope logic
        /// <paramref name="subScopeLogic"/>
        /// for the access scope of each element.
        /// </returns>
        public static IAccessScopeLogic<IInfiniteIntervalScope<TSubScope>>
            Create<TSubScope>(IAccessScopeLogic<TSubScope> subScopeLogic)
        {
            return new infiniteIntervalScopeLogic<TSubScope>(subScopeLogic);
        }

        #region private
        private class infiniteIntervalScopeLogic<TSubScope> :
            IAccessScopeLogic<IInfiniteIntervalScope<TSubScope>>
        {
            public bool Contains(IInfiniteIntervalScope<TSubScope> outer,
                IInfiniteIntervalScope<TSubScope> inner)
            {
                return subScopeLogic.Contains(
                    outer.SubScope, inner.SubScope) &&
                    (outer.LowerBound == null ||
                        inner.LowerBound != null &&
                            outer.LowerBound.Value <=
                                inner.LowerBound.Value) &&
                    (outer.ProperUpperBound == null ||
                        inner.ProperUpperBound != null &&
                            outer.ProperUpperBound.Value >=
                                inner.ProperUpperBound.Value);
            }

            public bool Influences(IInfiniteIntervalScope<TSubScope> first,
                IInfiniteIntervalScope<TSubScope> second)
            {
                return subScopeLogic.Influences(
                    first.SubScope, second.SubScope) &&
                    (first.ProperUpperBound == null ||
                        second.LowerBound == null ||
                        first.ProperUpperBound.Value >=
                            second.LowerBound.Value) &&
                    (first.LowerBound == null ||
                        second.ProperUpperBound == null ||
                        first.LowerBound.Value <=
                            second.ProperUpperBound.Value);
            }

            public IInfiniteIntervalScope<TSubScope> Intersect(
                IInfiniteIntervalScope<TSubScope> first,
                IInfiniteIntervalScope<TSubScope> second)
            {
                TSubScope subScope = subScopeLogic.Intersect(
                    first.SubScope, second.SubScope);
                long? start =
                    first.LowerBound == null
                        ? second.LowerBound
                        : second.LowerBound == null
                            ? first.LowerBound
                            : Math.Max(first.LowerBound.Value,
                                second.LowerBound.Value);
                long? echteUpperBound =
                    first.ProperUpperBound == null
                        ? second.ProperUpperBound
                        : second.ProperUpperBound == null
                            ? first.ProperUpperBound
                            : Math.Min(first.ProperUpperBound.Value,
                                second.ProperUpperBound.Value);
                if (start != null && echteUpperBound != null &&
                    start > echteUpperBound)
                    echteUpperBound = start;
                return InfiniteIntervalScope.Create(
                    subScope, start, echteUpperBound);
            }

            public infiniteIntervalScopeLogic(
                IAccessScopeLogic<TSubScope> subScopeLogic)
            {
                this.subScopeLogic = subScopeLogic;
            }

            #region private
            private readonly IAccessScopeLogic<TSubScope>
                subScopeLogic;
            #endregion
        }
        #endregion
    }
}