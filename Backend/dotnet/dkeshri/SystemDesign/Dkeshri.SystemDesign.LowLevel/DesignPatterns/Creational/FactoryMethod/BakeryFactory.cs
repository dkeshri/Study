﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.DesignPatterns.Creational.FactoryMethod
{
    public abstract class BakeryFactory
    {
        public abstract IBakedProduct CreateProduct();
    }
}
