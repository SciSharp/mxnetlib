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
using System;

namespace MxNet.Gluon.NN
{
    public class PixelShuffle3D : HybridBlock
    {
        public (int, int, int) factor;

        public PixelShuffle3D((int, int, int) factor)
        {
            this.factor = factor;
        }

        public override NDArrayOrSymbolList HybridForward(NDArrayOrSymbolList args)
        {
            var x = args[0];
            var (f1, f2, f3) = factor;
            x = F.reshape(x, new Shape(-2, -6, -1, f1 * f2 * f3, -2, -2, -2));
            x = F.swapaxes(x, 2, 3);
            x = F.reshape(x, new Shape(-2, -2, -2, -6, f1, f2 * f3, -2, -2));
            x = F.reshape(x, new Shape(-2, -2, -5, -2, -2, -2));
            x = F.swapaxes(x, 3, 4);
            x = F.reshape(x, new Shape(-2, -2, -2, -2, -6, f2, f3, -2));
            x = F.reshape(x, new Shape(-2, -2, -2, -5, -2, -2));
            x = F.swapaxes(x, 4, 5);
            x = F.reshape(x, new Shape(-2, -2, -2, -2, -5));
            return x;
        }
    }
}