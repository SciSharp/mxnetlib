﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MxNet.Gluon.RNN
{
    public class ZoneoutCell : ModifierCell
    {
        public float ZoneoutOutputs { get; }
        public float ZoneoutStates { get; }

        private NDArrayOrSymbol _prev_output;

        public ZoneoutCell(RecurrentCell base_cell, float zoneout_outputs = 0, float zoneout_states = 0) : base(base_cell)
        {
            ZoneoutOutputs = zoneout_outputs;
            ZoneoutStates = zoneout_states;
            _prev_output = null;
        }

        public override string Alias()
        {
            return "zoneout";
        }

        public override void Reset()
        {
            base.Reset();
            _prev_output = null;
        }

        public override (NDArrayOrSymbol, NDArrayOrSymbol[]) HybridForward(NDArrayOrSymbol x, params NDArrayOrSymbol[] args)
        {
            var (cell, p_outputs, p_states) = (BaseCell, ZoneoutOutputs, ZoneoutStates);
            var (next_output, next_states) = cell.Call(x, args);
            NDArrayOrSymbol mask(float p, NDArrayOrSymbol like)
            {
                if (x.IsNDArray)
                {
                    return nd.Dropout(nd.OnesLike(x), p);
                }

                return sym.Dropout(sym.OnesLike(x), p);
            }

            var prev_output = _prev_output;
            if (prev_output == null)
                prev_output = x.IsNDArray ? new NDArrayOrSymbol(nd.ZerosLike(next_output)) : new NDArrayOrSymbol(sym.ZerosLike(next_output));

            NDArrayOrSymbol output = null;
            NDArrayOrSymbol[] states = null;
            if (x.IsNDArray)
            {
                output = p_outputs != 0 ? 
                            new NDArrayOrSymbol(nd.Where(mask(p_outputs, next_output), next_output, prev_output)): next_output;

                if (p_states == 0)
                    states = next_states;
                else
                {
                    Enumerable.Zip(next_states, states, (new_s, old_s) =>
                    {
                        return new NDArrayOrSymbol(nd.Where(mask(p_states, new_s), new_s, old_s));
                    }).ToArray();
                }
            }
            else if (x.IsSymbol)
            {
                output = p_outputs != 0 ?
                            new NDArrayOrSymbol(sym.Where(mask(p_outputs, next_output), next_output, prev_output)) : next_output;

                if (p_states == 0)
                    states = next_states;
                else
                {
                    Enumerable.Zip(next_states, states, (new_s, old_s) =>
                    {
                        return new NDArrayOrSymbol(sym.Where(mask(p_states, new_s), new_s, old_s));
                    }).ToArray();
                }
            }

            _prev_output = output;

            return (output, states);
        }
    }
}
