﻿using MxNet.Gluon;
using System;
using System.Collections.Generic;
using System.Text;

namespace MxNet.GluonCV.ModelZoo.Ssd
{
    public class VGGAtrousBase : HybridBlock
    {
        public VGGAtrousBase(int[] layers, int[] filters, bool batch_norm= false, string prefix = null, ParameterDict @params = null) : base(prefix, @params)
        {
            throw new NotImplementedException();
        }

        public override NDArrayOrSymbol HybridForward(NDArrayOrSymbol x, params NDArrayOrSymbol[] args)
        {
            throw new NotImplementedException();
        }
    }
}
