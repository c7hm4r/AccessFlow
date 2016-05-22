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
    /// Auxiliary extension methods for
    /// <see cref="IChildContextualCreator{TScope,TChild}"/>.
    /// </summary>
    public static class ChildContextualCreatorExtensions
    {
        #region Exact scope given
        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with an access scope of <paramref name="subScope"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// The result of <paramref name="content"/> is passed as result.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result of <paramref name="content"/>.
        /// </typeparam>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="subScope">
        /// The access scope of the created child contextual.
        /// </param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        /// <returns>
        /// The asynchrous result of <paramref name="content"/>.
        /// </returns>
        public static async Task<TResult> InChild<TResult, TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            TScope subScope, Func<TChild, Task<TResult>> content)
            where TChild : IDisposable
        {
            using (TChild context = parent.CreateChild(subScope))
            {
                return await content(context);
            }
        }

        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with an access scope of <paramref name="subScope"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// </summary>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="subScope">
        /// The access scope of the created child contextual.
        /// </param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        /// <returns>
        /// A representation of the asynchronous execution of this method.
        /// </returns>
        public static async Task InChild<TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            TScope subScope, Func<TChild, Task> content)
            where TChild : IDisposable
        {
            using (TChild context = parent.CreateChild(subScope))
            {
                await content(context);
            }
        }

        #region 2 Non-async equivalents
        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with an access scope of <paramref name="subScope"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// The result of <paramref name="content"/> is passed as result.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result of <paramref name="content"/>.
        /// </typeparam>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="subScope">
        /// The access scope of the created child contextual.
        /// </param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        /// <returns>
        /// The result of <paramref name="content"/>.
        /// </returns>
        public static TResult InChild<TResult, TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            TScope subScope, Func<TChild, TResult> content)
            where TChild : IDisposable
        {
            using (TChild context = parent.CreateChild(subScope))
            {
                return content(context);
            }
        }

        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with an access scope of <paramref name="subScope"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// </summary>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="subScope">
        /// The access scope of the created child contextual.
        /// </param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        public static void InChild<TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            TScope subScope, Action<TChild> content)
            where TChild : IDisposable
        {
            using (TChild context = parent.CreateChild(subScope))
            {
                content(context);
            }
        }
        #endregion 2 Non-async equivalents

        #endregion Exact scope given

        #region Same scope as parent
        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with the same access scope as <paramref name="parent"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// The result of <paramref name="content"/> is passed as result.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result of <paramref name="content"/>.
        /// </typeparam>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        /// <returns>
        /// The asynchrous result of <paramref name="content"/>.
        /// </returns>
        public static async Task<TResult>
            InChildWithin<TResult, TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            Func<TChild, Task<TResult>> content)
            where TChild : IDisposable
        {
            using (TChild context = parent.CreateChildWithin())
            {
                return await content(context);
            }
        }

        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with the same access scope as <paramref name="parent"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// </summary>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        /// <returns>
        /// A representation of the asynchronous execution of this method.
        /// </returns>
        public static async Task InChildWithin<TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            Func<TChild, Task> content)
            where TChild : IDisposable
        {
            using (TChild context = parent.CreateChildWithin())
            {
                await content(context);
            }
        }

        #region 2 Non-async equivalents
        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with the same access scope as <paramref name="parent"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// The result of <paramref name="content"/> is passed as result.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result of <paramref name="content"/>.
        /// </typeparam>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        /// <returns>
        /// The result of <paramref name="content"/>.
        /// </returns>
        public static TResult InChildWithin<TResult, TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            Func<TChild, TResult> content)
            where TChild : IDisposable
        {
            using (TChild context = parent.CreateChildWithin())
            {
                return content(context);
            }
        }

        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with the same access scope as <paramref name="parent"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// </summary>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        public static void InChildWithin<TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            Action<TChild> content)
            where TChild : IDisposable
        {
            using (TChild context = parent.CreateChildWithin())
            {
                content(context);
            }
        }
        #endregion 2 Non-async equivalents

        #endregion Same scope as parent

        #region Intersection with parent scope
        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with an access scope of the intersection of
        /// the access scope of <paramref name="parent"/> and
        /// <paramref name="maximumAccessScope"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// The result of <paramref name="content"/> is passed as result.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result of <paramref name="content"/>.
        /// </typeparam>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="maximumAccessScope">
        /// The 
        /// </param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        /// <returns>
        /// The asynchrous result of <paramref name="content"/>.
        /// </returns>
        public static async Task<TResult>
            InChildWithin<TResult, TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            TScope maximumAccessScope, Func<TChild, Task<TResult>> content)
            where TChild : IDisposable
        {
            using (TChild context =
                parent.CreateChildWithin(maximumAccessScope))
            {
                return await content(context);
            }
        }

        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with an access scope of the intersection of
        /// the access scope of <paramref name="parent"/> and
        /// <paramref name="maximumAccessScope"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// </summary>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="maximumAccessScope">
        /// The 
        /// </param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        /// <returns>
        /// A representation of the asynchronous execution of this method.
        /// </returns>
        public static async Task InChildWithin<TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            TScope maximumAccessScope, Func<TChild, Task> content)
            where TChild : IDisposable
        {
            using (TChild context =
                parent.CreateChildWithin(maximumAccessScope))
            {
                await content(context);
            }
        }

        #region 2 Non-async equivalents
        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with an access scope of the intersection of
        /// the access scope of <paramref name="parent"/> and
        /// <paramref name="maximumAccessScope"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// The result of <paramref name="content"/> is passed as result.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result of <paramref name="content"/>.
        /// </typeparam>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="maximumAccessScope">
        /// The 
        /// </param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        /// <returns>
        /// The result of <paramref name="content"/>.
        /// </returns>
        public static TResult InChildWithin<TResult, TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            TScope maximumAccessScope, Func<TChild, TResult> content)
            where TChild : IDisposable
        {
            using (TChild context =
                parent.CreateChildWithin(maximumAccessScope))
            {
                return content(context);
            }
        }

        /// <summary>
        /// Creates a new child contextual from <paramref name="parent"/>
        /// with an access scope of the intersection of
        /// the access scope of <paramref name="parent"/> and
        /// <paramref name="maximumAccessScope"/> and
        /// executes <paramref name="content"/>
        /// with the new created child contextual.
        /// After <paramref name="content"/> has finished,
        /// the child contextual is disposed.
        /// </summary>
        /// <typeparam name="TScope">The type of the access scope.</typeparam>
        /// <typeparam name="TChild">
        /// The type of the child contextual.
        /// </typeparam>
        /// <param name="parent">
        /// The <see cref="IChildContextualCreator{TScope,TChild}"/>
        /// to create the child contextual,
        /// typically another access contextual.</param>
        /// <param name="maximumAccessScope">
        /// The 
        /// </param>
        /// <param name="content">
        /// The function operating on the new created child contextual.
        /// </param>
        public static void InChildWithin<TScope, TChild>(
            this IChildContextualCreator<TScope, TChild> parent,
            TScope maximumAccessScope, Action<TChild> content)
            where TChild : IDisposable
        {
            using (TChild context =
                parent.CreateChildWithin(maximumAccessScope))
            {
                content(context);
            }
        }
        #endregion 2 Non-async equivalents

        #endregion Intersection with parent scope
    }
}