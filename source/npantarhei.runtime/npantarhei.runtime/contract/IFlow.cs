﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace npantarhei.runtime.contract
{
    public interface IFlow
    {
        IEnumerable<IStream> Streams { get; }
        IEnumerable<IOperation> Operations { get; }
    }
}
