﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MxNet.Keras.Layers
{
    public class Cropping3D : _Cropping
    {
        public Cropping3D(((int, int), (int, int), (int, int))? cropping = null, string data_format = "")
            : base((1, 1), data_format)
        {
            throw new NotImplementedException();
        }

        public override ConfigDict GetConfig()
        {
            throw new NotImplementedException();
        }
    }
}
