﻿#region Copyright 2011 by Roger Knapp, Licensed under the Apache License, Version 2.0
/* Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion
using System;
using CSharpTest.Net.RpcLibrary;
using Google.ProtocolBuffers;
using Google.ProtocolBuffers.Rpc;
using Google.ProtocolBuffers.Rpc.Win32Rpc;

namespace ProtocolBuffers.Rpc.Benchmarks.TestSuites
{
    class ProtoBuf_LRPC : ProtoBufRpcBase
    {
        protected override void PrepareService(Win32RpcServer service, Guid iid)
        {
            service.AddProtocol(RpcProtseq.ncalrpc.ToString(), iid.ToString("N"));
        }

        protected override IRpcDispatch Connect(Guid iid)
        {
            return RpcClient.ConnectRpc(iid, RpcProtseq.ncalrpc.ToString(), null, iid.ToString("N"));
        }
    }
    class ProtoBuf_LRPC_Auth : ProtoBuf_LRPC
    {
        protected override void PrepareService(Win32RpcServer service, Guid iid)
        {
            base.PrepareService(service, iid);
            service.AddAuthWinNT();
        }

        protected override IRpcServerStub CreateStub(int responseSize)
        {
            return new ImpersonatingServerStub(base.CreateStub(responseSize));
        }

        protected override IRpcDispatch Connect(Guid iid)
        {
            return 
                ((RpcClient)base.Connect(iid)).Authenticate(RpcAuthenticationType.Self)
                ;
        }
    }
}
