﻿/*
 * EnumerableExtensions
 * Copyright (C) 2014-2015  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EnumerableExtensions
{
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Interleaves two sequences. Must be followed by an additional operator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IInterleavingEnumerable<T> Interleave<T>(this IEnumerable<T> sequence, IEnumerable<T> second)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return new InterleavingEnumerable<T>(sequence, second);
        }

        /// <summary>
        /// When one of the two source sequences runs out, the resulting sequence ends.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<T> Minimum<T>(this IInterleavingEnumerable<T> sequence)
        {
            using (var iterator1 = sequence.SequenceOne.GetEnumerator())
            using (var iterator2 = sequence.SequenceTwo.GetEnumerator())
                while (iterator1.MoveNext() && iterator2.MoveNext())
                {
                    yield return iterator1.Current;
                    yield return iterator2.Current;
                }
        }

        /// <summary>
        /// When one of the two source sequences runs out, it is padded with a default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static IEnumerable<T> PadMaximum<T>(this IInterleavingEnumerable<T> sequence, T defaultValue)
        {
            using (var iterator1 = sequence.SequenceOne.GetEnumerator())
            using (var iterator2 = sequence.SequenceTwo.GetEnumerator())
            {
                while (true)
                {
                    var more1 = iterator1.MoveNext();
                    var more2 = iterator2.MoveNext();
                    if (more1 && !more2)
                    {
                        yield return iterator1.Current;
                        yield return defaultValue;
                    }
                    else if (!more1 && more2)
                    {
                        yield return defaultValue;
                        yield return iterator2.Current;
                    }
                    else if (more1)
                    {
                        yield return iterator1.Current;
                        yield return iterator2.Current;
                    }
                    else yield break;
                }
            }
        }

        /// <summary>
        /// When one of the two source sequences runs out, it is padded with a default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<T> PadMaximum<T>(this IInterleavingEnumerable<T> sequence) => sequence.PadMaximum(default(T));

	    /// <summary>
        /// When one of the two source sequences runs out, the remaining elements of the other sequence are returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<T> AndAddRest<T>(this IInterleavingEnumerable<T> sequence)
        {
            using (var iterator1 = sequence.SequenceOne.GetEnumerator())
            using (var iterator2 = sequence.SequenceTwo.GetEnumerator())
            {
                while (true)
                {
                    var more1 = iterator1.MoveNext();
                    var more2 = iterator2.MoveNext();
                    if (more1) yield return iterator1.Current;
                    if (more2) yield return iterator2.Current;
                    if (!more1 && !more2) yield break;
                }
            }
        }
    }

    internal class InterleavingEnumerable<T> : IInterleavingEnumerable<T>
    {
        public IEnumerable<T> SequenceOne { get; }
        public IEnumerable<T> SequenceTwo { get; }

        public InterleavingEnumerable(IEnumerable<T> sequenceOne, IEnumerable<T> sequenceTwo)
        {
            SequenceOne = sequenceOne;
            SequenceTwo = sequenceTwo;
        }
    }

    /// <summary>
    /// Represents two sequences to be enumerated in an interleaved fashion.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IInterleavingEnumerable<out T> : IFluentInterface
    {
        /// <summary>
        /// Represents the first sequence.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<T> SequenceOne { get; }

        /// <summary>
        /// Represents the second sequence.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<T> SequenceTwo { get; }
    }
}
