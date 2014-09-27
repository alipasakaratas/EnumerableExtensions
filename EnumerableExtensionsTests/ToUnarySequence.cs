﻿/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * 
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EnumerableExtensions;

namespace EnumerableExtensionsTests
{
    [TestFixture]
    class ToUnarySequence
    {
        [Test]
        public void ToUnarySequenceIntegers()
        {
            Assert.AreEqual(1, 2.ToUnarySequence().Count());
            Assert.AreEqual(2, 2.ToUnarySequence().First());
        }
    }
}