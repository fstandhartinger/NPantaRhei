using System;
using System.Collections.Generic;
using npantarhei.runtime.contract;
using npantarhei.runtime.messagetypes;

namespace npantarhei.runtime.patterns
{
    internal class AutoResetJoin<T0, T1> : AutoResetJoinBase
    {
        public AutoResetJoin(string name) : base(name, 2, Create_join_tuple) { }

        private static object Create_join_tuple(List<object> joinList)
        {
            return new Tuple<T0, T1>((T0)joinList[0],
                                     (T1)joinList[1]);
        }
    }

    internal class AutoResetJoin<T0, T1, T2> : AutoResetJoinBase
    {
        public AutoResetJoin(string name) : base(name, 3, Create_join_tuple) { }

        private static object Create_join_tuple(List<object> joinList)
        {
            return new Tuple<T0, T1, T2>((T0)joinList[0],
                                         (T1)joinList[1],
                                         (T2)joinList[2]);
        }
    }

    /*
     * Port names:
     *      input: .in0, .in1, ..., .in9 (last char in port name is index of input)
     *      output: none (only name of operation)
     */
    internal class AutoResetJoinBase : IOperation
    {
        private readonly AutoResetJoin _arj;

        public AutoResetJoinBase(string name, int numberOfInputs, Func<List<object>, object> createJoinTuple)
        {
            if (numberOfInputs>10) throw new ArgumentException("Maximum of 10 input ports exceeded!");

            _name = name;
            _arj = new AutoResetJoin(numberOfInputs);

            _implementation = (input, continueWith, _) =>
                                  {
                                      var inputIndex = int.Parse(input.Port.Name.Substring(input.Port.Name.Length - 1));
                                      _arj.Process(inputIndex, input.Data, 
                                                   joinList => continueWith(new Message(_name, createJoinTuple(joinList))));
                                  };
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        private readonly OperationAdapter _implementation;
        public OperationAdapter Implementation
        {
            get { return _implementation; }
        }
    }
}