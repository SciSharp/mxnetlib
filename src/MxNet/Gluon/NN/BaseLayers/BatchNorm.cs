﻿using MxNet.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MxNet.Gluon.NN
{
    public class BatchNorm : HybridBlock
    {
        public int Axis { get; set; }
        public float Momentum { get; }
        public float Epsilon { get; }
        public bool Center { get; }
        public bool Scale { get; }
        public bool Use_Global_Stats { get; }
        public int In_Channels { get; }
        public NDArray Gamma { get; set; }
        public NDArray Beta { get; set; }
        public NDArray RunningMean { get; set; }
        public NDArray RunningVar { get; set; }

        public BatchNorm(int axis= 1, float momentum= 0.9f, float epsilon= 1e-5f, bool center= true, bool scale= true,
                        bool use_global_stats= false, string beta_initializer= "zeros", string gamma_initializer= "ones",
                        string running_mean_initializer= "zeros", string running_variance_initializer= "ones",
                        int in_channels= 0, string prefix = null, ParameterDict @params = null) : base(prefix, @params)
        {
            Momentum = momentum;
            Epsilon = epsilon;
            Center = center;
            Scale = scale;
            Use_Global_Stats = use_global_stats;
            In_Channels = in_channels;
            Gamma = Params.Get("gamma", new Shape((uint)in_channels), Initializer.Get(gamma_initializer), allow_deferred_init: true, differentiable: scale, grad_req: scale ? OpGradReq.Write : OpGradReq.Null);
            Beta = Params.Get("beta", new Shape((uint)in_channels), Initializer.Get(beta_initializer), allow_deferred_init: true, differentiable: center, grad_req: center ? OpGradReq.Write : OpGradReq.Null);
            RunningMean = Params.Get("running_mean", new Shape((uint)in_channels), Initializer.Get(running_mean_initializer), allow_deferred_init: true);
            RunningVar = Params.Get("running_var", new Shape((uint)in_channels), Initializer.Get(running_variance_initializer), allow_deferred_init: true);
        }

        public override NDArrayOrSymbol HybridForward(NDArrayOrSymbol x, params NDArrayOrSymbol[] args)
        {
            NDArrayOrSymbol gamma = args[0];
            NDArrayOrSymbol beta = args[1];
            NDArrayOrSymbol running_mean = args[2];
            NDArrayOrSymbol running_var = args[3];

            if (x.IsNDArray)
                return nd.BatchNorm(x.NdX, gamma.NdX, beta.NdX, running_mean.NdX, running_var.NdX);

            return sym.BatchNorm(x.SymX, gamma.SymX, beta.SymX, running_mean.SymX, running_var.SymX, symbol_name: "fwd");
        }

    }
}
