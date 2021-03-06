﻿/*****************************************************************************
   Copyright 2018 The MxNet.Sharp Authors. All Rights Reserved.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
******************************************************************************/
namespace MxNet.EventArgs
{
    public class EpochEndEventArgs : System.EventArgs
    {
        public EpochEndEventArgs(
            uint epoch,
            ulong samplesSeen,
            double loss,
            double validationLoss,
            double metric,
            double validationMetric)
        {
            Epoch = epoch;
            SamplesSeen = samplesSeen;
            Loss = loss;
            ValidationLoss = validationLoss;
            Metric = metric;
            ValidationMetric = validationMetric;
        }

        public uint Epoch { get; }
        public double Loss { get; }
        public double Metric { get; }
        public ulong SamplesSeen { get; }
        public double ValidationLoss { get; }
        public double ValidationMetric { get; }
    }
}