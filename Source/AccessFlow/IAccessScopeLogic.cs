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
    /// Represents the logic for certain scopes.
    /// </summary>
    /// <typeparam name="TScope"></typeparam>
    public interface IAccessScopeLogic<TScope>
    {
        /// <summary>
        /// Determines, whether the two given access scopes
        /// <paramref name="first"/> and <paramref name="second"/> overlap.
        /// More precisely, whether two access contexts a and b exist
        /// which are not in a child–parent relationship and
        /// there are no operations between a and b and
        /// where a and b have exactly
        /// the access scope <paramref name="first"/> or
        /// <paramref name="second"/> respectively
        /// and a and b each contain a sequence of operations in a an b
        /// and after a and b exists a sequence of operations
        /// so that the result or a side effect of a method
        /// in a or b or after a and b
        /// depends substantially on the order of a and b.
        /// Additionally this method may be used to specify
        /// whether operations can be executed in parallel or not.
        /// </summary>
        /// <param name='first'>
        /// The first access scope.
        /// </param>
        /// <param name='second'>
        /// The second access scope.
        /// </param>
        /// <returns>
        /// Whether the two access scopes overlap.
        /// </returns>
        /// <remarks>
        /// Influences(<paramref name="first"/>, <paramref name="second"/>) ==
        /// Influences(<paramref name="second"/>, <paramref name="first"/>).
        /// </remarks>
        bool Influences(TScope first, TScope second);

        /// <summary>
        /// Returns whether the access scope <paramref name="outer"/> contains
        /// the access scope <paramref name="inner"/>.
        /// More precisely, whether for each access scope a,
        /// if Influences(inner, a) then Influences(outer, a).
        /// </summary>
        /// <param name="outer">The possibly outer access scope.</param>
        /// <param name="inner">The possibly inner access scope.</param>
        /// <returns>
        /// Whether the access scope <paramref name="outer"/> contains
        /// the access scope <paramref name="inner"/>.
        /// </returns>
        bool Contains(TScope outer, TScope inner);

        /// <summary>
        /// Returns the intersection of <paramref name="first"/> and
        /// <paramref name="second"/>.
        /// More precisely, returns an access scope a,
        /// which is both contained in <paramref name="first"/> and
        /// <paramref name="second"/> and
        /// where there is no access scope b which is both contained in
        /// <paramref name="first"/> and <paramref name="second"/>
        /// and which is not contained in a.
        /// </summary>
        /// <param name="first">The first access scope.</param>
        /// <param name="second">The second access scope.</param>
        /// <returns>
        /// The intersection of <paramref name="first"/> and
        /// <paramref name="second"/>.
        /// </returns>
        TScope Intersect(TScope first, TScope second);
    }
}
