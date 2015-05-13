﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Practices.IoTJourney.Devices.Simulator
{
    public class EventEntry
    {
        private readonly double _frequency;

        private readonly double _jitter;

        private readonly Func<Random, object> _eventFactory;

        private double _frequencyWithJitter;

        private double _totalElapsedMilliseconds;

        private readonly Random _random;

        public EventEntry(Func<Random, object> eventFactory, TimeSpan frequency, double percentToJitter = 0f)
        {
            _eventFactory = eventFactory;
            _frequency = frequency.TotalMilliseconds;
            _jitter = percentToJitter;
            _random = new Random();

            ResetElapsedTime();
        }

        public TimeSpan ElapsedTime
        {
            get
            {
                return TimeSpan.FromMilliseconds(_totalElapsedMilliseconds);
            }
        }

        public object CreateNewEvent()
        {
            return _eventFactory(_random);
        }

        public void ResetElapsedTime()
        {
            _totalElapsedMilliseconds = 0;

            var nextJitter = (_random.NextDouble() * _jitter);
            _frequencyWithJitter = _frequency + (nextJitter * _frequency);
        }

        public bool ShouldSendEvent()
        {
            return _totalElapsedMilliseconds >= _frequencyWithJitter;
        }

        public void UpdateElapsedTime(TimeSpan elapsed)
        {
            _totalElapsedMilliseconds += elapsed.TotalMilliseconds;
        }
    }
}