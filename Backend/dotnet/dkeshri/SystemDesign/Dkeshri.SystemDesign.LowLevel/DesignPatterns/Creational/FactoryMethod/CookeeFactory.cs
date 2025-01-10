﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.DesignPatterns.Creational.FactoryMethod
{
    internal class CookeeFactory : BakeryFactory
    {
        public override IBakedProduct CreateProduct()
        {
            return new SweetCookee();
        }
    }
}
