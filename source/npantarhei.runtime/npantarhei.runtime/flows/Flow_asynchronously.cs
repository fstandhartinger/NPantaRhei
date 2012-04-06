using System;
using System.Collections.Generic;

using npantarhei.runtime.contract;
using npantarhei.runtime.operations;
using npantarhei.runtime.data;
using npantarhei.runtime.patterns;

namespace npantarhei.runtime.flows
{
	internal class Flow_asynchronously
	{
		private Process_message _processMessage;
		
		public Flow_asynchronously()
		{
			// Build
			var async = new Asynchronize<IMessage>();
		    var handleException = new Handle_exception();
			_processMessage = new Process_message();
			
			// Bind
			_process += async.Enqueue;
		    async.Dequeued += handleException.Process;
		    async.Dequeued += _ => Message(_);
            handleException.ContinueWith += _processMessage.Process;
		    handleException.ExceptionCaught += _ => Exception(_);
			_processMessage.Continue += async.Enqueue;
            _processMessage.Result += _ => Message(_);
			_processMessage.Result += _ => Result(_);
			
			_start += async.Start;
			_stop += async.Stop;
		}
		
		public void Inject(List<IStream> streams, Dictionary<string, IOperation> operations)
		{
			_processMessage.Inject(streams, operations);
		}
		
		private Action<IMessage> _process;
		public void Process(IMessage message) { _process(message); }

        public event Action<IMessage> Message = _ => { };
		public event Action<IMessage> Result;
	    public event Action<FlowRuntimeException> Exception;
		
		private Action _start;
		public void Start() { _start(); }
		
		private Action _stop;
		public void Stop() { _stop(); }
	}
}

