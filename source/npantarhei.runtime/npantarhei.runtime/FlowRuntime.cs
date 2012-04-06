using System;
using System.Collections.Generic;
using System.Linq;

using npantarhei.runtime.contract;
using npantarhei.runtime.messagetypes;
using npantarhei.runtime.operations;
using npantarhei.runtime.flows;
using npantarhei.runtime.data;

namespace npantarhei.runtime
{
	public class FlowRuntime : IFlowRuntime
	{		
		public FlowRuntime()
		{
			// Build
			var regStream = new Register_stream();
			var regOp = new Register_operation();
			
			var flow = new Flow_asynchronously();
			
			// Bind
			_addStream += regStream.Process;
			_addOperation += regOp.Process;
			
			_process += flow.Process;
		    flow.Message += _ => Message(_);
		    flow.Result += _ => Result(_);
		    flow.UnhandledException += _ => UnhandledException(_);

			_start += flow.Start;
			_stop += flow.Stop;
			
			// Inject
			var streams = new List<IStream>();
			regStream.Inject(streams);
			
			var operations = new Dictionary<string, IOperation>();
			regOp.Inject(operations);
			
			flow.Inject(streams, operations);
		}
		
		#region IFlowRuntime implementation
		private readonly Action<IMessage> _process;
		public void Process(IMessage message) { _process(message); }

	    public event Action<IMessage> Message = _ => { };
	    public event Action<FlowRuntimeException> UnhandledException;
	    public event Action<IMessage> Result;
		
		private readonly Action<IStream> _addStream;
		public void AddStream (IStream stream) { _addStream(stream);}
	
		private readonly Action<IOperation> _addOperation;
		public void AddOperation (IOperation operation) { _addOperation(operation); }
		public void AddOperations(IEnumerable<IOperation> operations) { operations.ToList().ForEach(this.AddOperation); }
		
		private readonly Action _start;
		public void Start() { _start(); }
		
		private readonly Action _stop;
	    public void Stop() { _stop(); }
		#endregion
		
		
		public void Dispose() { Stop(); }
	}
}

