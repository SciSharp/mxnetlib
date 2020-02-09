﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MxNet.Gluon.NN
{
    public class Sequential : Block
    {
        public List<Block> Blocks
        {
            get
            {
                return childrens.Values.ToList();
            }
        }

        public Sequential this[string key]
        {
            get
            {
                Sequential net = new Sequential(Prefix);
                net.Add(childrens[key]);
                return net;
            }
        }

        public int Length
        {
            get
            {
                return childrens.Count;
            }
        }

        public Sequential(string prefix = null, ParameterDict @params = null) : base(prefix, @params)
        {
        }

        public void Add(params Block[] blocks)
        {
            foreach (var item in blocks)
            {
                RegisterChild(item);
            }
        }

        public override NDArray Forward(NDArray input, params NDArray[] args)
        {
            foreach (var item in Blocks)
            {
                input = item.Call(input).NdX;
            }

            return input;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.GetType().Name, Name);
        }

        public override void Hybridize(bool active = true, bool static_alloc = false, bool static_shape = false)
        {
            if(childrens.Values.All(x=>(x.GetType() == typeof(HybridBlock))))
            {
                Logger.Warning(string.Format("All children of this Sequential layer '{0}' are HybridBlocks. Consider " +
                                                "using HybridSequential for the best performance.", Prefix));
            }

            base.Hybridize(active, static_alloc, static_shape);
        }
    }
}