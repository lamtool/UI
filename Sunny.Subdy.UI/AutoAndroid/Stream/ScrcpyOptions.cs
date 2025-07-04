﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAndroid.Stream
{
    public enum ScrcpyLockVideoOrientation
    {
        Unlocked = -1,
        /// <summary>
        /// Lock the current orientation when scrcpy starts.
        /// </summary>
        Initial = -2,
        /// <summary>
        /// Natural orientation.
        /// </summary>
        Orientation0 = 0,
        /// <summary>
        /// 90° counterclockwise.
        /// </summary>
        Orientation1,
        /// <summary>
        /// 180°, aka upside-down.
        /// </summary>
        Orientation2,
        /// <summary>
        /// 90° clockwise.
        /// </summary>
        Orientation3,
    };
}
