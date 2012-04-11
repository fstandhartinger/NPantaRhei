using System;
using npantarhei.runtime.contract;

namespace npantarhei.runtime.messagetypes
{
    public interface IAsynchronizer<T>
    {
        void Process(T message, Action<T> continueWith);

        void Start();
        void Stop();
    }

    public class AsyncWrapperOperation : IOperation
    {
        private readonly IAsynchronizer<IMessage> _asyncer;

        public AsyncWrapperOperation(IAsynchronizer<IMessage> asyncer, IOperation operationToWrap)
        {
            _asyncer = asyncer;
            this.Name = operationToWrap.Name;
            this.Implementation = (input, continueWith) => asyncer.Process(input, output => operationToWrap.Implementation(output, continueWith));
        }

        public void Start() { _asyncer.Start(); }
        public void Stop() { _asyncer.Stop(); }

        #region IOperation implementation
        public string Name { get; private set; }
        public OperationAdapter Implementation { get; private set; }
        #endregion
    }
}